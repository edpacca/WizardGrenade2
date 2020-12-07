using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WizardGrenade2
{
    public class SpriteMeter
    {
        private Sprite _sprite;
        public float Interval { get; set; } = 10f;

        public SpriteMeter(ContentManager contentManager, string fileName)
        {
            _sprite = new Sprite(contentManager, fileName);
        }

        public void SetSpriteScale(float scale)
        {
            _sprite.SpriteScale = scale;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, int value)
        {
            for (int i = 0; i < value; i++)
                _sprite.DrawSprite(spriteBatch, new Vector2(position.X + (i * Interval), position.Y));
        }
    }
}
