using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
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
        private int _framesV = 7;
        private readonly string _teamName;
        private Vector2 _teamNameOffset = new Vector2(-45, -2);
        private SpriteFont _spriteFont;

        private Dictionary<string, int[]> _animationState = new Dictionary<string, int[]>
        {
            ["bar"] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 3, 4, 5 }
        };

        public HealthBar(int teamNumber, int startTeamHealth, string teamName)
        {
            _fileName = "HealthBar" + teamNumber;
            _teamName = teamName;
            _startTeamHealth = startTeamHealth;
            _displayedTeamHealth = _startTeamHealth;
            _healthBarPosition = new Vector2(ScreenSettings.CentreScreenWidth, ScreenSettings.TARGET_HEIGHT -30 - (15 * teamNumber));
        }

        public void LoadContent(ContentManager contentManager)
        {
            LoadContent(contentManager, _fileName, _framesH, _framesV);
            _healthBarPosition -= GetSpriteOrigin();
            LoadAnimationContent(_animationState);
            _spriteFont = contentManager.Load<SpriteFont>("WizardHealthFont");
        }

        public void Update(GameTime gameTime, int actualTeamHealth)
        {
            UpdateAnimationSequence("bar", 10, gameTime);
            SmoothUpdateHealthBar(gameTime, actualTeamHealth);
            float healthPercentage = 1 - ((float)_displayedTeamHealth / (float)_startTeamHealth);
            MaskSpriteRectangleWidth(healthPercentage);
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
            spriteBatch.DrawString(_spriteFont, _teamName, _healthBarPosition + _teamNameOffset, Color.White);
        }
    }
}
