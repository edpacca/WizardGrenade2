using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WizardGrenade2
{
    public class StateMachine
    {
        private bool _isGameStarted;

        public enum GameStates 
        { 
            PlaceWizards,
            PlayerTurn,
            PlayerFired,
            PlayerOut,
            GameOver,
        }

        public GameStates GameState { get; set; }

        public StateMachine()
        {
            GameState = GameStates.PlaceWizards;
        }

        public void UpdateStateMachine(Teams wizardTeams)
        {
            if (_isGameStarted)
            {

            }
            else if (wizardTeams.IsTeamsPlaced)
            {
                GameState = GameStates.PlayerTurn;
            }
        }
    }
}
