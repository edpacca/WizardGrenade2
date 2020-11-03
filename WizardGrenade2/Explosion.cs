using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardGrenade2
{
    class Explosion : Sprite
    {
        private readonly string _fileName = "Explosion";
        private int _explosionRadius;
        private bool _isVisible;
        private float _explosionTime = 0.2f;
        private Timer _explosionTimer;
        private Vector2 _position;

        public Explosion(int explosionRadius)
        {
            _explosionRadius = explosionRadius;
            _explosionTimer = new Timer(_explosionTime);
        }

        public void LoadContent(ContentManager contentManager)
        {
            LoadContent(contentManager, _fileName);
            SetExplosionRadius(GetSpriteTexture());

        }

        private void SetExplosionRadius(Texture2D texture)
        {
            SetSpriteScale(_explosionRadius / texture.Width);
        }

        public void ShowExplosion(Vector2 position)
        {
            _isVisible = true;
            _position = position;
            _explosionTimer.ResetTimer(_explosionTime);
        }

        public void UpdateExplosion(GameTime gameTime)
        {
            if(_isVisible)
                _explosionTimer.Update(gameTime);

            if (!_explosionTimer.IsRunning)
                _isVisible = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_isVisible)
                DrawSprite(spriteBatch, _position, 0f);
        }


    }
}
