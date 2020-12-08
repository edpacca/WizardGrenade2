using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace WizardGrenade2
{
    public class UserInterface
    {
        private const float ROUND_TIME = 45f;
        private Sprite _cursor;
        private RoundTimer _timer;
        private List<HealthBar> _healthBars;
        private List<string> _teamNames = new List<string>();
        private int _numberOfTeams;
        private int _teamStartHealth;
        private List<Weapon> _weaponList;
        private DetonationTimer _detonationTimer;
        private Vector2 _weaponSymbolPosition = new Vector2(40, 40);

        public UserInterface(GameOptions gameOptions)
        {
            _numberOfTeams = gameOptions.NumberOfTeams;
            _teamStartHealth = gameOptions.TeamSize * gameOptions.WizardHealth;

            for (int i = 0; i < gameOptions.NumberOfTeams; i++)
            {
                _teamNames.Add("Team " + (i + 1));
            }
        }

        public void LoadContent(ContentManager contentManager)
        {

            _cursor = new Sprite(contentManager, @"UserInterface/cursor");
            _timer = new RoundTimer(ROUND_TIME);
            _detonationTimer = new DetonationTimer(contentManager);
            _timer.LoadContent(contentManager);
            _healthBars = new List<HealthBar>();

            for (int i = 0; i < _numberOfTeams; i++)
            {
                _healthBars.Add(new HealthBar(i, _teamStartHealth, _teamNames[i]));
                _healthBars[i].LoadContent(contentManager);
            }

            _weaponList = WeaponManager.Instance.Weapons;
        }

        public void Update(GameTime gameTime, int[] teamHealths)
        {
            //if (StateMachine.Instance.GameState == StateMachine.GameStates.PlayerTurn)
            _timer.Update(gameTime);
            //if (!_timer.IsRunning)
            //{
            //    StateMachine.Instance.ForceTurnEnd();
            //    _timer.ResetTimer(ROUND_TIME);
            //}

            _detonationTimer.SetTimer((int)WeaponManager.Instance.GetDetonationTime());

            for (int i = 0; i < _numberOfTeams; i++)
                _healthBars[i].Update(gameTime, teamHealths[i]);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var healthBar in _healthBars)
                healthBar.Draw(spriteBatch);

            //if (StateMachine.Instance.GameState == StateMachine.GameStates.PlayerTurn)
            _timer.Draw(spriteBatch);

            _cursor.DrawSprite(spriteBatch, InputManager.CursorPosition());
            _weaponList[WeaponManager.Instance.ActiveWeapon].DrawSymbol(spriteBatch, _weaponSymbolPosition, 6f);
            _detonationTimer.Draw(spriteBatch, _weaponSymbolPosition);
        }
    }
}
