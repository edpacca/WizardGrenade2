﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;

namespace WizardGrenade2
{
    public class CameraManager
    {
        private const int ZOOM_RATE = 5;
        private const int CAMERA_SPEED = 200;
        private const int BORDER_WIDTH = 150;
        private const float DEFAULT_SCALE = 1.1f;
        private const float MAX_SCALE = 1.5f;
        private const float MIN_SCALE = 0.7f;
        private readonly Vector2 CAMERA_MIN_POSITION;
        private readonly Vector2 CAMERA_MAX_POSITION;
        private Timer _cameraMoveTimer;
        private const float _cameraTime = 1.5f;

        private float _originScale;
        private float _scaleFactor = DEFAULT_SCALE;
        private float _previousScrollWheelValue;
        private float _currentScrollWheelValue;

        private Vector2 _cursor;
        private Vector2 _cameraPosition = ScreenSettings.ScreenResolutionCentre;
        private Vector2 _cameraOffset { get => (_cameraPosition / _originScale) - ScreenSettings.ScreenCentre; }

        private Rectangle _screenBounds;
        private Vector2 _newCameraPosition;
        private Vector2 _oldCameraPosition;
        private bool _movingCamera;

        private Camera _camera;
        private Matrix _originMatrix;
        public Matrix TransformMatrix { get => _originMatrix * _camera.Transform; }
        public Matrix OriginMatrix { get => _originMatrix; }

        //private SpriteFont _font;

        public CameraManager()
        {
            _camera = new Camera();
            _cameraMoveTimer = new Timer(_cameraTime);
            _originScale = ScreenSettings.RESOLUTION_WIDTH / ScreenSettings.TARGET_WIDTH;
            _originMatrix = Matrix.CreateScale(new Vector3(_originScale, _originScale, 1));
            _screenBounds = new Rectangle(BORDER_WIDTH, BORDER_WIDTH, (int)ScreenSettings.TARGET_WIDTH - BORDER_WIDTH, (int)ScreenSettings.TARGET_HEIGHT - BORDER_WIDTH);
            CAMERA_MIN_POSITION = new Vector2(ScreenSettings.RESOLUTION_WIDTH * 0.25f, ScreenSettings.RESOLUTION_HEIGHT * 0.25f);
            CAMERA_MAX_POSITION = new Vector2(ScreenSettings.RESOLUTION_WIDTH * 0.75f, ScreenSettings.RESOLUTION_HEIGHT * 0.75f);
        }

        public void Update(GameTime gameTime)
        {
            _cursor = InputManager.CursorPosition();
            
            Zoom(gameTime);
            CameraMouseControl(gameTime);
            MoveToNextPlayer(gameTime);
            SetCamera();

            if (InputManager.WasKeyPressed(Keys.LeftShift))
                ResetView();
        }

        private void SetCamera()
        {
            ApplyCameraLimits();
            _camera.Zoom = _scaleFactor;
            _camera.Position = _cameraPosition;
        }

        private void ResetView()
        {
            _scaleFactor = DEFAULT_SCALE;
            _cameraPosition = ScreenSettings.ScreenResolutionCentre;
        }

        private void MoveCamera(GameTime gameTime, int CameraSpeed, Vector2 vector)
        {
            _cameraPosition += vector * (float)gameTime.ElapsedGameTime.TotalSeconds * CameraSpeed;
            _camera.Position = _cameraPosition;
        }

        private void ApplyCameraLimits()
        {
            _scaleFactor = _scaleFactor > MAX_SCALE ? MAX_SCALE : _scaleFactor < MIN_SCALE ? MIN_SCALE : _scaleFactor;
            _cameraPosition = Vector2.Clamp(_cameraPosition, CAMERA_MIN_POSITION, CAMERA_MAX_POSITION);
        }

        private void Zoom(GameTime gameTime)
        {
            _previousScrollWheelValue = _currentScrollWheelValue;
            _currentScrollWheelValue = InputManager.GetScrollWheelValue();
            
            int direction = _previousScrollWheelValue < _currentScrollWheelValue ? 1 :
                _previousScrollWheelValue > _currentScrollWheelValue ? -1 : 0;

            _scaleFactor += (float)gameTime.ElapsedGameTime.TotalSeconds * direction * ZOOM_RATE;
        }

        private void CameraMouseControl(GameTime gameTime)
        {
            if (!IsCursorInsideScreenBounds())
            {
                Vector2 vector = Mechanics.NormalisedDifferenceVector(_cursor, ScreenSettings.ScreenCentre);
                MoveCamera(gameTime, CAMERA_SPEED, vector);
            }
        }

        private void MoveToNextPlayer(GameTime gameTime)
        {
            if (StateMachine.Instance.NewTurn())
            {
                _oldCameraPosition = _cameraPosition;
                _newCameraPosition = ConvertToScreenResolution(WeaponManager.Instance.ActiveWizardPosition);
                _cameraMoveTimer.ResetTimer(_cameraTime);
                _movingCamera = true;
            }

            if (_movingCamera)
            {
                _cameraMoveTimer.Update(gameTime);

                if (Math.Abs(Mechanics.VectorMagnitude(_cameraPosition - _newCameraPosition)) > 100 && _cameraMoveTimer.IsRunning)
                {
                    Vector2 vector = Mechanics.NormalisedDifferenceVector(_newCameraPosition, _oldCameraPosition);
                    MoveCamera(gameTime, CAMERA_SPEED * 2, vector);
                }
                else
                {
                    _cameraMoveTimer.ResetTimer(_cameraTime);
                    _movingCamera = false;
                }
            }
        }

        private void SetCameraScale(GameTime gameTime, float newScale)
        {
            if (_scaleFactor < newScale)
                _scaleFactor += (float)gameTime.ElapsedGameTime.TotalSeconds * ZOOM_RATE;
            else if (_scaleFactor > newScale)
                _scaleFactor -= (float)gameTime.ElapsedGameTime.TotalSeconds * ZOOM_RATE;
        }

        private bool IsCursorInsideScreenBounds()
        {
            Vector2 scaledCursor = _cursor / _scaleFactor;
            Point cursor = new Point((int)scaledCursor.X, (int)scaledCursor.Y);
            Rectangle screen = Utility.ShiftRectangle(_screenBounds, _cameraOffset, _scaleFactor);
            return screen.Contains(cursor);
        }

        private Vector2 TransformPointToWorldSpace(Vector2 point)
        {
            Matrix matrix = TransformMatrix * Matrix.Invert(OriginMatrix);
            return Vector2.Transform(point, matrix);
        }

        private Vector2 ConvertToTargetResolution(Vector2 screenResolutionPoint) => screenResolutionPoint / _originScale;
        private Vector2 ConvertToScreenResolution(Vector2 targetResolutionPoint) => targetResolutionPoint * _originScale;

        //public void LoadContent(ContentManager contentManager)
        //{
        //    _font = contentManager.Load<SpriteFont>("InfoFont");
        //}

        //public void Draw(SpriteBatch spriteBatch)
        //{
        //    spriteBatch.DrawString(_font, "moving: " + _movingCamera.ToString(), new Vector2(60, 60), Color.White);
        //    spriteBatch.DrawString(_font, "time: " + _cameraMoveTimer.Time.ToString("0.00"), new Vector2(60, 80), Color.White);
        //    spriteBatch.DrawString(_font, "vec mag: " + Mechanics.VectorMagnitude(_cameraPosition - _newCameraPosition).ToString("0.0"), new Vector2(60, 100), Color.White);
        //    //spriteBatch.DrawString(_font, _movingCamera.ToString(), new Vector2(60, 120), Color.White);
        //    //spriteBatch.DrawString(_font, _movingCamera.ToString(), new Vector2(60, 140), Color.White);
        //}
    }
}
