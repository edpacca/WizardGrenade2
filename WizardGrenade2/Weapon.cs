using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace WizardGrenade2
{
    public class Weapon
    {
        private GameObject _weapon;
        public Timer DetonationTimer { get; set; }
        public int WeaponPower { get; private set; }
        public float MaxChargeTime { get; private set; }
        public bool IsMoving { get; set; }
        public bool IsActive { get; set; }
        public bool HasCollided { get => _weapon.Collided; set => _weapon.Collided = value; }
        public virtual void SetToPlayerPosition(Vector2 newPosition, int activeDirection, float angle) => _weapon.SetPosition(newPosition);
        public virtual void WizardInteraction(List<Wizard> gameObjects) {}
        public Vector2 Position => _weapon.GetPosition();
        public Vector2 Velocity => _weapon.GetVelocity();
        public Texture2D Symbol => _weapon.GetSpriteTexture();

        public void LoadWeaponObject(GameObjectParameters parameters)
        {
            _weapon = new GameObject(parameters);
            WeaponPower = 100;
            MaxChargeTime = 5f;
        }

        public void LoadWeaponObject(GameObjectParameters parameters, int weaponPower, float maxChargeTime)
        {
            _weapon = new GameObject(parameters);
            WeaponPower = weaponPower;
            MaxChargeTime = maxChargeTime;
        }

        public virtual void LoadContent(ContentManager contentManager)
        {
            _weapon.LoadContent(contentManager);
        }

        public virtual void Update(GameTime gameTime, List<Wizard> gameObjects)
        {
            if (IsMoving)
                _weapon.Update(gameTime);
        }

        public virtual void FireProjectile(float chargeTime, float firingAngle)
        {
            _weapon.SetVelocity(Mechanics.VectorComponents(chargeTime * WeaponPower, firingAngle));
            IsMoving = true;
        }

        public virtual void KillProjectile()
        {
            _weapon.SetVelocity(Vector2.Zero);
            IsMoving = false;
            StateMachine.Instance.ShotLanded();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (IsMoving)
                _weapon.Draw(spriteBatch);
        }

        public void DrawSymbol(SpriteBatch spriteBatch, Vector2 position, float scale)
        {
            _weapon.DrawSpriteAtScale(spriteBatch, position, scale);
        }
    }
}
