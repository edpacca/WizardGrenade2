using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardGrenade2
{
    class Credits
    {
        private SpriteFont _textFont;
        private Vector2 _firstLine = new Vector2(ScreenSettings.CentreScreenWidth, 300);
        private Vector2 _shadowOffset = new Vector2(2, 1);
        private float _interval = 50f;

        public bool InCredits { get; set; }

        private List<string> _credits = new List<string>()
        {
            "Basically entirely created by",
            "Eddie Pace",
            "",
            "Except for the beautiful sky,",
            "moon and menu backgrounds",
            "by Alicja Przystup",
        };

        public void LoadContent(ContentManager contentManager)
        {
            _textFont = contentManager.Load<SpriteFont>("ScreenFont");
        }

        public void Update()
        {
            if (InputManager.WasKeyPressed(Keys.Back))
                InCredits = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 position = _firstLine;
            foreach (var line in _credits)
            {
                Vector2 textSize = _textFont.MeasureString(line) / 2;
                spriteBatch.DrawString(_textFont, line, position - textSize, Colours.Ink);
                spriteBatch.DrawString(_textFont, line, position - textSize + _shadowOffset, Colours.Gold);
                position.Y += _interval;
            }
        }
    }
}
