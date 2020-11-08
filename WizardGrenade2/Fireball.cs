using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace WizardGrenade2
{
    class Fireball : Weapon
    {
        private readonly string _fileName = "Fireball";
        private const int MASS = 35;
        private const int NUMBER_OF_COLLISION_POINTS = 6;
        private const float DAMPING_FACTOR = 0.6f;
        private const int CHARGE_POWER = 400;
        private const float MAX_CHARGE = 2f;
        private const float EXPLOSION_DAMPING = 0.6f;

        private float _detonationTime;
        private int _blastRadius;
        private Explosion _explosion;

        public Fireball(float detonationTime, int blastRadius)
        {
            DetonationDimer = new Timer(detonationTime);
            _detonationTime = detonationTime;
            _blastRadius = blastRadius;
            _explosion = new Explosion(_blastRadius);
        }

        public void LoadContent(ContentManager contentManager)
        {
            LoadContent(contentManager, new GameObjectParameters(_fileName, MASS, true, NUMBER_OF_COLLISION_POINTS, DAMPING_FACTOR));
            SetFiringBehaviour(CHARGE_POWER, MAX_CHARGE);
            _explosion.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime, List<Wizard> gameObjects)
        {
            if (GetMovementFlag())
                DetonationDimer.Update(gameTime);

            if (!DetonationDimer.IsRunning)
            {
                Explode(gameTime, gameObjects);
                KillProjectile();
                DetonationDimer.ResetTimer(WeaponManager.Instance.GetDetonationTime());
            }

            _explosion.UpdateExplosion(gameTime);
            base.Update(gameTime, gameObjects);
        }

        private void Explode(GameTime gameTime, List<Wizard> gameObjects)
        {
            Vector2 position = GetPosition();
            Map.Instance.DeformLevel(_blastRadius, position);
            _explosion.ShowExplosion(position);
            GameObjectInteraction(gameTime, gameObjects, position);
            SetMovementFlag(false);
        }

        public virtual void GameObjectInteraction(GameTime gameTime, List<Wizard> gameObjects, Vector2 explosionPosition)
        {
            foreach (var gameObject in gameObjects)
            {
                Vector2 explosionToObject = _explosion.ExplosionToObject(explosionPosition, gameObject.GetPosition());
                if (_explosion.IsWithinExplosionArea(explosionToObject, _blastRadius + 15))
                {
                    Vector2 pushBack = _explosion.ExplosionVector(explosionToObject) * EXPLOSION_DAMPING;
                    gameObject.AddVelocity(pushBack);
                    gameObject.entity.ApplyDamage((int)Mechanics.VectorMagnitude(pushBack) / 3);
                }
            }
        }

        private void SetDetonationTime(int time)
        {
            DetonationDimer.ResetTimer(time);
        }

        private int GetDetonationTime(Keys key)
        {
            if (InputManager.WasKeyPressed(key))
                return 1;
                //return key.ToString();
            return 0;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _explosion.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
    }
}
