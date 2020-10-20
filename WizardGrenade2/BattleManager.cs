using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace WizardGrenade2
{
    class BattleManager
    {
        private readonly string _mapFileName = "Map2";
        private Map _map = Map.Instance;
        private Wizard _testWizard;

        public void Initialise()
        {
            _testWizard = new Wizard(0, new Vector2(100, 100));
        }

        public void LoadContent(ContentManager contentManager)
        {
            _map.LoadContent(contentManager, _mapFileName, true);
            _testWizard.LoadContent(contentManager);
        }

        public void Update(GameTime gameTime)
        {
            _testWizard.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _map.Draw(spriteBatch);
            _testWizard.Draw(spriteBatch);
        }
    }
}
