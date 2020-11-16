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
        private List<Sprite> _wizards = new List<Sprite>();

        private const int HEALTH_INTERVAL = 25;
        private int _selectedOption = 0;
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


            _optionsFont = contentManager.Load<SpriteFont>("TimerFont");
            _title = new Sprite(contentManager, "Title");
        }

        public void Update(GameTime gameTime)
        {
            int optionDelta = 0;
            int valueDelta = 0;

            if (InputManager.WasKeyPressed(Keys.Down))
                optionDelta = 1;
            else if (InputManager.WasKeyPressed(Keys.Up))
                optionDelta = -1;

            _selectedOption = ChangeValue(_selectedOption + optionDelta, 0, _optionSettings.Length);

            if (InputManager.WasKeyPressed(Keys.Right))
                valueDelta = 1;
            else if (InputManager.WasKeyPressed(Keys.Left))
                valueDelta = -1;

            _optionSettings[_selectedOption, 0] = ChangeValue(_optionSettings[_selectedOption, 0] + valueDelta,
                _optionSettings[_selectedOption, 1],
                _optionSettings[_selectedOption, 2]);
        }

        private int ChangeValue(int nextValue, int minValue, int maxValue)
        {
            return nextValue < minValue ? minValue : nextValue > maxValue ? maxValue : nextValue;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _title.DrawSpriteAtScale(spriteBatch, _titlePosition, 0.4f);

            for (int i = 0; i < GameOptions.Options.Count; i++)
            {
                Color selected = i == _selectedOption ? Color.Red : Color.Yellow;

                spriteBatch.DrawString(_optionsFont, GameOptions.Options[i], GameOptions.OptionsLayout[i], selected);
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
    }
}
