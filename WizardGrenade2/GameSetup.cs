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

        private int _optionIndex = 0;
        private const int MIN_TEAMS = 2;
        private const int MAX_TEAMS = 4;
        private const int MIN_WIZARDS = 1;
        private const int MAX_WIZARDS = 1;
        private const int HEALTH_INTERVAL = 25;
        private const int MIN_HEALTH_INTERVALS = 1;
        private const int MAX_HEALTH_INTERVALS = 10;

        private int[] _optionValues;

        public GameSetup()
        {
            GameOptions = new GameOptions();
            _optionValues = new int[GameOptions.Options.Count];
            _optionValues[0] = MIN_TEAMS;
            _optionValues[1] = 3;
            _optionValues[2] = 100;
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
            int deltaValue = 0;

            //if (InputManager.WasKeyPressed(Keys.Up))
            //    _optionIndex = ChangeValue(_optionIndex + 1, 0, _optionValues.Length - 1);
            //else if (InputManager.WasKeyPressed(Keys.Down))
            //    _optionIndex = ChangeValue(_optionIndex - 1, 0, _optionValues.Length - 1);
            //else if (InputManager.WasKeyPressed(Keys.Right))
            //    deltaValue = 1;
            //else if (InputManager.WasKeyPressed(Keys.Left))
            //    deltaValue = -1;

                //_optionValues[_optionIndex] = ChangeValue(_optionValues[_optionIndex] + deltaValue, 0, 10);

            if (InputManager.WasKeyPressed(Keys.Right))
                _optionValues[1]++;
            else if (InputManager.WasKeyPressed(Keys.Left))
                _optionValues[1]--;

            _optionValues[1] = ChangeValue(_optionValues[1], 1, 8);
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
                spriteBatch.DrawString(_optionsFont, GameOptions.Options[i], GameOptions.OptionsLayout[i], Color.Yellow);
            }

            Vector2 wizardPosition = new Vector2(ScreenSettings.TARGET_WIDTH / 2.3f, GameOptions.OptionsLayout[0].Y);
            _wizards[0].SpriteColour = Color.White;
            for (int i = 0; i < 4; i++)
            {
                _wizards[i].DrawSprite(spriteBatch, wizardPosition);
                wizardPosition.X += 80;
            }

            wizardPosition = new Vector2(ScreenSettings.TARGET_WIDTH / 2.3f, GameOptions.OptionsLayout[1].Y);
            _wizards[0].SpriteColour = Color.Black;
            for (int i = 0; i < _optionValues[1]; i++)
            {
                _wizards[0].DrawSprite(spriteBatch, wizardPosition);
                wizardPosition.X += 80;
            }
        }
    }
}
