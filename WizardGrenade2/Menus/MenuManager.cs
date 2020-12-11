using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WizardGrenade2
{
    public class MenuManager
    {
        public GameOptions GameOptions { get => _gameSetup.GameOptions; }
        public int[] MainMenuSettings { get => _mainMenu.Settings; }
        public float Brightness { get => _mainMenu.Brightness; }
        public bool IsGameSetup { get => _gameSetup.isGameSetup; }

        private MainMenu _mainMenu = new MainMenu();
        private GameSetup _gameSetup = new GameSetup();
        private MenuSky _sky = new MenuSky();
        private MenuTitle _title = new MenuTitle();
        private MenuArrows _menuArrows = new MenuArrows();
        private bool _inGameSetup;

        public MenuManager(ContentManager contentManager)
        {
            LoadContent(contentManager);
        }

        private void LoadContent(ContentManager contentManager)
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

            if (_inGameSetup)
            {
                _gameSetup.Update(gameTime);
                _inGameSetup = _gameSetup.InGameSetup;
                _mainMenu.InGameSetup = _inGameSetup ? true : false;
            }
            else
            {
                _mainMenu.Update(gameTime);
                _inGameSetup = _mainMenu.InGameSetup;
                _gameSetup.InGameSetup = _inGameSetup ? true : false;
            }
        }

        public void ResetGame() => _gameSetup.ResetGame();
        public void ApplySettings(int[] settings, GameTime gameTime) => _mainMenu.ApplySettings(settings, gameTime);

        public void Draw(SpriteBatch spriteBatch)
        {
            _sky.Draw(spriteBatch);
            _title.Draw(spriteBatch);
            _menuArrows.Draw(spriteBatch, _mainMenu.CanGoBack, _mainMenu.CanGoForward);

            if (_mainMenu.InGameSetup)
                _gameSetup.Draw(spriteBatch);
            else
                _mainMenu.Draw(spriteBatch);
        }
    }
}
