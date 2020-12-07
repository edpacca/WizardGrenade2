using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace WizardGrenade2
{
    public class Teams
    {
        private readonly List<Wizard> _allWizards = new List<Wizard>();
        private List<Team> _teams = new List<Team>();
        private Marker _marker = new Marker();
        public List<string> TeamNames { get; private set; }
        public int[] TeamHealthValues { get; private set; }
        public bool AreTeamsPlaced { get; private set; }
        public bool IsGameOver { get; private set; }
        public string WinningTeam { get; private set; }

        private int _activeWizard;
        private int _activeTeam;
        private int _numberOfTeams;
        private int _teamSize;
        private const int MAXIMUM_TEAMS = 4;

        public Teams(GameOptions gameOptions)
        {
            _numberOfTeams = gameOptions.NumberOfTeams > MAXIMUM_TEAMS ? MAXIMUM_TEAMS : gameOptions.NumberOfTeams;
            _teamSize = gameOptions.TeamSize;
            TeamNames = new List<string>();

            for (int i = 0; i < _numberOfTeams; i++)
            {
                _teams.Add(new Team(i, _teamSize, gameOptions.WizardHealth));
                TeamNames.Add("Team " + (i + 1));
            }

            foreach (var team in _teams)
                foreach (var wizard in team.wizards)
                    _allWizards.Add(wizard);

            TeamHealthValues = new int[_numberOfTeams];
            LoadTeamHealth();

            _teams[_activeTeam].wizards[_activeWizard].IsActive = true;
        }

        public void LoadContent(ContentManager contentManager)
        {
            foreach (var team in _teams)
                team.LoadContent(contentManager);

            _marker.LoadContent(contentManager);
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < _numberOfTeams; i++)
            {
                _teams[i].Update(gameTime);
                TeamHealthValues[i] = _teams[i].GetTeamHealth();
            }

            if (StateMachine.Instance.NewTurn() && !IsGameOver)
            {
                ChangeTeam();
                SoundManager.Instance.PlaySound("wizardOh1");
            }


            if (StateMachine.Instance.GameState == StateMachine.GameStates.PlayerTurn ||
                StateMachine.Instance.GameState == StateMachine.GameStates.ShotTaken)
                _teams[_activeTeam].wizards[_activeWizard].UpdateControl(gameTime);

            CheckGameOverStatus();
            _marker.Update(gameTime, ActiveWizardPosition);
        }

        private void LoadTeamHealth()
        {
            for (int i = 0; i < _numberOfTeams; i++)
                TeamHealthValues[i] = _teams[i].GetTeamHealth();
        }

        public void PlaceTeams()
        {
            Vector2 position = InputManager.CursorPosition();
            _teams[_activeTeam].wizards[_activeWizard].Position = position;
            bool isValidPlacement = _teams[_activeTeam].wizards[_activeWizard].CheckPlacement();

            if (InputManager.WasLeftMousePressed() && isValidPlacement)
            {
                SoundManager.Instance.PlaySound("wizardCast");
                _teams[_activeTeam].wizards[_activeWizard].PlaceWizard(position);
                _activeTeam = Utility.WrapAroundCounter(_activeTeam, _numberOfTeams);

                if (_teams[_activeTeam].wizards[_activeWizard].IsPlaced)
                {
                    _activeWizard = Utility.WrapAroundCounter(_activeWizard, _teamSize);
                    if (_teams[_activeTeam].wizards[_activeWizard].IsPlaced)
                        StateMachine.Instance.WizardsPlaced();
                }
            }
        }

        public void ChangeTeam()
        {
            _teams[_activeTeam].wizards[_activeWizard].IsActive = false;
            _activeTeam = Utility.WrapAroundCounter(_activeTeam, _numberOfTeams);

            if (_teams[_activeTeam].IsTeamOut)
                ChangeTeam();
            else
            {
                _teams[_activeTeam].NextActiveWizard();
                _activeWizard = _teams[_activeTeam].ActiveWizard;
                _teams[_activeTeam].wizards[_activeWizard].IsActive = true;
            }
        }

        private void CheckGameOverStatus()
        {
            int remainingTeams = 0;

            foreach (var team in _teams)
            {
                if (!team.IsTeamOut)
                    remainingTeams++;
            }

            IsGameOver = remainingTeams < 2 ? true : false;
            
            if (IsGameOver)
            {
                for (int i = 0; i < _numberOfTeams; i++)
                {
                    if (!_teams[i].IsTeamOut)
                    {
                        WinningTeam = TeamNames[i];
                        StateMachine.Instance.EndCurrentGame(WinningTeam);
                        return;
                    }
                }

                StateMachine.Instance.EndCurrentGame("");
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if ((StateMachine.Instance.GameState == StateMachine.GameStates.PlayerTurn ||
                StateMachine.Instance.GameState == StateMachine.GameStates.ShotTaken) && !IsGameOver)
                _marker.Draw(spriteBatch);

            foreach (var team in _teams)
                team.Draw(spriteBatch);
        }

        public Vector2 ActiveWizardPosition { get => _teams[_activeTeam].wizards[_activeWizard].Position; }
        public int ActiveWizardDirection { get => _teams[_activeTeam].wizards[_activeWizard].DirectionCoefficient; }
        public List<Wizard> AllWizards { get => _allWizards; }
    }
}
