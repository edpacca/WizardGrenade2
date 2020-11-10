using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace WizardGrenade2
{
    class IceBomb : Weapon
    {
        private Explosion _explosion;

        public IceBomb()
        {
            _explosion = new Explosion(WeaponSettings.ICEBOMB_EXPLOSION_RADIUS);
            LoadWeaponObject(WeaponSettings.ICEBOMB_GAMEOBJECT, WeaponSettings.ICEBOMB_CHARGE_POWER, WeaponSettings.ICEBOMB_MAX_CHARGE_TIME);
        }

        public override void LoadContent(ContentManager contentManager)
        {
            _explosion.LoadContent(contentManager);
            base.LoadContent(contentManager);
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
            Map.Instance.DeformLevel(WeaponSettings.ICEBOMB_EXPLOSION_RADIUS, position);
            _explosion.ShowExplosion(position);
            GameObjectInteraction(gameTime, gameObjects, position);
            IsMoving = false;
        }

        public virtual void GameObjectInteraction(GameTime gameTime, List<Wizard> gameObjects, Vector2 explosionPosition)
        {
            foreach (var gameObject in gameObjects)
            {
                Vector2 explosionToObject = _explosion.ExplosionToObject(explosionPosition, gameObject.GetPosition());
                if (_explosion.IsWithinExplosionArea(explosionToObject, WeaponSettings.ICEBOMB_EFFECT_RADIUS))
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
