using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WizardGrenade2
{
    public class WGGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private CameraManager _cameraManager;
        private UserInterface _userInterface;
        private BrightnessManager _brightnessManager;
        private MenuManager _menuManager;
        private PauseMenu _pauseMenu;
        private GameScreen _gameScreen;
        private Scenery _scenery;
        private float _globalBrightness;
        private bool _isGameContentLoaded;

        public WGGame()
        {
            Content.RootDirectory = "Content";
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            Window.AllowUserResizing = false;
            Window.Title = "WizardGrenade";
        }

        protected override void Initialize()
        {
            Mouse.SetPosition(ScreenSettings.CentreScreenWidth, ScreenSettings.CentreScreenHeight);
            _cameraManager = new CameraManager();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _brightnessManager = new BrightnessManager(GraphicsDevice, Content);
            _menuManager = new MenuManager(Content);

            MediaPlayer.Volume = 0.5f;
            SoundEffect.MasterVolume = 0.5f;
            SoundManager.Instance.LoadContent(Content);
            SoundManager.Instance.PlaySong(0);
        }

        protected void LoadGameContent()
        {
            _pauseMenu = new PauseMenu(GraphicsDevice, Content);
            _gameScreen = new GameScreen(_menuManager.GameOptions, Content);
            _userInterface = new UserInterface(_menuManager.GameOptions, Content);
            _scenery = new Scenery(Content);
            StateMachine.Instance.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            InputManager.Update();
            UpdateGame(gameTime);
            UpdateBrightnessSettings();
            ExitGame();
            base.Update(gameTime);
        }

        private void UpdateGame(GameTime gameTime)
        {
            if (_menuManager.IsGameSetup)
            {
                _pauseMenu.Update(gameTime);
                _scenery.Update(gameTime);

                if (!_pauseMenu.IsPaused)
                    UpdateGamePlay(gameTime);
            }
            else
                UpdateMenus(gameTime);
        }

        private void UpdateGamePlay(GameTime gameTime)
        {
            _userInterface.Update(gameTime, _gameScreen.TeamHealths);
            _gameScreen.Update(gameTime);
            _cameraManager.Update(gameTime);
            StateMachine.Instance.UpdateStateMachine(gameTime);

            if (StateMachine.Instance.GameState == GameStates.Reset)
                ResetGame(gameTime);
        }

        private void UpdateBrightnessSettings()
        {
            _globalBrightness = _menuManager.IsGameSetup ? _pauseMenu.Brightness : _menuManager.Brightness;
            _brightnessManager.SetBrightness(_globalBrightness);
        }

        private void UpdateMenus(GameTime gameTime)
        {
            _menuManager.Update(gameTime);

            // Padlock ensures that game content is only loaded once per game
            if (_menuManager.IsGameSetup)
            {
                if (!_isGameContentLoaded)
                {
                    LoadGameContent();
                    _isGameContentLoaded = true;
                }
                
                // Settings transferred only once from MainMenu to PauseMenu
                _pauseMenu.ApplySettings(_menuManager.MainMenuSettings, gameTime);
            }
        }

        private void ExitGame()
        {
            if (InputManager.WasKeyPressed(Keys.Delete) || StateMachine.Instance.GameState == GameStates.ExitGame)
            {
                Content.Unload();
                Exit();
            }
        }

        private void ResetGame(GameTime gameTime)
        {
            _menuManager.ApplySettings(_pauseMenu.Settings, gameTime);
            _menuManager.ResetGame();
            _isGameContentLoaded = false;
            SoundManager.Instance.PlaySong(0);
            StateMachine.Instance.PlaceWizards();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Colours.BackgroundColour);

            if (_menuManager.IsGameSetup)
                DrawGame();
            else
                DrawMenus();

            DrawOverlay();
            base.Draw(gameTime);
        }

        private void DrawGame()
        {
            _spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, _cameraManager.TransformMatrix);

            _scenery.DrawBackground(_spriteBatch);
            _gameScreen.Draw(_spriteBatch);
            _scenery.DrawForeground(_spriteBatch);

            _spriteBatch.End();

            DrawUILayer();
        }

        private void DrawUILayer()
        {
            _spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, _cameraManager.OriginMatrix);

            StateMachine.Instance.Draw(_spriteBatch);
            _pauseMenu.Draw(_spriteBatch);
            _userInterface.Draw(_spriteBatch);

            _spriteBatch.End();
        }

        private void DrawMenus()
        {
            _spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, _cameraManager.OriginMatrix);
            _menuManager.Draw(_spriteBatch);
            _spriteBatch.End();
        }

        private void DrawOverlay()
        {
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, _cameraManager.OriginMatrix);
            _brightnessManager.Draw(_spriteBatch);
            _spriteBatch.End();
        }
    }
}
