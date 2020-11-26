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

        public static int WrapAroundCounter(int number, int listLength) => (number + 1) % listLength;
        public static int WrapAroundNegativeCounter(int number, int listLength) => number - 1 < 0 ? listLength - number - 1 : number - 1;
    }
}
