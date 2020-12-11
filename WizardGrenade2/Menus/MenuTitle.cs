using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace WizardGrenade2
{
    public class MenuTitle
    {
        private Sprite _title;
        private SpriteFont _taglineFont;
        private Vector2 _titlePosition;
        private Vector2 _taglinePosition;
        private string _tagelineText = "Beta!";
        private float _tagelineRotation = Mechanics.PI / 5;

        public void LoadContent(ContentManager contentManager)
        {
            _title = new Sprite(contentManager, @"Menu/Title");
            _taglineFont = contentManager.Load<SpriteFont>(@"Fonts/BetaFont");
            _title.SpriteScale = 0.4f;
            _titlePosition = new Vector2(ScreenSettings.CentreScreenWidth, _title.SpriteRectangle.Height / 2) - _title.Origin;
            _taglinePosition = new Vector2(_titlePosition.X + (_title.SpriteRectangle.Width * 0.78f), 0f);
        }

        public void Update(GameTime gameTime)
        {
            _taglinePosition.Y += (float)(Math.Sin(gameTime.TotalGameTime.TotalSeconds * 4)) * 0.2f;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _title.DrawSprite(spriteBatch, _titlePosition);
            spriteBatch.DrawString(_taglineFont, _tagelineText, _taglinePosition, Colours.Gold, _tagelineRotation, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
