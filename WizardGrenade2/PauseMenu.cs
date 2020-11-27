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

        private Options _pauseMenuOptions;
        private List<string> _pauseMenuOptionNames = new List<string>()
        { 
          "Resume",
          "Settings",
          "Quit",
          "Fireball"
        };

        private Scroll _scroll;
        public bool IsPaused { get; set; }
        private bool _previousPauseState;
        private bool _currentPauseState;

        public PauseMenu(GraphicsDevice graphics)
        {
            _graphics = graphics;
            _pauseMenuOptions = new Options(_pauseMenuOptionNames, false, true);
        }

        public void LoadContent(ContentManager contentManager)
        {
            _optionsFont = contentManager.Load<SpriteFont>("ScreenFont");
            _textPosition -= _optionsFont.MeasureString(_pausedText) / 2;
            _textPosition.Y -= 170;

            _backgroundRectangle = new Rectangle(0, 0, (int)ScreenSettings.TARGET_WIDTH, (int)ScreenSettings.TARGET_HEIGHT);
            _background = new Texture2D(_graphics, 1, 1);
            _background.SetData(new Color[] { _backgroundColour });

            _scroll = new Scroll(ScreenSettings.ScreenCentre);
            _scroll.LoadContent(contentManager);

            _pauseMenuOptions.SetOptionLayout(new Vector2(_textPosition.X, _textPosition.Y + 100), ScreenSettings.TARGET_HEIGHT - 210);
            _pauseMenuOptions.SetOptionColours(Colours.Ink, Colours.FadedInk);
            _pauseMenuOptions.LoadContent(contentManager);
        }

        public void PauseGame(Keys key)
        {
            if (InputManager.WasKeyPressed(key))
                IsPaused = IsPaused ? false : true;
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
            }

            else if (_currentPauseState != _previousPauseState)
                _scroll.ResetPauseMenu();
        }

        private void OptionFunctions(int selectedIndex)
        {
            if (selectedIndex == _pauseMenuOptionNames.IndexOf("Resume"))
                IsPaused = false;

            if (selectedIndex == _pauseMenuOptionNames.IndexOf("Quit"))
                StateMachine.Instance.ExitGame();
                
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsPaused)
            {
                spriteBatch.Draw(_background, _backgroundRectangle, _backgroundColour);
                _scroll.Draw(spriteBatch);
                if (_scroll.IsUnrolled)
                    _pauseMenuOptions.DrawOptions(spriteBatch);
                spriteBatch.DrawString(_optionsFont, _pausedText, _textPosition, Colours.Ink);
            }
        }
    }
}
