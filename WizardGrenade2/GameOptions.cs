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
        public List<string> Options { get => _options; }
        private readonly List<string> _options = new List<string> { "# Teams", "# Wizards", "Wizard Health" };
        public List<Vector2> OptionsLayout { get; private set; }

        public GameOptions()
        {
            OptionsLayout = new List<Vector2>();
            SetUpOptions();
        }

        private void SetUpOptions()
        {
            int numberOfOptions = _options.Count;
            float verticalInterval = ScreenSettings.TARGET_HEIGHT / (numberOfOptions + 2);

            for (int i = 0; i < numberOfOptions; i++)
                OptionsLayout.Add(new Vector2(ScreenSettings.TARGET_WIDTH / 5, verticalInterval * (i + 2)));
        }
    }
}
