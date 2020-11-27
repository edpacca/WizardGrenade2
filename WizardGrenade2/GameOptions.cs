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
    }
}
