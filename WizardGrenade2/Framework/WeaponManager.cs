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

        public List<Weapon> Weapons { get; private set; } = new List<Weapon>();
        public Vector2 ActiveWizardPosition { get; set; }
        public Rectangle WizardSpriteRectangle { get; private set; }
        public float ChargePower { get; private set; }
        public float DetonationTime { get => _isTimerNull ? 0 : Weapons[ActiveWeapon].DetonationTimer.Time; }
        public int ActiveWeapon { get; private set; }
        public bool IsCharging { get; private set; }

        private List<Wizard> _allWizards;
        private Fireball _fireball = new Fireball();
        private Arrow _arrow = new Arrow();
        private IceBomb _iceBomb = new IceBomb();
        private Crosshair _crosshair = new Crosshair();
        private HugeFireball _hugeFireball = new HugeFireball();
        private Random _random = new Random();
        private Vector2 _initialPosition;
        private int _numberOfWeapons;
        private float _timer = 4f;
        private bool _isLoaded;
        private bool _isTimerNull { get => (Weapons[ActiveWeapon].DetonationTimer == null); }
        
        public void LoadContent(ContentManager contentManager, List<Wizard> allWizards)
        {
            PopulateGameObjects(allWizards);

            Weapons.Add(_fireball);
            Weapons.Add(_arrow);
            Weapons.Add(_iceBomb);
            _numberOfWeapons = Weapons.Count;

            foreach (var weapon in Weapons)
                weapon.LoadContent(contentManager);

            _hugeFireball.LoadContent(contentManager);
            _crosshair.LoadContent(contentManager);
            _fireball.DetonationTimer.ResetTimer(WeaponSettings.FIREBALL_DETONATION_TIME);
        }

        public void Update(GameTime gameTime, Vector2 activeWizardPosition, int activeDirection)
        {
            ActiveWizardPosition = activeWizardPosition;
            _crosshair.Update(gameTime, activeWizardPosition, activeDirection, ChargePower, Weapons[ActiveWeapon].MaxChargeTime);
            CycleWeapons(Keys.Tab);

            if (_isLoaded)
                ChargeWeapon(gameTime, activeWizardPosition, activeDirection);
            else if (Vector2.Distance(_initialPosition, Weapons[ActiveWeapon].Position) > WeaponSettings.MAX_DISTANCE)
                 Weapons[ActiveWeapon].KillProjectile();
            
            Weapons[ActiveWeapon].Update(gameTime, _allWizards);
            _hugeFireball.Update(gameTime, _allWizards);
            UpdateGrenadeTimer();
            NewTurnReset();
        }

        public void PopulateGameObjects(List<Wizard> allWizards)
        {
            _allWizards = allWizards;
            WizardSpriteRectangle = _allWizards[0].SpriteRectangle;
        }

        private void CycleWeapons(Keys key)
        {
            if (InputManager.WasKeyPressed(key) && _isLoaded)
            {
                int randomSound = _random.Next(1, 5);
                SoundManager.Instance.PlaySound("magic" + randomSound);
                Weapons[ActiveWeapon].IsMoving = false;
                ActiveWeapon = Utility.WrapAroundCounter(ActiveWeapon, _numberOfWeapons);
                Weapons[ActiveWeapon].IsActive = true;
            }
        }

        private void ChargeWeapon(GameTime gameTime, Vector2 activePlayerPosition, int activeDirection)
        {
            if (InputManager.IsKeyDown(Keys.Space) && StateMachine.Instance.GameState == GameStates.PlayerTurn)
            {
                SoundManager.Instance.PlaySoundInstance(Weapons[ActiveWeapon].ChargingSoundFile);
                IsCharging = true;
                Weapons[ActiveWeapon].KillProjectile();
                Weapons[ActiveWeapon].SetToPlayerPosition(activePlayerPosition, activeDirection, _crosshair.CrosshairAngle);
                ChargePower += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (InputManager.WasKeyReleased(Keys.Space) || ChargePower >= Weapons[ActiveWeapon].MaxChargeTime)
            {
                SoundManager.Instance.StopSoundInstance(Weapons[ActiveWeapon].ChargingSoundFile);
                SoundManager.Instance.PlaySoundInstance(Weapons[ActiveWeapon].MovingSoundFile);
                _isLoaded = false;
                IsCharging = false;
                _initialPosition = activePlayerPosition;
                Weapons[ActiveWeapon].FireProjectile(ChargePower, _crosshair.CrosshairAngle);
                ChargePower = 0f;
                StateMachine.Instance.ShotTaken();
            }
        }

        private void NewTurnReset()
        {
            ResetCharge();
            ResetTimer();
        }

        private void UpdateGrenadeTimer()
        {
            if (_isLoaded)
            {
                int numberKey = InputManager.NumberKeys();

                foreach (var time in WeaponSettings.DETONATION_TIMES)
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
            if (!_isTimerNull)
                Weapons[ActiveWeapon].DetonationTimer.ResetTimer(time);
        }

        private void ResetCharge()
        {
            if (!Weapons[ActiveWeapon].IsMoving && StateMachine.Instance.GameState == GameStates.PlayerTurn)
                _isLoaded = true;
        }

        private void ResetTimer()
        {
            if (!_isTimerNull && StateMachine.Instance.NewTurn())
                Weapons[ActiveWeapon].DetonationTimer.ResetTimer(_timer);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Weapons[ActiveWeapon].Draw(spriteBatch);

            if (StateMachine.Instance.GameState == GameStates.PlayerTurn)
            _crosshair.Draw(spriteBatch, IsCharging);

            _hugeFireball.Draw(spriteBatch);
        }

        public void SpawnHugeFireballs()
        {
            _hugeFireball.SpawnFireballs();
        }
    }
}
