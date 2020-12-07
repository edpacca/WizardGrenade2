using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace WizardGrenade2
{
    class OptionArrows : Sprite
    {
        private readonly string _fileName = @"UserInterface/Marker";
        private const float OSCILLATION_RATE = 2.5f;
        private const float OSCILLATION_AMPLITUDE = 0.2f;
        private Vector2 _positionL = Vector2.Zero;
        private Vector2 _positionR = Vector2.Zero;
        bool _doubleArrow;

        public OptionArrows(bool doubleArrow)
        {
            _doubleArrow = doubleArrow;
        }

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

        public void SetPosition(Vector2 position)
        {
            _positionL = position;
        }

        public void Update(GameTime gameTime)
        {
            if (_doubleArrow)
                Update2(gameTime);
            else
                Update1(gameTime);
        }

        public void Update2(GameTime gameTime)
        {
            _positionL.X += ((float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * OSCILLATION_RATE)) * OSCILLATION_AMPLITUDE;
            _positionR.X -= ((float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * OSCILLATION_RATE)) * OSCILLATION_AMPLITUDE;
        }

        public void Update1(GameTime gameTime)
        {
            _positionL.X += ((float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * OSCILLATION_RATE)) * OSCILLATION_AMPLITUDE;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_doubleArrow)
                Draw2(spriteBatch);
            else
                Draw1(spriteBatch);
        }

        public void Draw2(SpriteBatch spriteBatch)
        {
            DrawSprite(spriteBatch, _positionR, 3 * Mechanics.PI / 2);
            DrawSprite(spriteBatch, _positionL, Mechanics.PI / 2);
        }

        public void Draw1(SpriteBatch spriteBatch)
        {
            DrawSprite(spriteBatch, _positionL, 3 * Mechanics.PI / 2);
        }
    }
}
