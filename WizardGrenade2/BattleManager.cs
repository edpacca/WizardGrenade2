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

        private Teams _battleTeams;
        private GameObject[] _allWizards;
        private WeaponManager _weaponManager = new WeaponManager();

        private const int WIZARD_HEALTH = 100;
        private const int NUMBER_OF_TEAMS = 3;
        private const int TEAM_SIZE = 3;

        public void Initialise()
        {
            _battleTeams = new Teams(NUMBER_OF_TEAMS, TEAM_SIZE, WIZARD_HEALTH);
            _allWizards = _battleTeams.GetAllWizards();
        }

        public void LoadContent(ContentManager contentManager)
        {
            _map.LoadContent(contentManager, _mapFileName, true);
            _battleTeams.LoadContent(contentManager);
            _weaponManager.LoadContent(contentManager);
            _debugFont = contentManager.Load<SpriteFont>("StatFont");
        }

        public void Update(GameTime gameTime)
        {
            _battleTeams.Update(gameTime);
            _weaponManager.Update(gameTime, _battleTeams.GetActiveWizardPosition());
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _map.Draw(spriteBatch);
            _battleTeams.Draw(spriteBatch);
            _weaponManager.Draw(spriteBatch);
        }
    }
}
