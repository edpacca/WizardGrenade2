using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

namespace WizardGrenade2
{
    public class WGGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GameScreen _gameScreen = new GameScreen();
        private CameraManager _cameraManager;
        private UserInterface _userInterface;
        private PauseMenu _pauseMenu;
        private Scenery _scenery = new Scenery();
        private MenuManager _menuManager = new MenuManager();
                private bool _isGameSetup;
        private BrightnessManager _brightnessManager;
        private float _globalBrightness = 0.5f;
        private bool _isGameContentLoaded;

        public WGGame()
        {
            Content.RootDirectory = "Content";
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
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
            _brightnessManager = new BrightnessManager(GraphicsDevice, Content);
            _menuManager.LoadContent(Content);

            MediaPlayer.Volume = 0.5f;
            SoundEffect.MasterVolume = 0.5f;
            SoundManager.Instance.LoadContent(Content);
            SoundManager.Instance.PlaySong(0);
        }

        protected void LoadGameContent()
        {
            _gameScreen.Initialise(_menuManager.GameOptions);
            _userInterface = new UserInterface(_menuManager.GameOptions);
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
            {
                Content.Unload();
                Exit();
            }

            InputManager.Update();

            if (!_menuManager.IsGameSetup)
            {
                _menuManager.Update(gameTime);

                if (_menuManager.IsGameSetup)
                {
                    if (!_isGameContentLoaded)
                    {
                        LoadGameContent();
                        _isGameContentLoaded = true;
                    }

                    _pauseMenu.ApplySettings(_menuManager.MainMenuSettings, gameTime);
                    _isGameSetup = true;
                }
            }

            else if (!_pauseMenu.IsPaused)
                UpdateGame(gameTime);

            _scenery.Update(gameTime);
            _globalBrightness = _isGameSetup ? _pauseMenu.GetBrightness() : _menuManager.Brightness;
            _brightnessManager.SetBrightness(_globalBrightness);
            base.Update(gameTime);
        }

        protected void UpdateGame(GameTime gameTime)
        {
            _cameraManager.Update(gameTime);
            _userInterface.Update(gameTime, _gameScreen.TeamHealths);
            StateMachine.Instance.UpdateStateMachine(gameTime);
            _gameScreen.Update(gameTime);

            if (StateMachine.Instance.GameState == StateMachine.GameStates.GameOver)
            {
                if (InputManager.WasKeyPressed(Keys.Enter))
                {
                    ResetGame(gameTime);
                }
            }

            if (StateMachine.Instance.GameState == StateMachine.GameStates.Reset)
                ResetGame(gameTime);
        }

        private void ResetGame(GameTime gameTime)
        {
            _menuManager.ApplySettings(_pauseMenu.GetSettings(), gameTime);
            _menuManager.ResetGame();
            _isGameSetup = false;
            _isGameContentLoaded = false;
            SoundManager.Instance.PlaySong(0);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Colours.BackgroundColour);

            if (_isGameSetup)
                DrawGame();
            else
                DrawMenus();

            DrawOverlay();

            base.Draw(gameTime);
        }

        protected void DrawGame()
        {
            _spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, _cameraManager.TransformMatrix);
            _scenery.DrawBackground(_spriteBatch);
            _gameScreen.Draw(_spriteBatch);
            _scenery.DrawForeground(_spriteBatch);
            _spriteBatch.End();

            DrawUILayer();
        }

        protected void DrawUILayer()
        {
            _spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, _cameraManager.OriginMatrix);

            if (!_pauseMenu.IsPaused && StateMachine.Instance.GameState != StateMachine.GameStates.PlaceWizards && StateMachine.Instance.GameState != StateMachine.GameStates.GameOver)
                _userInterface.Draw(_spriteBatch);

            StateMachine.Instance.Draw(_spriteBatch);
            _pauseMenu.Draw(_spriteBatch);

            _spriteBatch.End();
        }

        protected void DrawMenus()
        {
            _spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, _cameraManager.OriginMatrix);
            _menuManager.Draw(_spriteBatch);
            _spriteBatch.End();
        }

        protected void DrawOverlay()
        {
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, _cameraManager.OriginMatrix);
            _brightnessManager.Draw(_spriteBatch);
            _spriteBatch.End();
        }
    }
}
