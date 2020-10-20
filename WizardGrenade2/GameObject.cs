using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

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

        private Mechanics.Space2D Space;
        private Mechanics.Space2D PotentialSpace;
        private Polygon _collisionPoints;

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

            Space.position = position;
            Space.velocity = Vector2.Zero;
            Space.rotation = 0f;
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
            // Update gravity
            Space.velocity = Mechanics.ApplyGravity(gameTime, Space.velocity, _mass);

            // Update potential position in space
            PotentialSpace.position = Space.position + Space.velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_canRotate)
                PotentialSpace.rotation = Mechanics.CalculateRotation(Space.velocity);

            _collisionPoints.UpdateTransformedPolyPoints(PotentialSpace.position, PotentialSpace.rotation);

            Vector2 reflectionVector = CollisionManager.Instance.ResolveCollision
                (_collisionPoints.transformedPolyPoints, PotentialSpace.position, Space.velocity);

            if (reflectionVector != Vector2.Zero)
                Space.velocity = reflectionVector;

            Space.position += Space.velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_canRotate)
                Space.rotation = Mechanics.CalculateRotation(Space.velocity);
        }

        public void AddVelocity(GameTime gameTime, Vector2 deltaVelocity)
        {
            Space.velocity += deltaVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public new void Draw(SpriteBatch spriteBatch)
        {
            Draw(spriteBatch, Space.position, Space.rotation);
            _collisionPoints.DrawCollisionPoints(spriteBatch, Space.position);
        }
    }
}
