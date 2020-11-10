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
            _gameScreen.Initialise();
            _userInterface = new UserInterface(_gameScreen.GameOptions, _gameScreen.TeamNames);
            Mouse.SetPosition(ScreenSettings.CentreScreenWidth, ScreenSettings.CentreScreenHeight);
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
            _userInterface.Update(gameTime, _gameScreen.TeamHealths);
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
