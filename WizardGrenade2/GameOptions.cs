using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WizardGrenade2
{
    public class GameOptions
    {
        public int WizardHealth { get; set; }
        public int NumberOfTeams { get; set; }
        public int TeamSize { get; set; }
        public string MapFile { get; set; }
        public readonly List<string> options = new List<string> { "Players", "Wizards", "Health" };
        public readonly List<string> mapNames = new List<string> { "Castle", "Two-Towers", "City", "Clouds", "Arena" };
        public List<Vector2> OptionsLayout { get; private set; }

        public List<string> Options { get; set; }

        // To be done, make new class
        public GameOptions(/*List<string> options*/)
        {
            Options = options;
            OptionsLayout = new List<Vector2>();
            SetUpOptions();
        }

        private void SetUpOptions()
        {
            int numberOfOptions = options.Count;
            float verticalInterval = ScreenSettings.TARGET_HEIGHT / (numberOfOptions + 2);

            for (int i = 0; i < numberOfOptions; i++)
                OptionsLayout.Add(new Vector2(ScreenSettings.TARGET_WIDTH / 5, verticalInterval * (i + 2)));
        }
    }
}
