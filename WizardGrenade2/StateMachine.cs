using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WizardGrenade2
{
    class StateMachine
    {
        public enum GameStates 
        { 
            GameStart,
            PlayerTurn,
            PlayerFired,
            PlayerOut,
            GameOver,
        }

        public GameStates GameState { get; set; }

        public StateMachine()
        {
            GameState = GameStates.GameStart;
        }
    }
}
