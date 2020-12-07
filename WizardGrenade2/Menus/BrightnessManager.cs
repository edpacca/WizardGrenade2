using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WizardGrenade2
{
    public class BrightnessManager
    {
        private Texture2D _brightnessMask;
        private Texture2D _darknessMask;
        private Rectangle _screen;
        private Color _brightnessColour = new Color(160, 160, 160, 255);
        private Color _darknessColour = new Color(10, 10, 10, 255);

        private float _brightness = 0.5f;

        public BrightnessManager(GraphicsDevice graphics)
        {
            _screen = new Rectangle(0, 0, (int)ScreenSettings.TARGET_WIDTH, (int)ScreenSettings.TARGET_HEIGHT);
            _brightnessMask = new Texture2D(graphics, 1, 1);
            _darknessMask = new Texture2D(graphics, 1, 1);            
            _brightnessMask.SetData(new Color[] { _brightnessColour });
            _darknessMask.SetData(new Color[] { _darknessColour });
            _brightnessMask.GraphicsDevice.BlendState = BlendState.NonPremultiplied;
            _darknessMask.GraphicsDevice.BlendState = BlendState.NonPremultiplied;
        }

        public void SetBrightness(float brightness)
        {
            _brightness = brightness;
            float bright = 1 - brightness;
            float dark = 0.5f - brightness;

            _brightnessColour.A = (byte)(bright * 255);
            _darknessColour.A = (byte)(dark * 255);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_brightness > 0.5f)
                spriteBatch.Draw(_brightnessMask, _screen, _brightnessColour);
            else if (_brightness < 0.5f)
                spriteBatch.Draw(_darknessMask, _screen, _darknessColour);
        }

    }
}
