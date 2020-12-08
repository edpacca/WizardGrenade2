using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;


namespace WizardGrenade2
{
    class MainMenu
    {
        private Sprite _title;
        private Vector2 _titlePosition;
        private Vector2 _wizardPosition;
        private MenuWizard _wizard;
        private Vector2 _optionsFirstPosition = new Vector2(ScreenSettings.TARGET_WIDTH / 2.5f, ScreenSettings.TARGET_HEIGHT * 0.4f);
        private float _optionsLastYPosition = ScreenSettings.TARGET_HEIGHT * 0.8f;
        private Sprite _arrow;
        private Vector2 _arrowLPosition;
        private Vector2 _arrowRPosition;
        private const float ARROW_SCALE = 6f;
        private SpriteFont _infoFont;

        private Options _menuOptions;
        public bool SettingUpGame { get; set; }

        public Settings _settings { get; private set; }
        private Instructions _instructions;
        private Credits _credits;

        private Vector2 _settingsOffset = new Vector2(320, 0);
        private float _settingsSpriteMeterWidth = ScreenSettings.CentreScreenWidth - 200;

        public readonly List<string> _menuOptionNames = new List<string>()
        {
            "Play",
            "Settings",
            "How to Play",
            "Credits",
            "Quit"
        };

        public MainMenu()
        {
            _menuOptions = new Options(_menuOptionNames, false, true);
            _instructions = new Instructions();
            _credits = new Credits();
            SetMenuPositions();
        }

        public void SetMenuPositions()
        {
            _titlePosition = new Vector2(ScreenSettings.CentreScreenWidth, ScreenSettings.TARGET_HEIGHT / 6);
            _wizardPosition = new Vector2(100, ScreenSettings.CentreScreenHeight);
            _menuOptions.SetOptionLayout(_optionsFirstPosition, _optionsLastYPosition);
            _settings = new Settings(new Vector2(ScreenSettings.TARGET_WIDTH / 4f, _optionsFirstPosition.Y), _optionsLastYPosition, _settingsOffset, _settingsSpriteMeterWidth);
            _arrowLPosition = new Vector2(20, 20);
            _arrowRPosition = new Vector2(ScreenSettings.TARGET_WIDTH - 20, 20);
        }

        public void LoadContent(ContentManager contentManager)
        {
            _wizard = new MenuWizard(contentManager);
            _title = new Sprite(contentManager, @"Menu/Title");
            _menuOptions.LoadContent(contentManager);
            _settings.LoadContent(contentManager);
            _infoFont = contentManager.Load<SpriteFont>(@"Fonts/InfoFont2");
            _arrow = new Sprite(contentManager, @"GameObjects/MelfsAcidArrow");
            _arrow.SpriteScale = ARROW_SCALE;
            _arrowRPosition.X -= _arrow.GetSpriteRectangle().Width;
            _instructions.LoadContent(contentManager);
            _credits.LoadContent(contentManager);
        }

        public void Update(GameTime gameTime)
        {
            if (InputManager.WasKeyPressed(Keys.Back) || InputManager.WasKeyPressed(Keys.Enter))
                SoundManager.Instance.PlaySound("stone0");

            if (_settings.InSettings)
                UpdateSettingsMenu(gameTime);
            else if (_instructions.InInstructions)
                UpdateInstructions(gameTime);
            else if (_credits.InCredits)
                UpdateCredits();
            else
            {
                _wizard.Update(gameTime);
                _menuOptions.Update(gameTime);
                UpdateMainMenu();
            }
        }

        private void UpdateMainMenu()
        {
            if (InputManager.WasKeyPressed(Keys.Enter))
            {
                if (_menuOptions.SelectedOption == _menuOptionNames.IndexOf("Play"))
                    SettingUpGame = true;
                else if (_menuOptions.SelectedOption == _menuOptionNames.IndexOf("Quit"))
                    StateMachine.Instance.ExitGame();
                else if (_menuOptions.SelectedOption == _menuOptionNames.IndexOf("Settings"))
                    _settings.InSettings = true;
                else if (_menuOptions.SelectedOption == _menuOptionNames.IndexOf("How to Play"))
                    _instructions.InInstructions = true;
                else if (_menuOptions.SelectedOption == _menuOptionNames.IndexOf("Credits"))
                    _credits.InCredits = true;
            }
        }

        private void UpdateSettingsMenu(GameTime gameTime)
        {
            _settings.Update(gameTime);
        }

        private void UpdateInstructions(GameTime gameTime)
        {
            _instructions.Update(gameTime);
        }

        private void UpdateCredits()
        {
            _credits.Update();
        }

        public float GetBrightness() => _settings.Brightness.Value;

        public int[] GetSettings()
        {
            return _settings.GetSettings();
        }

        public void ApplySettings(int[] settings, GameTime gameTime)
        {
            _settings.ApplySettings(settings[0], settings[1], settings[2]);
            _settings.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _title.DrawSpriteAtScale(spriteBatch, _titlePosition, 0.4f);
            if (_settings.InSettings || _instructions.InInstructions || _credits.InCredits)
            {
                _arrow.DrawSprite(spriteBatch, _arrowLPosition + _arrow.GetSpriteOrigin(), Mechanics.PI);
                spriteBatch.DrawString(_infoFont, "BACKSPACE", new Vector2(_arrowLPosition.X - 7, _arrowLPosition.Y + 40), Colours.LightGrey);
            }
            _arrow.DrawSprite(spriteBatch, _arrowRPosition);
            spriteBatch.DrawString(_infoFont, "ENTER", new Vector2(_arrowRPosition.X + 5, _arrowRPosition.Y + 40), Colours.LightGrey);

            if (_settings.InSettings)
                DrawSettings(spriteBatch);
            else if (_instructions.InInstructions)
                DrawInstructions(spriteBatch);
            else if (_credits.InCredits)
                DrawCredits(spriteBatch);
            else
                DrawMainMenu(spriteBatch);
        }

        public void DrawMainMenu(SpriteBatch spriteBatch)
        {
            _menuOptions.DrawOptions(spriteBatch);
            _wizard.Draw(spriteBatch, _wizardPosition);
        }

        private void DrawSettings(SpriteBatch spriteBatch)
        {
            _settings.Draw(spriteBatch);
        }

        private void DrawInstructions(SpriteBatch spriteBatch)
        {
            _instructions.Draw(spriteBatch);
        }

        private void DrawCredits(SpriteBatch spriteBatch)
        {
            _credits.Draw(spriteBatch);
        }
    }
}
