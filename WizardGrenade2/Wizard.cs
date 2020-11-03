using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Schema;

namespace WizardGrenade2
{
    public class Wizard
    {
        private readonly string _baseFileName = "Wizard";
        private const int MASS = 100;
        private const int FRAMES_V = 1;
        private const int FRAMES_H = 1;
        private const int WALK_SPEED = 100;
        private const int COLLISION_POINTS = 15;
        private const bool CAN_ROTATE = false;
        private const float DAMPING_FACTOR = 0.5f;

        private int _health;

        private enum States
        { 
            Idle,
            Walking,
            Charging,
            Throwing,
            Jumping,
        }
        private States State;

        private enum Directions
        {
            None,
            Left,
            Right,
        }
        private Directions Direction;

        private GameObject _wizard;

        public Wizard(int skinNumber, Vector2 position, int startHealth)
        {
            GameObjectParameters parameters = new GameObjectParameters(_baseFileName + skinNumber,
                MASS, CAN_ROTATE, COLLISION_POINTS, DAMPING_FACTOR, FRAMES_H, FRAMES_V);

            _health = startHealth;
            _wizard = new GameObject(parameters, position);
        }

        public void LoadContent(ContentManager contentManager)
        {
            _wizard.LoadContent(contentManager);
        }

        public void Update(GameTime gameTime)
        {
            _wizard.Update(gameTime);
            //ResetMovement();
        }

        private void UpdateControl(GameTime gameTime)
        {

        }

        public void UpdateMovement(GameTime gameTime)
        {
            if (InputManager.IsKeyDown(Keys.Left) && State != States.Jumping)
                Walk(Directions.Left, -1, SpriteEffects.None, gameTime);
            else if (InputManager.IsKeyDown(Keys.Right) && State != States.Jumping)
                Walk(Directions.Right, 1, SpriteEffects.FlipHorizontally, gameTime);
        }

        private void Walk(Directions direction, int directionCoefficient, SpriteEffects effect, GameTime gameTime)
        {
            _wizard.ModifyVelocityX(directionCoefficient * WALK_SPEED);
            State = States.Walking;

            Direction = direction;
            _wizard.SetSpriteEffect(effect);
        }

        private void ResetMovement()
        {
            _wizard.ModifyVelocityX(0);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _wizard.Draw(spriteBatch);
        }

        public Vector2 GetPosition()
        {
            return _wizard.GetPosition();
        }

        public GameObject GetWizard()
        {
            return _wizard;
        }
    }
}
