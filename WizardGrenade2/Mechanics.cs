using Microsoft.Xna.Framework;
using System;

namespace WizardGrenade2
{
    public class Mechanics
    {
        public const float GRAVITY = 9.8f;

        public struct Space2D
        {
            public Vector2 position;
            public Vector2 velocity;
            public float rotation;
        }

        public static Vector2 ApplyGravity(GameTime gameTime, Vector2 velocity, float mass)
        {
            Vector2 acceleration = new Vector2(0, GRAVITY * mass);
            return velocity + acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public static Vector2 GetVectorComponents(float magnitude, float angle)
        {
            return new Vector2((float)Math.Sin(angle) * magnitude, (float)Math.Cos(angle) * magnitude);
        }

    }
}
