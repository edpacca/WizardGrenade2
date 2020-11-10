using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WizardGrenade2
{
    class GameScreen
    {
        private const int NUMBER_OF_TEAMS = 3;
        private const int TEAM_SIZE = 3;
        private const int WIZARD_HEALTH = 100;

        public BattleManager _battleManager = new BattleManager("Map2");
        private GameOptions _gameOptions;

        public void Initialise()
        {
            _gameOptions = new GameOptions(NUMBER_OF_TEAMS, TEAM_SIZE, WIZARD_HEALTH);
            _battleManager.Initialise(_gameOptions);
        }

        public int[] GetTeamHealths() => _battleManager.GetTeamHealths();
        public GameOptions GetGameOptions() => _gameOptions;

        public void LoadContent(ContentManager contentManager)
        {
            _battleManager.LoadContent(contentManager);
        }

        public void Update(GameTime gameTime)
        {
            _battleManager.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _battleManager.Draw(spriteBatch);
        }
    }
}
