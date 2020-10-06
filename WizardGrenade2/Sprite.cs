using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace WizardGrenade2
{
    class Sprite
    {
        private Texture2D _spriteTexture;
        private Rectangle _spriteRectangle;
        private Color _spriteColour = Color.White;
        private SpriteEffects _spriteEffect = SpriteEffects.None;
        private float _layerDepth = 0f;
        private float _spriteScale = 1f;

        public void LoadContent(ContentManager contentManager, string fileName)
        {
            _spriteTexture = contentManager.Load<Texture2D>(fileName);
            _spriteRectangle = new Rectangle(0, 0, _spriteTexture.Width, _spriteTexture.Height);
        }

        public void LoadContent(ContentManager contentManager, string fileName, int framesH, int framesV)
        {
            _spriteTexture = contentManager.Load<Texture2D>(fileName);
            _spriteRectangle = new Rectangle(0, 0, _spriteTexture.Width / framesH, _spriteTexture.Height / framesV);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_spriteTexture, Vector2.Zero, _spriteRectangle, _spriteColour, 0f, Vector2.Zero, _spriteScale, _spriteEffect, _layerDepth);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, float rotation)
        {
            Vector2 rotationOffset = CalcRotationOffset(rotation);
            spriteBatch.Draw(_spriteTexture, position + rotationOffset, _spriteRectangle, _spriteColour, rotation, Vector2.Zero, _spriteScale, _spriteEffect, _layerDepth);
        }

        // Utility methods
        private Vector2 CalcRotationOffset(float rotation)
        {
            float xOffset = -_spriteTexture.Width / 2 * (float)Math.Cos(rotation) + (_spriteTexture.Height / 2 * (float)Math.Sin(rotation));
            float yOffset = -_spriteTexture.Height / 2 * (float)Math.Cos(rotation) - (_spriteTexture.Width / 2 * (float)Math.Sin(rotation));

            return new Vector2(xOffset, yOffset);
        } 

        public void SetColour(Color colour)
        {
            _spriteColour = colour;
        }

        public void SetSpriteEffect(SpriteEffects spriteEffect)
        {
            _spriteEffect = spriteEffect;
        }
    }
}
