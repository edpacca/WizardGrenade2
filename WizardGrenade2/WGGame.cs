using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq.Expressions;

namespace WizardGrenade2
{
    public class WGGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GameScreen _gameScreen = new GameScreen();
        private InterfaceManager _interfaceManager;
        private UserInterface _userInterface;
        private GameSetup _gameSetup = new GameSetup();
        private PauseMenu _pauseMenu;
        private Scenery _scenery = new Scenery();

        private bool _isGameSetup;

        Color backgroundColour = new Color(40, 40, 45);

        public WGGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _graphics.PreferredBackBufferWidth = ScreenSettings.RESOLUTION_WIDTH;
            _graphics.PreferredBackBufferHeight = ScreenSettings.RESOLUTION_HEIGHT;
            _interfaceManager = new InterfaceManager();
            Window.AllowUserResizing = true;
        }

        protected override void Initialize()
        {
            Mouse.SetPosition(ScreenSettings.CentreScreenWidth, ScreenSettings.CentreScreenHeight);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _pauseMenu = new PauseMenu(GraphicsDevice);
            _gameSetup.LoadContent(Content);
        }

        protected void LoadGameContent()
        {
            _gameScreen.Initialise(_gameSetup.GameOptions);
            _userInterface = new UserInterface(_gameSetup.GameOptions);
            _gameScreen.LoadContent(Content);
            _scenery.LoadContent(Content);
            _userInterface.LoadContent(Content);
            _pauseMenu.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (_isGameSetup)
            {
                _pauseMenu.PauseGame(Keys.Escape);
                _pauseMenu.Update(gameTime);
            }

            if (InputManager.WasKeyPressed(Keys.Delete))
                Exit();

            InputManager.Update();

            if (!_isGameSetup)
            {
                _gameSetup.Update(gameTime);
                if (_gameSetup.isGameSetup())
                {
                    LoadGameContent();
                    _isGameSetup = true;
                }
            }

            else if (!_pauseMenu.IsPaused)
                UpdateGame(gameTime);

            _scenery.Update(gameTime);
            base.Update(gameTime);
        }

        protected void UpdateGame(GameTime gameTime)
        {
            _interfaceManager.Update(gameTime);
            _userInterface.Update(gameTime, _gameScreen.TeamHealths);
            _gameScreen.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(backgroundColour);

            if (_isGameSetup)
                DrawGame();
            else
                DrawGameSetup();

            base.Draw(gameTime);
        }

        protected void DrawGame()
        {
            _spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, _interfaceManager.GetScaleMatrix());
            _scenery.DrawBackground(_spriteBatch);
            _gameScreen.Draw(_spriteBatch);
            _scenery.DrawForeground(_spriteBatch);
            _spriteBatch.End();

            _spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, _interfaceManager.GetOriginMatrix());
            if (StateMachine.Instance.GameState != StateMachine.GameStates.PlaceWizards && !_pauseMenu.IsPaused)
                _userInterface.Draw(_spriteBatch);
            _pauseMenu.Draw(_spriteBatch);
            _spriteBatch.End();
        }

        protected void DrawGameSetup()
        {
            _spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, _interfaceManager.GetOriginMatrix());
            _gameSetup.Draw(_spriteBatch);
            _spriteBatch.End();
        }
    }
}
