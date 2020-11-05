using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace WizardGrenade2
{
    class Teams
    {
        private List<Team> _teams = new List<Team>();
        private readonly List<GameObject> _allWizards;

        private int _activeWizard;
        private int _activeTeam;
        private int _numberOfTeams;
        private int _teamSize;
        private const int MAXIMUM_TEAMS = 4;

        public Teams(int numberOfTeams, int teamSize, int wizardHealth)
        {
            _teamSize = teamSize;
            _numberOfTeams = numberOfTeams > MAXIMUM_TEAMS ? MAXIMUM_TEAMS : numberOfTeams;

            for (int i = 0; i < numberOfTeams; i++)
                _teams.Add(new Team(i, "Team " + i + 1, teamSize, wizardHealth));

            _allWizards = new List<GameObject>();

            foreach (var team in _teams)
                foreach (var wizard in team._wizards)
                    _allWizards.Add(wizard.GetWizard());
        }

        public void LoadContent(ContentManager contentManager)
        {
            foreach (var team in _teams)
            {
                team.LoadContent(contentManager);
            }
        }

        private void ChangeWizard()
        {
            _activeWizard = Utility.WrapAroundCounter(_activeWizard, _teamSize);
        }

        private void ChangeTeam()
        {
            _activeTeam = Utility.WrapAroundCounter(_activeTeam, _numberOfTeams);
        }

        public void ControlActiveTeamWizard(Keys changeTeam, Keys changeWizard)
        {
            if (InputManager.WasKeyReleased(changeTeam))
                ChangeTeam();
            if (InputManager.WasKeyReleased(changeWizard))
                ChangeWizard();
        }

        public void Update(GameTime gameTime)
        {
            foreach (var team in _teams)
            {
                team.Update(gameTime);
            }

            ControlActiveTeamWizard(Keys.CapsLock, Keys.Tab);

            _teams[_activeTeam]._wizards[_activeWizard].UpdateMovement(gameTime);
        }

        public Vector2 GetActiveWizardPosition()
        {
            return _teams[_activeTeam]._wizards[_activeWizard].GetPosition();
        }

        public int GetActiveWizardDirection()
        {
            return _teams[_activeTeam]._wizards[_activeWizard].GetDirection();
        }

        public List<GameObject> GetAllWizards()
        {
            return _allWizards;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var team in _teams)
                team.Draw(spriteBatch);
        }
    }
}
