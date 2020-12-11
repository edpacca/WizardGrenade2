using System.Collections.Generic;

namespace WizardGrenade2
{
    public static class InstructionsScript
    {
        public static readonly List<string> InstructionsList = new List<string>()
        {
            "Overview",
            "Controls",
            "Weapons",
            "Fireball",
            "Melf's Acid Arrow",
            "Ice Bomb",
            "Items",
        };

        public static readonly List<string> OverviewScript = new List<string>()
        {
            "Wizard Grenade is a turn based battle game for two or more players.",
            "Destroy the wizards from the other teams by damaging them with your spells,",
            "or by pushing them into the abyss...",
            " ",
            "Players take turns to control a wizard from their team, and take one shot with a weapon.",
            "Your turn is over after taking your shot, or when the timer runs out.",
            " ",
            "The last team standing will be crowned victorious!"
        };

        public static readonly List<string> MovingScript = new List<string>()
        {
            "Move your wizards using the 'Left' and 'Right' arrow keys.",
            "Press 'Enter' to jump. Press repeatedly for multiple jumps",
            " ",
            "Press 'Escape' to pause the game. Press 'Delete' to Exit instantly",
            "Use the mouse to move the view, and scroll to change the zoom",
            "Press 'L Shift' to Reset the view",
        };

        public static readonly List<string> WeaponsScript = new List<string>()
        {
            "Aim your weapons using the 'Up' and 'Down' arrow keys.",
            "Press 'Tab' to change weapons.",
            " ",
            "Hold 'Space' to charge your shot, and release to fire!",
            "More power will make your shot go further.",
        };

        public static readonly List<string> FireballsScript = new List<string>()
        {
            "Fireballs are of medium weight and inflict high damage.",
            "They will bounce around off the map and explode when the timer reaches 0",
            "Use the top number keys to set the timer from 1 - 5 seconds.",
            "When a fireball explodes it destroys the terrain and damages wizards with small knock-back"
        };

        public static readonly List<string> ArrowsScript = new List<string>()
        {
            "Arrows are very light and inflict low damage.",
            "They will charge up very quickly, and cause a lot of knock-back at max power"
        };

        public static readonly List<string> IceBombScript = new List<string>()
        {
            "The Ice Bomb is very heavy, and does medium damage.",
            "They have a slow charge, and can be tricky to throw.",
            "Ice Bombs explode on impact with the terrain, with a wide area of affect.",
            "Wizards caught in the blast will be damaged and knocked back",
        };

        public static readonly List<string> ItemsScript = new List<string>()
        {
            "Potions.... coming soon!",
            //"There are some items in the game which can help you.",
            //"A Potion O' Healing will heal your wizard for 25 points.",
            //"A Potion O' Leaping will let your wizard jump infinitely for a turn.",
            //"A Potion O' Stamina will let your wizard move faster for a turn",
        };

        public static readonly List<List<string>> InstructionScripts = new List<List<string>>()
        {
            OverviewScript,
            MovingScript,
            WeaponsScript,
            FireballsScript,
            ArrowsScript,
            IceBombScript,
            ItemsScript,
        };
    }
}
