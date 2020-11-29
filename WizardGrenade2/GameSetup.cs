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

        private BattleSettings _battleSettings = new BattleSettings();

        private int[,] _battleOptionValues = new int[3, 3]
        {
            {2, 2, 4 },
            {3, 1, 8 },
            {4, 1, 10 }
        };

        public GameSetup()
        {
            GameOptions = new GameOptions();
            _maps = new List<Sprite>();
            _wizards = new List<Sprite>();
            _battleOptions = new Options(_battleOptionNames, true, true);
            _mapOptions = new Options(_mapNames, true, false);
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
                _wizards.Add(new Sprite(contentManager, "Wizard" + i));
                _wizards[i].SpriteScale = 2f;
            }

            for (int i = 0; i < 5; i++)
            {
                _maps.Add(new Sprite(contentManager, "Map" + i));
                _maps[i].SpriteScale = 0.5f;
            }

            _battleOptions.LoadContent(contentManager);
            _mapOptions.LoadContent(contentManager);

            _optionsFont = contentManager.Load<SpriteFont>("OptionFont");
            _infoFont = contentManager.Load<SpriteFont>("InfoFont2");
            _title = new Sprite(contentManager, "Title");
            _arrow = new Sprite(contentManager, "MelfsAcidArrow");
            _arrow.SpriteScale = ARROW_SCALE;
            _arrowRPosition.X -= _arrow.GetSpriteRectangle().Width * ARROW_SCALE;
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
        }

        private void UpdateOptionsMenu()
        {
            if (InputManager.WasKeyPressed(Keys.Back))
                SettingUpGame = false;

            int valueChange = InputManager.WasKeyPressed(Keys.Right) ? 1 : InputManager.WasKeyPressed(Keys.Left) ? -1 : 0 ;

            _battleOptionValues[_battleOptions.SelectedOption, 0] = Utility.ChangeValueInLimits(
                _battleOptionValues[_battleOptions.SelectedOption, 0] + valueChange,
                _battleOptionValues[_battleOptions.SelectedOption, 1],
                _battleOptionValues[_battleOptions.SelectedOption, 2]);

            if (InputManager.WasKeyPressed(Keys.Enter))
            {
                StoreSettings();
                _AreBattleOptionsSet = true;
            }
        }

        private void UpdateMapMenu()
        {
            if (InputManager.WasKeyPressed(Keys.Back))
                _AreBattleOptionsSet = false;
            
            else if (InputManager.WasKeyPressed(Keys.Enter))
            {
                GameOptions.MapFile = "Map" + _mapOptions.SelectedOption;
                _isMapSet = true;
            }
        }

        private void StoreSettings()
        {
            GameOptions.NumberOfTeams = _battleOptionValues[0, 0];
            GameOptions.TeamSize = _battleOptionValues[1, 0];
            GameOptions.WizardHealth = _battleOptionValues[2, 0] * HEALTH_INTERVAL;
        }

        public void ResetGame()
        {
            _isMapSet = false;
            _AreBattleOptionsSet = false;
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
                _wizards[i].SpriteColour = _battleOptionValues[0, 0] <= i ? Colours.GreyedOut : Color.White;
                _wizards[i].DrawSprite(spriteBatch, _wizardPosition + new Vector2(i * 80, 0));
            }

            _wizardPosition.Y = _battleOptions.OptionsLayout[1].Y;
            _wizards[0].SpriteColour = Color.Black;

            for (int i = 0; i < _battleOptionValues[1, 0]; i++)
                _wizards[0].DrawSprite(spriteBatch, _wizardPosition + new Vector2(i * 80, 0));

            spriteBatch.DrawString(_optionsFont, (_battleOptionValues[2, 0] * HEALTH_INTERVAL).ToString(), _healthTextPosition, Color.White);
        }

        private void DrawMapOptions(SpriteBatch spriteBatch)
        {
            _mapOptions.DrawOptions(spriteBatch);
            _maps[_mapOptions.SelectedOption].DrawSprite(spriteBatch, _mapPosition);
        }
    }
}
