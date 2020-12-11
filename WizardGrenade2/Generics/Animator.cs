using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace WizardGrenade2
{
    public class Animator
    {
        private readonly Dictionary<string, int[]> _animationStates;
        private readonly int _frameSize;
        private string _currentState;
        private float _elapsedFrameTime;
        private int _currentFrameIndex;
        private int _framesInCurrentState;

        public Animator(Dictionary<string, int[]> animationStates, int frameSize)
        {
            _animationStates = animationStates;
            _frameSize = frameSize;
        }

        private void CheckForNewState(string state)
        {
            if (_currentState != state)
                UpdateCurrentState(state);
        }

        private void UpdateCurrentState(string state)
        {
            _currentState = state;
            _currentFrameIndex = 0;
            _framesInCurrentState = _animationStates[state].Length;
        }

        public int GetAnimationFrames(string state, float targetFrameRate, GameTime gameTime) => GetFramePosition(GetCurrentFrame(state, targetFrameRate, gameTime));
        public int GetFramePosition(int currentFrame) => currentFrame * _frameSize;

        public int GetSingleFrame(string state, int frame)
        {
            if (frame > _animationStates[state].Length)
                return GetFramePosition(_animationStates[state][0]);

            return GetFramePosition(_animationStates[state][frame]);
        }

        public int GetCurrentFrame(string state, float targetFrameRate, GameTime gameTime)
        {
            CheckForNewState(state);
            _elapsedFrameTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_elapsedFrameTime >= (1 / targetFrameRate))
            {
                _currentFrameIndex = Utility.WrapAroundCounter(_currentFrameIndex, _framesInCurrentState);
                _elapsedFrameTime = 0;
            }

            return _currentFrameIndex >= _framesInCurrentState ? 0 : _animationStates[state][_currentFrameIndex];
        }
    }
}
