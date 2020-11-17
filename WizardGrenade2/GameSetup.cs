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
        private Sprite _title;
        private Vector2 _titlePosition = new Vector2(ScreenSettings.CentreScreenWidth, 100f);
        private Vector2 _mapPosition = new Vector2(300, 300);
        private Vector2 _mapTitlePosition = new Vector2(550, 230);
        private List<Sprite> _wizards = new List<Sprite>();
        private List<Sprite> _maps = new List<Sprite>();
        private bool _isOptionsSet;
        private bool _isMapSet;
        private OptionArrows _arrows = new OptionArrows();


        private const int HEALTH_INTERVAL = 25;
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
        }

        public void Initialise()
        {
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
            _title.SpriteScale = 0.4f;
            _titlePosition -= _title.GetSpriteOrigin();
        }

        private void SetArrowPositions()
        {
            Vector2 optionPosition = _isOptionsSet ? _mapTitlePosition : GameOptions.OptionsLayout[_selectedOption];
            optionPosition.Y += 28;
            optionPosition.X += 60;
            float width = 220;
            _arrows.SetPositions(optionPosition, width);
        }

        public void Update(GameTime gameTime)
        {
            if (_isOptionsSet)
                UpdateMapMenu();
            else
                UpdateOptionsMenu();

            _arrows.Update(gameTime);
        }

        private void UpdateOptionsMenu()
        {
            int valueDelta = 0;

            if (InputManager.WasKeyPressed(Keys.Down))
            {
                _selectedOption = ChangeValue(_selectedOption + 1, 0, _optionSettings.Length);
                SetArrowPositions();
            }
            else if (InputManager.WasKeyPressed(Keys.Up))
            {
                _selectedOption = ChangeValue(_selectedOption - 1, 0, _optionSettings.Length);
                SetArrowPositions();
            }

            if (InputManager.WasKeyPressed(Keys.Right))
                valueDelta = 1;
            else if (InputManager.WasKeyPressed(Keys.Left))
                valueDelta = -1;

            _optionSettings[_selectedOption, 0] = ChangeValue(_optionSettings[_selectedOption, 0] + valueDelta,
                _optionSettings[_selectedOption, 1],
                _optionSettings[_selectedOption, 2]);

            if (InputManager.WasKeyPressed(Keys.Enter))
            {
                StoreSettings();
                _isOptionsSet = true;
                SetArrowPositions();
            }
        }

        private void UpdateMapMenu()
        {
            if (InputManager.WasKeyPressed(Keys.Right))
                _selectedMap = Utility.WrapAroundCounter(_selectedMap, _maps.Count);
            else if (InputManager.WasKeyPressed(Keys.Left))
                _selectedMap = Utility.WrapAroundNegativeCounter(_selectedMap, _maps.Count);

            if (InputManager.WasKeyPressed(Keys.Back))
            {
                _isOptionsSet = false;
                SetArrowPositions();
            }
            
            else if (InputManager.WasKeyPressed(Keys.Enter))
                _isMapSet = true;
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
            _title.DrawSprite(spriteBatch, _titlePosition);
            if (!_isMapSet)
            {
                _arrows.Draw(spriteBatch);

                if (_isOptionsSet)
                    DrawMapOptions(spriteBatch);
                else
                    DrawGameOptions(spriteBatch);
            }
        }

        public void DrawGameOptions(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < GameOptions.options.Count; i++)
            {
                Color selected = i == _selectedOption ? Color.Yellow : Color.DimGray;

                spriteBatch.DrawString(_optionsFont, GameOptions.options[i], GameOptions.OptionsLayout[i], selected);
            }

            Vector2 wizardPosition = new Vector2(ScreenSettings.TARGET_WIDTH / 2.3f, GameOptions.OptionsLayout[0].Y);
            _wizards[0].SpriteColour = Color.White;
            for (int i = 0; i < 4; i++)
            {
                _wizards[i].SpriteColour = _optionSettings[0, 0] <= i ? Color.DarkSlateGray : Color.White;
                _wizards[i].DrawSprite(spriteBatch, wizardPosition);
                wizardPosition.X += 80;
            }

            wizardPosition = new Vector2(ScreenSettings.TARGET_WIDTH / 2.3f, GameOptions.OptionsLayout[1].Y);
            _wizards[0].SpriteColour = Color.Black;
            for (int i = 0; i < _optionSettings[1, 0]; i++)
            {
                _wizards[0].DrawSprite(spriteBatch, wizardPosition);
                wizardPosition.X += 80;
            }

            Vector2 healthPosition = new Vector2(ScreenSettings.TARGET_WIDTH / 2.3f, GameOptions.OptionsLayout[2].Y);
            spriteBatch.DrawString(_optionsFont, (_optionSettings[2, 0] * HEALTH_INTERVAL).ToString(), healthPosition, Color.White);
        }

        private void DrawMapOptions(SpriteBatch spriteBatch)
        {
            _maps[_selectedMap].DrawSprite(spriteBatch, _mapPosition);
            spriteBatch.DrawString(_optionsFont, GameOptions.mapNames[_selectedMap], new Vector2(550, 230), Color.Yellow);
        }
    }
}
