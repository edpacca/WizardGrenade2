using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace WizardGrenade2
{
    public class Marker : Sprite
    {
        private Vector2 _position;
        private Vector2 _markerOffset;
        private readonly string _fileName = @"UserInterface/Marker";
        private const int OSCILLATION_RATE = 2;
        private const int OSCILLATION_AMPLITUDE = 3;

        public void LoadContent(ContentManager contentManager)
        {
            LoadContent(contentManager, _fileName);
            _markerOffset = new Vector2(SpriteRectangle.Width / 2, 36);
        }

        public void Update(GameTime gameTime, Vector2 activePlayerPosition)
        {
            _position = activePlayerPosition - _markerOffset;
            _position.Y += ((float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * OSCILLATION_RATE)) * OSCILLATION_AMPLITUDE;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawSprite(spriteBatch, _position);
        }
    }
}
