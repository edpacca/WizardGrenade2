using Microsoft.Xna.Framework;
using System;

namespace WizardGrenade2
{
    public static class Mechanics
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

        public static Vector2 VectorComponents(float magnitude, float angle)
        {
            return new Vector2((float)Math.Sin(angle) * magnitude, (float)Math.Cos(angle) * magnitude);
        }

        public static float VectorMagnitude(Vector2 vector)
        {
            return (float)Math.Sqrt(Math.Pow(vector.X, 2) + Math.Pow(vector.Y, 2));
        }

        public static Vector2 NormaliseVector(Vector2 vector)
        {
            return vector / (VectorMagnitude(vector));
        }

        public static float DotProduct(Vector2 vector1, Vector2 vector2)
        {
            return vector1.X * vector2.X + vector1.Y * vector2.Y;
        }

        public static float VectorAngle(Vector2 vector1, Vector2 vector2)
        {
            float v1Mag = VectorMagnitude(vector1);
            float v2Mag = VectorMagnitude(vector2);

            return (float)Math.Acos(DotProduct(vector1, vector2) / (v1Mag * v2Mag));
        }

        public static Vector2 ReflectionVector(Vector2 incident, Vector2 normal)
        {
            return incident + (-2 * normal * (DotProduct(incident, normal)) / (float)Math.Pow(VectorMagnitude(normal), 2));
        }

        public static Vector2 RelativeProjectilePosition(Vector2 vectorComponents, GameTime gameTime, TimeSpan startTime, float mass)
        {
            Vector2 delta = vectorComponents * (float)(gameTime.TotalGameTime.TotalSeconds - startTime.TotalSeconds);
            delta.Y += GRAVITY * mass / 2 * (float)Math.Pow((float)(gameTime.TotalGameTime.TotalSeconds - startTime.TotalSeconds), 2);

            return delta;
        }

        public static float CalculateRotation(Vector2 velocity)
        {
            return (float)Math.Atan2(velocity.Y, velocity.X);
        }
    }
}
