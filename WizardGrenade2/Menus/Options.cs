using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace WizardGrenade2
{
    public class Options
    {
        public List<Vector2> OptionsLayout { get; private set; }
        public List<string> OptionNames { get; private set; }
        public int SelectedOption { get; private set; }
        private SpriteFont _optionFont;
        private OptionArrows _arrows;
        private int _numberOfOptions;

        private bool _isLayoutVertical;
        private Vector2 _firstOptionPosition = Vector2.Zero;
        private float _lastOptionPositionY = ScreenSettings.TARGET_HEIGHT;

        private Color _selectedColour = Colours.Gold;
        private Color _unselectedColour = Colours.LightGreenBlue;

        public Options(List<string> optionNames, bool doubleArrow, bool verticalLayout)
        {
            OptionNames = optionNames;
            _numberOfOptions = OptionNames.Count;
            _isLayoutVertical = verticalLayout;
            OptionsLayout = new List<Vector2>();

            if (_isLayoutVertical)
                ApplyVerticalOptionLayout();

            _arrows = new OptionArrows(doubleArrow);
        }

        public void LoadContent(ContentManager contentManager)
        {
            _optionFont = contentManager.Load<SpriteFont>("OptionFont");

            if (_isLayoutVertical)
                ApplyVerticalOptionLayout();
            else
                ApplySingleOptionLayout();

            _arrows.LoadContent(contentManager);
            SetArrowPositions();
        }

        public void SetOptionLayout(Vector2 firstOptionPosition, float lastOptionY)
        {
            _firstOptionPosition = firstOptionPosition;
            _lastOptionPositionY = lastOptionY;
            ApplyVerticalOptionLayout();
        }

        public void SetSinglePosition(Vector2 centre) => _firstOptionPosition = centre;
        public void SetOptionColours(Color selected) => _selectedColour = selected;

        public void SetOptionColours(Color selected, Color unselected)
        {
            _selectedColour = selected;
            _unselectedColour = unselected;
        }

        private void ApplyVerticalOptionLayout()
        {
            OptionsLayout.Clear();
            float verticalInterval = (_lastOptionPositionY - _firstOptionPosition.Y) / (_numberOfOptions - 1);

            for (int i = 0; i < _numberOfOptions; i++)
                OptionsLayout.Add(new Vector2(_firstOptionPosition.X, _firstOptionPosition.Y + (verticalInterval * i)));
        }

        private void ApplySingleOptionLayout()
        {
            OptionsLayout.Clear();
            for (int i = 0; i < _numberOfOptions; i++)
            {
                Vector2 textSize = _optionFont.MeasureString(OptionNames[i]) / 2;
                OptionsLayout.Add(_firstOptionPosition - textSize);
            }
        }

        private void SetArrowPositions()
        {
            Vector2 fontSize = _optionFont.MeasureString(OptionNames[SelectedOption]);
            Vector2 _optionArrowOffset = new Vector2(-25, fontSize.Y / 2);
            _arrows.SetPositions(OptionsLayout[SelectedOption] + _optionArrowOffset, fontSize.X + 47);
        }

        public void Update(GameTime gameTime)
        {
            if (_isLayoutVertical)
                ChangeVerticalOption();
            else
                CycleInPlaceOption();

            _arrows.Update(gameTime);
        }

        private void ChangeVerticalOption()
        {
            int difference = InputManager.WasKeyPressed(Keys.Down) ? 1 : InputManager.WasKeyPressed(Keys.Up) ? -1 : 0;
            SelectedOption = Utility.ChangeValueInLimits(SelectedOption + difference, 0, _numberOfOptions - 1);
            
            if (difference != 0)
            {
                SetArrowPositions();
                SoundManager.Instance.PlaySound("stone1");
            }
        }

        private void CycleInPlaceOption()
        {
            if (InputManager.WasKeyPressed(Keys.Right))
            {
                SelectedOption = Utility.WrapAroundCounter(SelectedOption, _numberOfOptions);
                SetArrowPositions();
                SoundManager.Instance.PlaySound("stone1");
            }
            else if (InputManager.WasKeyPressed(Keys.Left))
            {
                SelectedOption = Utility.WrapAroundNegativeCounter(SelectedOption, _numberOfOptions);
                SetArrowPositions();
                SoundManager.Instance.PlaySound("stone1");
            }
        }

        public void DrawOptions(SpriteBatch spriteBatch)
        {
            _arrows.Draw(spriteBatch);

            if (_isLayoutVertical)
                DrawVertical(spriteBatch);
            else
                DrawSinglePosition(spriteBatch);
        }

        private void DrawVertical(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _numberOfOptions; i++)
            {
                Color colour = i == SelectedOption ? _selectedColour : _unselectedColour;
                spriteBatch.DrawString(_optionFont, OptionNames[i], OptionsLayout[i], colour);
            }
        }

        private void DrawSinglePosition(SpriteBatch spriteBatch)
        {
            Vector2 textSize = _optionFont.MeasureString(OptionNames[SelectedOption]) / 2;
            spriteBatch.DrawString(_optionFont, OptionNames[SelectedOption], _firstOptionPosition - textSize, _selectedColour);
        }
    }
}
