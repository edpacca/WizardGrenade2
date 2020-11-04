using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace WizardGrenade2
{
    class Weapon
    {
        private GameObject _weapon;
        private bool _isActiveWeapon;
        private bool _isMoving;
        private int _weaponPower = 100;
        private float _maxCharge = 5f;

        public void LoadContent(ContentManager contentManager, GameObjectParameters parameters)
        {
            _weapon = new GameObject(parameters);
            _weapon.LoadContent(contentManager);
        }

        public virtual void Update(GameTime gameTime, List<GameObject> gameObjects)
        {
            if (_isMoving)
                _weapon.Update(gameTime);
        }

        public virtual void FireProjectile(float chargeTime, float firingAngle)
        {
            _weapon.SetVelocity(Mechanics.VectorComponents(chargeTime * _weaponPower, firingAngle));
            _isMoving = true;
        }

        public void KillProjectile()
        {
            _weapon.SetVelocity(Vector2.Zero);
            _isMoving = false;
        }

        public virtual void GameObjectInteraction(List<GameObject> gameObjects)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (_isMoving)
                _weapon.Draw(spriteBatch);
        }

        public void SetToPlayerPosition(Vector2 newPosition) => _weapon.SetPosition(newPosition);
        public void SetWeapon(bool isActive) => _isActiveWeapon = isActive;
        public void SetMovementFlag(bool isMoving) => _isMoving = isMoving;
        public bool GetMovementFlag() =>_isMoving;
        public float GetMaxCharge() => _maxCharge;
        public Vector2 GetPosition() => _weapon.GetPosition();
        public void SetFiringBehaviour(int power) => _weaponPower = power;

        public void SetFiringBehaviour(int power, float maxCharge)
        {
            _weaponPower = power;
            _maxCharge = maxCharge;
        }
    }
}
