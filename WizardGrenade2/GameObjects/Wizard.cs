using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WizardGrenade2
{
    public class Wizard
    {
        public Entity Entity { get; set; }
        public Vector2 Position { get => _wizard.Position; set => _wizard.Position = value; }
        public Rectangle SpriteRectangle { get => _wizard.SpriteRectangle; }
        public int Health { get => Entity.Health; }
        public int DirectionCoefficient { get => _direction == WizardSettings.Directions.Left ? -1 : 1; }
        public bool IsPlaced { get; set; }
        public bool IsActive { get; set; }
        public bool IsDead { get => Entity.IsDead; }
        public bool JustDied { get => Entity.JustDied; }

        private GameObject _wizard;
        private Timer _animationTimer = new Timer(6);
        private WizardSettings.States _state;
        private WizardSettings.Directions _direction;
        private WizardSettings.Directions _previousDirection;
        private WizardSettings.Directions _currentDirection;
        private int _animationCounter = 0;
        private int _jumpCounter = 0;

        public void AddVelocity(Vector2 velocity) => _wizard.AddVelocity(velocity);

        public Wizard(int skinNumber, Vector2 position, int startHealth)
        {
            GameObjectParameters parameters = WizardSettings.GetWizardParameters(skinNumber);
            Entity = new Entity(startHealth);
            _wizard = new GameObject(parameters, position);
            _wizard.SpriteColour = Colours.Transparent;
        }

        public void LoadContent(ContentManager contentManager)
        {
            _wizard.LoadContent(contentManager);
            _wizard.LoadAnimationContent(WizardSettings.animationStates);
        }

        public void Update(GameTime gameTime)
        {
            if (!(Position.Y > WizardSettings.DEATH_HEIGHT))
                _wizard.Update(gameTime);

            Entity.Update();
            CheckForDeath();
        }

        public void UpdateControl(GameTime gameTime)
        {
            Charge(gameTime);

            if (_state != WizardSettings.States.Charging)
            {
                UpdateMovement(gameTime);
                Jump(gameTime);
            }
        }

        public void UpdateMovement(GameTime gameTime)
        {
            DirectionChange();

            if (InputManager.IsKeyDown(Keys.Left))
                Walk(WizardSettings.Directions.Left, -1, SpriteEffects.None, gameTime);

            else if (InputManager.IsKeyDown(Keys.Right))
                Walk(WizardSettings.Directions.Right, 1, SpriteEffects.FlipHorizontally, gameTime);

            else if (_wizard.Velocity == Vector2.Zero)
                Idle(gameTime);
        }

        private void Walk(WizardSettings.Directions direction, int directionCoefficient, SpriteEffects effect, GameTime gameTime)
        {
            _state = WizardSettings.States.Walking;
            _direction = direction;

            if (!WasDirectionChanged)
                _wizard.ModifyVelocityX(directionCoefficient * WizardSettings.WALK_SPEED);

            _wizard.UpdateAnimationSequence("Walking", 10f, gameTime);
            _wizard.SpriteVisualEffect = effect;
        }

        private void DirectionChange()
        {
            _previousDirection = _currentDirection;
            _currentDirection = _direction;

        }
        private bool WasDirectionChanged { get => _previousDirection != _currentDirection; }

        private void Jump(GameTime gameTime)
        {
            if (InputManager.IsKeyDown(Keys.Enter))
                _wizard.UpdateAnimationSequence("Jumping", 10f, gameTime);

            if (InputManager.WasKeyReleased(Keys.Enter) && _jumpCounter < 5)
            {
                SoundManager.Instance.PlaySound("wizardJump");
                _state = WizardSettings.States.Jumping;
                _jumpCounter++;
                _wizard.ModifyVelocityY(- WizardSettings.JUMP_HEIGHT);
            }
        }

        private void Charge(GameTime gameTime)
        {
            if (WeaponManager.Instance.IsCharging)
            {
                _wizard.UpdateAnimationSequence("Charging", 4, gameTime);
                _state = WizardSettings.States.Charging;
            }
            else if (_wizard.Velocity == Vector2.Zero)
                _state = WizardSettings.States.Idle;
        }

        private void Idle(GameTime gameTime)
        {
            _jumpCounter = 0;
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
                _wizard.UpdateAnimationFrame("Idle", 0);

            // Check for true Idle state
            if (_wizard.Velocity == Vector2.Zero)
                _state = WizardSettings.States.Idle;
        }

        private void IdleLook(GameTime gameTime)
        {
           _wizard.UpdateAnimationSequence("Looking", 1, gameTime);
            if (!_animationTimer.IsRunning)
                _animationCounter = 0;
        }

        private void CheckForDeath()
        {
            if (Position.Y > ScreenSettings.TARGET_HEIGHT)
                Entity.Kill();

            if (Entity.JustDied)
            {
                SoundManager.Instance.PlaySound("wizardSad");
                SoundManager.Instance.PlaySound("stone1");
                _wizard.SpriteColour = Colours.WizardIsDeadColor;
                _wizard.DrawRotation = (Mechanics.PI / 2);
            }
        }

        public bool CheckPlacement()
        {
            bool isValidPlacement = CollisionManager.Instance.CheckObjectPlacement(_wizard.GetTransformedPolyPoints(Position));
            
            if (!isValidPlacement)
                _wizard.SpriteColour = Color.Black;
            else
                _wizard.SpriteColour = Colours.WizardPlacementColour;

            return isValidPlacement;
        }

        public void PlaceWizard(Vector2 position)
        {
            Position = position;
            IsPlaced = true;
            _wizard.SpriteColour = Color.White;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _wizard.Draw(spriteBatch);
        }
    }
}
