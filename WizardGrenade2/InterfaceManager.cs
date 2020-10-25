using Microsoft.Xna.Framework;
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

        public InterfaceManager(float backBufferWidth, float backBufferHeight, float targetWidth, float targetHeight)
        {
            _mainScaleX = backBufferWidth / targetWidth;
            _mainScaleY = backBufferHeight / targetHeight;
            _originMatrix = Matrix.CreateScale(new Vector3(_mainScaleX, _mainScaleY, 1));
            _scaleMatrix = Matrix.CreateScale(new Vector3(_mainScaleX, _mainScaleY, 1));
        }

        public void Update()
        {
            Scroll();
        }

        private void Scroll()
        {
            if (InputManager.HasScrollWheelMoved())
                _scaleFactor = 1 + (InputManager.GetScrollWheelValue() / 10000);

            _scaleMatrix.M11 = _scaleFactor * _mainScaleX;
            _scaleMatrix.M22 = _scaleFactor * _mainScaleY;
        }

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
