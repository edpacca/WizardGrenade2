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

        private Teams _wizardTeams;
        private List<Wizard> _allWizards;
        private WeaponManager _weaponManager = WeaponManager.Instance;

        public void Initialise(GameOptions gameOptions)
        {
            _wizardTeams = new Teams(gameOptions);
            _allWizards = _wizardTeams.GetAllWizards();
        }

        public void LoadContent(ContentManager contentManager)
        {
            _map.LoadContent(contentManager, _mapFileName, true);
            _wizardTeams.LoadContent(contentManager);
            _weaponManager.LoadContent(contentManager, _allWizards);
            _debugFont = contentManager.Load<SpriteFont>("StatFont");
        }

        public int[] GetTeamHealths() => _wizardTeams.GetTeamHealths();

        public void Update(GameTime gameTime)
        {
            _wizardTeams.Update(gameTime);
            _weaponManager.Update(gameTime, _wizardTeams.GetActiveWizardPosition(), _wizardTeams.GetActiveWizardDirection());
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _map.Draw(spriteBatch);
            _wizardTeams.Draw(spriteBatch);
            _weaponManager.Draw(spriteBatch);
        }
    }
}
