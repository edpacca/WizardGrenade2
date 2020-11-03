using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardGrenade2
{
    class WeaponManager
    {
        private Fireball _fireball = new Fireball(4f, 40);
        private Arrow _arrow = new Arrow();
        private Crosshair _crosshair = new Crosshair();

        private List<Weapon> _weapons = new List<Weapon>();
        private int _activeWeapon = 0;
        private int _numberOfWeapons;

        private float _chargeTime = 0f;
        private bool _hasFired;

        public void LoadContent(ContentManager contentManager)
        {
            _weapons.Add(_fireball);
            _weapons.Add(_arrow);

            _fireball.LoadContent(contentManager);
            _arrow.LoadContent(contentManager);

            _numberOfWeapons = _weapons.Count;
            _crosshair.LoadContent(contentManager);
        }

        public void Update(GameTime gameTime, Vector2 activePlayerPosition)
        {
            _crosshair.UpdateCrosshair(gameTime, activePlayerPosition);
            CycleWeapons(Keys.Q);
            
            if (!_hasFired)
                ChargeWeapon(gameTime, activePlayerPosition);
            ResetCharge();
            
            _weapons[_activeWeapon].Update(gameTime);
        }

        public void CycleWeapons(Keys key)
        {
            if (InputManager.WasKeyPressed(key))
            {
                _weapons[_activeWeapon].SetWeapon(false);
                _activeWeapon = Utility.WrapAroundCounter(_activeWeapon, _numberOfWeapons);
                _weapons[_activeWeapon].SetWeapon(true);
            }
        }

        public void ChargeWeapon(GameTime gameTime, Vector2 activePlayerPosition)
        {
            if (InputManager.IsKeyDown(Keys.Space))
            {
                _weapons[_activeWeapon].KillProjectile();
                _weapons[_activeWeapon].SetToPlayerPosition(activePlayerPosition);
                _chargeTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (InputManager.WasKeyReleased(Keys.Space) || _chargeTime >= _weapons[_activeWeapon].GetMaxCharge())
            {
                _hasFired = true;
                _weapons[_activeWeapon].FireProjectile(_chargeTime, _crosshair.GetAimAngle());
                _chargeTime = 0f;
            }
        }

        private void ResetCharge()
        {
            if (InputManager.WasKeyReleased(Keys.Space))
                _hasFired = false;
        }

        public float GetChargePower()
        {
            return _chargeTime;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _weapons[_activeWeapon].Draw(spriteBatch);
            _crosshair.Draw(spriteBatch);
        }
    }
}
