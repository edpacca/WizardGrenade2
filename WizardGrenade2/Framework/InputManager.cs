using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace WizardGrenade2
{
    public static class InputManager
    {
        private static KeyboardState _currentKeyboardState;
        private static KeyboardState _previousKeyboardState;
        private static MouseState _currentMouseState;
        private static MouseState _previousMouseState;

        public static void Update()
        {
            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();

            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();
        }

        public static int NumberKeys()
        {
            Keys[] keys = _currentKeyboardState.GetPressedKeys();
            int numberKey = 0;

            foreach (var key in keys)
            {
                try { numberKey = Convert.ToByte(key.ToString().Substring(1));}
                catch (Exception) { return 0; }
            }

            return numberKey;
        }

        public static MouseState GetMouseState() => _currentMouseState;
        public static Vector2 CursorPosition() => new Vector2(_currentMouseState.X, _currentMouseState.Y);
        public static bool IsKeyDown(Keys key) => _currentKeyboardState.IsKeyDown(key);
        public static bool IsKeyUp(Keys key) => _currentKeyboardState.IsKeyUp(key);
        public static bool WasKeyPressed(Keys key) => _currentKeyboardState.IsKeyDown(key) && _previousKeyboardState.IsKeyUp(key);
        public static bool WasKeyReleased(Keys key) => _currentKeyboardState.IsKeyUp(key) && _previousKeyboardState.IsKeyDown(key);
        public static bool HasScrollWheelMoved() => _currentMouseState.ScrollWheelValue != _previousMouseState.ScrollWheelValue;
        public static bool WasLeftMousePressed() => _currentMouseState.LeftButton == ButtonState.Pressed && _currentMouseState.LeftButton != _previousMouseState.LeftButton;
        public static float GetScrollWheelValue() => _currentMouseState.ScrollWheelValue;
    }
}
