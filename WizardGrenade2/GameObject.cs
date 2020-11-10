using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace WizardGrenade2
{
    public class GameObject : Sprite
    {
        private readonly GameObjectParameters _parameters = new GameObjectParameters();
        private Space2D _realSpace = new Space2D();
        private Vector2 _acceleration = Vector2.Zero;
        private Space2D _potentialSpace = new Space2D();
        private Polygon _collisionPoints;
        private CollisionManager Collider = CollisionManager.Instance;
        public bool Collided { get; set; }

        public GameObject(GameObjectParameters inputParameters)
        {
            _parameters = inputParameters;
            _realSpace.velocity = Vector2.Zero;
        }

        public GameObject(GameObjectParameters inputParameters, Vector2 position)
            : this (inputParameters)
        {
            _realSpace.position = position;
        }

        public void LoadContent(ContentManager contentManager)
        {
            // Check if animated object and call relevant method from Sprite class
            if (_parameters.framesH == 0 || _parameters.framesV == 0)
                LoadContent(contentManager, _parameters.fileName);
            else
                LoadContent(contentManager, _parameters.fileName, _parameters.framesH, _parameters.framesV);

            _collisionPoints = new Polygon(GetSpriteRectangle(), _parameters.numberOfCollisionPoints);
            _collisionPoints.LoadPolyContent(contentManager);
        }

        public void Update(GameTime gameTime)
        {
            _acceleration += Mechanics.ApplyGravity(_parameters.mass);
            _realSpace.velocity += _acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
            _acceleration = Vector2.Zero;
            UpdatePotentialSpace(gameTime);
            ResolveCollisions(gameTime);
        }

        private void UpdatePotentialSpace(GameTime gameTime)
        {
            _potentialSpace.position = _realSpace.position + _realSpace.velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            _potentialSpace.rotation = CalculateRotation(_realSpace.velocity);
            _collisionPoints.UpdateCollisionPoints(_potentialSpace.position, _potentialSpace.rotation);
        }

        private void UpdateRealSpace(GameTime gameTime)
        {
            _realSpace.position += _realSpace.velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            _realSpace.rotation = CalculateRotation(_realSpace.velocity);
        }

        private void ResolveCollisions(GameTime gameTime)
        {
            // Check for a collision in potential space by evaluating possible reflection vector
            List<Vector2> collidingPoints = Collider.CheckCollision(_collisionPoints.transformedPolyPoints);

            if (collidingPoints.Count > 0)
            {
                Collided = true;
                Vector2 reflectionVector = Collider.ResolveCollision
                    (collidingPoints, _potentialSpace.position, _realSpace.velocity);

                // If colliding in potential space then update position with damped reflection vector
                _realSpace.velocity = ApplyDamping(reflectionVector, _parameters.dampingFactor);
                UpdateRealSpace(gameTime);
                // Update collision points from potential position to real position
                _collisionPoints.UpdateCollisionPoints(_realSpace.position, _realSpace.rotation);

                // Perform second check to see if still colliding in real space
                if (Collider.CheckCollision(_collisionPoints.transformedPolyPoints).Count != 0)
                {
                    // If still colliding update position along reflection vector without damping
                    _realSpace.velocity = reflectionVector;
                    UpdateRealSpace(gameTime);
                }
            }
            else
            {
                // If no collision, set real position to potential position
                //_realSpace.position = _potentialSpace.position;
                UpdateRealSpace(gameTime);
            }
        }

        private Vector2 ApplyDamping(Vector2 velocity, float dampingFactor)
        {
            Vector2 dampedVelocity = velocity *= dampingFactor;
            return (Mechanics.VectorMagnitude(dampedVelocity) < 20f) ? Vector2.Zero : dampedVelocity;
        }

        public void AddDisplacementVelocity(GameTime gameTime, Vector2 deltaVelocity)
        {
            _realSpace.velocity += deltaVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        private float CalculateRotation(Vector2 velocity) => _parameters.CanRotate ? Mechanics.CalculateRotation(velocity) : 0f;
        public void AddVelocity(Vector2 deltaVelocity) => _realSpace.velocity += deltaVelocity;
        public void AddRotation(float rotation) => _realSpace.rotation += rotation;
        public Vector2 GetVelocity() => _realSpace.velocity;
        public Vector2 GetPosition() => _realSpace.position;
        public void SetVelocity(Vector2 velocity) => _realSpace.velocity = velocity;
        public void SetPosition(Vector2 position) => _realSpace.position = position;
        public void ModifyVelocityX(float xVelocity) => _realSpace.velocity.X = xVelocity;
        public void ModifyVelocityY(float yVelocity) => _realSpace.velocity.Y = yVelocity;

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawSprite(spriteBatch, _realSpace.position, _realSpace.rotation);
            //_collisionPoints.DrawCollisionPoints(spriteBatch, _realSpace.position);
        }
    }
}
