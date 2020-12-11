using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WizardGrenade2
{
    public class Credits
    {
        public bool InCredits { get; set; }
        private SpriteFont _textFont;

        public void LoadContent(ContentManager contentManager)
        {
            _textFont = contentManager.Load<SpriteFont>(@"Fonts/ScreenFont");
        }

        public void Update()
        {
            if (InputManager.WasKeyPressed(Keys.Back))
                InCredits = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 position = MenuSettings.CreditsPosition;
            foreach (var line in MenuSettings.Credits)
            {
                Vector2 textSize = _textFont.MeasureString(line) / 2;
                spriteBatch.DrawString(_textFont, line, position - textSize, Colours.Ink);
                spriteBatch.DrawString(_textFont, line, position - textSize + Colours.ShadowOffset, Colours.Gold);
                position.Y += textSize.Y * 2;
            }
        }
    }
}
