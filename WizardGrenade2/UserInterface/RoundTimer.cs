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
        private Vector2 _offset = new Vector2(1, 2);
        private const float TIMER_INSET_POSITION = 30f;
        private float _roundTime;

        private Sprite _hourglass;
        private Sprite _sandTop;
        private Sprite _sandBottom;
        private Vector2 _hourglassPosition;
        private float _sandBottomYOffset;
        private float _sandTopYOffset;
        private int _spriteHeight;
        private float _percentage;
        private const float TOP_PERCENT = 0.6f;
        private const float BOTTOM_PERCENT = 0.53f;
        private float _bottomPercentage;
        private float _topPercentage;
        private bool _timerReversing;

        public RoundTimer(float startTime) : base(startTime)
        {
            _roundTime = startTime + 0.5f;
        }

        public void LoadContent(ContentManager contentManager)
        {
            _timerFont = contentManager.Load<SpriteFont>("TimerFont");
            _hourglass = new Sprite(contentManager, "Timer");
            _sandBottom = new Sprite(contentManager, "SandBottom");
            _sandTop = new Sprite(contentManager, "SandTop");
            _timeTextSize = _timerFont.MeasureString(Time.ToString("0"));
            _timerPosition = new Vector2(TIMER_INSET_POSITION + (_hourglass.GetSpriteRectangle().Width / 2), ScreenSettings.TARGET_HEIGHT - (_timeTextSize.Y + TIMER_INSET_POSITION + 48));
            _hourglassPosition = new Vector2(TIMER_INSET_POSITION, ScreenSettings.TARGET_HEIGHT - (_hourglass.GetSpriteRectangle().Height + TIMER_INSET_POSITION));
            _spriteHeight = _sandBottom.GetSpriteTexture().Height;
        }

        public override void Update(GameTime gameTime)
        {
            if (StateMachine.Instance.GameState == StateMachine.GameStates.PlayerTurn)
            {
                base.Update(gameTime);
                RunSandTimerForward();
            }
            else if (StateMachine.Instance.GameState == StateMachine.GameStates.BetweenTurns)
            {
                ReverseSandTimer(gameTime);
            }

            if (!IsRunning)
            {
                StateMachine.Instance.ForceTurnEnd();
                ResetTimer(_roundTime);
            }

            if (StateMachine.Instance.NewTurn())
            {
                ResetTimer(_roundTime);
            }
        }

        private void RunSandTimerForward()
        {
            _percentage = Time / _roundTime;
            RunSandTimer();
            _timerReversing = false;
        }

        private void RunSandTimer()
        {
            // Percentage of time remaining 100 to 0;
            _bottomPercentage = Utility.FractionPercentage(BOTTOM_PERCENT, _percentage, 1);
            _topPercentage = Utility.FractionPercentage(TOP_PERCENT, _percentage, -1);

            _sandBottom.MaskSpriteFromBottom(_bottomPercentage);
            _sandBottomYOffset = (int)(_bottomPercentage * _spriteHeight);

            _sandTop.MaskSpriteFromTop(_topPercentage);
            _sandTopYOffset = (int)(_topPercentage * _spriteHeight);
        }

        private void ReverseSandTimer(GameTime gameTime)
        {
            if (!_timerReversing)
                SoundManager.Instance.PlaySoundInstance("magic0");
            
            _timerReversing = true;

            if (_percentage < 1)
                _percentage += (float)gameTime.ElapsedGameTime.TotalSeconds / 2.5f;
            RunSandTimer();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _sandBottom.DrawSprite(spriteBatch, new Vector2(_hourglassPosition.X, _hourglassPosition.Y + _sandBottomYOffset));
            _sandTop.DrawSprite(spriteBatch, new Vector2(_hourglassPosition.X, _hourglassPosition.Y + _sandTopYOffset));
            _hourglass.DrawSprite(spriteBatch, _hourglassPosition);

            _timerColour = Time > 3.5f ? Colours.Gold : Color.Red;

            Vector2 fontSize = _timerFont.MeasureString(Time.ToString("0")) / 2;

            if (StateMachine.Instance.GameState == StateMachine.GameStates.PlayerTurn && Time > 0.5f)
            {
                spriteBatch.DrawString(_timerFont, Time.ToString("0"), _timerPosition - fontSize - _offset, Colours.Ink);
                spriteBatch.DrawString(_timerFont, Time.ToString("0"), _timerPosition - fontSize, _timerColour);
            }
        }
    }
}
