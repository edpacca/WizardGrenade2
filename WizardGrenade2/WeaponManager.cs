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
        private WeaponManager() { }

        private static readonly Lazy<WeaponManager> lazyManager = new Lazy<WeaponManager>(() => new WeaponManager());

        public static WeaponManager Instance
        { 
            get
            {
                return lazyManager.Value;
            }
        }
        private SpriteFont _debugFont;
        private Fireball _fireball = new Fireball(4f, 40);
        private Arrow _arrow = new Arrow();
        private IceBomb _iceBomb = new IceBomb(70);
        private Crosshair _crosshair = new Crosshair();
        private List<Wizard> _gameObjects;

        private List<Weapon> _weapons = new List<Weapon>();
        private int _activeWeapon = 0;
        private int _numberOfWeapons;
        private int _timer = 4;

        private float _chargeTime = 0f;
        private bool _isLoaded;

        public bool IsCharging { get; private set; }

        private readonly int[] _detonationTimes = new int[] { 1, 2, 3, 4, 5 };
        
        public void LoadContent(ContentManager contentManager)
        {
            _weapons.Add(_fireball);
            _weapons.Add(_arrow);
            _weapons.Add(_iceBomb);

            _fireball.LoadContent(contentManager);
            _arrow.LoadContent(contentManager);
            _iceBomb.LoadContent(contentManager);

            _numberOfWeapons = _weapons.Count;
            _crosshair.LoadContent(contentManager);
            _debugFont = contentManager.Load<SpriteFont>("StatFont");
        }

        public void Update(GameTime gameTime, Vector2 activeWizardPosition, int activeDirection)
        {
            _crosshair.UpdateCrosshair(gameTime, activeWizardPosition, activeDirection);
            CycleWeapons(Keys.Q);
            
            if (_isLoaded)
                ChargeWeapon(gameTime, activeWizardPosition, activeDirection);
            
            if (!_weapons[_activeWeapon].GetMovementFlag())
                ResetCharge();
            
            _weapons[_activeWeapon].Update(gameTime, _gameObjects);
            UpdateGrenadeTimer();
            ResetTimer();
        }

        private void CycleWeapons(Keys key)
        {
            if (InputManager.WasKeyPressed(key))
            {
                _weapons[_activeWeapon].SetWeapon(false);
                _activeWeapon = Utility.WrapAroundCounter(_activeWeapon, _numberOfWeapons);
                _weapons[_activeWeapon].SetWeapon(true);
            }
        }

        private void ChargeWeapon(GameTime gameTime, Vector2 activePlayerPosition, int activeDirection)
        {
            if (InputManager.IsKeyDown(Keys.Space))
            {
                IsCharging = true;
                _weapons[_activeWeapon].KillProjectile();
                _weapons[_activeWeapon].SetToPlayerPosition(activePlayerPosition, activeDirection);
                _chargeTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (InputManager.WasKeyReleased(Keys.Space) || _chargeTime >= _weapons[_activeWeapon].GetMaxCharge())
            {
                _isLoaded = false;
                IsCharging = false;
                _weapons[_activeWeapon].FireProjectile(_chargeTime, _crosshair.GetAimAngle());
                _chargeTime = 0f;
            }
        }

        private void ResetCharge()
        {
            if (!InputManager.IsKeyDown(Keys.Space))
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
                _weapons[_activeWeapon].DetonationDimer.ResetTimer(time);
        }

        private void ResetTimer()
        {
            if ((!TimerIsNull()) && _weapons[_activeWeapon].DetonationDimer.Time < 0)
                _weapons[_activeWeapon].DetonationDimer.ResetTimer(_timer);
        }

        private bool TimerIsNull()
        {
            return (_weapons[_activeWeapon].DetonationDimer == null);
        }

        public float GetDetonationTime()
        {
            if (TimerIsNull())
                return 0;
            
            return _weapons[_activeWeapon].DetonationDimer.Time;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _weapons[_activeWeapon].Draw(spriteBatch);
            _crosshair.Draw(spriteBatch);
        }

        public float GetChargePower() => _chargeTime;
        public void PopulateGameObjects(List<Wizard> gameObjects) => _gameObjects = gameObjects;
        public int GetActiveWeapon() => _activeWeapon;
        public List<Weapon> GetWeaponList() => _weapons;
    }
}
