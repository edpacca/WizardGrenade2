using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WizardGrenade2
{
    class UserInterface
    {
        private Sprite _cursor;
        private Timer _timer;

        public void LoadContent(ContentManager contentManager)
        {
            _cursor = new Sprite(contentManager, "Cursor");
            _timer = new Timer(60f);
            _timer.LoadContent(contentManager);
        }

        public void Update(GameTime gameTime)
        {
            _timer.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _cursor.DrawSprite(spriteBatch, InputManager.CursorPosition(), 0f);
            _timer.Draw(spriteBatch);
        }
    }
}
