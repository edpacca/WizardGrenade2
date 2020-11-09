using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace WizardGrenade2
{
    public sealed class WeaponManager
    {
        private WeaponManager() {}
        private static readonly Lazy<WeaponManager> lazyManager = new Lazy<WeaponManager>(() => new WeaponManager());
        public static WeaponManager Instance { get => lazyManager.Value; }

        private Fireball _fireball = new Fireball();
        private Arrow _arrow = new Arrow();
        private IceBomb _iceBomb = new IceBomb();
        private Crosshair _crosshair = new Crosshair();
        private List<Wizard> _allWizards;

        private int _numberOfWeapons;
        private int _timer = 4;
        private bool _isLoaded;

        public float ChargePower { get; private set; }
        public int ActiveWeapon { get; private set; }
        public bool IsCharging { get; private set; }
        public List<Weapon> Weapons { get; private set; }
        public Rectangle WizardSpriteRectangle { get; private set; }

        private readonly int[] _detonationTimes = new int[] { 1, 2, 3, 4, 5 };
        
        public void LoadContent(ContentManager contentManager, List<Wizard> allWizards)
        {
            PopulateGameObjects(allWizards);

            Weapons = new List<Weapon>();
            Weapons.Add(_fireball);
            Weapons.Add(_arrow);
            Weapons.Add(_iceBomb);

            foreach (var weapon in Weapons)
                weapon.LoadContent(contentManager);

            _numberOfWeapons = Weapons.Count;
            _crosshair.LoadContent(contentManager);
        }

        public void Update(GameTime gameTime, Vector2 activeWizardPosition, int activeDirection)
        {
            _crosshair.UpdateCrosshair(gameTime, activeWizardPosition, activeDirection);
            CycleWeapons(Keys.Q);
            
            if (_isLoaded)
                ChargeWeapon(gameTime, activeWizardPosition, activeDirection);
            
            if (!Weapons[ActiveWeapon].IsMoving)
                ResetCharge();
            
            Weapons[ActiveWeapon].Update(gameTime, _allWizards);
            UpdateGrenadeTimer();
            ResetTimer();
        }

        public void PopulateGameObjects(List<Wizard> allWizards)
        {
            _allWizards = allWizards;
            WizardSpriteRectangle = _allWizards[0].GetSpriteRectangle();
        }

        private void CycleWeapons(Keys key)
        {
            if (InputManager.WasKeyPressed(key))
            {
                Weapons[ActiveWeapon].IsMoving = false;
                ActiveWeapon = Utility.WrapAroundCounter(ActiveWeapon, _numberOfWeapons);
                Weapons[ActiveWeapon].IsActive = true;
            }
        }

        private void ChargeWeapon(GameTime gameTime, Vector2 activePlayerPosition, int activeDirection)
        {
            if (InputManager.IsKeyDown(Keys.Space))
            {
                IsCharging = true;
                Weapons[ActiveWeapon].KillProjectile();
                Weapons[ActiveWeapon].SetToPlayerPosition(activePlayerPosition, activeDirection);
                ChargePower += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (InputManager.WasKeyReleased(Keys.Space) || ChargePower >= Weapons[ActiveWeapon].MaxChargeTime)
            {
                _isLoaded = false;
                IsCharging = false;
                Weapons[ActiveWeapon].FireProjectile(ChargePower, _crosshair.GetAimAngle());
                ChargePower = 0f;
            }
        }

        private void ResetCharge()
        {
            if (InputManager.IsKeyUp(Keys.Space))
            _isLoaded = true;
        }

        private void UpdateGrenadeTimer()
        {
            if (_isLoaded)
            {
                int numberKey = InputManager.NumberKeys();

                foreach (var time in _detonationTimes)
                {
                    if (numberKey == time)
                    {
                        SetTimer(numberKey);
                        _timer = numberKey;
                    }
                }
            }
        }

        private void SetTimer(int time)
        {
            if (!TimerIsNull())
                Weapons[ActiveWeapon].DetonationTimer.ResetTimer(time);
        }

        private void ResetTimer()
        {
            if ((!TimerIsNull()) && Weapons[ActiveWeapon].DetonationTimer.Time < 0)
                Weapons[ActiveWeapon].DetonationTimer.ResetTimer(_timer);
        }

        private bool TimerIsNull()
        {
            return (Weapons[ActiveWeapon].DetonationTimer == null);
        }

        public float GetDetonationTime()
        {
            if (TimerIsNull())
                return 0;
            
            return Weapons[ActiveWeapon].DetonationTimer.Time;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Weapons[ActiveWeapon].Draw(spriteBatch);
            _crosshair.Draw(spriteBatch);
        }
    }
}
