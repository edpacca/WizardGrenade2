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
        public List<Wizard> wizards = new List<Wizard>();
        public bool IsTeamOut { get; private set; }
        public int ActiveWizard { get; private set; }
        public int TeamSize { get; }
        private Vector2 _healthTextOffset = new Vector2(0, 36);
        private SpriteFont _spriteFont;

        public Team(int teamNumber, int teamSize, int wizardHealth)
        {
            TeamSize = teamSize;

            for (int i = 0; i < TeamSize; i++)
                wizards.Add(new Wizard(teamNumber, new Vector2(0, -100), wizardHealth));

            ActiveWizard = 0;
        }

        public void LoadContent(ContentManager contentManager)
        {
            _spriteFont = contentManager.Load<SpriteFont>("WizardHealthFont");

            foreach (var wizard in wizards)
                wizard.LoadContent(contentManager);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var wizard in wizards)
                wizard.Update(gameTime);

            if (wizards[ActiveWizard].JustDied)
                StateMachine.Instance.ForceTurnEnd();
        }

        public void NextActiveWizard()
        {
            if (IsTeamOut)
                return;

            ActiveWizard = Utility.WrapAroundCounter(ActiveWizard, TeamSize);

            if (wizards[ActiveWizard].IsDead)
                NextActiveWizard();
        }

        public int GetTeamHealth()
        {
            int teamHealth = 0;

            foreach (var wizard in wizards)
                teamHealth += wizard.Health;

            if (teamHealth == 0)
                IsTeamOut = true;

            return teamHealth;
        }

        public void DrawHealth(SpriteBatch spriteBatch, Wizard wizard)
        {
            Vector2 healthTextSize = _spriteFont.MeasureString(wizard.Health.ToString()) / 2;
           spriteBatch.DrawString(_spriteFont, wizard.Health.ToString(), wizard.Position - _healthTextOffset - healthTextSize, Color.White);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var wizard in wizards)
            {
                if (!wizard.IsActive && !wizard.IsDead)
                    DrawHealth(spriteBatch, wizard);

                wizard.Draw(spriteBatch);
            }
        }
    }
}
