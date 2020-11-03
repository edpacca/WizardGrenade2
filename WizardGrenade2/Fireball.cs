using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WizardGrenade2
{
    class Fireball : Weapon
    {
        private readonly string _fileName = "Fireball";
        private const int MASS = 35;
        private const int NUMBER_OF_COLLISION_POINTS = 6;
        private const float DAMPING_FACTOR = 0.6f;
        private const int CHARGE_POWER = 400;
        private const float MAX_CHARGE = 2f;
        private int _blastRadius = 40;

        private Timer _timer;
        private float _detonationTime;
        private Explosion _explosion;

        public Fireball(float detonationTime, int blastRadius)
        {
            _timer = new Timer(detonationTime);
            _detonationTime = detonationTime;
            _blastRadius = blastRadius;
            _explosion = new Explosion(_blastRadius);
            
        }

        public void LoadContent(ContentManager contentManager)
        {
            LoadContent(contentManager, new GameObjectParameters(_fileName, MASS, true, NUMBER_OF_COLLISION_POINTS, DAMPING_FACTOR));
            SetFiringBehaviour(CHARGE_POWER, MAX_CHARGE);
            _explosion.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            _timer.Update(gameTime);

            if (!_timer.IsRunning)
            {
                Explode();
                KillProjectile();
                _timer.ResetTimer(_detonationTime);
            }
            _explosion.UpdateExplosion(gameTime);
            base.Update(gameTime);
        }

        private void Explode()
        {
            Vector2 position = GetPosition();
            Map.Instance.DeformLevel(_blastRadius, position);
            _explosion.ShowExplosion(position);
            SetMovementFlag(false);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _explosion.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
    }
}
