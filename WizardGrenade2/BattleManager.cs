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
        private readonly string _mapFileName = "TestMap2";
        private Map _map = Map.Instance;

        private Teams _wizardTeams;
        private List<GameObject> _allWizards;
        private WeaponManager _weaponManager = new WeaponManager();

        private const int WIZARD_HEALTH = 100;
        private const int NUMBER_OF_TEAMS = 3;
        private const int TEAM_SIZE = 3;

        public void Initialise()
        {
            _wizardTeams = new Teams(NUMBER_OF_TEAMS, TEAM_SIZE, WIZARD_HEALTH);
            _allWizards = _wizardTeams.GetAllWizards();
        }

        public void LoadContent(ContentManager contentManager)
        {
            _map.LoadContent(contentManager, _mapFileName, true);
            _wizardTeams.LoadContent(contentManager);
            _weaponManager.LoadContent(contentManager);
            _weaponManager.PopulateGameObjects(_allWizards);
            _debugFont = contentManager.Load<SpriteFont>("StatFont");
        }

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
