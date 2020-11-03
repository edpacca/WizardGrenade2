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

        public void LoadContent(ContentManager contentManager)
        {
            LoadContent(contentManager, _fileName);
        }

        public void UpdateCrosshair(GameTime gameTime, Vector2 parentPosition)
        {
            if (InputManager.IsKeyDown(Keys.Up))
                _crosshairAngle += Utility.DifferentialGameTimeValue(gameTime, AIM_SPEED, 1);
            else if (InputManager.IsKeyDown(Keys.Down))
                _crosshairAngle -= Utility.DifferentialGameTimeValue(gameTime, AIM_SPEED, 1);

            UpdateCrosshairPosition(parentPosition);
        }

        private void UpdateCrosshairPosition(Vector2 parentPosition)
        {
            _position.X = parentPosition.X + ((float)Math.Sin(_crosshairAngle) * CROSSHAIR_RADIUS);
            _position.Y = parentPosition.Y + ((float)Math.Cos(_crosshairAngle) * CROSSHAIR_RADIUS);
        }

        public float GetAimAngle()
        {
            return _crosshairAngle;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawSprite(spriteBatch, _position, 0f);
        }
    }
}
