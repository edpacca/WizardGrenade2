using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WizardGrenade2
{
    class Crosshair : Sprite
    {
        private readonly string _fileName = "Crosshair";
        private const int AIM_SPEED = 2;
        private const int CROSSHAIR_RADIUS = 50;
        private const int START_ANGLE = 120;
        private const float RAD = (float)Math.PI / 180;
        private float _crosshairAngle = START_ANGLE * RAD;
        private Vector2 _position;
        private int _previousDirection = 1;
        private int _currentDirection = 1;

        public void LoadContent(ContentManager contentManager)
        {
            LoadContent(contentManager, _fileName);
        }

        public void UpdateCrosshair(GameTime gameTime, Vector2 parentPosition, int direciontCoefficient)
        {
            _previousDirection = _currentDirection;
            _currentDirection = direciontCoefficient;

            if (_currentDirection != _previousDirection)
                _crosshairAngle = MathsExt.FlipAngle(_crosshairAngle);

            RestrictAngle(direciontCoefficient);
            
            if (InputManager.IsKeyDown(Keys.Up))
                _crosshairAngle += Utility.DifferentialGameTimeValue(gameTime, AIM_SPEED, direciontCoefficient);
            else if (InputManager.IsKeyDown(Keys.Down))
                _crosshairAngle -= Utility.DifferentialGameTimeValue(gameTime, AIM_SPEED, direciontCoefficient);

            UpdateCrosshairPosition(parentPosition);
        }

        private void UpdateCrosshairPosition(Vector2 parentPosition)
        {
            _position.X = parentPosition.X + ((float)Math.Sin(_crosshairAngle) * CROSSHAIR_RADIUS);
            _position.Y = parentPosition.Y + ((float)Math.Cos(_crosshairAngle) * CROSSHAIR_RADIUS);
        }

        private void RestrictAngle(int directionCoeff)
        {
            if (directionCoeff == 1)
            {
                if (_crosshairAngle > Mechanics.PI)
                    _crosshairAngle = Mechanics.PI;
                if (_crosshairAngle < 0)
                    _crosshairAngle = 0;
            }

            if (directionCoeff == -1)
            {
                if (_crosshairAngle < Mechanics.PI)
                    _crosshairAngle = Mechanics.PI;
                if (_crosshairAngle > Mechanics.TAO)
                    _crosshairAngle = Mechanics.TAO;
            }
        }

        public float GetAimAngle()
        {
            return _crosshairAngle;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawSprite(spriteBatch, _position - GetSpriteOrigin());
        }
    }
}
