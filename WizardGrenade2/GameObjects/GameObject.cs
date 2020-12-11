using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace WizardGrenade2
{
    public class GameObject : Sprite
    {
        public Vector2 Velocity { get => _realSpace.velocity; set => _realSpace.velocity = value; }
        public Vector2 Position { get => _realSpace.position; set => _realSpace.position = value; }
        public bool Collided { get; set; }
        public float DrawRotation { get; set; }

        private readonly GameObjectParameters _parameters = new GameObjectParameters();
        private CollisionManager Collider = CollisionManager.Instance;
        private Space2D _realSpace = new Space2D();
        private Space2D _potentialSpace = new Space2D();
        private Polygon _collisionPoints;
        private Vector2 _acceleration = Vector2.Zero;
        private string _impactAudio;
        private bool _hasImpactAudio;

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
            if (_parameters.framesH == 0 || _parameters.framesV == 0)
                LoadContent(contentManager, _parameters.fileName);
            else
                LoadContent(contentManager, _parameters.fileName, _parameters.framesH, _parameters.framesV);

            _collisionPoints = new Polygon(SpriteRectangle, _parameters.numberOfCollisionPoints);
            _collisionPoints.LoadPolyContent(contentManager);
        }

        public void LoadAudio(string fileName)
        {
            _impactAudio = fileName;
            _hasImpactAudio = true;
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
            _potentialSpace.rotation = Mechanics.CalculateRotation(_realSpace.velocity);
            _collisionPoints.UpdateCollisionPoints(_potentialSpace.position, _potentialSpace.rotation);
        }

        private void UpdateRealSpace(GameTime gameTime)
        {
            _realSpace.position += _realSpace.velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            _realSpace.rotation = Mechanics.CalculateRotation(_realSpace.velocity);
        }

        private void ResolveCollisions(GameTime gameTime)
        {
            List<Vector2> collidingPoints = Collider.CheckCollision(_collisionPoints.TransformedPolyPoints);

            if (collidingPoints.Count > 0)
            {
                Collided = true;
                Vector2 reflectionVector = Collider.ResolveCollision(collidingPoints, _potentialSpace.position, _realSpace.velocity);
                _realSpace.velocity = ApplyDamping(reflectionVector, _parameters.dampingFactor);
                UpdateRealSpace(gameTime);
                _collisionPoints.UpdateCollisionPoints(_realSpace.position, _realSpace.rotation);

                if (_hasImpactAudio && Mechanics.VectorMagnitude(_realSpace.velocity) > 10)
                    SoundManager.Instance.PlaySound(_impactAudio);
            }
            else
            {
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

        public List<Vector2> GetTransformedPolyPoints(Vector2 position)
        {
            _collisionPoints.UpdateCollisionPoints(position, 0f);
            return _collisionPoints.TransformedPolyPoints;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            float rotation = _parameters.CanRotate ? _realSpace.rotation : DrawRotation;
            DrawSprite(spriteBatch, _realSpace.position, rotation);
            //_collisionPoints.DrawCollisionPoints(spriteBatch, _realSpace.position);
        }

        public void AddVelocity(Vector2 deltaVelocity) => _realSpace.velocity += deltaVelocity;
        public void ModifyVelocityX(float xVelocity) => _realSpace.velocity.X = xVelocity;
        public void ModifyVelocityY(float yVelocity) => _realSpace.velocity.Y = yVelocity;
        public void AddRotation(float rotation) => _realSpace.rotation += rotation;
    }
}
