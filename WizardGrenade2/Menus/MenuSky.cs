using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace WizardGrenade2
{
    public class MenuSky
    {
        private Sprite _sky;
        private Sprite _moon;

        private Vector2 _skyPosition;
        private Vector2 _moonPosition;
        private Vector2 _skyVector;
        private Vector2 _skyMinPosition;
        private Vector2 _moonOrbitCentre;

        private const float MOON_ORBIT_RADIUS = ScreenSettings.TARGET_WIDTH - 30;
        private const float MOON_SPEED = 0.1f;
        private const float SKY_SPEED = 10f;

        private float _moonRotation = 0f;
        private bool _isMovingLeft = true;
        private int _direction = -1;

        public void LoadContent(ContentManager contentManager)
        {
            _sky = new Sprite(contentManager, @"Background/MenuSky");
            _moon = new Sprite(contentManager, @"Background/Moon");
            _moon.SpriteScale = 0.3f;
            _moon.SpriteColour = Colours.MoonGrey;
            _sky.SpriteColour = Colours.LighterGrey;
            SetLayout();
        }

        private void SetLayout()
        {
            _skyPosition = Vector2.Zero;
            _moonPosition = Vector2.Zero;
            _skyVector = Mechanics.NormaliseVector(new Vector2(ScreenSettings.RESOLUTION_WIDTH, ScreenSettings.RESOLUTION_HEIGHT));
            _skyMinPosition = new Vector2(-710, 415);
            _moonOrbitCentre = new Vector2(ScreenSettings.TARGET_WIDTH, ScreenSettings.TARGET_HEIGHT + 500);
        }

        public void Update(GameTime gameTime)
        {
            CheckDirection();
             _skyPosition += _direction * _skyVector * (float)gameTime.ElapsedGameTime.TotalSeconds * SKY_SPEED;

            MoonOrbit(gameTime);
            _moonRotation -= (float)gameTime.ElapsedGameTime.TotalSeconds / 3f;
        }

        private void CheckDirection()
        {
            if (_skyPosition.X <= _skyMinPosition.X && _isMovingLeft)
            {
                _isMovingLeft = false;
                _direction = 1;
            }

            if (_skyPosition.X >= -5 && !_isMovingLeft)
            {
                _isMovingLeft = true;
                _direction = -1;
            }
        }

        private void MoonOrbit(GameTime gameTime)
        {
            _moonPosition.X = _moonOrbitCentre.X + ((float)Math.Sin(-gameTime.TotalGameTime.TotalSeconds * MOON_SPEED) * MOON_ORBIT_RADIUS);
            _moonPosition.Y = _moonOrbitCentre.Y + ((float)Math.Cos(-gameTime.TotalGameTime.TotalSeconds * MOON_SPEED) * MOON_ORBIT_RADIUS);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _sky.DrawSprite(spriteBatch, _skyPosition);
            _moon.DrawSprite(spriteBatch, _moonPosition, _moonRotation);
        }
    }
}
