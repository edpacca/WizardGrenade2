using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WizardGrenade2
{
    public class Explosion : Sprite
    {
        private Timer _explosionTimer;
        private Vector2 _position;
        private readonly string _fileName = @"GameObjects/Explosion";
        private readonly int _explosionDiameter;
        private readonly int _pushbackFactor = 6000;
        private bool _isVisible;
        private float _explosionTime = 0.1f;
        private int _minimumPushback = 100;

        public Explosion(int blastRadius)
        {
            _explosionDiameter = blastRadius * 2 - 12;
            _explosionTimer = new Timer(_explosionTime);
        }

        public Explosion(int blastRadius, int pushbackFactor)
            : this(blastRadius)
        {
            _pushbackFactor = pushbackFactor;
        }

        public void LoadContent(ContentManager contentManager)
        {
            LoadContent(contentManager, _fileName);
            SetExplosionRadius(SpriteTexture);
        }

        private void SetExplosionRadius(Texture2D texture) => SpriteScale = (float)_explosionDiameter / (float)texture.Width;
        public void SetMinimumPushBack(int minimumPushBack) => _minimumPushback = minimumPushBack;
        public Vector2 ExplosionToObject(Vector2 explosionPosition, Vector2 objectPosition) => objectPosition - explosionPosition;
        public float ExplosionMagnitude(Vector2 explosionToObject) => (_pushbackFactor / (1 + Mechanics.VectorMagnitude(explosionToObject))) + _minimumPushback;

        public void UpdateExplosion(GameTime gameTime)
        {
            if (_isVisible)
                _explosionTimer.Update(gameTime);

            if (!_explosionTimer.IsRunning)
                _isVisible = false;
        }

        public void ShowExplosion(Vector2 position)
        {
            _isVisible = true;
            _position = position - Origin;
            _explosionTimer.ResetTimer(_explosionTime);
        }

        public bool IsWithinExplosionArea(Vector2 explosionToObject, int blastArea)
        {
            if (Mechanics.VectorMagnitude(explosionToObject) <= blastArea)
                return true;

            return false;
        }

        public Vector2 ExplosionVector(Vector2 explosionToObject)
        {
            Vector2 pushBack = Mechanics.NormaliseVector(explosionToObject);
            return pushBack * ExplosionMagnitude(explosionToObject);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_isVisible)
                DrawSprite(spriteBatch, _position);
        }
    }
}
