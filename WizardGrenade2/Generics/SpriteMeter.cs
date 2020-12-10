using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WizardGrenade2
{
    public class SpriteMeter
    {
        public Sprite Sprite { get; set; }
        public float Interval { get; set; } = 10f;

        public SpriteMeter(ContentManager contentManager, string fileName)
        {
            Sprite = new Sprite(contentManager, fileName);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, int value)
        {
            for (int i = 0; i < value; i++)
                Sprite.DrawSprite(spriteBatch, new Vector2(position.X + (i * Interval), position.Y));
        }
    }
}
