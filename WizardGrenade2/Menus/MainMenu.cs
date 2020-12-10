using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WizardGrenade2
{
    class MainMenu
    {
        public bool SettingUpGame { get; set; }
        public float Brightness { get => _settings.Brightness.Value; }
        public bool CanGoBack { get => SettingUpGame || _settings.InSettings || _instructions.InInstructions || _credits.InCredits; }

        private Options _menuOptions;
        private Settings _settings;
        private Instructions _instructions;
        private Credits _credits;
        private MenuWizard _wizard;
        private Vector2 _wizardPosition;

        public MainMenu()
        {
            _menuOptions = new Options(MenuSettings.MenuOptionsList, false, true);
            _instructions = new Instructions();
            _credits = new Credits();
            SetMenuPositions();
        }

        public void SetMenuPositions()
        {
            _wizardPosition = new Vector2(100, ScreenSettings.CentreScreenHeight);
            _menuOptions.SetOptionLayout(MenuSettings.MenuOptionsPosition, MenuSettings.MenuOptionsLastPosition);
            _settings = new Settings(MenuSettings.MenuSettingsPosition, MenuSettings.MenuOptionsLastPosition, 
                                     MenuSettings.MenuSettingsOffset, MenuSettings.MenuSettingsSpritesSpan);
        }

        public void LoadContent(ContentManager contentManager)
        {
            _menuOptions.LoadContent(contentManager);
            _settings.LoadContent(contentManager);
            _instructions.LoadContent(contentManager);
            _credits.LoadContent(contentManager);
            _wizard = new MenuWizard(contentManager);
        }

        public int[] GetSettings() => _settings.GetSettings();

        public void ApplySettings(int[] settings, GameTime gameTime)
        {
            _settings.ApplySettings(settings[0], settings[1], settings[2]);
            _settings.Update(gameTime);
        }

        public void Update(GameTime gameTime)
        {
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

            if (InputManager.WasKeyPressed(Keys.Back) || InputManager.WasKeyPressed(Keys.Enter))
                SoundManager.Instance.PlaySound("stone0");
        }

        private void UpdateMainMenu()
        {
            if (InputManager.WasKeyPressed(Keys.Enter))
            {
                if (_menuOptions.SelectedOption == MenuSettings.MenuOptionsList.IndexOf("Play"))
                    SettingUpGame = true;

                else if (_menuOptions.SelectedOption == MenuSettings.MenuOptionsList.IndexOf("Settings"))
                    _settings.InSettings = true;

                else if (_menuOptions.SelectedOption == MenuSettings.MenuOptionsList.IndexOf("How to Play"))
                    _instructions.InInstructions = true;

                else if (_menuOptions.SelectedOption == MenuSettings.MenuOptionsList.IndexOf("Credits"))
                    _credits.InCredits = true;

                else if (_menuOptions.SelectedOption == MenuSettings.MenuOptionsList.IndexOf("Quit"))
                    StateMachine.Instance.ExitGame();
            }
        }

        private void UpdateSettingsMenu(GameTime gameTime) => _settings.Update(gameTime);
        private void UpdateInstructions(GameTime gameTime) => _instructions.Update(gameTime);
        private void UpdateCredits() => _credits.Update();

        public void Draw(SpriteBatch spriteBatch)
        {
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

        private void DrawSettings(SpriteBatch spriteBatch) => _settings.Draw(spriteBatch);
        private void DrawInstructions(SpriteBatch spriteBatch) => _instructions.Draw(spriteBatch);
        private void DrawCredits(SpriteBatch spriteBatch) => _credits.Draw(spriteBatch);
    }
}
