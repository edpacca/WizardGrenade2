using Microsoft.Xna.Framework;
using System;


namespace WizardGrenade2
{
    public class StateMachine
    {
        private StateMachine()
        {
            GameState = GameStates.PlaceWizards;
        }
        private static readonly Lazy<StateMachine> lazyStateMachine = new Lazy<StateMachine>(() => new StateMachine());
        public static StateMachine Instance { get => lazyStateMachine.Value; }
        private const float TIME_BETWEEN_TURNS = 3f;
        private Timer _timer = new Timer(TIME_BETWEEN_TURNS);

        public enum GameStates 
        { 
            PlaceWizards,
            PlayerTurn,
            ShotTaken,
            BetweenTurns,
            PlayerOut,
            GameOver,
        }

        public GameStates GameState { get; private set; }
        private GameStates _previousGameState;
        private GameStates _currentGameState;

        public void UpdateStateMachine(GameTime gameTime)
        {
            _previousGameState = _currentGameState;
            _currentGameState = GameState;

            if (GameState == GameStates.BetweenTurns)
            {
                _timer.Update(gameTime);
                if (!_timer.IsRunning)
                {
                    GameState = GameStates.PlayerTurn;
                    _timer.ResetTimer(TIME_BETWEEN_TURNS);
                }
            }
        }

        public void WizardsPlaced()
        {
            if (GameState == GameStates.PlaceWizards)
                GameState = GameStates.PlayerTurn;
        }

        public void ShotTaken()
        {
            if (GameState == GameStates.PlayerTurn)
                GameState = GameStates.ShotTaken;
        }

        public void ShotLanded()
        {
            if (GameState == GameStates.ShotTaken)
                GameState = GameStates.BetweenTurns;
        }

        public bool NewTurn()
        {
            return GameState == GameStates.PlayerTurn && 
                _previousGameState != _currentGameState && 
                _previousGameState != GameStates.PlaceWizards;
        }

        public void TimerRanOut()
        {
            if (GameState == GameStates.PlayerTurn)
                GameState = GameStates.BetweenTurns;
        }
    }
}
