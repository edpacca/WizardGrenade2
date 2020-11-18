using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace WizardGrenade2
{
    class OptionArrows : Sprite
    {
        private readonly string _fileName = "Marker";
        private const float OSCILLATION_RATE = 2.5f;
        private const float OSCILLATION_AMPLITUDE = 0.2f;
        private Vector2 _positionL = Vector2.Zero;
        private Vector2 _positionR = Vector2.Zero;

        public void LoadContent(ContentManager contentManager)
        {
            LoadContent(contentManager, _fileName);
            SpriteScale = 1.5f;
        }

        public void SetPositions(Vector2 position, float distance)
        {
            _positionL.Y = position.Y;
            _positionR.Y = position.Y;
            _positionL.X = position.X;
            _positionR.X = position.X + distance;
        }

        public void Update(GameTime gameTime)
        {
            _positionL.X += ((float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * OSCILLATION_RATE)) * OSCILLATION_AMPLITUDE;
            _positionR.X -= ((float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * OSCILLATION_RATE)) * OSCILLATION_AMPLITUDE;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawSprite(spriteBatch, _positionR, 3 * Mechanics.PI / 2);
            DrawSprite(spriteBatch, _positionL, Mechanics.PI / 2);
        }
    }
}
