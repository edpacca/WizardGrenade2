using Microsoft.Xna.Framework;
using System;

namespace WizardGrenade2
{
    public static class Utility
    {
        public static float DifferentialGameTimeValue(GameTime gameTime, int rateFactor, int magnitude)
        {
            return (float)gameTime.ElapsedGameTime.TotalSeconds * rateFactor * magnitude;
        }

        public static bool Approximate(float float1, float float2)
        {
            return Math.Abs(float1 - float2) < 0.00001f;
        }

        public static int ChangeValueInLimits(int nextValue, int minValue, int maxValue)
        {
            return nextValue < minValue ? minValue : nextValue > maxValue ? maxValue : nextValue;
        }

        public static Rectangle ShiftRectangle(Rectangle rectangle, Vector2 offset, float scaleFactor)
        {
            return new Rectangle((int)((rectangle.X + offset.X) / scaleFactor), (int)((rectangle.Y + offset.Y) / scaleFactor), (int)(rectangle.Width / scaleFactor), (int)(rectangle.Height / scaleFactor));
        }

        public static int WrapAroundCounter(int number, int listLength) => (number + 1) % listLength;
        public static int WrapAroundNegativeCounter(int number, int listLength) => number - 1 < 0 ? listLength - number - 1 : number - 1;

        public static float FractionPercentage(float fraction, float percentage, int sign)
        {
            return fraction + (sign * fraction * percentage);
        }
    }
}
