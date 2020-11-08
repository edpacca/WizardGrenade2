using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;


namespace WizardGrenade2
{
    class HealthBar : Sprite
    {
        private string _fileName;
        private int _startTeamHealth;
        private int _displayedTeamHealth;
        private Vector2 _healthBarPosition;
        private int _framesH = 1;
        private int _framesV = 6;

        private Dictionary<string, int[]> _animationState = new Dictionary<string, int[]>
        {
            ["bar"] = new int[] { 0, 1, 2, 3, 4, 5, 4, 3, 2, 1 }
        };

        public HealthBar(int teamNumber, int startTeamHealth)
        {
            _fileName = "HealthBar";
            _startTeamHealth = startTeamHealth;
            _displayedTeamHealth = _startTeamHealth;
            _healthBarPosition = new Vector2((WGGame.GetScreenWidth / 2), WGGame.GetScreenHeight - 20 - (15 * teamNumber));
        }

        public void LoadContent(ContentManager contentManager)
        {
            LoadContent(contentManager, _fileName, _framesH, _framesV);
            _healthBarPosition -= GetSpriteOrigin();
            LoadAnimationContent(_animationState);
        }

        public void Update(GameTime gameTime, int actualTeamHealth)
        {
            UpdateAnimationStateV("bar", 5, gameTime);
            SmoothUpdateHealthBar(gameTime, actualTeamHealth);
            float healthPercentage = 1 - ((float)_displayedTeamHealth / (float)_startTeamHealth);
            SpriteRectangleMaskX(healthPercentage);
        }

        private void SmoothUpdateHealthBar(GameTime gameTime, int actualTeamHealth)
        {
            if (_displayedTeamHealth > actualTeamHealth)
                _displayedTeamHealth -= (int)(gameTime.ElapsedGameTime.TotalSeconds * 100);
            else
                _displayedTeamHealth = actualTeamHealth;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawSprite(spriteBatch, _healthBarPosition);
        }
    }
}
