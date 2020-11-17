using Microsoft.Xna.Framework;

namespace WizardGrenade2
{
    public static class Utility
    {
        public static float DifferentialGameTimeValue(GameTime gameTime, int rateFactor, int magnitude)
        {
            return (float)gameTime.ElapsedGameTime.TotalSeconds * rateFactor * magnitude;
        }

        public static int WrapAroundCounter(int number, int listLength) => (number + 1) % listLength;
        public static int WrapAroundNegativeCounter(int number, int listLength) => number - 1 < 0 ? listLength - number - 1 : number - 1;
    }
}
