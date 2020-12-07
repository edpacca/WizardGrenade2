using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardGrenade2
{
    public class ValueMinMax
    {
        public int[] _valueMinMax { get; private set; }
        private Sprite _valueSprite;
        private string _fileName;
        private Vector2 _startPosition;
        private Vector2 _endPosition;
        private Vector2 _interval;

        public ValueMinMax(int startValue, int minValue, int maxValue, string spriteFile)
        {
            _valueMinMax = new int[3] { startValue, minValue, maxValue };
            _fileName = spriteFile;
        }

        public void SetPositions(Vector2 start, Vector2 end)
        {
            _startPosition = start;
            _endPosition = end;
            _interval = (_endPosition - _startPosition) / _valueMinMax[0];
        }

        public void SetScale(float scale)
        {
            _valueSprite.SpriteScale = scale;
        }

        public void LoadContent(ContentManager contentManager)
        {
            _valueSprite = new Sprite(contentManager, _fileName);
        }

        public void DrawValue(SpriteBatch spriteBatch, int value)
        {
            for (int i = 0; i < value; i++)
            {
                _valueSprite.DrawSprite(spriteBatch, _startPosition + (_interval * i));
            }
        }
    }
}
