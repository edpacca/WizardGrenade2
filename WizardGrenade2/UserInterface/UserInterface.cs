using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace WizardGrenade2
{
    public class UserInterface
    {
        private List<HealthBar> _healthBars;
        private List<Weapon> _weaponList;
        private List<string> _teamNames = new List<string>();
        private DetonationTimer _detonationTimer;
        private RoundTimer _timer;
        private Sprite _cursor;
        private Vector2 _weaponSymbolPosition = new Vector2(40, 40);
        private float _roundTime = 45f;
        private int _numberOfTeams;
        private int _teamStartHealth;

        public UserInterface(GameOptions gameOptions, ContentManager contentManager)
        {
            _numberOfTeams = gameOptions.NumberOfTeams;
            _teamStartHealth = gameOptions.TeamSize * gameOptions.WizardHealth;
            _roundTime = gameOptions.RoundTime;

            for (int i = 0; i < gameOptions.NumberOfTeams; i++)
                _teamNames.Add("Team " + (i + 1));

            LoadContent(contentManager);
        }

        private void LoadContent(ContentManager contentManager)
        {
            _cursor = new Sprite(contentManager, @"UserInterface/cursor");
            _timer = new RoundTimer(_roundTime);
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
            _timer.Update(gameTime);
            _detonationTimer.SetTimer((int)WeaponManager.Instance.DetonationTime);

            for (int i = 0; i < _numberOfTeams; i++)
                _healthBars[i].Update(gameTime, teamHealths[i]);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (StateMachine.Instance.GameState != GameStates.GameOver && StateMachine.Instance.GameState != GameStates.PlaceWizards)
            {
                foreach (var healthBar in _healthBars)
                    healthBar.Draw(spriteBatch);

                _timer.Draw(spriteBatch);
                _cursor.DrawSprite(spriteBatch, InputManager.CursorPosition());
                _weaponList[WeaponManager.Instance.ActiveWeapon].DrawSymbol(spriteBatch, _weaponSymbolPosition, 6f);
                _detonationTimer.Draw(spriteBatch, _weaponSymbolPosition);
            }
        }
    }
}
