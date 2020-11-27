using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace WizardGrenade2
{
    class MainMenu
    {
        private Sprite _title;
        private Sprite _wizard;
        private Vector2 _titlePosition;
        private Vector2 _wizardPosition;
        private Vector2 _optionsFirstPosition = new Vector2(ScreenSettings.TARGET_WIDTH / 2.5f, ScreenSettings.TARGET_HEIGHT * 0.4f);
        private float _optionsLastYPosition = ScreenSettings.TARGET_HEIGHT * 0.8f;

        private Options _menuOptions;
        public bool PlayGame { get; private set; }

        public readonly List<string> _menuOptionNames = new List<string>()
        {
            "Play",
            "Settings",
            "Credits",
            "Quit"
        };

        public MainMenu()
        {
            _menuOptions = new Options(_menuOptionNames, false, true);
            SetMenuPositions();
        }

        public void SetMenuPositions()
        {
            _titlePosition = new Vector2(ScreenSettings.CentreScreenWidth, ScreenSettings.TARGET_HEIGHT / 6);
            _wizardPosition = new Vector2();
            _menuOptions.SetOptionLayout(_optionsFirstPosition, _optionsLastYPosition);
        }

        public void LoadContent(ContentManager contentManager)
        {
            _wizard = new Sprite(contentManager, "Wizard0");
            _title = new Sprite(contentManager, "Title");
            _menuOptions.LoadContent(contentManager);
        }

        public void Update(GameTime gameTime)
        {
            _menuOptions.Update(gameTime);
            UpdateMainMenu();

        }

        private void UpdateMainMenu()
        {
            if (InputManager.WasKeyPressed(Keys.Enter))
            {
                if (_menuOptions.SelectedOption == _menuOptionNames.IndexOf("Play"))
                    PlayGame = true;
                else if (_menuOptions.SelectedOption == _menuOptionNames.IndexOf("Quit"))
                    StateMachine.Instance.ExitGame();
            }
        }

        private void UpdateSettingsMenu()
        {

        }

        private void UpdateCredits()
        {

        }

        private void StoreSettings()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _title.DrawSpriteAtScale(spriteBatch, _titlePosition, 0.4f);
            DrawMainMenu(spriteBatch);
        }

        public void DrawMainMenu(SpriteBatch spriteBatch)
        {
            _menuOptions.DrawOptions(spriteBatch);
        }

        private void DrawSettings(SpriteBatch spriteBatch)
        {
        }
    }
}
