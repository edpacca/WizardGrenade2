using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WizardGrenade2
{
    class Fireball : Weapon
    {
        private readonly string _fileName = "Fireball";
        private const int MASS = 30;
        private const int NUMBER_OF_COLLISION_POINTS = 6;
        private const float DAMPING_FACTOR = 0.5f;
        private const int FIREBALL_POWER = 300;

        private Timer _timer;
        private float _detonationTime;

        public Fireball(float detonationTime)
        {
            _timer = new Timer(detonationTime);
            _detonationTime = detonationTime;
        }

        public void LoadContent(ContentManager contentManager)
        {
            LoadContent(contentManager, new GameObjectParameters(_fileName, MASS, true, NUMBER_OF_COLLISION_POINTS, DAMPING_FACTOR));
            SetWeaponPower(FIREBALL_POWER);
        }

        public new void Update(GameTime gameTime)
        {
            _timer.Update(gameTime);

            if (!_timer.IsRunning)
            {
                KillProjectile();
                _timer.ResetTimer(_detonationTime);
            }

            base.Update(gameTime);
        }
    }
}
