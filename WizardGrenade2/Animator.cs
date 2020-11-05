using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace WizardGrenade2
{
    public class Animator
    {
        private readonly Dictionary<string, int[]> _animationStates;
        private readonly int _frameSize;
        
        private int _currentFrameIndex = 0;
        private float _elapsedFrameTime = 0;
        private bool _isSequenceReset = true;

        public Animator(Dictionary<string, int[]> animationStates, int frameSize)
        {
            _animationStates = animationStates;
            _frameSize = frameSize;
        }

        public int GetAnimationFrames(string state, float targetFrameRate, GameTime gameTime)
        {
            return GetFramePosition(GetCurrentFrame(state, targetFrameRate, gameTime));
        }

        public int GetSingleFrame(string state)
        {
            return GetFramePosition(_animationStates[state][0]);
        }

        public int GetFramePosition(int currentFrame)
        {
            return currentFrame * _frameSize;
        }

        public int GetCurrentFrame(string state, float targetFrameRate, GameTime gameTime)
        {
            int numberOfFrames = _animationStates[state].Length;
            _elapsedFrameTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_currentFrameIndex >= numberOfFrames)
                _currentFrameIndex = 0;

            if (_elapsedFrameTime < (1 / targetFrameRate))
                return _animationStates[state][_currentFrameIndex];
            else
            {
                _currentFrameIndex = (_currentFrameIndex + 1) % numberOfFrames;
                _elapsedFrameTime = 0;
                return _animationStates[state][_currentFrameIndex];
            }
        }

        public int GetCurrentFrameSingleSequence(string state, float targetFrameRate, GameTime gameTime)
        {
            int numberOfFrames = _animationStates[state].Length;
            _elapsedFrameTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_currentFrameIndex >= numberOfFrames)
                _currentFrameIndex = 0;

            if (_elapsedFrameTime < (1 / targetFrameRate))
                return _animationStates[state][_currentFrameIndex];
            else
            {
                _currentFrameIndex++;
                _elapsedFrameTime = 0;

                if (_currentFrameIndex < numberOfFrames)
                    return _animationStates[state][_currentFrameIndex];
                else
                {
                    _currentFrameIndex = 0;
                    return 255;
                }
            }
        }

        public void ResetSequence()
        {
            _isSequenceReset = true;
        }

        public int GetAnimationFrameSequence(string state1, string state2, float targetFrameRate1, float targetFrameRate2, GameTime gameTime)
        {
            int currentSequenceFrame;

            if (_isSequenceReset)
            {
                currentSequenceFrame = GetCurrentFrameSingleSequence(state1, targetFrameRate1, gameTime);

                if (currentSequenceFrame == 255)
                {
                    _isSequenceReset = false;
                    currentSequenceFrame = GetCurrentFrame(state2, targetFrameRate2, gameTime);
                }
            }
            else
                currentSequenceFrame = GetCurrentFrame(state2, targetFrameRate2, gameTime);

            return GetFramePosition(currentSequenceFrame);
        }
    }
}
