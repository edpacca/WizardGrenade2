using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using WizardGrenade2.Generics;

namespace WizardGrenade2
{
    class GameSetup
    {
        public GameOptions GameOptions { get; private set; }
        public bool SettingUpGame { get; set; }
        private SpriteFont _optionsFont;
        private SpriteFont _infoFont;
        private Sprite _title;
        private Sprite _arrow;
        private Vector2 _titlePosition;
        private Vector2 _mapPosition;
        private Vector2 _arrowLPosition;
        private Vector2 _arrowRPosition;
        private Vector2 _healthTextPosition;
        private Vector2 _wizardPosition;
        private const float MAP_SCALE = 0.5f;
        private const float ARROW_SCALE = 6f;
        private const int HEALTH_INTERVAL = 25;
        private List<Sprite> _maps;
        private List<Sprite> _wizards;
        private Options _battleOptions;
        private Options _mapOptions;
        private bool _AreBattleOptionsSet;
        private bool _isMapSet;
        private readonly List<string> _battleOptionNames = new List<string> { "Players", "Wizards", "Health" };
        private readonly List<string> _mapNames = new List<string> { "Castle", "Two-Towers", "City", "Clouds", "Arena" };

        private List<Setting> _battleSettings;
        private Setting _players;
        private Setting _teamSize;
        private Setting _health;

        public GameSetup()
        {
            GameOptions = new GameOptions();
            _maps = new List<Sprite>();
            _wizards = new List<Sprite>();
            _battleOptions = new Options(_battleOptionNames, true, true);
            _mapOptions = new Options(_mapNames, true, false);

            _players = new Setting(2, 2, 4);
            _teamSize = new Setting(3, 1, 8);
            _health = new Setting(4, 1, 10);
            _battleSettings = new List<Setting>();
            _battleSettings.Add(_players);
            _battleSettings.Add(_teamSize);
            _battleSettings.Add(_health);

            SetMenuPositions();
        }

        public bool isGameSetup() => _isMapSet && _AreBattleOptionsSet;

        public void SetMenuPositions()
        {
            _titlePosition = new Vector2(ScreenSettings.CentreScreenWidth, ScreenSettings.TARGET_HEIGHT / 6);
            _mapOptions.SetSinglePosition(new Vector2(ScreenSettings.CentreScreenWidth, ScreenSettings.TARGET_HEIGHT * 0.9f));
            _battleOptions.SetOptionLayout(new Vector2(ScreenSettings.TARGET_WIDTH / 5, _titlePosition.Y + 150), ScreenSettings.TARGET_HEIGHT * 0.8f);
            _mapPosition = ScreenSettings.ScreenCentre * MAP_SCALE + new Vector2(0, ScreenSettings.TARGET_HEIGHT / 10);
            _wizardPosition = new Vector2(ScreenSettings.TARGET_WIDTH / 2.3f, _battleOptions.OptionsLayout[0].Y);
            _healthTextPosition = new Vector2(ScreenSettings.TARGET_WIDTH / 2.3f, _battleOptions.OptionsLayout[2].Y);
            _arrowLPosition = new Vector2(20, 20);
            _arrowRPosition = new Vector2(ScreenSettings.TARGET_WIDTH - 20, 20);
        }

        public void LoadContent(ContentManager contentManager)
        {
            for (int i = 0; i < 4; i++)
            {
                _wizards.Add(new Sprite(contentManager, @"Menu/Wizard" + i));
                _wizards[i].SpriteScale = 2f;
            }

            for (int i = 0; i < 5; i++)
            {
                _maps.Add(new Sprite(contentManager, @"Maps/Map" + i));
                _maps[i].SpriteScale = 0.5f;
            }

            _battleOptions.LoadContent(contentManager);
            _mapOptions.LoadContent(contentManager);

            _optionsFont = contentManager.Load<SpriteFont>(@"Fonts/OptionFont");
            _infoFont = contentManager.Load<SpriteFont>(@"Fonts/InfoFont2");
            _title = new Sprite(contentManager, @"Menu/Title");
            _arrow = new Sprite(contentManager, @"GameObjects/MelfsAcidArrow");
            _arrow.SpriteScale = ARROW_SCALE;
            _arrowRPosition.X -= _arrow.GetSpriteRectangle().Width;
        }

        public void Update(GameTime gameTime)
        {
            if (InputManager.WasKeyPressed(Keys.Back) || InputManager.WasKeyPressed(Keys.Enter))
                SoundManager.Instance.PlaySound("stone0");

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
        }

        private void UpdateOptionsMenu()
        {
            if (InputManager.WasKeyPressed(Keys.Back))
                SettingUpGame = false;

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
            GameOptions.WizardHealth = _health.IntValue * HEALTH_INTERVAL;
        }

        public void ResetGame()
        {
            _isMapSet = false;
            _AreBattleOptionsSet = false;
            StateMachine.Instance.RestartGame();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _arrow.DrawSprite(spriteBatch, _arrowLPosition + _arrow.GetSpriteOrigin(), Mechanics.PI);
            _arrow.DrawSprite(spriteBatch, _arrowRPosition);
            spriteBatch.DrawString(_infoFont, "BACKSPACE", new Vector2(_arrowLPosition.X - 7, _arrowLPosition.Y + 40), Colours.LightGrey);
            spriteBatch.DrawString(_infoFont, "ENTER", new Vector2(_arrowRPosition.X + 5, _arrowRPosition.Y + 40), Colours.LightGrey);

            _title.DrawSpriteAtScale(spriteBatch, _titlePosition, 0.4f);

            if (_AreBattleOptionsSet)
                DrawMapOptions(spriteBatch);
            else
                DrawGameOptions(spriteBatch);
        }

        public void DrawGameOptions(SpriteBatch spriteBatch)
        {

            _battleOptions.DrawOptions(spriteBatch);

            _wizardPosition.Y = _battleOptions.OptionsLayout[0].Y;
            _wizards[0].SpriteColour = Color.White;

            for (int i = 0; i < 4; i++)
            {
                _wizards[i].SpriteColour = _players.IntValue <= i ? Colours.GreyedOut : Color.White;
                _wizards[i].DrawSprite(spriteBatch, _wizardPosition + new Vector2(i * 80, 0));
            }

            _wizardPosition.Y = _battleOptions.OptionsLayout[1].Y;
            _wizards[0].SpriteColour = Color.Black;

            for (int i = 0; i < _teamSize.IntValue; i++)
                _wizards[0].DrawSprite(spriteBatch, _wizardPosition + new Vector2(i * 80, 0));

            spriteBatch.DrawString(_optionsFont, (_health.IntValue * HEALTH_INTERVAL).ToString(), _healthTextPosition, Color.White);
        }

        private void DrawMapOptions(SpriteBatch spriteBatch)
        {
            _mapOptions.DrawOptions(spriteBatch);
            _maps[_mapOptions.SelectedOption].DrawSprite(spriteBatch, _mapPosition);
        }
    }
}
