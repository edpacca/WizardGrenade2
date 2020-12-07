using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace WizardGrenade2
{
    public class HugeFireball
    {
        private List<Fireball> _fireballs;
        private List<Vector2> _positions;
        private const int _numberOfFireballs = 7;
        public bool Spawned { get; private set; }
        private Random _randomGenerator;

        public HugeFireball()
        {
            _fireballs = new List<Fireball>();
            _positions = new List<Vector2>();
            _randomGenerator = new Random();

            float xPosition = ScreenSettings.TARGET_WIDTH / (_numberOfFireballs + 2);

            for (int i = 0; i < _numberOfFireballs; i++)
            {
                _fireballs.Add(new Fireball(100));
                _positions.Add(new Vector2(xPosition * (i + 1), -500));
                _fireballs[i].SetSpriteScale(7f);
            }
        }

        public void LoadContent(ContentManager contentManager)
        {
            foreach (var fireball in _fireballs)
            {
                fireball.LoadContent(contentManager);
            }
        }

        public void SpawnFireballs()
        {
            for (int i = 0; i < _numberOfFireballs; i++)
            {
                _fireballs[i].SetToPlayerPosition(_positions[i], 1, 0f);
                _fireballs[i].DetonationTimer.ResetTimer(3f + i);
                _fireballs[i].FireProjectile(0.1f, (float)_randomGenerator.NextDouble() * Mechanics.TAO);
            }

            Spawned = true;
        }

        public void Update(GameTime gameTime, List<Wizard> wizards)
        {
            if (Spawned)
                foreach (var fireball in _fireballs)
                {
                    fireball.Update(gameTime, wizards);
                }

            if (!_fireballs[_numberOfFireballs - 1].IsMoving)
                Spawned = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Spawned)
                foreach (var fireball in _fireballs)
                    fireball.Draw(spriteBatch);
        }
    }
}
