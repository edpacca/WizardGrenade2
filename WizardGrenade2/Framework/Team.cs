using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace WizardGrenade2
{
    public class Team
    {
        public List<Wizard> Wizards { get; private set; } = new List<Wizard>();
        public bool IsTeamOut { get; private set; }
        public int ActiveWizard { get; private set; }

        private SpriteFont _spriteFont;
        private Vector2 _healthTextOffset = new Vector2(0, 36);
        private int _teamSize;

        public Team(int teamNumber, int teamSize, int wizardHealth)
        {
            _teamSize = teamSize;
            ActiveWizard = 0;

            for (int i = 0; i < _teamSize; i++)
                Wizards.Add(new Wizard(teamNumber, new Vector2(0, -100), wizardHealth));
        }

        public void LoadContent(ContentManager contentManager)
        {
            _spriteFont = contentManager.Load<SpriteFont>(@"Fonts/WizardHealthFont");

            foreach (var wizard in Wizards)
                wizard.LoadContent(contentManager);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var wizard in Wizards)
                wizard.Update(gameTime);

            if (Wizards[ActiveWizard].JustDied)
                StateMachine.Instance.ForceTurnEnd();
        }

        public void NextActiveWizard()
        {
            if (IsTeamOut)
                return;

            ActiveWizard = Utility.WrapAroundCounter(ActiveWizard, _teamSize);

            if (Wizards[ActiveWizard].IsDead)
                NextActiveWizard();
        }

        public int GetTeamHealth()
        {
            int teamHealth = 0;

            foreach (var wizard in Wizards)
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
            foreach (var wizard in Wizards)
            {
                if (!wizard.IsActive && !wizard.IsDead)
                    DrawHealth(spriteBatch, wizard);

                wizard.Draw(spriteBatch);
            }
        }
    }
}
