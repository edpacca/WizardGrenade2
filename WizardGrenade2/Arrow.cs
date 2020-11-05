using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;

namespace WizardGrenade2
{
    class Arrow : Weapon
    {
        private readonly string _fileName = "MelfsAcidArrow";
        private const int MASS = 15;
        private const int NUMBER_OF_COLLISION_POINTS = 0;
        private const float DAMPING_FACTOR = 0f;
        private const int CHARGE_POWER = 1500;
        private const float MAX_CHARGE = 0.6f;

        public void LoadContent(ContentManager contentManager)
        {
            LoadContent(contentManager, new GameObjectParameters(_fileName, MASS, true, NUMBER_OF_COLLISION_POINTS, DAMPING_FACTOR));
            SetFiringBehaviour(CHARGE_POWER, MAX_CHARGE);
        }

        public override void GameObjectInteraction(List<GameObject> gameObjects)
        {
            foreach (var gameObject in gameObjects)
            {
                float distance = Math.Abs(Mechanics.VectorMagnitude(GetPosition() - gameObject.GetPosition()));
                if (distance <= 16)
                {
                    gameObject.AddVelocity(GetVelocity());
                    KillProjectile();
                }
            }
        }

        public override void SetToPlayerPosition(Vector2 newPosition, int activeDirection)
        {
            base.SetToPlayerPosition(newPosition + new Vector2(17 * activeDirection, 0), 1);
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects)
        {
            GameObjectInteraction(gameObjects);

            if (GetVelocity() == Vector2.Zero)
                KillProjectile();

            base.Update(gameTime, gameObjects);
        }
    }
}
