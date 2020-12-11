using Microsoft.Xna.Framework;
using System;

namespace WizardGrenade2
{
    public static class Mechanics
    {
        public const float GRAVITY = 9.8f;
        public const float PI = (float)Math.PI;
        public const float TAO = 2 * PI;

        public static Vector2 ApplyGravity(float mass) => new Vector2(0, GRAVITY * mass);
        public static Vector2 VectorComponents(float magnitude, float angle) => new Vector2((float)Math.Sin(angle) * magnitude, (float)Math.Cos(angle) * magnitude);
        public static Vector2 NormaliseVector(Vector2 vector) => vector / (VectorMagnitude(vector));
        public static Vector2 VectorBetweenPoints(Vector2 point1, Vector2 point2) => point1 - point2;
        public static Vector2 NormalisedDifferenceVector(Vector2 position1, Vector2 position2) => NormaliseVector(Vector2.Subtract(position1, position2));
        public static float VectorMagnitude(Vector2 vector) => (float)Math.Sqrt(Math.Pow(vector.X, 2) + Math.Pow(vector.Y, 2));
        public static float DotProduct(Vector2 vector1, Vector2 vector2) => vector1.X * vector2.X + vector1.Y * vector2.Y;
        public static float CalculateRotation(Vector2 velocity) => (float)Math.Atan2(velocity.Y, velocity.X);

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

        public static float CrossProduct(Vector2 vector1, Vector2 vector2)
        {
            Vector3 vector1z = new Vector3(vector1.X, vector1.Y, 0);
            Vector3 vector2z = new Vector3(vector2.X, vector2.Y, 0);

            return Vector3.Cross(vector1z, vector2z).Z;
        }

        public static float VectorAngle(Vector2 vector1, Vector2 vector2)
        {
            float v1Mag = VectorMagnitude(vector1);
            float v2Mag = VectorMagnitude(vector2);

            return (float)Math.Acos(DotProduct(vector1, vector2) / (v1Mag * v2Mag));
        }
    }
}
