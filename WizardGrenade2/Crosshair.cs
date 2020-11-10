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
        public float CrosshairAngle { get; private set; }

        private Vector2 _position;
        private int _previousDirection = 1;
        private int _currentDirection = 1;

        public void LoadContent(ContentManager contentManager)
        {
            LoadContent(contentManager, _fileName);
            CrosshairAngle = START_ANGLE * RAD;
        }

        public void UpdateCrosshair(GameTime gameTime, Vector2 parentPosition, int direciontCoefficient)
        {
            _previousDirection = _currentDirection;
            _currentDirection = direciontCoefficient;

            if (_currentDirection != _previousDirection)
                CrosshairAngle = MathsExt.FlipAngle(CrosshairAngle);

            RestrictAngle(direciontCoefficient);
            
            if (InputManager.IsKeyDown(Keys.Up))
                CrosshairAngle += Utility.DifferentialGameTimeValue(gameTime, AIM_SPEED, direciontCoefficient);
            else if (InputManager.IsKeyDown(Keys.Down))
                CrosshairAngle -= Utility.DifferentialGameTimeValue(gameTime, AIM_SPEED, direciontCoefficient);

            UpdateCrosshairPosition(parentPosition);
        }

        private void UpdateCrosshairPosition(Vector2 parentPosition)
        {
            _position.X = parentPosition.X + ((float)Math.Sin(CrosshairAngle) * CROSSHAIR_RADIUS);
            _position.Y = parentPosition.Y + ((float)Math.Cos(CrosshairAngle) * CROSSHAIR_RADIUS);
        }

        private void RestrictAngle(int directionCoeff)
        {
            if (directionCoeff == 1)
            {
                if (CrosshairAngle > Mechanics.PI)
                    CrosshairAngle = Mechanics.PI;
                if (CrosshairAngle < 0)
                    CrosshairAngle = 0;
            }

            if (directionCoeff == -1)
            {
                if (CrosshairAngle < Mechanics.PI)
                    CrosshairAngle = Mechanics.PI;
                if (CrosshairAngle > Mechanics.TAO)
                    CrosshairAngle = Mechanics.TAO;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawSprite(spriteBatch, _position - GetSpriteOrigin());
        }
    }
}
