using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace WizardGrenade2
{
    class BattleManager
    {
        private readonly string _mapFileName;
        private Map _map = Map.Instance;
        private WeaponManager _weaponManager = WeaponManager.Instance;
        private Teams _wizardTeams;
        private List<Wizard> _allWizards;
        public int[] TeamHealths => _wizardTeams.TeamHealthValues;
        public List<string> TeamNames => _wizardTeams.TeamNames;
        
        public BattleManager(string mapFileName)
        {
            _mapFileName = mapFileName;
        }

        public void LoadContent(ContentManager contentManager)
        {
            _map.LoadContent(contentManager, _mapFileName, true);
            _wizardTeams.LoadContent(contentManager);
            _weaponManager.LoadContent(contentManager, _allWizards);
        }

        public void Initialise(GameOptions gameOptions)
        {
            _wizardTeams = new Teams(gameOptions);
            _allWizards = _wizardTeams.AllWizards;
        }

        public void Update(GameTime gameTime)
        {
            if (StateMachine.Instance.GameState == StateMachine.GameStates.PlaceWizards)
            {
                _wizardTeams.PlaceTeams();
            }
            else
            {
                _wizardTeams.Update(gameTime);
                _weaponManager.Update(gameTime, _wizardTeams.ActiveWizardPosition, _wizardTeams.ActiveWizardDirection);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _map.Draw(spriteBatch);
            _wizardTeams.Draw(spriteBatch);
            _weaponManager.Draw(spriteBatch);
        }
    }
}
