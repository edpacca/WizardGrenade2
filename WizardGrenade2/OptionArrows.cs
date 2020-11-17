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
        private const float OSCILLATION_AMPLITUDE = 0.5f;
        private Vector2 _positionL = Vector2.Zero;
        private Vector2 _positionR = Vector2.Zero;

        public void LoadContent(ContentManager contentManager)
        {
            LoadContent(contentManager, _fileName);
            SpriteScale = 1.5f;
        }

        public void SetPositions(Vector2 centre, float distance)
        {
            _positionL.Y = centre.Y;
            _positionR.Y = centre.Y;
            _positionL.X = centre.X - (distance / 2);
            _positionR.X = centre.X + (distance / 2);
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
