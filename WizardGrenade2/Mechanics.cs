using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardGrenade2
{
    public class Mechanics
    {
        public const float GRAVITY = 9.8f;

        public static Vector2 ApplyGravity(GameTime gameTime, Vector2 velocity, float mass)
        {
            Vector2 acceleration = new Vector2(0, GRAVITY * mass);
            return velocity + acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
