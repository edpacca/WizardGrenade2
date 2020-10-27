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
        private Fireball _fireball;
        private Arrow _arrow;

        private List<Weapon> _weapons = new List<Weapon>();
        private int _activeWeapon;
        private int _numberOfWeapons;
        private Vector2 _activePlayerPosition;
        
        public void LoadContent(ContentManager contentManager)
        {
            _weapons.Add(_fireball);
            _weapons.Add(_arrow);
            _numberOfWeapons = _weapons.Count;
        }

        public void Update(GameTime gameTime)
        {
            CycleWeapons(Keys.Tab);

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


        public void SetActivePlayerPosition(Vector2 activePlayerPosition)
        {
            foreach (var weapon in _weapons)
                weapon.SetToPlayerPosition(activePlayerPosition);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _weapons[_activeWeapon].Draw(spriteBatch);
        }
    }
}
