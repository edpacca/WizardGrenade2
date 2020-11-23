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
    class Scroll
    {
        private Sprite _scrollTop;
        private Sprite _scrollBottom;
        private Vector2 _position;
        private Vector2 _topPosition;
        private Vector2 _bottomPosition;
        private float _yOffset;
        private float _weeOffset = 10f;
        private float _percentage;
        private const float ANIMATION_SPEED = 800;

        public Scroll(Vector2 position)
        {
            _position = position;
        }

        public void LoadContent(ContentManager contentManager)
        {
            _scrollTop = new Sprite(contentManager, "Scroll0");
            _scrollBottom = new Sprite(contentManager, "Scroll1");

            _topPosition = _position - _scrollTop.GetSpriteOrigin();
            _bottomPosition = _topPosition;
            _yOffset = _scrollBottom.GetSpriteRectangle().Height;
            ResetPauseMenu();
        }

        public void ResetPauseMenu()
        {
            _bottomPosition.Y = _position.Y - _yOffset;
            _scrollTop.MaskSpriteRectangleHeight(_topPosition.Y - _bottomPosition.Y / _topPosition.Y);
        }

        public void Update(GameTime gameTime)
        {
            if (_bottomPosition.Y < _topPosition.Y - _weeOffset)
            {
                _bottomPosition.Y += (float)gameTime.ElapsedGameTime.TotalSeconds * ANIMATION_SPEED;
                _percentage = 1 - ((_topPosition.Y - _bottomPosition.Y + 80) / _yOffset);
                _scrollTop.MaskSpriteRectangleHeight(_percentage);
            }
            else
                _scrollTop.MaskSpriteRectangleHeight(1);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _scrollTop.DrawSprite(spriteBatch, _topPosition);
            _scrollBottom.DrawSprite(spriteBatch, _bottomPosition);
        }
    }
}
