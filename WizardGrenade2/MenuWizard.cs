using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;


namespace WizardGrenade2
{
    class MenuWizard
    {
        private Sprite _wizard;
        private Timer _animationTimer = new Timer(6);
        private int _animationCounter = 0;
        private const float SCALE = 8f;

        public MenuWizard(int fileNumber, ContentManager contentManager)
        {
            string fileName = (fileNumber >= 0 && fileNumber < 4) ? "WizardSpritesheet" + fileNumber : "WizardSpritesheet0";
            _wizard = new Sprite();
            _wizard.LoadContent(contentManager, fileName, WizardSettings.FRAMES_H, WizardSettings.FRAMES_V);
            _wizard.LoadAnimationContent(WizardSettings.animationStates);
            _wizard.SpriteScale = SCALE;
        }

        public void Update(GameTime gameTime)
        {
            Idle(gameTime);
        }

        private void Idle(GameTime gameTime)
        {
            _animationTimer.Update(gameTime);

            if (_animationCounter > 2)
            {
                if (_animationCounter == WizardSettings.BLINK_CYCLES)
                    _animationTimer.ResetTimer(0.5f);

                IdleLook(gameTime);
            }

            else if (_animationTimer.Time <= 1f)
            {
                _wizard.UpdateAnimationSequence("Idle", 4f, gameTime);

                if (!_animationTimer.IsRunning)
                {
                    _animationTimer.ResetTimer(6);
                    _animationCounter++;
                }
            }
            else
            {
                _wizard.UpdateAnimationFrame("Idle", 0);
            }
        }

        private void IdleLook(GameTime gameTime)
        {
            _wizard.UpdateAnimationSequence("Looking", 1, gameTime);
            if (!_animationTimer.IsRunning)
            {
                _animationCounter = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            _wizard.DrawSprite(spriteBatch, position);
        }
    }
}
