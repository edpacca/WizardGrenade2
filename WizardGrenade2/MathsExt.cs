using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace WizardGrenade2
{
    class MathsExt
    {
        public static bool Approx(float f1, float f2)
        {
            return (Math.Abs(f1 - f2) < (Math.Abs(f1) * 1e-9));
        }

        public static float CalcMinTheta(float radius, float minLength)
        {
            return 2 * (float)Math.Asin(minLength / (2 * radius));
        }

        public static float FlipAngle(float initialAngle)
        {
            float flippedAngle = (float)(Math.PI + (Math.PI - initialAngle));
            return flippedAngle;
        }

        public static bool isWithinCircleInSquare(int radius, int x, int y)
        {
            if (Math.Pow((x - radius), 2) + Math.Pow((y - radius), 2) <= Math.Pow(radius, 2))
                return true;

            return false;
        }

        public static bool isPointWithinCircle(Vector2 testPosition, Vector2 circleCentre, float circleRadius)
        {
            Vector2 deltaRadius = testPosition - circleCentre;
            float deltaMagnitude = (float)Math.Pow(Mechanics.VectorMagnitude(deltaRadius), 2);
            if (deltaMagnitude <= Math.Pow(circleRadius, 2))
                return true;

            return false;
        }

        public static bool IntersectOnEdge(Vector2 intersection, Vector2 p1, Vector2 p2)
        {
            if (intersection.X >= Math.Min(p1.X, p2.X)
                && intersection.X <= Math.Max(p1.X, p2.X)
                && intersection.Y >= Math.Min(p1.Y, p2.Y)
                && intersection.Y <= Math.Max(p1.Y, p2.Y))
                return true;

            return false;
        }


    }
}
