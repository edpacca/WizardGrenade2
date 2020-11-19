namespace WizardGrenade2
{
    public class WeaponSettings
    {
        public const bool ROTATION = true;

        // Fireball settings
        private const string FIREBALL_FILENAME = "Fireball";
        private const int FIREBALL_MASS = 35;
        private const int FIREBALL_COLLISION_POINTS = 6;
        private const float FIREBALL_BOUNCE_FACTOR = 0.6f;

        public const float FIREBALL_DETONATION_TIME = 4f;
        public const int FIREBALL_EXPLOSION_RADIUS = 40;
        public const int FIREBALL_CHARGE_POWER = 400;
        public const float FIREBALL_MAX_CHARGE_TIME = 2f;
        public const float FIREBALL_EXPLOSION_DAMPING = 0.4f;

        public static GameObjectParameters FIREBALL_GAMEOBJECT = new GameObjectParameters
            (FIREBALL_FILENAME, FIREBALL_MASS, ROTATION, FIREBALL_COLLISION_POINTS, FIREBALL_BOUNCE_FACTOR);

        // Arrow settings
        private const string ARROW_FILENAME = "MelfsAcidArrow";
        private const int ARROW_MASS = 15;
        private const int ARROW_COLLISION_POINTS = 0;
        private const float ARROW_BOUNCE_FACTOR = 0f;

        public const int ARROW_POWER = 1500;
        public const float ARROW_MAX_CHARGE_TIME = 0.6f;
        public const float ARROW_KNOCKBACK_FACTOR = 0.96f;
        public const float ARROW_DAMAGE_FACTOR = 0.028f;

        public static GameObjectParameters ARROW_GAMEOBJECT = new GameObjectParameters
            (ARROW_FILENAME, ARROW_MASS, ROTATION, ARROW_COLLISION_POINTS, ARROW_BOUNCE_FACTOR);

        // Icebomb Settings
        private const string ICEBOMB_FILENAME = "IceBomb";
        private const int ICEBOMB_MASS = 80;
        private const int ICEBOMB_COLLISION_POINTS = 6;
        private const float ICEBOMB_BOUNCE_FACTOR = 0.1f;

        public const int ICEBOMB_CHARGE_POWER = 400;
        public const float ICEBOMB_MAX_CHARGE_TIME = 3f;
        public const int ICEBOMB_EXPLOSION_RADIUS = 35;
        public const int ICEBOMB_EFFECT_RADIUS = 80;
        public const float ICEBOMB_PUSHBACK_FACTOR = 0.8f;

        public static GameObjectParameters ICEBOMB_GAMEOBJECT = new GameObjectParameters
            (ICEBOMB_FILENAME, ICEBOMB_MASS, ROTATION, ICEBOMB_COLLISION_POINTS, ICEBOMB_BOUNCE_FACTOR);
    }
}
