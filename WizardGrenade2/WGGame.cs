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
        private CameraManager _cameraManager;
        private UserInterface _userInterface;
        private MainMenu _mainMenu = new MainMenu();
        private GameSetup _gameSetup = new GameSetup();
        private PauseMenu _pauseMenu;
        private Scenery _scenery = new Scenery();

        private bool _isGameSetup;
        private bool _setupGame;

        public WGGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _graphics.PreferredBackBufferWidth = ScreenSettings.RESOLUTION_WIDTH;
            _graphics.PreferredBackBufferHeight = ScreenSettings.RESOLUTION_HEIGHT;
            _cameraManager = new CameraManager();
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
            _mainMenu.LoadContent(Content);
            _gameSetup.LoadContent(Content);
            _cameraManager.LoadContent(Content);
        }

        protected void LoadGameContent()
        {
            _gameScreen.Initialise(_gameSetup.GameOptions);
            _userInterface = new UserInterface(_gameSetup.GameOptions);
            _gameScreen.LoadContent(Content);
            _scenery.LoadContent(Content);
            _userInterface.LoadContent(Content);
            _pauseMenu.LoadContent(Content);
            StateMachine.Instance.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (_isGameSetup)
            {
                _pauseMenu.PauseGame(Keys.Escape);
                _pauseMenu.Update(gameTime);
            }

            if (InputManager.WasKeyPressed(Keys.Delete) || StateMachine.Instance.GameState == StateMachine.GameStates.ExitGame)
                Exit();

            InputManager.Update();

            if (!_isGameSetup)
            {
                if (_setupGame)
                {
                    _setupGame = _gameSetup.SettingUpGame;
                    _mainMenu.SettingUpGame = _setupGame ? true : false;
                    _gameSetup.Update(gameTime);
                }
                else
                {
                    _setupGame = _mainMenu.SettingUpGame;
                    _gameSetup.SettingUpGame = _setupGame ? true : false;
                    _mainMenu.Update(gameTime);
                }

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
            _cameraManager.Update(gameTime);
            _userInterface.Update(gameTime, _gameScreen.TeamHealths);
            StateMachine.Instance.UpdateStateMachine(gameTime);
            _gameScreen.Update(gameTime);
            //if (StateMachine.Instance.GameState == StateMachine.GameStates.GameOver)
            //{
            //    if(InputManager.WasKeyPressed(Keys.End))
            //    {
            //        _gameSetup.ResetGame();
            //        LoadContent();
            //        _isGameSetup = false;
            //    }
            //}
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Colours.BackgroundColour);

            if (_isGameSetup)
                DrawGame();
            else
                DrawGameSetup();

            base.Draw(gameTime);
        }

        protected void DrawGame()
        {
            _spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, _cameraManager.GetScaleMatrix());
            _scenery.DrawBackground(_spriteBatch);
            _gameScreen.Draw(_spriteBatch);
            _scenery.DrawForeground(_spriteBatch);
            _spriteBatch.End();

            _spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, _cameraManager.GetOriginMatrix());
            if (!_pauseMenu.IsPaused && StateMachine.Instance.GameState != StateMachine.GameStates.PlaceWizards && StateMachine.Instance.GameState != StateMachine.GameStates.GameOver)
                _userInterface.Draw(_spriteBatch);
            StateMachine.Instance.Draw(_spriteBatch);
            _pauseMenu.Draw(_spriteBatch);
            _cameraManager.Draw(_spriteBatch);
            _spriteBatch.End();
        }

        protected void DrawGameSetup()
        {
            _spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, _cameraManager.GetOriginMatrix());
            if (_mainMenu.SettingUpGame)
                _gameSetup.Draw(_spriteBatch);
            else
                _mainMenu.Draw(_spriteBatch);
            _spriteBatch.End();
        }
    }
}
