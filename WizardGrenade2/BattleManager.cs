using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace WizardGrenade2
{
    class BattleManager
    {
        private SpriteFont _debugFont;
        private readonly string _mapFileName = "Map2";
        private Map _map = Map.Instance;

        private Wizard _testWizard;

        private WeaponManager _weaponManager = new WeaponManager();

        public void Initialise()
        {
            _testWizard = new Wizard(0, new Vector2(100, 100));
        }

        public void LoadContent(ContentManager contentManager)
        {
            _map.LoadContent(contentManager, _mapFileName, true);
            _testWizard.LoadContent(contentManager);
            _weaponManager.LoadContent(contentManager);
            _debugFont = contentManager.Load<SpriteFont>("StatFont");
        }

        public void Update(GameTime gameTime)
        {
            _testWizard.Update(gameTime);
            _weaponManager.Update(gameTime, _testWizard.GetPosition());
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _map.Draw(spriteBatch);
            _testWizard.Draw(spriteBatch);
            _weaponManager.Draw(spriteBatch);
            spriteBatch.DrawString(_debugFont, _weaponManager.GetChargePower().ToString("0.00"), new Vector2(1100, 10), Color.White);
        }
    }
}
