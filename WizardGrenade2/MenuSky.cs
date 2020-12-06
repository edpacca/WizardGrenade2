using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace WizardGrenade2
{
    public class MenuSky
    {
        private Sprite _sky;
        private Vector2 _skyPosition = Vector2.Zero;
        private Vector2 _vector = Mechanics.NormaliseVector(new Vector2(ScreenSettings.RESOLUTION_WIDTH, ScreenSettings.RESOLUTION_HEIGHT));
        private Vector2 _moonOrbitCentre = new Vector2(ScreenSettings.TARGET_WIDTH, ScreenSettings.TARGET_HEIGHT + 500);
        private float MOON_ORBIT_RADIUS = ScreenSettings.TARGET_WIDTH - 30;
        private Vector2 _moonPosition = Vector2.Zero;
        private const float MOON_SPEED = 0.1f;
        private float _moonRotation = 0f;
        private Vector2 _minPosition = new Vector2(-710, 415);
        private bool _negativeDirection = true;
        private int _direction = -1;
        private Sprite _moon;

        public void LoadContent(ContentManager contentManager)
        {
            _sky = new Sprite(contentManager, "MenuSky");
            _sky.SpriteColour = Colours.LighterGrey;
            _moon = new Sprite(contentManager, "Moon");
            _moon.SpriteScale = 0.3f;
            _moon.SpriteColour = new Color(50, 50, 50);
        }

        public void Update(GameTime gameTime)
        {
            CheckDirection();
             _skyPosition += _direction * _vector * (float)gameTime.ElapsedGameTime.TotalSeconds * 10;
            MoonOrbit(gameTime);
            _moonRotation -= (float)gameTime.ElapsedGameTime.TotalSeconds / 3f;
        }

        private void CheckDirection()
        {
            if (_skyPosition.X <= _minPosition.X && _negativeDirection)
            {
                _negativeDirection = false;
                _direction = 1;
            }

            if (_skyPosition.X >= -5 && !_negativeDirection)
            {
                _negativeDirection = true;
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
