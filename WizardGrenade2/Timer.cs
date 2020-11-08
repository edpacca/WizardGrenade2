using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WizardGrenade2
{
    public class Timer
    {
        private SpriteFont _timerFont;

        public bool IsRunning { get; private set; }
        public float Time { get; private set; }

        public Timer(float startTime)
        {
            Time = startTime;
            IsRunning = true;
        }

        public void LoadContent(ContentManager contentManager)
        {
            _timerFont = contentManager.Load<SpriteFont>("TimerFont");
        }

        public void Update(GameTime gameTime)
        {
            if (IsRunning)
                Time -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Time <= 0)
                IsRunning = false;
        }

        public void ResetTimer(float resetTime)
        {
            Time = resetTime;
            IsRunning = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_timerFont, Time.ToString("0"), new Vector2(10, 600), Color.Yellow);
        }
    }
}
