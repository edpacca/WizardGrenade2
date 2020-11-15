using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace WizardGrenade2
{
    public class InterfaceManager
    {
        private const int ZOOM_RATE = 3;
        private const int CAMERA_SPEED = 2;
        private float _mainScaleX;
        private float _mainScaleY;
        private float _scaleFactor = 1;
        private Vector2 _camera = Vector2.Zero;
        private Vector2 _screenBounds;
        private Matrix _scaleMatrix;
        private Matrix _originMatrix;

        public Matrix GetScaleMatrix() => _scaleMatrix;
        public Matrix GetOriginMatrix() => _originMatrix;

        public InterfaceManager()
        {
            _mainScaleX = ScreenSettings.RESOLUTION_WIDTH / ScreenSettings.TARGET_WIDTH;
            _mainScaleY = ScreenSettings.RESOLUTION_HEIGHT / ScreenSettings.TARGET_HEIGHT;
            _screenBounds = new Vector2(ScreenSettings.TARGET_WIDTH, ScreenSettings.TARGET_HEIGHT);
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
            int directionX = cursor.X <= 0 ? -1 : cursor.X >= _screenBounds.X ? 1 : 0;
            int directionY = cursor.Y <= 0 ? -1 : cursor.Y >= _screenBounds.Y ? 1 : 0;

            _camera.X += (Utility.DifferentialGameTimeValue(gameTime, CAMERA_SPEED * 100, directionX));
            _camera.Y += (Utility.DifferentialGameTimeValue(gameTime, CAMERA_SPEED * 100, directionY));

            _scaleMatrix.Translation = new Vector3(-_camera.X, -_camera.Y, 1);
        }
    }
}
