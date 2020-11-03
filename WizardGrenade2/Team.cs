using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WizardGrenade2
{
    class Team
    {
        private readonly string _teamName;
        public List<Wizard> _wizards = new List<Wizard>();

        private bool _isActiveTeam;
        private int _teamHealth;

        public Team(int teamNumber, string teamName, int teamSize, int wizardHealth)
        {
            _teamName = teamName;
            _teamHealth = wizardHealth * teamSize;

            for (int i = 0; i < teamSize; i++)
            {
                _wizards.Add(new Wizard(0 /*change when skins are included*/, new Vector2((100 + i * 100) + teamNumber * 300, 300), wizardHealth));
            }
        }

        public void LoadContent(ContentManager contentManager)
        {
            foreach (var wizard in _wizards)
                wizard.LoadContent(contentManager);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var wizard in _wizards)
                wizard.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var wizard in _wizards)
                wizard.Draw(spriteBatch);
        }


    }
}
