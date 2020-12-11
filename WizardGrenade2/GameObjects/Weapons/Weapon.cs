using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace WizardGrenade2
{
    public class Weapon
    {
        public Timer DetonationTimer { get; set; }
        public Texture2D Symbol { get => _weapon.SpriteTexture; }
        public Vector2 Position { get => _weapon.Position; }
        public Vector2 Velocity { get => _weapon.Velocity; }
        public string ChargingSoundFile { get; set; }
        public string MovingSoundFile { get; set; }
        public string ImpaceSoundFile { get; set; }
        public float MaxChargeTime { get; private set; }
        public int WeaponPower { get; private set; }
        public bool HasCollided { get => _weapon.Collided; set => _weapon.Collided = value; }
        public bool IsMoving { get; set; }
        public bool IsActive { get; set; }

        protected GameObject _weapon;

        public virtual void SetToPlayerPosition(Vector2 newPosition, int activeDirection, float angle) => _weapon.Position = newPosition;
        public virtual void WizardInteraction(List<Wizard> gameObjects) { }
        public void SetSpriteScale(float scale) => _weapon.SpriteScale = scale;
        public void DrawSymbol(SpriteBatch spriteBatch, Vector2 position, float scale) => _weapon.DrawSpriteAtScale(spriteBatch, position, scale);

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
            _weapon.Velocity = Mechanics.VectorComponents(chargeTime * WeaponPower, firingAngle);
            IsMoving = true;
        }

        public virtual void KillProjectile()
        {
            _weapon.Velocity = Vector2.Zero;
            IsMoving = false;
            StateMachine.Instance.ShotLanded();
        }

        public virtual void KillProjectile(string SoundEffect)
        {
            SoundManager.Instance.PlaySound(SoundEffect);
            KillProjectile();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (IsMoving)
                _weapon.Draw(spriteBatch);
        }
    }
}
