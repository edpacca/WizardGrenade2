using Microsoft.Xna.Framework;

namespace WizardGrenade2
{
    class ScreenSettings
    {
        public const float TARGET_WIDTH = 1200;
        public const float TARGET_HEIGHT = TARGET_WIDTH * 0.5625f;
        public const int RESOLUTION_WIDTH = 1920;
        public const int RESOLUTION_HEIGHT = 1080;

        public static int CentreScreenWidth => (int)(TARGET_WIDTH / 2);
        public static int CentreScreenHeight => (int)(TARGET_HEIGHT / 2);
        public static Vector2 ScreenCentre => new Vector2(TARGET_WIDTH / 2, TARGET_HEIGHT / 2);
        public static Vector2 ScreenResolutionCentre => new Vector2(RESOLUTION_WIDTH / 2, RESOLUTION_HEIGHT / 2);
    }
}
