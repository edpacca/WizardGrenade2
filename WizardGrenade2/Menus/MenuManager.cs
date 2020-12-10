using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WizardGrenade2
{
    public class MenuManager
    {
        public GameOptions GameOptions { get => _gameSetup.GameOptions; }
        public float Brightness { get => _mainMenu.Brightness; }
        public int[] MainMenuSettings { get => _mainMenu.GetSettings(); }
        public bool IsGameSetup { get => _gameSetup.isGameSetup; }

        private MainMenu _mainMenu = new MainMenu();
        private GameSetup _gameSetup = new GameSetup();
        private MenuSky _sky = new MenuSky();
        private MenuTitle _title = new MenuTitle();
        private MenuArrows _menuArrows = new MenuArrows();
        private bool _settingUpGame;

        public void LoadContent(ContentManager contentManager)
        {
            _title.LoadContent(contentManager);
            _mainMenu.LoadContent(contentManager);
            _sky.LoadContent(contentManager);
            _gameSetup.LoadContent(contentManager);
            _menuArrows.LoadContent(contentManager);
        }

        public void Update(GameTime gameTime)
        {
            _sky.Update(gameTime);
            _title.Update(gameTime);

            if (_settingUpGame)
            {
                _settingUpGame = _gameSetup.InGameSetup;
                _mainMenu.SettingUpGame = _settingUpGame ? true : false;
                _gameSetup.Update(gameTime);
            }
            else
            {
                _settingUpGame = _mainMenu.SettingUpGame;
                _gameSetup.InGameSetup = _settingUpGame ? true : false;
                _mainMenu.Update(gameTime);
            }
        }

        public void ResetGame() => _gameSetup.ResetGame();
        public void ApplySettings(int[] settings, GameTime gameTime) => _mainMenu.ApplySettings(settings, gameTime);

        public void Draw(SpriteBatch spriteBatch)
        {
            _sky.Draw(spriteBatch);
            _title.Draw(spriteBatch);
            _menuArrows.Draw(spriteBatch, _mainMenu.CanGoBack);

            if (_mainMenu.SettingUpGame)
                _gameSetup.Draw(spriteBatch);
            else
                _mainMenu.Draw(spriteBatch);
        }
    }
}
