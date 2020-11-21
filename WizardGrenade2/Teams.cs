﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace WizardGrenade2
{
    public class Teams
    {
        private List<Team> _teams = new List<Team>();
        private Marker _marker = new Marker();
        private readonly List<Wizard> _allWizards;
        public List<string> TeamNames { get; private set; }
        public int[] TeamHealthValues { get; private set; }

        private int _activeWizard;
        private int _activeTeam;
        private int _numberOfTeams;
        private int _teamSize;
        private const int MAXIMUM_TEAMS = 4;
        public bool IsTeamsPlaced { get; private set; }

        private List<List<int>> _remainingWizards;

        public Teams(GameOptions gameOptions)
        {
            _teamSize = gameOptions.TeamSize;
            _numberOfTeams = gameOptions.NumberOfTeams > MAXIMUM_TEAMS ? MAXIMUM_TEAMS : gameOptions.NumberOfTeams;
            TeamNames = new List<string>();

            for (int i = 0; i < _numberOfTeams; i++)
            {
                _teams.Add(new Team(i, "Team " + i + 1, _teamSize, gameOptions.WizardHealth));
                TeamNames.Add("Team " + i);

                //_remainingWizards.Add(new List<int>());
                //for (int j = 0; j < _teamSize; j++)
                //    _remainingWizards[i].Add(j);
            }
            
            _allWizards = new List<Wizard>();
            TeamHealthValues = new int[_numberOfTeams];

            LoadTeamHealth();
            _teams[_activeTeam]._wizards[_activeWizard].IsActive = true;


            foreach (var team in _teams)
                foreach (var wizard in team._wizards)
                    _allWizards.Add(wizard);
        }

        public void LoadContent(ContentManager contentManager)
        {
            _marker.LoadContent(contentManager);

            foreach (var team in _teams)
                team.LoadContent(contentManager);
        }

        private void ChangeWizard()
        {
            _teams[_activeTeam]._wizards[_activeWizard].IsActive = false;
            _activeWizard = Utility.WrapAroundCounter(_activeWizard, _teamSize);

            if (_teams[_activeTeam]._wizards[_activeWizard].IsDead)
                ChangeWizard();

            _teams[_activeTeam]._wizards[_activeWizard].IsActive = true;

        }

        public void ChangeTeam()
        {
            _teams[_activeTeam]._wizards[_activeWizard].IsActive = false;
            _activeTeam = Utility.WrapAroundCounter(_activeTeam, _numberOfTeams);

            if (_teams[_activeTeam].IsTeamOut)
                ChangeTeam();
            if (_teams[_activeTeam]._wizards[_activeWizard].IsDead)
                ChangeWizard();

            _teams[_activeTeam]._wizards[_activeWizard].IsActive = true;

            if (_activeTeam == 0)
                ChangeWizard();
        }

        public void TurnCycle()
        {

        }

        private void UpdateRemainingWizardList()
        {
            for (int i = 0; i < _numberOfTeams; i++)
            {
                
            }
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
            for (int i = 0; i < _numberOfTeams; i++)
            {
                _teams[i].Update(gameTime);
                TeamHealthValues[i] = _teams[i].GetTeamHealth();
            }

            if (_teams[_activeTeam]._wizards[_activeWizard].IsActive && _teams[_activeTeam]._wizards[_activeWizard].IsDead)
            {
                ChangeWizard();
            }

            if (StateMachine.Instance.NewTurn())
                ChangeTeam();

            if (StateMachine.Instance.GameState == StateMachine.GameStates.PlayerTurn ||
                StateMachine.Instance.GameState == StateMachine.GameStates.ShotTaken)
                _teams[_activeTeam]._wizards[_activeWizard].UpdateControl(gameTime);

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

            _teams[_activeTeam]._wizards[_activeWizard].Position = position;
            
            bool isValidPlacement = _teams[_activeTeam]._wizards[_activeWizard].CheckPlacement();

            if (InputManager.WasLeftMousePressed() && isValidPlacement)
            {
                PlaceWizard(position);
                _activeTeam = Utility.WrapAroundCounter(_activeTeam, _numberOfTeams);

                if (_teams[_activeTeam]._wizards[_activeWizard].IsPlaced)
                {
                    _activeWizard = Utility.WrapAroundCounter(_activeWizard, _teamSize);
                    if (_teams[_activeTeam]._wizards[_activeWizard].IsPlaced)
                        StateMachine.Instance.WizardsPlaced();
                }
            }
        }

        private void PlaceWizard(Vector2 position)
        {
            _teams[_activeTeam]._wizards[_activeWizard].Position = position;
            _teams[_activeTeam]._wizards[_activeWizard].IsPlaced = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (StateMachine.Instance.GameState == StateMachine.GameStates.PlayerTurn ||
                StateMachine.Instance.GameState == StateMachine.GameStates.ShotTaken)
                _marker.Draw(spriteBatch);

            foreach (var team in _teams)
                team.Draw(spriteBatch);
        }

        public Vector2 ActiveWizardPosition { get => _teams[_activeTeam]._wizards[_activeWizard].Position; }
        public int ActiveWizardDirection { get => _teams[_activeTeam]._wizards[_activeWizard].DirectionCoefficient; }
        public List<Wizard> AllWizards { get => _allWizards; }
    }
}
