using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WizardGrenade2
{
    public class WGGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GameScreen _gameScreen = new GameScreen();
        private InterfaceManager _interfaceManager;
        private UserInterface _userInterface;

        private const float TARGET_SCREEN_WIDTH = 1200;
        private const float TARGET_SCREEN_HEIGHT = TARGET_SCREEN_WIDTH * 0.5625f;
        private const int SCREEN_RESOLUTION_WIDTH = 1920;
        private const int SCREEN_RESOLUTION_HEIGHT = 1080;

        public static float GetScreenWidth => TARGET_SCREEN_WIDTH;
        public static float GetScreenHeight => TARGET_SCREEN_HEIGHT;
        public Vector2 GetScreenCentre => new Vector2(TARGET_SCREEN_WIDTH / 2, TARGET_SCREEN_HEIGHT / 2);

        Color backgroundColour = new Color(40, 40, 45);

        public WGGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _graphics.PreferredBackBufferWidth = SCREEN_RESOLUTION_WIDTH;
            _graphics.PreferredBackBufferHeight = SCREEN_RESOLUTION_HEIGHT;

            _interfaceManager = new InterfaceManager
                (SCREEN_RESOLUTION_WIDTH, SCREEN_RESOLUTION_HEIGHT, 
                TARGET_SCREEN_WIDTH, TARGET_SCREEN_HEIGHT);

            Window.AllowUserResizing = true;
        }

        protected override void Initialize()
        {
            _gameScreen.Initialise();
            _userInterface = new UserInterface(_gameScreen.GetGameOptions());
            Mouse.SetPosition((int)TARGET_SCREEN_WIDTH / 2, (int)TARGET_SCREEN_HEIGHT / 2);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _gameScreen.LoadContent(Content);
            _userInterface.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (InputManager.IsKeyDown(Keys.Escape))
                Exit();

            InputManager.Update();
            _interfaceManager.Update(gameTime);
            _userInterface.Update(gameTime, _gameScreen.GetTeamHealths());
            _gameScreen.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(backgroundColour);

            _spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null,  _interfaceManager.GetScaleMatrix());
            _gameScreen.Draw(_spriteBatch);
            _spriteBatch.End();

            _spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, _interfaceManager.GetOriginMatrix());
            _userInterface.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
