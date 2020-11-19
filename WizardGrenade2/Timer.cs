using Microsoft.Xna.Framework;

namespace WizardGrenade2
{
    public class Timer
    {
        public bool IsRunning { get; protected set; }
        public float Time { get; protected set; }

        public Timer(float startTime)
        {
            Time = startTime;
            IsRunning = true;
        }

        public virtual void Update(GameTime gameTime)
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
    }
}
