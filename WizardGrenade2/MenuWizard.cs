using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace WizardGrenade2
{
    class MenuWizard
    {
        private List<Sprite> _wizards = new List<Sprite>();
        private Timer _animationTimer = new Timer(6f);
        private int _animationCounter = 0;
        private const float SCALE = 8f;
        private int _activeWizard = 2;
        private int _nextWizard;

        private const float WALK_TIME = 3f;
        private const float ONSCREEN_TIME = 12f;
        private const float WIZARD_CYCLE_TIME = ONSCREEN_TIME + WALK_TIME;
        private const float X_DIFFERENCE = 400f;
        private float _walkingSpeed = X_DIFFERENCE / (WIZARD_CYCLE_TIME - ONSCREEN_TIME);

        private Timer _wizardTimer = new Timer(WIZARD_CYCLE_TIME);
        private float _nextWizardXOffset = -X_DIFFERENCE;
        private float _activeWizardXOffset = 0;
 

        public MenuWizard(ContentManager contentManager)
        {
            _nextWizard = Utility.WrapAroundCounter(_activeWizard, 4);
            string fileName;

            for (int i = 0; i < 4; i++)
            {
                fileName = "WizardSpritesheet" + i;
                _wizards.Add(new Sprite());
                _wizards[i].LoadContent(contentManager, fileName, WizardSettings.FRAMES_H, WizardSettings.FRAMES_V);
                _wizards[i].LoadAnimationContent(WizardSettings.animationStates);
                _wizards[i].SpriteScale = SCALE;
            }
        }

        public void Update(GameTime gameTime)
        {
            _wizardTimer.Update(gameTime);
            WizardAnimationCycle(gameTime);

            if (_nextWizardXOffset >= 0)
            {
                _nextWizardXOffset = 0;
                CycleActiveWizard();
            }
        }

        private void WizardAnimationCycle(GameTime gameTime)
        {
            if (_wizardTimer.Time > WALK_TIME)
                Idle(gameTime);
            else
                Walking(gameTime);
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
                _wizards[_activeWizard].UpdateAnimationSequence("Idle", 4f, gameTime);

                if (!_animationTimer.IsRunning)
                {
                    _animationTimer.ResetTimer(6);
                    _animationCounter++;
                }
            }
            else
            {
                _wizards[_activeWizard].UpdateAnimationFrame("Idle", 0);
            }
        }

        private void IdleLook(GameTime gameTime)
        {
            _wizards[_activeWizard].UpdateAnimationSequence("Looking", 1, gameTime);
            if (!_animationTimer.IsRunning)
            {
                _animationCounter = 0;
            }
        }

        private void Walking(GameTime gameTime)
        {
            _activeWizardXOffset -= _walkingSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            _nextWizardXOffset += _walkingSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            _wizards[_activeWizard].UpdateAnimationSequence("Walking", 10, gameTime);
            _wizards[_nextWizard].UpdateAnimationSequence("Walking", 10, gameTime);
        }

        private void CycleActiveWizard()
        {
            _activeWizard = Utility.WrapAroundCounter(_activeWizard, 4);
            _nextWizard = Utility.WrapAroundCounter(_activeWizard, 4);
            _wizardTimer.ResetTimer(WIZARD_CYCLE_TIME);
            _nextWizardXOffset = -X_DIFFERENCE;
            _activeWizardXOffset = 0;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
                Vector2 activeWizardPosition = new Vector2(position.X + _activeWizardXOffset, position.Y);
                Vector2 nextWizardPosition = new Vector2(position.X + _nextWizardXOffset, position.Y);

                _wizards[_activeWizard].DrawSprite(spriteBatch, activeWizardPosition);
                _wizards[_nextWizard].DrawSprite(spriteBatch, nextWizardPosition);
        }
    }
}
