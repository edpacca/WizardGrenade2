using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace WizardGrenade2
{
    public class ScreenText
    {
        private SpriteFont _mainFont;
        private SpriteFont _secondaryFont;
        private Vector2 _mainTextPosition = new Vector2(ScreenSettings.CentreScreenWidth, 150);
        private Vector2 _secondaryTextPosition = new Vector2(ScreenSettings.CentreScreenWidth, 190);
        private Vector2 _shadowOffset = new Vector2(2, 1);

        public bool IsDisplaying { get; set; }
        public string MainText { get; set; }
        public string InfoText { get; set; }

        public void LoadContent(ContentManager contentManager)
        {
            _mainFont = contentManager.Load<SpriteFont>(@"Fonts/ScreenFont");
            _secondaryFont = contentManager.Load<SpriteFont>(@"Fonts/InfoFont");
            MainText = "Place yer wizards!";
            InfoText = "use the mouse and click to confirm position";
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsDisplaying)
            {
                Vector2 mainTextSize = _mainFont.MeasureString(MainText) / 2;
                spriteBatch.DrawString(_mainFont, MainText, _mainTextPosition - mainTextSize, Colours.Ink);
                spriteBatch.DrawString(_mainFont, MainText, _mainTextPosition - mainTextSize + _shadowOffset, Colours.Gold);
                Vector2 infoTextSize = _secondaryFont.MeasureString(InfoText) / 2;
                spriteBatch.DrawString(_secondaryFont, InfoText, _secondaryTextPosition - infoTextSize, Colours.Gold);
            }
        }
    }
}
