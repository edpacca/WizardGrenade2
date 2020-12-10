using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace WizardGrenade2
{
    public class ScreenSettings
    {
        public const float TARGET_WIDTH = 1200;
        public const float TARGET_HEIGHT = TARGET_WIDTH * 0.5625f;
        public static int RESOLUTION_WIDTH = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        public static int RESOLUTION_HEIGHT = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

        public static int CentreScreenWidth => (int)(TARGET_WIDTH / 2);
        public static int CentreScreenHeight => (int)(TARGET_HEIGHT / 2);
        public static Vector2 ScreenCentre => new Vector2(TARGET_WIDTH / 2, TARGET_HEIGHT / 2);
        public static Vector2 ScreenResolutionCentre => new Vector2(RESOLUTION_WIDTH / 2, RESOLUTION_HEIGHT / 2);
    }
}
