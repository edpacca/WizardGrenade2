using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace WizardGrenade2
{
    class IceBomb : Weapon
    {
        private readonly string _fileName = "IceBomb";
        private const int MASS = 80;
        private const int NUMBER_OF_COLLISION_POINTS = 6;
        private const float DAMPING_FACTOR = 0.1f;
        private const int CHARGE_POWER = 400;
        private const float MAX_CHARGE = 2f;
        private const float PUSHBACK_DAMPING = 0.8f;
        private const int KNOCKBACK_AREA = 20;

        private float _detonationTime;
        private int _blastRadius;
        private Explosion _explosion;

        public IceBomb(int blastRadius)
        {
            _blastRadius = blastRadius;
            _explosion = new Explosion(_blastRadius / 2);
        }

        public void LoadContent(ContentManager contentManager)
        {
            LoadContent(contentManager, new GameObjectParameters(_fileName, MASS, true, NUMBER_OF_COLLISION_POINTS, DAMPING_FACTOR));
            SetFiringBehaviour(CHARGE_POWER, MAX_CHARGE);
            _explosion.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime, List<Wizard> gameObjects)
        {

            if (HasCollided)
            {
                Explode(gameTime, gameObjects);
                KillProjectile();
                HasCollided = false; 
            }

            _explosion.UpdateExplosion(gameTime);
            base.Update(gameTime, gameObjects);
        }

        private void Explode(GameTime gameTime, List<Wizard> gameObjects)
        {
            Vector2 position = GetPosition();
            Map.Instance.DeformLevel(_blastRadius / 2, position);
            _explosion.ShowExplosion(position);
            GameObjectInteraction(gameTime, gameObjects, position);
            SetMovementFlag(false);
        }

        public virtual void GameObjectInteraction(GameTime gameTime, List<Wizard> gameObjects, Vector2 explosionPosition)
        {
            foreach (var gameObject in gameObjects)
            {
                Vector2 explosionToObject = _explosion.ExplosionToObject(explosionPosition, gameObject.GetPosition());
                if (_explosion.IsWithinExplosionArea(explosionToObject, _blastRadius + KNOCKBACK_AREA))
                {
                    Vector2 pushBack = _explosion.ExplosionVector(explosionToObject) / 2;
                    gameObject.AddVelocity(pushBack);
                    gameObject.entity.ApplyDamage((int)Mechanics.VectorMagnitude(pushBack) / 10);
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _explosion.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
        
    }
}
