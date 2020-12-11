using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WizardGrenade2
{
    public class DetonationTimer
    {
        public float Time { get; private set; }
        private SpriteFont _timerFont;
        private readonly string _fileName = @"Fonts/StatFont";

        public DetonationTimer(ContentManager contentManager)
        {
            _timerFont = contentManager.Load<SpriteFont>(_fileName);
        }

        public void UpdateTimer() => Time = WeaponManager.Instance.DetonationTime;
        public void SetTimer(int time) => Time = time;

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            Vector2 timerTextSize = _timerFont.MeasureString(Time.ToString("0"));

            if (Time > 0)
                spriteBatch.DrawString(_timerFont, Time.ToString("0"), position - timerTextSize / 2, Color.Black);
        }
    }
}
