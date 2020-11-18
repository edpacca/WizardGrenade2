using System.Collections.Generic;
namespace WizardGrenade2
{
    public class WizardSettings
    {
        private const string BASE_FILENAME = "WizardSpritesheet";

        private const int FRAMES_V = 1;
        private const int FRAMES_H = 15;
        public const int BLINK_CYCLES = 4;

        public static Dictionary<string, int[]> animationStates = new Dictionary<string, int[]>()
        {
            ["Idle"] = new int[] { 0, 12 },
            ["Looking"] = new int[] { 13, 14 },
            ["Walking"] = new int[] { 1, 2 },
            ["Charging"] = new int[] { 3, 4, 5, 6, 7, 8, 7, 8, 7, 8 },
            ["Firing"] = new int[] { 9 },
            ["Weak"] = new int[] { 10 },
            ["Jumping"] = new int[] { 11 },
        };

        private const int MASS = 100;
        private const int COLLISION_POINTS = 15;
        private const bool CAN_ROTATE = false;
        private const float BOUNCE_FACTOR = 0.3f;

        public const int WALK_SPEED = 100;
        public const int JUMP_HEIGHT = 200;

        public static GameObjectParameters GetWizardParameters(int skinNumber)
        {
            return new GameObjectParameters(BASE_FILENAME + skinNumber, MASS, CAN_ROTATE, COLLISION_POINTS, BOUNCE_FACTOR, FRAMES_H, FRAMES_V);
        }
    }
}
