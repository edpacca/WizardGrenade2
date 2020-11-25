using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace WizardGrenade2
{
    public class Sprite
    {
        private Texture2D _spriteTexture;
        private Rectangle _spriteRectangle;
        private Animator _animator;
        private float _layerDepth = 0f;
        private int _frameSize;
        private bool _verticalAnimation;

        public Color SpriteColour { get; set; }
        public SpriteEffects SpriteVisualEffect { get; set; }
        public float SpriteScale { get; set; }
 
        public Texture2D GetSpriteTexture() => _spriteTexture;
        public Rectangle GetSpriteRectangle() => _spriteRectangle;

        // Constructors
        public Sprite()
        {
            SpriteColour = Color.White;
            SpriteVisualEffect = SpriteEffects.None;
            SpriteScale = 1f;
        }

        public Sprite(ContentManager contentManager, string fileName)
            : this()
        {
            LoadContent(contentManager, fileName);
        }

        // LoadContent for single sprite
        protected internal void LoadContent(ContentManager contentManager, string fileName)
        {
            _spriteTexture = contentManager.Load<Texture2D>(fileName);
            _spriteRectangle = new Rectangle(0, 0, _spriteTexture.Width, _spriteTexture.Height);
        }

        // LoadContent from animation spriteSheet
        protected internal void LoadContent(ContentManager contentManager, string fileName, int framesH, int framesV)
        {
            _spriteTexture = contentManager.Load<Texture2D>(fileName);
            _spriteRectangle = new Rectangle(0, 0, _spriteTexture.Width / framesH, _spriteTexture.Height / framesV);

            if (framesV > framesH)
            {
                _frameSize = _spriteRectangle.Height;
                _verticalAnimation = true;
            }
            else
                _frameSize = _spriteRectangle.Width;
        }

        public void LoadAnimationContent(Dictionary<string, int[]> animationStates)
        {
            _animator = new Animator(animationStates, _frameSize);
        }

        public Vector2 GetSpriteOrigin()
        {
            return new Vector2((float)_spriteRectangle.Width / 2, (float)_spriteRectangle.Height / 2) * SpriteScale;
        }

        private Vector2 CalcRotationOffset(float rotation)
        {
            float xOffset = -_spriteRectangle.Width  / 2 * (float)Math.Cos(rotation) + (_spriteRectangle.Height  / 2 * (float)Math.Sin(rotation)) * SpriteScale;
            float yOffset = -_spriteRectangle.Height  / 2 * (float)Math.Cos(rotation) - (_spriteRectangle.Width  / 2 * (float)Math.Sin(rotation)) * SpriteScale;

            return new Vector2(xOffset, yOffset);
        }

        public void DrawSprite(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(_spriteTexture, position, _spriteRectangle, SpriteColour, 0f, Vector2.Zero, SpriteScale, SpriteVisualEffect, _layerDepth);
        }

        public void DrawSprite(SpriteBatch spriteBatch, Vector2 position, float rotation)
        {
            Vector2 rotationOffset = CalcRotationOffset(rotation);
            spriteBatch.Draw(_spriteTexture, position + rotationOffset, _spriteRectangle, SpriteColour, rotation, Vector2.Zero, SpriteScale, SpriteVisualEffect, _layerDepth);
        }

        public void DrawSpriteAtScale(SpriteBatch spriteBatch, Vector2 position, float scale)
        {
            spriteBatch.Draw(_spriteTexture, position - (GetSpriteOrigin() * scale), _spriteRectangle, SpriteColour, 0f, Vector2.Zero, scale, SpriteVisualEffect, _layerDepth);
        }

        public void UpdateAnimationSequence(string state, float targetFrameRate, GameTime gameTime)
        {
            int frame = _animator.GetCurrentFrame(state, targetFrameRate, gameTime);

            if (_verticalAnimation)
                _spriteRectangle.Y = frame * _frameSize;
            else
                _spriteRectangle.X = frame * _frameSize;
        }

        public void UpdateAnimationFrame(string state, int frame)
        {
            int frameNumber = _animator.GetSingleFrame(state, frame);

            if (_verticalAnimation)
                _spriteRectangle.Y = frameNumber * _frameSize;
            else
                _spriteRectangle.X = frameNumber * _frameSize;
        }

        public void MaskSpriteRectangleX(float maskPercentage)
        {
            float percentage = maskPercentage <= 1 && maskPercentage >= 0 ? maskPercentage : 1;
            _spriteRectangle.X = (int)(_spriteTexture.Width * percentage);
        }

        public void MaskSpriteRectangleY(float maskPercentage)
        {
            float percentage = maskPercentage <= 1 && maskPercentage >= 0 ? maskPercentage : 1;
            _spriteRectangle.Y = (int)(_spriteTexture.Height * percentage);
        }

        public void MaskSpriteRectangleWidth(float maskPercentage)
        {
            float percentage = maskPercentage <= 1 && maskPercentage >= 0 ? maskPercentage : 1;
            _spriteRectangle.Width = (int)(_spriteTexture.Width * percentage);
        }

        public void MaskSpriteRectangleHeight(float maskPercentage)
        {
            float percentage = maskPercentage <= 1 && maskPercentage >= 0 ? maskPercentage : 1;
            _spriteRectangle.Height = (int)(_spriteTexture.Height * percentage);
        }
    }
}
