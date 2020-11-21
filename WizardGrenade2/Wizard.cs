using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace WizardGrenade2
{
    public class Wizard
    {
        public Entity entity;
        private GameObject _wizard;
        private Timer _animationTimer = new Timer(6);
        private int _animationCounter = 0;
        public bool IsActive { get; set; }
        public bool IsDead { get => entity.IsDead; }
        public int Health { get => entity.Health; }
        public bool IsPlaced { get; set; }

        public Vector2 Position { get => _wizard.GetPosition(); set => _wizard.SetPosition(value); }
        public Rectangle GetSpriteRectangle() => _wizard.GetSpriteRectangle();
        public int DirectionCoefficient { get => Direction == Directions.Left ? -1 : 1; }

        public void AddVelocity(Vector2 velocity) => _wizard.AddVelocity(velocity);

        private enum States { Idle, Walking, Charging, Firing, Jumping, Weak, }
        private enum Directions { None, Left, Right, }
        private States State;
        private Directions Direction;

        public Wizard(int skinNumber, Vector2 position, int startHealth)
        {
            GameObjectParameters parameters = WizardSettings.GetWizardParameters(skinNumber);
            entity = new Entity(startHealth);
            _wizard = new GameObject(parameters, position);
        }

        public void LoadContent(ContentManager contentManager)
        {
            _wizard.LoadContent(contentManager);
            _wizard.LoadAnimationContent(WizardSettings.animationStates);
        }

        public void Update(GameTime gameTime)
        {
            if (!(Position.Y > ScreenSettings.TARGET_HEIGHT))
                _wizard.Update(gameTime);

            Die();
        }

        public void UpdateControl(GameTime gameTime)
        {
            Charge(gameTime);

            if (State != States.Charging)
            {
                UpdateMovement(gameTime);

                if (State != States.Walking && State != States.Jumping)
                    Jump(gameTime);
            }
        }

        public void UpdateMovement(GameTime gameTime)
        {
            if (InputManager.IsKeyDown(Keys.Left) && State != States.Jumping)
                Walk(Directions.Left, -1, SpriteEffects.None, gameTime);

            else if (InputManager.IsKeyDown(Keys.Right) && State != States.Jumping)
                Walk(Directions.Right, 1, SpriteEffects.FlipHorizontally, gameTime);

            else
                Idle(gameTime);
        }

        private void Walk(Directions direction, int directionCoefficient, SpriteEffects effect, GameTime gameTime)
        {
            _wizard.ModifyVelocityX(directionCoefficient * WizardSettings.WALK_SPEED);
            State = States.Walking;
            _wizard.UpdateAnimationSequence("Walking", 10f, gameTime);
            Direction = direction;
            _wizard.SpriteVisualEffect = effect;
        }

        private void Jump(GameTime gameTime)
        {
            if (InputManager.IsKeyDown(Keys.Enter))
                _wizard.UpdateAnimationSequence("Jumping", 10f, gameTime);

            if (InputManager.WasKeyReleased(Keys.Enter) && State != States.Jumping)
            {
                State = States.Jumping;
                _wizard.ModifyVelocityY(- WizardSettings.JUMP_HEIGHT);
            }
        }

        private void Charge(GameTime gameTime)
        {
            if (WeaponManager.Instance.IsCharging)
            {
                _wizard.UpdateAnimationSequence("Charging", 4, gameTime);
                State = States.Charging;
            }
            else
                State = States.Idle;
        }

        private void Idle(GameTime gameTime)
        {
            _animationTimer.Update(gameTime);

            // Periodically shows looking around animation
            if (_animationCounter > WizardSettings.BLINK_CYCLES)
            {
                if (_animationCounter == WizardSettings.BLINK_CYCLES)
                    _animationTimer.ResetTimer(0.5f);

                IdleLook(gameTime);
            }
            // More frequently shows blinking animation
            else if (_animationTimer.Time <= 1f)
            {
                _wizard.UpdateAnimationSequence("Idle", 4f, gameTime);

                if (!_animationTimer.IsRunning)
                {
                    _animationTimer.ResetTimer(6);
                    _animationCounter++;
                }
            }
            // Otherwise shows Idle frame
            else
            {
                _wizard.UpdateAnimationFrame("Idle", 0);
            }

            // Check for true Idle state
            if (_wizard.GetVelocity() == Vector2.Zero)
            {
                State = States.Idle;
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

        private void Die()
        {
            if (Position.Y > ScreenSettings.TARGET_HEIGHT)
                entity.Kill();

            if (entity.IsDead)
            {
                _wizard.SpriteColour = Color.Red;
                _wizard.AddRotation(Mechanics.PI / 2);
            }
        }

        public bool CheckPlacement()
        {
            bool isValidPlacement = CollisionManager.Instance.CheckObjectPlacement(_wizard.GetTransformedPolyPoints(Position));
            if (!isValidPlacement)
                _wizard.SpriteColour = Color.Black;
            else
                _wizard.SpriteColour = Color.White;

            return isValidPlacement;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _wizard.Draw(spriteBatch);
        }
    }
}
