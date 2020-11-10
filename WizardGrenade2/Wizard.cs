using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Resources;

namespace WizardGrenade2
{
    public class Wizard
    {
        private readonly string _baseFileName = "WizardSpritesheet";
        private const int MASS = 100;
        private const int FRAMES_V = 1;
        private const int FRAMES_H = 15;
        private const int WALK_SPEED = 100;
        private const int COLLISION_POINTS = 15;
        private const bool CAN_ROTATE = false;
        private const float DAMPING_FACTOR = 0.5f;
        private const int JUMP_HEIGHT = 200;
        public bool isActive { get; set; }
        public Entity entity;
        private GameObject _wizard;
        private Timer _animationTimer = new Timer(6);
        private int _animationCounter = 0;
        private const int BLINK_CYCLES = 4;

        private Dictionary<string, int[]> _animationStates = new Dictionary<string, int[]>()
        {
            ["Idle"] = new int[] { 0, 12 },
            ["Looking"] = new int[] { 13, 14 },
            ["Walking"] = new int[] { 1, 2 },
            ["Charging"] = new int[] { 3, 4, 5, 6, 7, 8, 7, 8, 7, 8 },
            ["Firing"] = new int[] { 9 },
            ["Weak"] = new int[] { 10 },
            ["Jumping"] = new int[] { 11 },
        };

        private enum States
        { 
            Idle,
            Walking,
            Charging,
            Firing,
            Jumping,
            Weak,
        }
        private States State;

        private enum Directions
        {
            None,
            Left,
            Right,
        }
        private Directions Direction;

        public Wizard(int skinNumber, Vector2 position, int startHealth)
        {
            GameObjectParameters parameters = new GameObjectParameters(_baseFileName + skinNumber,
                MASS, CAN_ROTATE, COLLISION_POINTS, DAMPING_FACTOR, FRAMES_H, FRAMES_V);

            entity = new Entity(startHealth);
            _wizard = new GameObject(parameters, position);
        }

        public void LoadContent(ContentManager contentManager)
        {
            _wizard.LoadContent(contentManager);
            _wizard.LoadAnimationContent(_animationStates);
        }

        public void Update(GameTime gameTime)
        {
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
            {
                Idle(gameTime);
            }
        }

        private void Walk(Directions direction, int directionCoefficient, SpriteEffects effect, GameTime gameTime)
        {
            _wizard.ModifyVelocityX(directionCoefficient * WALK_SPEED);
            State = States.Walking;
            _wizard.UpdateAnimationSequence("Walking", 10f, gameTime);
            Direction = direction;
            _wizard.SetSpriteEffect(effect);
        }

        private void Jump(GameTime gameTime)
        {
            if (InputManager.IsKeyDown(Keys.Enter))
                _wizard.UpdateAnimationSequence("Jumping", 10f, gameTime);

            if (InputManager.WasKeyReleased(Keys.Enter))
            {
                State = States.Jumping;
                _wizard.ModifyVelocityY(-JUMP_HEIGHT);
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
            if (_animationCounter > BLINK_CYCLES)
            {
                if (_animationCounter == BLINK_CYCLES)
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
            if (entity.IsDead)
            {
                _wizard.SetColour(Color.Red);
                _wizard.AddRotation(Mechanics.PI / 2);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _wizard.Draw(spriteBatch);
        }

        public Vector2 GetPosition() => _wizard.GetPosition();
        public int GetDirection() => Direction == Directions.Left ? -1 : 1;
        public int GetHealth() => entity.Health;
        public bool IsDead() => entity.IsDead;
        public Wizard GetWizard() => this;
        public Rectangle GetSpriteRectangle() => _wizard.GetSpriteRectangle();
        public void AddVelocity(Vector2 velocity) => _wizard.AddVelocity(velocity);
        
    }
}
