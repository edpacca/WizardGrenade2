using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace WizardGrenade2
{
    public class Wizard
    {
        private readonly string _baseFileName = "Wizard";
        private const int MASS = 10;
        private const int FRAMES_V = 1;
        private const int FRAMES_H = 1;
        private const int WALK_SPEED = 500;
        private const int COLLISION_POINTS = 10;

        private GameObject _wizard;

        public Wizard(int skinNumber, Vector2 position)
        {
            _wizard = new GameObject(_baseFileName + skinNumber, FRAMES_H, FRAMES_V, position, MASS, COLLISION_POINTS);
        }

        public void LoadContent(ContentManager contentManager)
        {
            _wizard.LoadContent(contentManager);
        }

        public void Update(GameTime gameTime)
        {
            _wizard.Update(gameTime);

            if (InputManager.IsKeyDown(Keys.Up)) 
                _wizard.AddVelocity(gameTime, new Vector2(0, -WALK_SPEED));
            if (InputManager.IsKeyDown(Keys.Down))
                _wizard.AddVelocity(gameTime, new Vector2(0, WALK_SPEED));
            if (InputManager.IsKeyDown(Keys.Left))
                _wizard.AddVelocity(gameTime, new Vector2(-WALK_SPEED, 0));
            if (InputManager.IsKeyDown(Keys.Right))
                _wizard.AddVelocity(gameTime, new Vector2(WALK_SPEED, 0));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _wizard.Draw(spriteBatch);
        }
    }
}
