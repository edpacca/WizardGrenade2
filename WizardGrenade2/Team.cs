using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace WizardGrenade2
{
    class Team
    {
        public string TeamName { get; private set; }
        public List<Wizard> _wizards = new List<Wizard>();
        private Vector2 _healthTextOffset = new Vector2(10, 36);
        private SpriteFont _spriteFont;
        public bool IsTeamOut { get; private set; }

        public Team(int teamNumber, string teamName, int teamSize, int wizardHealth)
        {
            TeamName = teamName;

            for (int i = 0; i < teamSize; i++)
            {
                 _wizards.Add(new Wizard(teamNumber, new Vector2(teamNumber * 100, 0), wizardHealth));
            }
        }

        public void LoadContent(ContentManager contentManager)
        {
            _spriteFont = contentManager.Load<SpriteFont>("WizardHealthFont");

            foreach (var wizard in _wizards)
                wizard.LoadContent(contentManager);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var wizard in _wizards)
                wizard.Update(gameTime);
        }

        public int GetTeamHealth()
        {
            int teamHealth = 0;

            foreach (var wizard in _wizards)
                teamHealth += wizard.Health;

            if (teamHealth == 0)
                IsTeamOut = true;

            return teamHealth;
        }

        public void DrawHealth(SpriteBatch spriteBatch, Wizard wizard)
        {
           spriteBatch.DrawString(_spriteFont, wizard.Health.ToString(), wizard.Position - _healthTextOffset, Color.White);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var wizard in _wizards)
            {
                if (!wizard.IsActive && !wizard.IsDead)
                    DrawHealth(spriteBatch, wizard);

                wizard.Draw(spriteBatch);
            }
        }
    }
}
