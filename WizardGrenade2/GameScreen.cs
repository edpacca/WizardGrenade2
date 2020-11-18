using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace WizardGrenade2
{
    class GameScreen
    {
        public BattleManager _battleManager;
        public int[] TeamHealths { get => _battleManager.TeamHealths; }
        public List<string> TeamNames { get => _battleManager.TeamNames; }
        private StateMachine _stateMachine = new StateMachine();

        public void Initialise(GameOptions gameOptions)
        {
            _battleManager = new BattleManager(gameOptions.MapFile);
            _battleManager.Initialise(gameOptions);
        }

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
