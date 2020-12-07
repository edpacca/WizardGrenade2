using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WizardGrenade2.Generics
{
    public class Setting
    {
        public int IntValue { get; private set; }
        public float Value { get; private set; }
        public int MinValue { get; private set; }
        public int MaxValue { get; private set; }
        private SpriteMeter _spriteMeter;

        public Setting(int initialValue, int minValue, int maxValue)
        {
            IntValue = initialValue;
            MinValue = minValue;
            MaxValue = maxValue;
            Value = (float)IntValue / (float)MaxValue;
        }

        public void SetValue(int value)
        {
            IntValue = value >= MaxValue ? MaxValue : value <= MinValue ? MinValue : value;
            Value = (float)IntValue / (float)MaxValue;
        }

        public void ChangeValue(int diff)
        {
            SetValue(IntValue + diff);
        }

        public void LoadContent(ContentManager contentManager, string spriteFile)
        {
            _spriteMeter = new SpriteMeter(contentManager, spriteFile);
        }

        public void SetSpriteMeter(float maxWidth, float spriteScale)
        {
            _spriteMeter.Interval = maxWidth / (MaxValue - 1);
            _spriteMeter.SetSpriteScale(spriteScale);
        }

        public void DrawSetting(SpriteBatch spriteBatch, Vector2 startPosition)
        {
            _spriteMeter.Draw(spriteBatch, startPosition, IntValue);
        }
    }
}
