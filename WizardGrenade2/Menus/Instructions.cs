using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WizardGrenade2
{
    class Instructions
    {
        public bool InInstructions { get; set; }

        private Options _instructions;
        private SpriteFont _infoFont;
        private List<Sprite> _images;
        private List<List<string>> _instructionScripts;
        private Vector2 _imagePosition;

        public Instructions()
        {
            _instructions = new Options(InstructionsScript.InstructionsList, true, false);
            _instructions.SetSinglePosition(MenuSettings.MenuBottomOptionPosition);
            _instructionScripts = InstructionsScript.InstructionScripts;
            _imagePosition = MenuSettings.InstructionsImagePosition;
        }

        public void LoadContent(ContentManager contentManager)
        {
            _instructions.LoadContent(contentManager);
            _infoFont = contentManager.Load<SpriteFont>(@"Fonts/InfoFont");
            
            _images = new List<Sprite>();
            _images.Add(new Sprite(contentManager, @"GameObjects/Fireball"));
            _images.Add(new Sprite(contentManager, @"GameObjects/MelfsAcidArrow"));
            _images.Add(new Sprite(contentManager, @"GameObjects/IceBomb"));
            _images.Add(new Sprite(contentManager, @"GameObjects/Potions", 3, 1));

            for (int i = 0; i < _images.Count; i++)
                _images[i].SpriteScale = 10f;
            
            _images[3].SpriteScale = 6f;
        }

        public void Update(GameTime gameTime)
        {
            if (InputManager.WasKeyPressed(Keys.Back))
                InInstructions = false;

            _instructions.Update(gameTime);
            _imagePosition.Y += ((float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * MenuSettings.ITEM_OSCILLATION_RATE)) * MenuSettings.ITEM_OSCILLATION_AMPLITUDE;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 position = MenuSettings.InstructionsTextPosition;
            Vector2 textSize;

            foreach (var line in _instructionScripts[_instructions.SelectedOption])
            {
                textSize = _infoFont.MeasureString(line) / 2;
                spriteBatch.DrawString(_infoFont, line, position - textSize, Colours.Gold);
                position.Y += textSize.Y * 2f;
            }

            if (_instructions.OptionNames[_instructions.SelectedOption] == "Fireball")
                _images[0].DrawSprite(spriteBatch, _imagePosition - _images[0].Origin);

            else if (_instructions.OptionNames[_instructions.SelectedOption] == "Melf's Acid Arrow")
                _images[1].DrawSprite(spriteBatch, _imagePosition - _images[1].Origin);

            else if (_instructions.OptionNames[_instructions.SelectedOption] == "Ice Bomb")
                _images[2].DrawSprite(spriteBatch, _imagePosition - _images[2].Origin);

            else if (_instructions.OptionNames[_instructions.SelectedOption] == "Items")
                _images[3].DrawSprite(spriteBatch, _imagePosition - _images[3].Origin);

            _instructions.DrawOptions(spriteBatch);
        }
    }
}
