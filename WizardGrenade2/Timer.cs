using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardGrenade2
{
    class Timer
    {
        private bool isRunning = true;
        private float _timer;
        private SpriteFont _timerFont;

        public bool IsRunning { get => isRunning; }

        public Timer(float startTime)
        {
            _timer = startTime;
        }

        public void LoadContent(ContentManager contentManager)
        {
            _timerFont = contentManager.Load<SpriteFont>("TimerFont");
        }

        public void Update(GameTime gameTime)
        {
            if (IsRunning)
                _timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_timer <= 0)
                isRunning = false;
        }

        public void ResetTimer(float resetTime)
        {
            _timer = resetTime;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_timerFont, _timer.ToString("0"), new Vector2(10, 600), Color.Yellow);
        }
    }
}
