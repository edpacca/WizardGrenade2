using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace WizardGrenade2
{
    class GameSetup
    {
        public GameOptions GameOptions { get; private set; }
        public bool InGameSetup { get; set; }
        public bool isGameSetup { get => _isMapSet && _AreBattleOptionsSet; }

        private SpriteFont _optionsFont;
        private List<Sprite> _maps;
        private List<Sprite> _wizards;
        private Options _battleOptions;
        private Options _mapOptions;
        private List<Setting> _battleSettings;
        private Setting _players;
        private Setting _teamSize;
        private Setting _health;
        private Vector2 _wizardPosition;
        private Vector2 _teamSizePosition;
        private Vector2 _healthTextPosition;
        private Vector2 _mapPosition;
        private bool _AreBattleOptionsSet;
        private bool _isMapSet;

        public GameSetup()
        {
            GameOptions = new GameOptions();
            _maps = new List<Sprite>();
            _wizards = new List<Sprite>();
            _battleOptions = new Options(MenuSettings.BattleOptionsList, true, true);
            _mapOptions = new Options(MenuSettings.MapTitles, true, false);

            _players = new Setting(2, 2, 4);
            _teamSize = new Setting(3, 1, 8);
            _health = new Setting(4, 1, 10);

            _battleSettings = new List<Setting>();
            _battleSettings.Add(_players);
            _battleSettings.Add(_teamSize);
            _battleSettings.Add(_health);

            SetMenuPositions();
        }

        public void SetMenuPositions()
        {
            _mapOptions.SetSinglePosition(MenuSettings.MenuBottomOptionPosition);
            _battleOptions.SetOptionLayout(MenuSettings.GameSetupOptionsPosition, MenuSettings.GameSetupOptionsLastPosition);
            _wizardPosition = new Vector2(MenuSettings.GameSetupSettingsPosition, _battleOptions.OptionsLayout[0].Y);
            _teamSizePosition = new Vector2(_wizardPosition.X, _battleOptions.OptionsLayout[1].Y);
            _healthTextPosition = new Vector2(MenuSettings.GameSetupSettingsPosition, _battleOptions.OptionsLayout[2].Y);
            _mapPosition = MenuSettings.MapPosition;
        }

        public void LoadContent(ContentManager contentManager)
        {
            for (int i = 0; i < 4; i++)
            {
                _wizards.Add(new Sprite(contentManager, @"Menu/Wizard" + i));
                _wizards[i].SpriteScale = MenuSettings.WIZARD_SPRITE_SCALE;
            }

            for (int i = 0; i < MenuSettings.MapTitles.Count; i++)
            {
                _maps.Add(new Sprite(contentManager, @"Maps/Map" + i));
                _maps[i].SpriteScale = MenuSettings.MENU_MAPS_SCALE;
            }

            _mapPosition -= _maps[0].GetSpriteOrigin();
            _teamSize.LoadContent(contentManager, @"Menu/Wizard0");
            _teamSize.SetSpriteMeterColour(Color.Black);
            _teamSize.SetSpriteMeter(MenuSettings.TeamSizeSpriteSpan, MenuSettings.WIZARD_SPRITE_SCALE);
            _battleOptions.LoadContent(contentManager);
            _mapOptions.LoadContent(contentManager);
            _optionsFont = contentManager.Load<SpriteFont>(@"Fonts/OptionFont");
        }

        public void Update(GameTime gameTime)
        {
            if (_AreBattleOptionsSet)
            {
                _mapOptions.Update(gameTime);
                UpdateMapMenu();
            }
            else
            {
                _battleOptions.Update(gameTime);
                UpdateOptionsMenu();
            }

            if (InputManager.WasKeyPressed(Keys.Back) || InputManager.WasKeyPressed(Keys.Enter))
                SoundManager.Instance.PlaySound("stone0");
        }

        private void UpdateOptionsMenu()
        {
            if (InputManager.WasKeyPressed(Keys.Back))
                InGameSetup = false;

            int valueChange = InputManager.WasKeyPressed(Keys.Right) ? 1 : InputManager.WasKeyPressed(Keys.Left) ? -1 : 0;
            _battleSettings[_battleOptions.SelectedOption].ChangeValue(valueChange);

            if (valueChange != 0)
            {
                string soundEffect = "stone0";

                if (_battleOptions.SelectedOption == 0)
                    soundEffect = valueChange == 1 ? "wizardOh0" : "wizardOh1";

                else if (_battleOptions.SelectedOption == 1)
                    soundEffect = valueChange == 1 ? "wizardCast" : "wizardSad";

                else if (_battleOptions.SelectedOption == 2)
                    soundEffect = "potion";

                SoundManager.Instance.PlaySound(soundEffect);
            }

            if (InputManager.WasKeyPressed(Keys.Enter))
            {
                StoreSettings();
                _AreBattleOptionsSet = true;
                SoundManager.Instance.PlaySound("magicChord");
            }
        }

        private void UpdateMapMenu()
        {
            if (InputManager.WasKeyPressed(Keys.Back))
                _AreBattleOptionsSet = false;
            
            else if (InputManager.WasKeyPressed(Keys.Enter))
            {
                GameOptions.MapFile = @"Maps/Map" + _mapOptions.SelectedOption;
                _isMapSet = true;
                SoundManager.Instance.StopMediaPlayer();
                SoundManager.Instance.PlaySound("magic0");
            }
        }

        private void StoreSettings()
        {
            GameOptions.NumberOfTeams = _players.IntValue;
            GameOptions.TeamSize = _teamSize.IntValue;
            GameOptions.WizardHealth = _health.IntValue * WizardSettings.HEALTH_INTERVAL;
        }

        public void ResetGame()
        {
            GameOptions = new GameOptions();
            _isMapSet = false;
            _AreBattleOptionsSet = false;
            StateMachine.Instance.RestartGame();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_AreBattleOptionsSet)
                DrawMapOptions(spriteBatch);
            else
                DrawGameOptions(spriteBatch);
        }

        public void DrawGameOptions(SpriteBatch spriteBatch)
        {
            _battleOptions.DrawOptions(spriteBatch);
 
            for (int i = 0; i < _players.MaxValue; i++)
            {
                _wizards[i].SpriteColour = _players.IntValue <= i ? Colours.GreyedOut : Color.White;
                _wizards[i].DrawSprite(spriteBatch, _wizardPosition + new Vector2(i * 80, 0));
            }

            _teamSize.DrawSetting(spriteBatch, _teamSizePosition);
            spriteBatch.DrawString(_optionsFont, (_health.IntValue * WizardSettings.HEALTH_INTERVAL).ToString(), _healthTextPosition, Color.White);
        }

        private void DrawMapOptions(SpriteBatch spriteBatch)
        {
            _mapOptions.DrawOptions(spriteBatch);
            _maps[_mapOptions.SelectedOption].DrawSprite(spriteBatch, _mapPosition);
        }
    }
}
