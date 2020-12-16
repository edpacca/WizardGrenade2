using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace WizardGrenade2
{
    public static class MenuSettings
    {
        // MainMenu layout and settings
        public static Vector2 MenuOptionsPosition = new Vector2(ScreenSettings.TARGET_WIDTH / 2.5f, ScreenSettings.TARGET_HEIGHT * 0.4f);
        public static float MenuOptionsLastPosition = ScreenSettings.TARGET_HEIGHT * 0.8f;
        public static Vector2 MenuBottomOptionPosition = new Vector2(ScreenSettings.CentreScreenWidth, ScreenSettings.TARGET_HEIGHT * 0.9f);
        public const float MENU_ARROW_SCALE = 6f;

        // Settings layout and settings
        public static Vector2 MenuSettingsPosition = new Vector2(ScreenSettings.TARGET_WIDTH / 4f, MenuOptionsPosition.Y);
        public static Vector2 MenuSettingsOffset = new Vector2(320, 0);
        public static float MenuSettingsSpritesSpan = ScreenSettings.CentreScreenWidth - 180;

        // GameSetup layout and settings
        public static Vector2 GameSetupOptionsPosition = new Vector2(ScreenSettings.TARGET_WIDTH / 5, (ScreenSettings.TARGET_HEIGHT / 6) + 150);
        public static Vector2 MapPosition = new Vector2(ScreenSettings.CentreScreenWidth, ScreenSettings.CentreScreenHeight + 30);
        public static float GameSetupOptionsLastPosition = ScreenSettings.TARGET_HEIGHT * 0.8f;
        public static float GameSetupSettingsPosition = ScreenSettings.TARGET_WIDTH / 2.3f;
        public static float TeamSizeSpriteSpan = ScreenSettings.CentreScreenWidth - 40;
        public const float MENU_MAPS_SCALE = 0.55f;
        public const float WIZARD_SPRITE_SCALE = 2f;
        public const int ROUND_TIMER_INTERVAL = 5;

        // Instructions layout and settings
        public static Vector2 InstructionsTextPosition = new Vector2(ScreenSettings.CentreScreenWidth, 300);
        public static Vector2 InstructionsImagePosition = new Vector2(ScreenSettings.CentreScreenWidth, ScreenSettings.TARGET_HEIGHT * 0.72f);
        public const int ITEM_OSCILLATION_RATE = 2;
        public const float ITEM_OSCILLATION_AMPLITUDE = 0.6f;

        // Credits layout and settings
        public static Vector2 CreditsPosition = new Vector2(ScreenSettings.CentreScreenWidth, 280);

        // Main Menu options
        public static readonly List<string> MenuOptionsList = new List<string>()
        {
            "Play",
            "Settings",
            "How to Play",
            "Credits",
            "Quit"
        };

        // GameSetup - Game Settings options
        public static readonly List<string> BattleOptionsList = new List<string> 
        { 
            "Players",
            "Wizards",
            "Health",
            "Round",
        };

        //GameSetup - Maps options
        public static readonly List<string> MapTitles = new List<string> 
        { 
            "Castle", 
            "Two-Towers", 
            "City", 
            "Clouds", 
            "Arena" 
        };

        // Credits text
        public static readonly List<string> Credits = new List<string>()
        {
            "Basically entirely created by",
            "Eddie Pace",
            " ",
            "Except for the beautiful sky,",
            "moon and menu backgrounds",
            "by Alicja Przystup",
        };
    }
}
