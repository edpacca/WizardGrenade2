using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace WizardGrenade2
{
    public class Options
    {
        public List<Vector2> OptionsLayout { get; private set; }
        public List<string> OptionNames { get; private set; }
        private OptionArrows _arrows;
        private int _numberOfOptions;
        public int SelectedOption { get; private set; }
        private SpriteFont _optionFont;
        private Vector2 _firstOptionPosition = Vector2.Zero;
        private float _lastOptionPositionY = ScreenSettings.TARGET_HEIGHT;
        private Color _selectedColour = Colours.Gold;
        private Color _unselectedColour = Color.DimGray;

        public Options(List<string> optionNames, bool _doubleArrow)
        {
            OptionNames = optionNames;
            _numberOfOptions = OptionNames.Count;
            OptionsLayout = new List<Vector2>();
            ApplyOptionLayout();
            _arrows = new OptionArrows(false);
        }

        public void LoadContent(ContentManager contentManager)
        {
            _optionFont = contentManager.Load<SpriteFont>("OptionFont");
            _arrows.LoadContent(contentManager);
            SetArrowPositions();
        }

        public void SetOptionLayout(Vector2 firstOptionPosition, float lastOptionY)
        {
            _firstOptionPosition = firstOptionPosition;
            _lastOptionPositionY = lastOptionY;
            ApplyOptionLayout();
        }

        public void SetOptionColours(Color selected, Color unselected)
        {
            _selectedColour = selected;
            _unselectedColour = unselected;
        }

        private void ApplyOptionLayout()
        {
            OptionsLayout.Clear();
            float verticalInterval = (_lastOptionPositionY - _firstOptionPosition.Y) / (_numberOfOptions - 1);

            for (int i = 0; i < _numberOfOptions; i++)
                OptionsLayout.Add(new Vector2(_firstOptionPosition.X, _firstOptionPosition.Y + (verticalInterval * i)));
        }

        private void SetArrowPositions()
        {
            Vector2 fontSize = _optionFont.MeasureString(OptionNames[SelectedOption]);
            Vector2 _optionArrowOffset = new Vector2(-25, fontSize.Y / 2);
            _arrows.SetPositions(OptionsLayout[SelectedOption] + _optionArrowOffset, fontSize.X + 47);
        }

        public void Update(GameTime gameTime)
        {
            ChangeOption();
            _arrows.Update1(gameTime);
        }

        public void ChangeOption()
        {
            int difference = InputManager.WasKeyPressed(Keys.Down) ? 1 : InputManager.WasKeyPressed(Keys.Up) ? -1 : 0;
            SelectedOption = Utility.ChangeValueInLimits(SelectedOption + difference, 0, _numberOfOptions - 1);
            
            if (difference != 0)
                SetArrowPositions();
        }

        public void DrawOptions(SpriteBatch spriteBatch)
        {
            _arrows.Draw(spriteBatch);

            for (int i = 0; i < _numberOfOptions; i++)
            {
                Color colour = i == SelectedOption ? _selectedColour : _unselectedColour;
                spriteBatch.DrawString(_optionFont, OptionNames[i], OptionsLayout[i], colour);
            }
        }
    }
}
