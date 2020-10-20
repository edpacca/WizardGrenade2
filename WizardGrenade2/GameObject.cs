using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace WizardGrenade2
{
    public class GameObject : Sprite
    {
        private string _fileName;
        private int _framesH;
        private int _framesV;
        private float _mass;
        private int _numberOfCollisionPoints;
        private bool _canRotate;
        private float _dampingFactor;

        private Mechanics.Space2D RealSpace;
        private Mechanics.Space2D PotentialSpace;
        private Polygon _collisionPoints;
        private CollisionManager Collider = CollisionManager.Instance;

        public GameObject(string fileName)
        {
            _fileName = fileName;
        }

        public GameObject(ContentManager contentManager, string fileName, int numberOfCollisionPoints)
            : this (fileName)
        {
            _numberOfCollisionPoints = numberOfCollisionPoints;
            LoadContent(contentManager);
        }

        public GameObject(string fileName, int framesH, int framesV, 
            Vector2 position, float mass, float dampingFactor, int numberOfCollisionPoints, bool canRotate)
        {
            _framesH = framesH;
            _framesV = framesV;
            _fileName = fileName;
            _numberOfCollisionPoints = numberOfCollisionPoints;
            _canRotate = canRotate;

            RealSpace.position = position;
            RealSpace.velocity = Vector2.Zero;
            RealSpace.rotation = 0f;
            _mass = mass;
            _dampingFactor = dampingFactor;
        }

        public void LoadContent(ContentManager contentManager)
        {
            if (_framesH == 0 || _framesV == 0)
                LoadContent(contentManager, _fileName);
            else
                LoadContent(contentManager, _fileName, _framesH, _framesV);

            _collisionPoints = new Polygon(GetSpriteTexture(), _numberOfCollisionPoints);
            _collisionPoints.LoadPolyContent(contentManager);
        }

        public void Update(GameTime gameTime)
        {
            RealSpace.velocity += Mechanics.ApplyGravity(_mass) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            UpdatePotentialSpace(gameTime);
            ResolveCollisions(gameTime);
        }

        private void UpdatePotentialSpace(GameTime gameTime)
        {
            PotentialSpace.position = RealSpace.position + RealSpace.velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            PotentialSpace.rotation = ApplyRotation(RealSpace.velocity);
            _collisionPoints.TransformPolyPoints(PotentialSpace.position, PotentialSpace.rotation);
        }

        private void UpdateRealSpace(GameTime gameTime)
        {
            RealSpace.position += RealSpace.velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            RealSpace.rotation = ApplyRotation(RealSpace.velocity);
        }

        private void ResolveCollisions(GameTime gameTime)
        {
            // Check for a collision in potential space by evaluating possible reflection vector
            List<Vector2> collidingPoints = Collider.CheckCollision(_collisionPoints.transformedPolyPoints);

            if (collidingPoints.Count > 0)
            {
                Vector2 reflectionVector = Collider.ResolveCollision
                    (collidingPoints, RealSpace.position, RealSpace.velocity);

                // If colliding in potential space then update position with damped reflection vector
                RealSpace.velocity = ApplyDamping(reflectionVector, _dampingFactor);
                UpdateRealSpace(gameTime);
                // Update collision points from potential position to real position
                _collisionPoints.TransformPolyPoints(RealSpace.position, RealSpace.rotation);

                // Perform second check to see if still colliding in real space
                if (Collider.CheckCollision(_collisionPoints.transformedPolyPoints).Count != 0)
                {
                    // If still colliding update position along reflection vector without damping
                    RealSpace.velocity = reflectionVector;
                    UpdateRealSpace(gameTime);
                }
            }
            else
            {
                // If no collision, set real position to potential position
                RealSpace.position = PotentialSpace.position;
            }
        }

        private float ApplyRotation(Vector2 velocity)
        {
            if (_canRotate)
                return Mechanics.CalculateRotation(velocity);
            
            return 0f;
        }

        private Vector2 ApplyDamping(Vector2 velocity, float dampingFactor)
        {
            Vector2 dampedVelocity = velocity *= dampingFactor;

            return (Mechanics.VectorMagnitude(dampedVelocity) < 16f) ? Vector2.Zero : dampedVelocity;
        }

        public void AddVelocity(GameTime gameTime, Vector2 deltaVelocity)
        {
            RealSpace.velocity += deltaVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public new void Draw(SpriteBatch spriteBatch)
        {
            Draw(spriteBatch, RealSpace.position, RealSpace.rotation);
            _collisionPoints.DrawCollisionPoints(spriteBatch, RealSpace.position);
        }
    }
}
