using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WizardGrenade2
{
    class Explosion : Sprite
    {
        private readonly string _fileName = "Explosion";
        private int _explosionDiameter;
        private bool _isVisible;
        private float _explosionTime = 0.1f;
        private Timer _explosionTimer;
        private Vector2 _position;
        private readonly int _pushbackFactor = 6000;
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
            SetExplosionRadius(GetSpriteTexture());
        }

        private void SetExplosionRadius(Texture2D texture)
        {
            SetSpriteScale((float)_explosionDiameter / (float)texture.Width);
        }

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
            _position = position - GetSpriteOrigin();
            _explosionTimer.ResetTimer(_explosionTime);
        }

        public Vector2 ExplosionToObject(Vector2 explosionPosition, Vector2 objectPosition)
        {
            return objectPosition - explosionPosition;
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

        public float ExplosionMagnitude(Vector2 explosionToObject)
        {
            return (_pushbackFactor / (1 + Mechanics.VectorMagnitude(explosionToObject))) + _minimumPushback;
        }

        public void SetMinimumPushBack(int minimumPushBack)
        {
            _minimumPushback = minimumPushBack;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_isVisible)
                DrawSprite(spriteBatch, _position);
        }
    }
}
