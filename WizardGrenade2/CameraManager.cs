using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace WizardGrenade2
{
    public class CameraManager
    {
        private const int ZOOM_RATE = 5;
        private const int CAMERA_SPEED = 200;
        private float _mainScaleX;
        private float _mainScaleY;
        private const float DEFAULT_SCALE = 0.8f;
        private float _scaleFactor = DEFAULT_SCALE;
        private Vector2 _cameraPosition = ScreenSettings.ScreenResolutionCentre;
        private Vector2 _cameraTransform { get => new Vector2(_cameraPosition.X / _mainScaleX, _cameraPosition.Y / _mainScaleY); }
        private SpriteFont _spriteFont;
        private Rectangle _screenBounds;
        private Vector2 _cursor;
        private const int BORDER_WIDTH = 10;
        private readonly Vector2 CAMERA_MIN_POSITION;
        private readonly Vector2 CAMERA_MAX_POSITION;
        private const float MAX_SCALE = 1.5f;
        private const float MIN_SCALE = 0.6f;

        private Matrix _originMatrix;

        private Camera _camera;

        public Matrix GetScaleMatrix() => _originMatrix * _camera.Transform;
        public Matrix GetOriginMatrix() => _originMatrix;


        private float _previousScrollWheelValue;
        private float _currentScrollWheelValue;

        public CameraManager()
        {
            _mainScaleX = ScreenSettings.RESOLUTION_WIDTH / ScreenSettings.TARGET_WIDTH;
            _mainScaleY = ScreenSettings.RESOLUTION_HEIGHT / ScreenSettings.TARGET_HEIGHT;
            _camera = new Camera();
            _originMatrix = Matrix.CreateScale(new Vector3(_mainScaleX, _mainScaleY, 1));

            _screenBounds = new Rectangle(BORDER_WIDTH, BORDER_WIDTH, (int)ScreenSettings.TARGET_WIDTH - BORDER_WIDTH, (int)ScreenSettings.TARGET_HEIGHT - BORDER_WIDTH);
            float xMin = ScreenSettings.RESOLUTION_WIDTH / 6;
            float yMin = ScreenSettings.RESOLUTION_HEIGHT / 6;
            float xMax = ScreenSettings.ScreenResolutionCentre.X + xMin;
            float yMax = ScreenSettings.ScreenResolutionCentre.Y + yMin;
            CAMERA_MIN_POSITION = new Vector2(xMin, yMin);
            CAMERA_MAX_POSITION = new Vector2(xMax, yMax);
        }

        public void Update(GameTime gameTime)
        {
            _cursor = InputManager.CursorPosition();
            Zoom(gameTime);
            CameraPosition(gameTime);

            if (InputManager.WasKeyPressed(Keys.LeftShift))
                ResetView();
        }

        public void ResetView()
        {
            _scaleFactor = DEFAULT_SCALE;
            _cameraPosition = ScreenSettings.ScreenResolutionCentre;
        }

        private void Zoom(GameTime gameTime)
        {
            _previousScrollWheelValue = _currentScrollWheelValue;
            _currentScrollWheelValue = InputManager.GetScrollWheelValue();
            
            int direction = _previousScrollWheelValue < _currentScrollWheelValue ? 1 :
                _previousScrollWheelValue > _currentScrollWheelValue ? -1 : 0;

            _scaleFactor += (float)gameTime.ElapsedGameTime.TotalSeconds * direction * ZOOM_RATE;
            _scaleFactor = _scaleFactor > MAX_SCALE ? MAX_SCALE : _scaleFactor < MIN_SCALE ? MIN_SCALE : _scaleFactor;
            _camera.Zoom = _scaleFactor;
        }

        private void CameraPosition(GameTime gameTime)
        {
            if (!IsCursorInsideWindow())
                _cameraPosition += Mechanics.NormaliseVector(Vector2.Subtract(_cursor, ScreenSettings.ScreenCentre)) * (float)gameTime.ElapsedGameTime.TotalSeconds * CAMERA_SPEED;

            _cameraPosition = Vector2.Clamp(_cameraPosition, CAMERA_MIN_POSITION, CAMERA_MAX_POSITION);
            _camera.Position = _cameraPosition;
        }

        private Vector2 TransformPoint(Vector2 point)
        {
            return Vector2.Transform(point, Matrix.Invert(_camera.Transform));
        }

        private Rectangle TransformRectangle(Rectangle rectangle)
        {
            Vector2 transformOrigin = TransformPoint(new Vector2(rectangle.X, rectangle.Y));
            Vector2 transformSize = TransformPoint(new Vector2(rectangle.Width, rectangle.Height));
            return new Rectangle((int)transformOrigin.X, (int)transformOrigin.Y, (int)transformSize.X, (int)transformSize.Y);
        }

        private bool IsCursorInsideWindow()
        {
            Point cursor = new Point((int)_cursor.X, (int)_cursor.Y);
            Rectangle screen = Utility.ShiftRectangle(_screenBounds, _cameraTransform - ScreenSettings.ScreenCentre);
            return screen.Contains(cursor);
        }

        //public void Draw(SpriteBatch spriteBatch)
        //{
        //    Rectangle rect = Utility.ShiftRectangle(_screenBounds, _cameraTransform - ScreenSettings.ScreenCentre);
        //    Vector2 transformCursor = TransformPoint(InputManager.CursorPosition());
        //    spriteBatch.DrawString(_spriteFont, "cursor: "+ InputManager.CursorPosition().X.ToString() + ", " + InputManager.CursorPosition().Y.ToString(), new Vector2(30, 80), Color.White);
        //    spriteBatch.DrawString(_spriteFont, "cursor trans: " + transformCursor.X.ToString("0") + ", " + transformCursor.Y.ToString("0"), new Vector2(30, 90), Color.White);
        //    spriteBatch.DrawString(_spriteFont, "camera: " + _cameraPosition.X.ToString() + ", " + _cameraPosition.Y.ToString(), new Vector2(30, 100), Color.White);
        //    spriteBatch.DrawString(_spriteFont, "rect x: " + rect.X.ToString() + ", " + rect.Y.ToString() + ", " + rect.Width.ToString() + ", " + rect.Height.ToString(), new Vector2(30, 110), Color.White);

        //}

        //public void LoadContent(ContentManager contentManager)
        //{
        //    _spriteFont = contentManager.Load<SpriteFont>("WizardHealthFont");
        //}
    }
}
