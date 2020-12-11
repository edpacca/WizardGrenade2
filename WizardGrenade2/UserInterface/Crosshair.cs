using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WizardGrenade2
{
    public class Crosshair
    {
        public float CrosshairAngle { get; private set; }

        private Sprite _crosshair;
        private Sprite _chargeBar;
        private Vector2 _crosshairPosition;
        private Vector2 _chargeBarPosition;
        private readonly string _crosshairFileName = @"UserInterface/Crosshair";
        private readonly string _chargebarFileName = @"UserInterface/ChargeBar";
        private const int AIM_SPEED = 2;
        private const int CROSSHAIR_RADIUS = 50;
        private const int START_ANGLE = (int)(120f * Mechanics.PI / 180f);
        private float _rotation = 0f;
        private float _chargePercentage;
        private int _previousDirection = 1;
        private int _currentDirection = 1;

        public void LoadContent(ContentManager contentManager)
        {
            _crosshair = new Sprite(contentManager, _crosshairFileName);
            _chargeBar = new Sprite(contentManager, _chargebarFileName);
            CrosshairAngle = START_ANGLE;
        }

        public void Update(GameTime gameTime, Vector2 parentPosition, int directionCoefficient, float currentCharge, float maxCharge)
        {
            UpdateCrosshair(gameTime, parentPosition, directionCoefficient);
            UpdateChargeBar(parentPosition, currentCharge, maxCharge);
            _rotation = CrosshairAngle * -1;
        }

        private void UpdateCrosshair(GameTime gameTime, Vector2 parentPosition, int directionCoefficient)
        {
            _previousDirection = _currentDirection;
            _currentDirection = directionCoefficient;

            if (_currentDirection != _previousDirection)
                CrosshairAngle = Utility.FlipAngle(CrosshairAngle);

            RestrictAngle(directionCoefficient);
            
            if (InputManager.IsKeyDown(Keys.Up))
                CrosshairAngle += Utility.DifferentialGameTimeValue(gameTime, AIM_SPEED, directionCoefficient);
            else if (InputManager.IsKeyDown(Keys.Down))
                CrosshairAngle -= Utility.DifferentialGameTimeValue(gameTime, AIM_SPEED, directionCoefficient);

            _crosshairPosition = CalculatePosition(parentPosition, CROSSHAIR_RADIUS);
        }

        private void UpdateChargeBar(Vector2 parentPosition, float currentCharge, float maxCharge)
        {
            _chargePercentage = currentCharge / maxCharge;
            _chargeBarPosition = CalculatePosition(parentPosition, _chargePercentage * CROSSHAIR_RADIUS);
        }

        private Vector2 CalculatePosition(Vector2 parentPosition, float radius)
        {
            Vector2 newPosition = new Vector2();
            newPosition.X = parentPosition.X + ((float)Math.Sin(CrosshairAngle) * radius);
            newPosition.Y = parentPosition.Y + ((float)Math.Cos(CrosshairAngle) * radius);
            return newPosition;
        }

        private void RestrictAngle(int directionCoeff)
        {
            if (directionCoeff == 1)
                CrosshairAngle = CrosshairAngle > Mechanics.PI ? Mechanics.PI : 
                    CrosshairAngle < 0 ? 0 :
                    CrosshairAngle;

            if (directionCoeff == -1)
                CrosshairAngle = CrosshairAngle < Mechanics.PI ? Mechanics.PI :
                    CrosshairAngle > Mechanics.TAO ? Mechanics.TAO :
                    CrosshairAngle;
        }

        public void Draw(SpriteBatch spriteBatch, bool isCharging)
        {
            _crosshair.DrawSprite(spriteBatch, _crosshairPosition, _rotation);
            if (isCharging)
                _chargeBar.DrawSprite(spriteBatch, _chargeBarPosition, _rotation);
        }
    }
}
