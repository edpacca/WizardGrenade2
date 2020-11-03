using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardGrenade2
{
    public class InterfaceManager
    {
        private Matrix _scaleMatrix;
        private Matrix _originMatrix;

        private float _mainScaleX;
        private float _mainScaleY;
        private float _scaleFactor = 1;
        private const int ZOOM_RATE = 2;

        public float cross = 0;

        private Vector2 _camera = Vector2.Zero;
        private Vector2 _screenBounds;
        private const int CAMERA_SPEED = 1;

        public InterfaceManager(float backBufferWidth, float backBufferHeight, float targetWidth, float targetHeight)
        {
            _mainScaleX = backBufferWidth / targetWidth;
            _mainScaleY = backBufferHeight / targetHeight;
            _screenBounds = new Vector2(targetWidth, targetHeight);
            _originMatrix = Matrix.CreateScale(new Vector3(_mainScaleX, _mainScaleY, 1));
            _scaleMatrix = Matrix.CreateScale(new Vector3(_mainScaleX, _mainScaleY, 1));
        }

        public void Update(GameTime gameTime)
        {
            Zoom();
            CameraPosition(gameTime);
        }

        private void Zoom()
        {
            if (InputManager.HasScrollWheelMoved())
                _scaleFactor = 1 + (InputManager.GetScrollWheelValue() * ZOOM_RATE / 10000);

            _scaleMatrix.M11 = _scaleFactor * _mainScaleX;
            _scaleMatrix.M22 = _scaleFactor * _mainScaleY;
        }

        private void CameraPosition(GameTime gameTime)
        {
            MouseState cursor = Mouse.GetState();
            int directionX = 0;
            int directionY = 0;

            // Make better
            if (cursor.X <= 0)
                directionX = -1;
            else if (cursor.X >= _screenBounds.X)
                directionX = 1;
            else
                directionX = 0;

            if (cursor.Y <= 0)
                directionY = -1;
            else if (cursor.Y >= _screenBounds.Y)
                directionY = 1;
            else
                directionY = 0;

            _camera.X += (Utility.DifferentialGameTimeValue(gameTime, CAMERA_SPEED * 100, directionX));
            _camera.Y += (Utility.DifferentialGameTimeValue(gameTime, CAMERA_SPEED * 100, directionY));

            _scaleMatrix.Translation = new Vector3(-_camera.X, -_camera.Y, 1);
        }

        //private float CursorPosition()
        //{
        //    Vector2 cursor = InputManager.CursorPosition();
        //    Vector2 cursorToOrigin = Mechanics.VectorBetweenPoints(Vector2.Zero, cursor);
        //    Vector2 cursorToScreenBounds = Mechanics.VectorBetweenPoints(_screenBounds, cursor);
        //    return Mechanics.CrossProduct(cursorToOrigin, cursorToScreenBounds);
        //}

        public Matrix GetScaleMatrix()
        {
            return _scaleMatrix;
        }

        public Matrix GetOriginMatrix()
        {
            return _originMatrix;
        }

    }
}
