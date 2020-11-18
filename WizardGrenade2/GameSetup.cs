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
        private SpriteFont _optionsFont;
        private OptionArrows _arrows;

        private Sprite _title;
        private Vector2 _titlePosition;

        private const float MAP_SCALE = 0.5f;
        private Vector2 _mapTextPosition;
        private Vector2 _mapPosition;
        private List<Sprite> _maps;

        private List<Sprite> _wizards;
        Vector2 _wizardPosition;
        private Color _greyedOut = new Color(35, 35, 35);

        private const int HEALTH_INTERVAL = 25;
        private Vector2 _healthTextPosition;

        private bool _isNewOptionSelected;
        private bool _isOptionsSet;
        private bool _isMapSet;

        private int _selectedOption = 0;
        private int _selectedMap = 0;
        private int[,] _optionSettings = new int[3, 3]
        {
            {2, 2, 4 },
            {3, 1, 8},
            {4, 1, 10}
        };

        public GameSetup()
        {
            GameOptions = new GameOptions();
            _arrows = new OptionArrows();
            _maps = new List<Sprite>();
            _wizards = new List<Sprite>();
            SetMenuPositions();
        }

        public bool isGameSetup()
        {
            return _isMapSet;
        }

        public void SetMenuPositions()
        {
            _titlePosition = new Vector2(ScreenSettings.CentreScreenWidth, ScreenSettings.TARGET_HEIGHT / 6);
            _mapTextPosition = new Vector2(ScreenSettings.CentreScreenWidth, ScreenSettings.TARGET_HEIGHT * 0.9f);
            _mapPosition = ScreenSettings.ScreenCentre * MAP_SCALE + new Vector2(0, ScreenSettings.TARGET_HEIGHT / 10);
            _wizardPosition = new Vector2(ScreenSettings.TARGET_WIDTH / 2.3f, GameOptions.OptionsLayout[1].Y);
            _healthTextPosition = new Vector2(ScreenSettings.TARGET_WIDTH / 2.3f, GameOptions.OptionsLayout[2].Y);
        }

        private void SetArrowPositions()
        {
            Vector2 fontWidth = _isOptionsSet ?
            _optionsFont.MeasureString(GameOptions.mapNames[_selectedMap]) :
            _optionsFont.MeasureString(GameOptions.options[_selectedOption]);

            Vector2 optionPosition = _isOptionsSet ? _mapTextPosition - (fontWidth / 2) : GameOptions.OptionsLayout[_selectedOption];

            optionPosition.Y += fontWidth.Y / 2;
            optionPosition.X -= 20;
            _arrows.SetPositions(optionPosition, fontWidth.X + 37);
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

            _optionsFont = contentManager.Load<SpriteFont>("TimerFont");
            _arrows.LoadContent(contentManager);
            SetArrowPositions();

            _title = new Sprite(contentManager, "Title");
        }

        public void Update(GameTime gameTime)
        {
            if (_isOptionsSet)
                UpdateMapMenu();
            else
                UpdateOptionsMenu();

            if (_isNewOptionSelected)
            {
                SetArrowPositions();
                _isNewOptionSelected = false;
            }
            _arrows.Update(gameTime);
        }

        private void UpdateOptionsMenu()
        {
            int optionChange = InputManager.WasKeyPressed(Keys.Down) ? 1 : InputManager.WasKeyPressed(Keys.Up) ? -1 : 0;
            _selectedOption = ChangeValue(_selectedOption + optionChange, 0, _optionSettings.Length);

            _isNewOptionSelected = InputManager.WasKeyPressed(Keys.Down) || InputManager.WasKeyPressed(Keys.Up) ? true : false;

            int valueChange = InputManager.WasKeyPressed(Keys.Right) ? 1 : InputManager.WasKeyPressed(Keys.Left) ? -1 : 0 ;

            _optionSettings[_selectedOption, 0] = ChangeValue(_optionSettings[_selectedOption, 0] + valueChange,
                _optionSettings[_selectedOption, 1],
                _optionSettings[_selectedOption, 2]);

            if (InputManager.WasKeyPressed(Keys.Enter))
            {
                StoreSettings();
                _isOptionsSet = true;
                _isNewOptionSelected = true;
            }
        }

        private void UpdateMapMenu()
        {
            _selectedMap =
                InputManager.WasKeyPressed(Keys.Right) ? Utility.WrapAroundCounter(_selectedMap, _maps.Count) :
                InputManager.WasKeyPressed(Keys.Left) ? Utility.WrapAroundNegativeCounter(_selectedMap, _maps.Count) : _selectedMap;

            _isNewOptionSelected = InputManager.WasKeyPressed(Keys.Left) || InputManager.WasKeyPressed(Keys.Right) ? true : false;

            if (InputManager.WasKeyPressed(Keys.Back))
            {
                _isOptionsSet = false;
                _isNewOptionSelected = true;
            }
            
            else if (InputManager.WasKeyPressed(Keys.Enter))
            {
                GameOptions.MapFile = "Map" + _selectedMap;
                _isMapSet = true;
            }
        }

        private int ChangeValue(int nextValue, int minValue, int maxValue)
        {
            return nextValue < minValue ? minValue : nextValue > maxValue ? maxValue : nextValue;
        }

        private void StoreSettings()
        {
            GameOptions.NumberOfTeams = _optionSettings[0, 0];
            GameOptions.TeamSize = _optionSettings[1, 0];
            GameOptions.WizardHealth = _optionSettings[2, 0] * HEALTH_INTERVAL;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _title.DrawSpriteAtScale(spriteBatch, _titlePosition, 0.4f);

            if (_isOptionsSet)
                DrawMapOptions(spriteBatch);
            else
                DrawGameOptions(spriteBatch);

            _arrows.Draw(spriteBatch);
        }

        public void DrawGameOptions(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < GameOptions.options.Count; i++)
            {
                Color selected = i == _selectedOption ? Color.Yellow : Color.DimGray;
                spriteBatch.DrawString(_optionsFont, GameOptions.options[i], GameOptions.OptionsLayout[i], selected);
            }

            _wizardPosition.Y = GameOptions.OptionsLayout[0].Y;
            _wizards[0].SpriteColour = Color.White;

            for (int i = 0; i < 4; i++)
            {
                _wizards[i].SpriteColour = _optionSettings[0, 0] <= i ? _greyedOut : Color.White;
                _wizards[i].DrawSprite(spriteBatch, _wizardPosition + new Vector2(i * 80, 0));
            }

            _wizardPosition.Y = GameOptions.OptionsLayout[1].Y;
            _wizards[0].SpriteColour = Color.Black;

            for (int i = 0; i < _optionSettings[1, 0]; i++)
            {
                _wizards[0].DrawSprite(spriteBatch, _wizardPosition + new Vector2(i * 80, 0));
            }

            spriteBatch.DrawString(_optionsFont, (_optionSettings[2, 0] * HEALTH_INTERVAL).ToString(), _healthTextPosition, Color.White);
        }

        private void DrawMapOptions(SpriteBatch spriteBatch)
        {
            Vector2 textLength = _optionsFont.MeasureString(GameOptions.mapNames[_selectedMap]);
            _maps[_selectedMap].DrawSprite(spriteBatch, _mapPosition);
            spriteBatch.DrawString(_optionsFont, GameOptions.mapNames[_selectedMap], _mapTextPosition - (textLength / 2), Color.Yellow);
        }
    }
}
