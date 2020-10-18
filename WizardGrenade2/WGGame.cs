using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WizardGrenade2
{
    public class WGGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Matrix _scale;
        private float _mainScaleX;
        private float _mainScaleY;

        private const float TARGET_SCREEN_WIDTH = 1200;
        private const float TARGET_SCREEN_HEIGHT = TARGET_SCREEN_WIDTH * 0.5625f;
        private const int SCREEN_RESOLUTION_WIDTH = 1920;
        private const int SCREEN_RESOLUTION_HEIGHT = 1080;

        private Wizard _wizard;

        public WGGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _graphics.PreferredBackBufferWidth = SCREEN_RESOLUTION_WIDTH;
            _graphics.PreferredBackBufferHeight = SCREEN_RESOLUTION_HEIGHT;
            Window.AllowUserResizing = true;
        }

        protected override void Initialize()
        {
            _mainScaleX = _graphics.PreferredBackBufferWidth / TARGET_SCREEN_WIDTH;
            _mainScaleY = _graphics.PreferredBackBufferHeight / TARGET_SCREEN_HEIGHT;
            _scale = Matrix.CreateScale(new Vector3(_mainScaleX, _mainScaleY, 1));

            _wizard = new Wizard(0, new Vector2(400, 100));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _wizard.LoadContent(Content);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            InputManager.Update();

            if (InputManager.IsKeyDown(Keys.Escape))
                Exit();
            _wizard.Update(gameTime);

            base.Update(gameTime);
        }

        public static int GetScreenWidth() => (int)TARGET_SCREEN_WIDTH;
        public static int GetScreenHeight() => (int)TARGET_SCREEN_HEIGHT;

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, _scale);
            _wizard.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
