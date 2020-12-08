using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WizardGrenade2
{
    public class BrightnessManager
    {
        private Texture2D _darknessMask;
        private Sprite _brightnessMask5;
        private Sprite _brightnessMask10;
        private Sprite _brightnessMask15;
        private Rectangle _screen;
        private Color _darknessColour = new Color(10, 10, 10, 255);
        private float _brightness = 0.5f;

        public BrightnessManager(GraphicsDevice graphics, ContentManager contentManager)
        {
            _screen = new Rectangle(0, 0, (int)ScreenSettings.TARGET_WIDTH, (int)ScreenSettings.TARGET_HEIGHT);
            _darknessMask = new Texture2D(graphics, 1, 1);            
            _darknessMask.SetData(new Color[] { _darknessColour });
            _darknessMask.GraphicsDevice.BlendState = BlendState.NonPremultiplied;
            _brightnessMask5 = new Sprite(contentManager, @"UserInterface/BrightnessMask5");
            _brightnessMask10 = new Sprite(contentManager, @"UserInterface/BrightnessMask10");
            _brightnessMask15 = new Sprite(contentManager, @"UserInterface/BrightnessMask15");
        }

        public void SetBrightness(float brightness)
        {
            _brightness = brightness;
            float dark = 0.5f - brightness;
            _darknessColour.A = (byte)(dark * 255);
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            if (_brightness > 0.5f)
            {
                if (_brightness < 0.7f)
                    _brightnessMask5.DrawSprite(spriteBatch, Vector2.Zero);
                else if (_brightness < 0.85f)
                    _brightnessMask10.DrawSprite(spriteBatch, Vector2.Zero);
                else
                    _brightnessMask15.DrawSprite(spriteBatch, Vector2.Zero);
            }
            else if (_brightness < 0.5f)
                spriteBatch.Draw(_darknessMask, _screen, _darknessColour);
        }

    }
}
