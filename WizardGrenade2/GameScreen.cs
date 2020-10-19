using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace WizardGrenade2
{
    class GameScreen
    {
        private BattleManager _battleManager = new BattleManager();

        public void Initialise()
        {
            _battleManager.Initialise();
        }

        public void LoadContent(ContentManager contentManager)
        {
            _battleManager.LoadContent(contentManager);
        }

        public void Update(GameTime gameTime)
        {
            _battleManager.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _battleManager.Draw(spriteBatch);
        }
    }
}
