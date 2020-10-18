using Microsoft.Xna.Framework.Input;

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
            _previousMouseState = _currentMouseState;

            _currentKeyboardState = Keyboard.GetState();
            _currentMouseState = Mouse.GetState();
        }

        public static bool IsKeyDown(Keys key) => _currentKeyboardState.IsKeyDown(key);
        public static bool IsKeyUp(Keys key) => _currentKeyboardState.IsKeyUp(key);

        public static bool KeyPressed(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key) && _previousKeyboardState.IsKeyUp(key);
        }

        public static bool KeyReleased(Keys key)
        {
            return _currentKeyboardState.IsKeyUp(key) && _previousKeyboardState.IsKeyDown(key);
        }

        public static bool ScrollWheelMoved()
        {
            if (_currentMouseState.ScrollWheelValue != _previousMouseState.ScrollWheelValue)
                return true;

            return false;
        }


    }
}
