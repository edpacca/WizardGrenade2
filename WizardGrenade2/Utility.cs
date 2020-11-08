using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WizardGrenade2
{
    public static class Utility
    {
        public static float DifferentialGameTimeValue(GameTime gameTime, int rateFactor, int magnitude)
        {
            return (float)gameTime.ElapsedGameTime.TotalSeconds * rateFactor * magnitude;
        }

        public static int WrapAroundCounter(int i, int listLength)
        {
            return (i + 1) % listLength;
        }

    }
}
