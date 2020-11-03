using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
