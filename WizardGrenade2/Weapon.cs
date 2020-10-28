using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WizardGrenade2
{
    class Weapon
    {
        private GameObject _weapon;
        private bool _isActiveWeapon;
        private bool _isFired;
        private int _weaponPower = 100;

        public void LoadContent(ContentManager contentManager, GameObjectParameters parameters)
        {
            _weapon = new GameObject(parameters);
            _weapon.LoadContent(contentManager);
        }

        public void Update(GameTime gameTime)
        {
            _weapon.Update(gameTime);
        }

        public void SetToPlayerPosition(Vector2 newPosition)
        {
            _weapon.SetPosition(newPosition);
        }

        public void SetWeapon(bool isActive)
        {
            _isActiveWeapon = isActive;
        }

        public void SetWeaponPower(int power)
        {
            _weaponPower = power;
        }

        public virtual void FireProjectile(float chargeTime, float firingAngle)
        {
            _weapon.SetVelocity(Mechanics.VectorComponents(chargeTime * _weaponPower, firingAngle));
            _isFired = true;
        }

        public virtual void FireWeapon(GameTime gameTime)
        {

        }

        public void KillProjectile()
        {
            _weapon.SetVelocity(Vector2.Zero);
            _isFired = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_isFired)
                _weapon.Draw(spriteBatch);
        }

    }
}
