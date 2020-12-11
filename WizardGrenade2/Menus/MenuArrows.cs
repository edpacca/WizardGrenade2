using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WizardGrenade2
{
    public class MenuArrows
    {
        private Sprite _arrow;
        private SpriteFont _infoFont;
        private Vector2 _arrowLPosition;
        private Vector2 _arrowRPosition;
        private float _textYOffset;
        private const float INSET = 20;

        public void LoadContent(ContentManager contentManager)
        {
            _arrow = new Sprite(contentManager, @"GameObjects/MelfsAcidArrow");
            _infoFont = contentManager.Load<SpriteFont>(@"Fonts/InfoFont2");
            _arrow.SpriteScale = MenuSettings.MENU_ARROW_SCALE;
            _arrowLPosition = new Vector2(INSET, INSET);
            _arrowRPosition = new Vector2(ScreenSettings.TARGET_WIDTH - _arrow.SpriteRectangle.Width - INSET, INSET);
            _textYOffset = _arrow.SpriteRectangle.Height + 4;
        }

        public void Draw(SpriteBatch spriteBatch, bool canGoBack, bool canGoForward)
        {
            if (canGoBack)
            {
                _arrow.DrawSprite(spriteBatch, _arrowLPosition + _arrow.Origin, Mechanics.PI);
                spriteBatch.DrawString(_infoFont, "BACKSPACE", new Vector2(_arrowLPosition.X - 7, _arrowLPosition.Y + _textYOffset), Colours.LightGrey);
            }

            if (canGoForward)
            {
                _arrow.DrawSprite(spriteBatch, _arrowRPosition);
                spriteBatch.DrawString(_infoFont, "ENTER", new Vector2(_arrowRPosition.X + 5, _arrowRPosition.Y + _textYOffset), Colours.LightGrey);
            }
        }
    }
}
