using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardGrenade2
{
    class UserInterface
    {
        private Sprite _cursor;

        public void LoadContent(ContentManager contentManager)
        {
            _cursor = new Sprite(contentManager, "mouse");
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _cursor.Draw(spriteBatch, InputManager.CursorPosition(), 0f);
        }
    }
}
