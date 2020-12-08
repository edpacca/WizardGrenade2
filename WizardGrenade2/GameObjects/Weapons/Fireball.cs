using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace WizardGrenade2
{
    class Fireball : Weapon
    {
        private Explosion _explosion;
        private int _explosionRadius;

        public Fireball()
        {
            DetonationTimer = new Timer(WeaponSettings.FIREBALL_DETONATION_TIME);
            _explosion = new Explosion(WeaponSettings.FIREBALL_EXPLOSION_RADIUS);
            LoadWeaponObject(WeaponSettings.FIREBALL_GAMEOBJECT, WeaponSettings.FIREBALL_CHARGE_POWER, WeaponSettings.FIREBALL_MAX_CHARGE_TIME);
            ChargingSoundFile = "fireCharge";
            MovingSoundFile = "fireCast";
            _explosionRadius = WeaponSettings.FIREBALL_EXPLOSION_RADIUS;
            _weapon.LoadAudio("fireBounce");
        }

        public Fireball(int blastRadius)
        {
            DetonationTimer = new Timer(WeaponSettings.FIREBALL_DETONATION_TIME);
            _explosion = new Explosion(blastRadius);
            LoadWeaponObject(WeaponSettings.FIREBALL_GAMEOBJECT, WeaponSettings.FIREBALL_CHARGE_POWER, WeaponSettings.FIREBALL_MAX_CHARGE_TIME);
            _explosionRadius = blastRadius;
            _weapon.LoadAudio("stone1");
        }

        public override void LoadContent(ContentManager contentManager)
        {
            _explosion.LoadContent(contentManager);
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime, List<Wizard> gameObjects)
        {
            if (IsMoving)
                DetonationTimer.Update(gameTime);

            if (!DetonationTimer.IsRunning)
            {
                Explode(gameTime, gameObjects);
                KillProjectile("fireHit");
                DetonationTimer.ResetTimer(WeaponManager.Instance.GetDetonationTime());
            }

            _explosion.UpdateExplosion(gameTime);
            base.Update(gameTime, gameObjects);
        }

        private void Explode(GameTime gameTime, List<Wizard> gameObjects)
        {
            Vector2 position = Position;
            Map.Instance.DeformLevel(_explosionRadius, position);
            _explosion.ShowExplosion(position);
            GameObjectInteraction(gameTime, gameObjects, position);
            IsMoving = false;
        }

        public virtual void GameObjectInteraction(GameTime gameTime, List<Wizard> gameObjects, Vector2 explosionPosition)
        {
            foreach (var gameObject in gameObjects)
            {
                Vector2 explosionToObject = _explosion.ExplosionToObject(explosionPosition, gameObject.Position);
                if (_explosion.IsWithinExplosionArea(explosionToObject, WeaponSettings.FIREBALL_EXPLOSION_RADIUS + 15))
                {
                    SoundManager.Instance.PlaySound("wizardHit0");
                    Vector2 pushBack = _explosion.ExplosionVector(explosionToObject) * WeaponSettings.FIREBALL_EXPLOSION_DAMPING;
                    gameObject.AddVelocity(pushBack);
                    gameObject.entity.ApplyDamage((int)Mechanics.VectorMagnitude(pushBack) / 3);
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
