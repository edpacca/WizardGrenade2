using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardGrenade2
{
    class Marker : Sprite
    {
        private readonly string _fileName = "Marker";
        private Vector2 _position = Vector2.Zero;
        private Vector2 _markerOffset = new Vector2(0, 36);
        private const int OSCILLATION_RATE = 2;
        private const int OSCILLATION_AMPLITUDE = 3;

        public void LoadContent(ContentManager contentManager)
        {
            LoadContent(contentManager, _fileName);
            _markerOffset.X = GetSpriteRectangle().Width / 2;
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
