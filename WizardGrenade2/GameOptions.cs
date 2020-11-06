namespace WizardGrenade2
{
    class GameOptions
    {
        public int WizardHealth { get; private set; }
        public int NumberOfTeams { get; private set; }
        public int TeamSize { get; private set; }

        public GameOptions(int numberOfTeams, int teamSize, int wizardHealth)
        {
            NumberOfTeams = numberOfTeams;
            TeamSize = teamSize;
            WizardHealth = wizardHealth;
        }
    }
}
