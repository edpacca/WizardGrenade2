using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq.Expressions;

namespace WizardGrenade2
{
    class PauseMenu
    {
        private SpriteFont _optionsFont;
        private Vector2 _textPosition = ScreenSettings.ScreenCentre;
        private Vector2 _exitTextPosition = new Vector2(ScreenSettings.TARGET_WIDTH - 10, 10);
        private readonly string _pausedText = "Paused";
        private readonly string _exitText = "Exit = 'del'";
        private GraphicsDevice _graphics;
        private Texture2D _background;
        private Rectangle _backgroundRectangle;
        private Color _backgroundColour = new Color(0, 0, 0, 150);

        public bool IsPaused { get; set; }

        public PauseMenu(GraphicsDevice graphics)
        {
            _graphics = graphics;
        }

        public void LoadContent(ContentManager contentManager)
        {
            _optionsFont = contentManager.Load<SpriteFont>("OptionFont");
            _textPosition -= _optionsFont.MeasureString(_pausedText) / 2;
            _textPosition.Y -= 200;
            _exitTextPosition.X -= _optionsFont.MeasureString(_exitText).X;

            _backgroundRectangle = new Rectangle(0, 0, (int)ScreenSettings.TARGET_WIDTH, (int)ScreenSettings.TARGET_HEIGHT);
            _background = new Texture2D(_graphics, 1, 1);
            _background.SetData(new Color[] { _backgroundColour });
        }

        public void PauseGame(Keys key)
        {
            if (InputManager.WasKeyPressed(key))
                IsPaused = IsPaused ? false : true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsPaused)
            {
                spriteBatch.Draw(_background, _backgroundRectangle, _backgroundColour);
                spriteBatch.DrawString(_optionsFont, _pausedText, _textPosition, Color.White);
                spriteBatch.DrawString(_optionsFont, _exitText, _exitTextPosition, Color.Yellow);
            }

        }
    }
}
