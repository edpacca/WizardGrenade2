using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WizardGrenade2
{
    class Weapon
    {
        private GameObject _weapon;
        private bool _isActiveWeapon;
        private bool _isFired;

        public Weapon(GameObjectParameters parameters)
        {
            _weapon = new GameObject(parameters);
        }

        public void LoadContent(ContentManager contentManager)
        {
            _weapon.LoadContent(contentManager);
        }

        public void Update(GameTime gameTime)
        {
            _weapon.Update(gameTime);
        }

        public void SetToPlayerPosition(Vector2 newPosition) => _weapon.SetPosition(newPosition);
        public void SetWeapon(bool isActive) => _isActiveWeapon = isActive;

        public void FireProjectile(float firingPower, float firingAngle)
        {
            _weapon.SetVelocity(Mechanics.VectorComponents(firingPower, firingAngle));
            _isFired = true;
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
