using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace WizardGrenade2
{
    class UserInterface
    {
        private Sprite _cursor;
        private Timer _timer;
        private List<HealthBar> _healthBars;
        private int _numberOfTeams;
        private int _teamStartHealth;
        private const float ROUND_TIME = 60f;

        public UserInterface(GameOptions gameOptions)
        {
            _numberOfTeams = gameOptions.NumberOfTeams;
            _teamStartHealth = gameOptions.TeamSize * gameOptions.WizardHealth;
        }

        public void LoadContent(ContentManager contentManager)
        {
            _cursor = new Sprite(contentManager, "Cursor");
            _timer = new Timer(ROUND_TIME);
            _timer.LoadContent(contentManager);

            _healthBars = new List<HealthBar>();
            for (int i = 0; i < _numberOfTeams; i++)
            {
                _healthBars.Add(new HealthBar(i, _teamStartHealth));
                _healthBars[i].LoadContent(contentManager);
            }
        }

        public void Update(GameTime gameTime, int[] teamHealths)
        {
            _timer.Update(gameTime);

            for (int i = 0; i < _numberOfTeams; i++)
            {
                _healthBars[i].Update(gameTime, teamHealths[i]);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _timer.Draw(spriteBatch);
            foreach (var healthBar in _healthBars)
                healthBar.Draw(spriteBatch);

            _cursor.DrawSprite(spriteBatch, InputManager.CursorPosition());
        }
    }
}
