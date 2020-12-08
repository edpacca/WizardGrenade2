using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace WizardGrenade2
{
    public class StateMachine
    {
        private StateMachine()
        {
            PlaceWizards();
        }
        private static readonly Lazy<StateMachine> lazyStateMachine = new Lazy<StateMachine>(() => new StateMachine());
        public static StateMachine Instance { get => lazyStateMachine.Value; }
        private const float TIME_BETWEEN_TURNS = 3f;
        private Timer _timer = new Timer(TIME_BETWEEN_TURNS);
        private ScreenText _screenText = new ScreenText();

        public void LoadContent(ContentManager contentManager) => _screenText.LoadContent(contentManager);
        public void Draw(SpriteBatch spriteBatch) => _screenText.Draw(spriteBatch);

        public enum GameStates 
        {
            GameSetup,
            PlaceWizards,
            GameStarted,
            PlayerTurn,
            ShotTaken,
            BetweenTurns,
            PlayerOut,
            GameOver,
            Reset,
            ExitGame,
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

        public void PlaceWizards()
        {
            GameState = GameStates.PlaceWizards;
            _screenText.IsDisplaying = true;
        }

        public void WizardsPlaced()
        {
            if (GameState == GameStates.PlaceWizards)
            {
                GameState = GameStates.PlayerTurn;
                _screenText.IsDisplaying = false;
            }
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

        public bool NewGameState() => _previousGameState != _currentGameState;

        public void ForceTurnEnd()
        {
            GameState = GameStates.BetweenTurns;
        }

        public void EndCurrentGame(string winningTeam)
        {
            GameState = GameStates.GameOver;

            if (NewGameState())
            {
                _screenText.IsDisplaying = true;
                _screenText.MainText = String.IsNullOrEmpty(winningTeam) ? "It's a Draw!" : winningTeam + " wins!";
                _screenText.InfoText = "Press 'Enter' to restart, or 'Delete' to quit";

                string gameEndSound = String.IsNullOrEmpty(winningTeam) ? "Draw" : "Win";
                SoundManager.Instance.PlaySoundInstance(gameEndSound);
            }
        }

        public void ExitGame()
        {
            GameState = GameStates.ExitGame;
        }

        public void RestartGame()
        {
            GameState = GameStates.PlaceWizards;
        }

        public void ResetGame()
        {
            GameState = GameStates.Reset;
        }

        public bool IsInGameState(GameStates gameState)
        {
            return GameState == gameState;
        }
    }
}
