using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace WizardGrenade2
{
    class PauseMenu
    {
        private SpriteFont _optionsFont;
        private Vector2 _textPosition = ScreenSettings.ScreenCentre;
        private readonly string _pausedText = "Paused";
        private GraphicsDevice _graphics;
        private Texture2D _background;
        private Rectangle _backgroundRectangle;
        private Color _backgroundColour = new Color(0, 0, 0, 150);
        private Sprite _bigScroll;

        private Settings _settings;

        private Options _pauseMenuOptions;
        private List<string> _pauseMenuOptionNames = new List<string>()
        { 
          "Resume",
          "Settings",
          "Restart",
          "Quit",
          "Fireball?"
        };

        private Scroll _scroll;
        public bool IsPaused { get; set; }
        private bool _previousPauseState;
        private bool _currentPauseState;
        public bool InSettings { get; set; }
        public bool Reset { get; set; }

        public PauseMenu(GraphicsDevice graphics)
        {
            _graphics = graphics;
            _pauseMenuOptions = new Options(_pauseMenuOptionNames, false, true);
        }

        public void LoadContent(ContentManager contentManager)
        {
            _optionsFont = contentManager.Load<SpriteFont>(@"Fonts/ScreenFont");
            _textPosition = ScreenSettings.ScreenCentre - _optionsFont.MeasureString(_pausedText) / 2;
            _textPosition.Y -= 170;

            _backgroundRectangle = new Rectangle(0, 0, (int)ScreenSettings.TARGET_WIDTH, (int)ScreenSettings.TARGET_HEIGHT);
            _background = new Texture2D(_graphics, 1, 1);
            _background.SetData(new Color[] { _backgroundColour });

            _scroll = new Scroll(ScreenSettings.ScreenCentre);
            _scroll.LoadContent(contentManager);

            _pauseMenuOptions.SetOptionLayout(new Vector2(_textPosition.X, _textPosition.Y + 100), ScreenSettings.TARGET_HEIGHT - 210);
            _pauseMenuOptions.SetOptionColours(Colours.Ink, Colours.FadedInk);
            _pauseMenuOptions.LoadContent(contentManager);
            _settings = new Settings(new Vector2(_textPosition.X - 200, _textPosition.Y + 100), ScreenSettings.TARGET_HEIGHT - 210, new Vector2(300, 0), 300f);
            _settings.LoadContent(contentManager);
            _bigScroll = new Sprite(contentManager, @"Menu/bigScroll");
            _settings.SetOptionColours(Colours.Ink, Colours.FadedInk);
        }

        public void PauseGame(Keys key)
        {
            if (InputManager.WasKeyPressed(key))
            {
                IsPaused = IsPaused ? false : true;

                if (IsPaused)
                    SoundManager.Instance.PlaySound("scroll");
            }
        }

        public void Update(GameTime gameTime)
        {
            _previousPauseState = _currentPauseState;
            _currentPauseState = IsPaused;

            if (IsPaused)
            {
                _scroll.Update(gameTime);
                _pauseMenuOptions.Update(gameTime);

                if (InputManager.WasKeyPressed(Keys.Enter))
                    OptionFunctions(_pauseMenuOptions.SelectedOption);

                if (InSettings)
                {
                    _settings.Update(gameTime);
                    if (InputManager.WasKeyPressed(Keys.Back))
                        InSettings = false;
                }
            }

            else if (_currentPauseState != _previousPauseState)
            {
                _scroll.ResetPauseMenu();
            }
        }

        public void ApplySettings(int[] settings, GameTime gameTime)
        {
            _settings.ApplySettings(settings[0], settings[1], settings[2]);
            _settings.Update(gameTime);
        }

        public int[] GetSettings()
        {
            return _settings.GetSettings();
        }

        public float GetBrightness() => _settings.Brightness.Value;

        private void OptionFunctions(int selectedIndex)
        {
            if (selectedIndex == _pauseMenuOptionNames.IndexOf("Resume"))
                IsPaused = false;

            if (selectedIndex == _pauseMenuOptionNames.IndexOf("Settings"))
                InSettings = true;

            if (selectedIndex == _pauseMenuOptionNames.IndexOf("Quit"))
                StateMachine.Instance.ExitGame();

            if (selectedIndex == _pauseMenuOptionNames.IndexOf("Restart"))
            {
                StateMachine.Instance.ResetGame();
                IsPaused = false;
            }

            if (selectedIndex == _pauseMenuOptionNames.IndexOf("Fireball?"))
            {
                WeaponManager.Instance.SpawnHugeFireballs();
                IsPaused = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsPaused)
            {
                spriteBatch.Draw(_background, _backgroundRectangle, _backgroundColour);
                _scroll.Draw(spriteBatch);

                if (InSettings)
                {
                    _bigScroll.DrawSprite(spriteBatch, ScreenSettings.ScreenCentre - _bigScroll.GetSpriteOrigin());
                    _settings.Draw(spriteBatch);
                }

                if (_scroll.IsUnrolled && !InSettings)
                    _pauseMenuOptions.DrawOptions(spriteBatch);

                spriteBatch.DrawString(_optionsFont, _pausedText, _textPosition, Colours.Ink);
            }
        }
    }
}
