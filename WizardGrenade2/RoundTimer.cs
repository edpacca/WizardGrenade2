using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WizardGrenade2
{
    public class RoundTimer : Timer
    {
        private SpriteFont _timerFont;
        private Color _timerColour;
        private Vector2 _timeTextSize;
        private Vector2 _timerPosition;
        private const float TIMER_INSET_POSITION = 10f;
        private float _roundTime;

        public RoundTimer(float startTime) : base(startTime)
        {
            _roundTime = startTime + 0.5f;
        }

        public void LoadContent(ContentManager contentManager)
        {
            _timerFont = contentManager.Load<SpriteFont>("TimerFont");
            _timeTextSize = _timerFont.MeasureString(Time.ToString("0"));
            _timerPosition = new Vector2(TIMER_INSET_POSITION, ScreenSettings.TARGET_HEIGHT - (_timeTextSize.Y + TIMER_INSET_POSITION));
        }

        public override void Update(GameTime gameTime)
        {
            if (StateMachine.Instance.GameState == StateMachine.GameStates.PlayerTurn)
                base.Update(gameTime);

            if (!IsRunning)
            {
                StateMachine.Instance.ForceTurnEnd();
                ResetTimer(_roundTime);
            }
            if (StateMachine.Instance.NewTurn())
                ResetTimer(_roundTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _timerColour = Time > 3.5f ? Color.Yellow : Color.Red;

            if (Time != 0f)
                spriteBatch.DrawString(_timerFont, Time.ToString("0"), _timerPosition, _timerColour);
        }
    }
}
