using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace WizardGrenade2
{
    public class Sprite
    {
        public Texture2D SpriteTexture { get; private set; }
        public Rectangle SpriteRectangle { get => GetSpriteRectangle(); }
        public Vector2 Origin { get => GetSpriteOrigin(); }
        public Color SpriteColour { get; set; }
        public SpriteEffects SpriteVisualEffect { get; set; }
        public float SpriteScale { get; set; }

        private Animator _animator;
        private Rectangle _spriteRectangle;
        private int _frameSize;
        private bool _verticalAnimation;

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

        public Sprite(ContentManager contentManager, string fileName, int framesH, int framesV)
            : this()
        {
            LoadContent(contentManager, fileName, framesH, framesV);
        }

        // LoadContent for single sprite
        protected internal void LoadContent(ContentManager contentManager, string fileName)
        {
            SpriteTexture = contentManager.Load<Texture2D>(fileName);
            _spriteRectangle = new Rectangle(0, 0, SpriteTexture.Width, SpriteTexture.Height);
        }

        // LoadContent from animation spriteSheet
        protected internal void LoadContent(ContentManager contentManager, string fileName, int framesH, int framesV)
        {
            SpriteTexture = contentManager.Load<Texture2D>(fileName);
            _spriteRectangle = new Rectangle(0, 0, SpriteTexture.Width / framesH, SpriteTexture.Height / framesV);

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

        private Rectangle GetSpriteRectangle()
        {
            return new Rectangle(_spriteRectangle.X, _spriteRectangle.Y, (int)(_spriteRectangle.Width * SpriteScale), (int)(_spriteRectangle.Height * SpriteScale));
        }

        private Vector2 GetSpriteOrigin()
        {
            return new Vector2((float)_spriteRectangle.Width / 2, (float)_spriteRectangle.Height / 2) * SpriteScale;
        }

        private Vector2 CalcRotationOffset(float rotation)
        {
            float xOffset = -_spriteRectangle.Width  / 2 * (float)Math.Cos(rotation) + (_spriteRectangle.Height  / 2 * (float)Math.Sin(rotation));
            float yOffset = -_spriteRectangle.Height  / 2 * (float)Math.Cos(rotation) - (_spriteRectangle.Width  / 2 * (float)Math.Sin(rotation));

            return new Vector2(xOffset, yOffset) * SpriteScale;
        }

        public void DrawSprite(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(SpriteTexture, position, _spriteRectangle, SpriteColour, 0f, Vector2.Zero, SpriteScale, SpriteVisualEffect, 0f);
        }

        public void DrawSprite(SpriteBatch spriteBatch, Vector2 position, float rotation)
        {
            Vector2 rotationOffset = CalcRotationOffset(rotation);
            spriteBatch.Draw(SpriteTexture, position + rotationOffset, _spriteRectangle, SpriteColour, rotation, Vector2.Zero, SpriteScale, SpriteVisualEffect, 0f);
        }

        // For drawing at scale without changing base scaling
        public void DrawSpriteAtScale(SpriteBatch spriteBatch, Vector2 position, float scale)
        {
            spriteBatch.Draw(SpriteTexture, position - (Origin * scale), _spriteRectangle, SpriteColour, 0f, Vector2.Zero, scale, SpriteVisualEffect, 0f);
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

        public void MaskSpriteRectangleX(float maskPercentage) => _spriteRectangle.X = (int)(SpriteTexture.Width * maskPercentage);
        public void MaskSpriteRectangleY(float maskPercentage) => _spriteRectangle.Y = (int)(SpriteTexture.Height * maskPercentage);
        public void MaskSpriteRectangleWidth(float maskPercentage) => _spriteRectangle.Width = (int)(SpriteTexture.Width * maskPercentage);
        public void MaskSpriteRectangleHeight(float maskPercentage) => _spriteRectangle.Height = (int)(SpriteTexture.Height * maskPercentage);

        public void MaskSpriteFromBottom(float maskPercentage)
        {
            _spriteRectangle.Height = SpriteTexture.Height - (int)(SpriteTexture.Height * maskPercentage);
            _spriteRectangle.Y = (int)(SpriteTexture.Height * maskPercentage);
        }

        public void MaskSpriteFromTop(float maskPercentage)
        {
            _spriteRectangle.Y = (int)(SpriteTexture.Height * maskPercentage);
            _spriteRectangle.Height = (int)(SpriteTexture.Height * (1 - maskPercentage));
        }
    }
}
