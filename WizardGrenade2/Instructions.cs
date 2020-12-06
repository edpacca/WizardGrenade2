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
        private List<Sprite> _images = new List<Sprite>();
        private const int OSCILLATION_RATE = 2;
        private const float OSCILLATION_AMPLITUDE = 0.6f;
        private List<string> _instructionNames = new List<string>()
        {
            "Overview",
            "Controls",
            "Weapons",
            "Fireball",
            "Melf's Acid Arrow",
            "Ice Bomb",
            "Items",
        };
        private List<List<string>> _instructionScripts = new List<List<string>>();
        private Vector2 _firstLine = new Vector2(ScreenSettings.CentreScreenWidth, 300);
        private Vector2 _imagePosition = new Vector2(ScreenSettings.CentreScreenWidth, ScreenSettings.TARGET_HEIGHT * 0.75f);
        private float _interval = 30f;

        public Instructions()
        {
            _instructions = new Options(_instructionNames, true, false);
            _instructions.SetSinglePosition(new Vector2(ScreenSettings.CentreScreenWidth, ScreenSettings.TARGET_HEIGHT * 0.9f));
            _instructionScripts.Add(_overviewScript);
            _instructionScripts.Add(_movingScript);
            _instructionScripts.Add(_weaponScript);
            _instructionScripts.Add(_fireballScript);
            _instructionScripts.Add(_arrowScript);
            _instructionScripts.Add(_iceBombScript);
            _instructionScripts.Add(_itemsScript);
        }

        public void LoadContent(ContentManager contentManager)
        {
            _instructions.LoadContent(contentManager);
            _infoFont = contentManager.Load<SpriteFont>("InfoFont");
            _images.Add(new Sprite(contentManager, "Fireball"));
            _images.Add(new Sprite(contentManager, "MelfsAcidArrow"));
            _images.Add(new Sprite(contentManager, "IceBomb"));
            _images.Add(new Sprite());
            _images[3].LoadContent(contentManager, "Potions", 3, 1);

            for (int i = 0; i < _images.Count; i++)
            {
                _images[i].SpriteScale = 10f;
                _images[3].SpriteScale = 6f;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (InputManager.WasKeyPressed(Keys.Back))
                InInstructions = false;

            _instructions.Update(gameTime);

            _imagePosition.Y += ((float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * OSCILLATION_RATE)) * OSCILLATION_AMPLITUDE;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 position = _firstLine;
            foreach (var line in _instructionScripts[_instructions.SelectedOption])
            {
                Vector2 textSize = _infoFont.MeasureString(line) / 2;
                spriteBatch.DrawString(_infoFont, line, position - textSize, Colours.Gold);
                position.Y += _interval;
            }

            if (_instructions.OptionNames[_instructions.SelectedOption] == "Fireball")
                _images[0].DrawSprite(spriteBatch, _imagePosition - _images[0].GetSpriteOrigin());
            else if (_instructions.OptionNames[_instructions.SelectedOption] == "Melf's Acid Arrow")
                _images[1].DrawSprite(spriteBatch, _imagePosition - _images[1].GetSpriteOrigin());
            else if (_instructions.OptionNames[_instructions.SelectedOption] == "Ice Bomb")
                _images[2].DrawSprite(spriteBatch, _imagePosition - _images[2].GetSpriteOrigin());
            else if (_instructions.OptionNames[_instructions.SelectedOption] == "Items")
                _images[3].DrawSprite(spriteBatch, _imagePosition - _images[3].GetSpriteOrigin());

            _instructions.DrawOptions(spriteBatch);
        }

        private List<string> _overviewScript = new List<string>()
        {
            "Wizard Grenade is a turn based battle game for two or more players.",
            "Destroy the wizards from the other teams by damaging them with your spells, or by pushing them into the abyss.",
            "Players take turns to control a wizard from their team, and take one shot with a weapon.",
            "Your turn is over when your shot lands or when the timer runs out.",
            "The last team standing will be crowned victorious!"
        };

        private List<string> _movingScript = new List<string>()
        {
            "Move your wizards using the 'Left' and 'Right' arrow keys.",
            "Press 'Enter' to jump. Press repeatedly for multiple jumps",
            "",
            "Press 'Escape' to pause the game. Press 'Delete' to Exit instantly",
            "Use the mouse to move the view, and scroll to change the zoom",
            "Press 'L Shift' to Reset the view",
        };

        private List<string> _weaponScript = new List<string>()
        {
            "Aim your weapons using the 'Up' and 'Down' arrow keys.",
            "Press 'Tab' to change weapons.",
            "Hold 'Space' to charge your shot, and release to fire!",
            "More power will make your shot go further.",
        };

        private List<string> _fireballScript = new List<string>()
        {
            "Fireballs are of medium weight and inflict high damage.",
            "They will bounce around off the map and explode when the timer reaches 0",
            "Use the top number keys to set the timer from 1 - 5 seconds.",
            "When a fireball explodes it destroys the terrain and damages wizards with small knock-back"
        };

        private List<string> _arrowScript = new List<string>()
        {
            "Arrows are very light and inflict low damage.",
            "They will charge up very quickly, and cause a lot of knock-back at max power"
        };

        private List<string> _iceBombScript = new List<string>()
        {
            "The Ice Bomb is very heavy, and does medium damage.",
            "They have a slow charge, and can be tricky to throw.",
            "Ice Bombs explode on impact with the terrain, with a wide area of affect.",
            "Wizards caught in the blast will be damaged and knocked back",
        };

        private List<string> _itemsScript = new List<string>()
        {
            "Potions.... coming soon!",
            //"There are some items in the game which can help you.",
            //"A Potion O' Healing will heal your wizard for 25 points.",
            //"A Potion O' Leaping will let your wizard jump infinitely for a turn.",
            //"A Potion O' Stamina will let your wizard move faster for a turn",
        };
    }
}
