using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace WizardGrenade2
{
    public class GameObject : Sprite
    {
        private string _fileName;
        private int _framesH = 0;
        private int _framesV = 0;
        private float _mass = 0;
        private int _numberOfCollisionPoints = 0;

        private Mechanics.Space2D Space;
        private Polygon _collisionPoints;

        public GameObject(string fileName)
        {
            _fileName = fileName;
        }

        public GameObject(ContentManager contentManager, string fileName)
        {
            _fileName = fileName;
            LoadContent(contentManager);
        }

        public GameObject(string fileName, int framesH, int framesV, Vector2 position, float mass, int collisionPoints)
        {
            _framesH = framesH;
            _framesV = framesV;
            _fileName = fileName;
            _numberOfCollisionPoints = collisionPoints;

            Space.position = position;
            Space.velocity = Vector2.Zero;
            Space.rotation = 0f;
            _mass = mass;
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
            Space.velocity = Mechanics.ApplyGravity(gameTime, Space.velocity, _mass);
            Space.position += Space.velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Space.rotation = (float)Math.Atan2(Space.velocity.Y, Space.velocity.X);

            _collisionPoints.UpdateTransformedPolyPoints(Space.position, Space.rotation);
        }

        public void AddVelocity(GameTime gameTime, Vector2 addedVecloity)
        {
            Space.velocity += addedVecloity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public new void Draw(SpriteBatch spriteBatch)
        {
            Draw(spriteBatch, Space.position, Space.rotation);
            _collisionPoints.DrawCollisionPoints(spriteBatch, Space.position);
        }


    }
}
