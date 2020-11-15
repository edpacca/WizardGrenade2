using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace WizardGrenade2
{
    class GameScreen
    {
        private const int NUMBER_OF_TEAMS = 3;
        private const int TEAM_SIZE = 3;
        private const int WIZARD_HEALTH = 100;

        //public BattleManager _battleManager = new BattleManager("Map2");
        //public int[] TeamHealths { get => _battleManager.TeamHealths; }
        //public List<string> TeamNames { get => _battleManager.TeamNames; }
        //public GameOptions GameOptions { get; private set; }
        private StateMachine _stateMachine = new StateMachine();


        public void Initialise()
        {
            //GameOptions = new GameOptions(NUMBER_OF_TEAMS, TEAM_SIZE, WIZARD_HEALTH);
            //_battleManager.Initialise(GameOptions);
        }

        public void LoadContent(ContentManager contentManager)
        {

            //_battleManager.LoadContent(contentManager);
        }

        public void Update(GameTime gameTime)
        {

            //_battleManager.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            //_battleManager.Draw(spriteBatch);
        }
    }
}
