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

        private Scroll _scroll;

        public bool IsPaused { get; set; }
        private bool _previousPauseState;
        private bool _currentPauseState;

        public PauseMenu(GraphicsDevice graphics)
        {
            _graphics = graphics;
        }

        public void LoadContent(ContentManager contentManager)
        {
            _optionsFont = contentManager.Load<SpriteFont>("OptionFont");
            _textPosition -= _optionsFont.MeasureString(_pausedText) / 2;
            _textPosition.Y -= 180;
            _exitTextPosition.X -= _optionsFont.MeasureString(_exitText).X;

            _backgroundRectangle = new Rectangle(0, 0, (int)ScreenSettings.TARGET_WIDTH, (int)ScreenSettings.TARGET_HEIGHT);
            _background = new Texture2D(_graphics, 1, 1);
            _background.SetData(new Color[] { _backgroundColour });

            _scroll = new Scroll(new Vector2(ScreenSettings.CentreScreenWidth, ScreenSettings.CentreScreenHeight));
            _scroll.LoadContent(contentManager);
        }

        public void PauseGame(Keys key)
        {
            if (InputManager.WasKeyPressed(key))
                IsPaused = IsPaused ? false : true;
        }

        public void Update(GameTime gameTime)
        {
            _previousPauseState = _currentPauseState;
            _currentPauseState = IsPaused;

            if (IsPaused)
                _scroll.Update(gameTime);
            else if (_currentPauseState != _previousPauseState)
            {
                _scroll.ResetPauseMenu();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsPaused)
            {
                spriteBatch.Draw(_background, _backgroundRectangle, _backgroundColour);
                _scroll.Draw(spriteBatch);
                spriteBatch.DrawString(_optionsFont, _pausedText, _textPosition, Color.Black);
                spriteBatch.DrawString(_optionsFont, _exitText, _exitTextPosition, Color.Yellow);
            }
        }
    }
}
