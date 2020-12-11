using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System;

namespace WizardGrenade2
{
    public class Settings
    {
        public Setting MusicVolume { get; private set; }
        public Setting AudioVolume { get; private set; }
        public Setting Brightness { get; private set; }
        public bool InSettings { get; set; }

        private List<Setting> _settings;
        private Options _options;
        private Random _randomGenerator;
        private Vector2 _offset;
        private float _spriteMeterWidth;

        private readonly List<string> _settingNames = new List<string>()
        {
            "Music Volume",
            "Audio Volume",
            "Brightness"
        };

        public Settings(Vector2 firstOptionPosition, float lastOptionPosition, Vector2 textSpriteOffset, float spriteMeterWidth)
        {
            _options = new Options(_settingNames, true, true);
            _options.SetOptionLayout(firstOptionPosition, lastOptionPosition);

            MusicVolume = new Setting(3, 0, 6);
            AudioVolume = new Setting(3, 0, 6);
            Brightness = new Setting(3, 0, 6);

            _settings = new List<Setting>();
            _settings.Add(MusicVolume);
            _settings.Add(AudioVolume);
            _settings.Add(Brightness);

            _offset = textSpriteOffset;
            _spriteMeterWidth = spriteMeterWidth;
            _randomGenerator = new Random();
        }

        public void LoadContent(ContentManager contentManager)
        {
            _options.LoadContent(contentManager);
            MusicVolume.LoadContent(contentManager, @"Menu/Lute");
            AudioVolume.LoadContent(contentManager, @"Menu/Bell");
            Brightness.LoadContent(contentManager, @"GameObjects/Fireball");

            MusicVolume.SetSpriteMeter(_spriteMeterWidth, 1.5f);
            AudioVolume.SetSpriteMeter(_spriteMeterWidth, 1.5f);
            Brightness.SetSpriteMeter(_spriteMeterWidth, 4f);
        }

        public void Update(GameTime gameTime)
        {
            if (InputManager.WasKeyPressed(Keys.Back))
                InSettings = false;

            _options.Update(gameTime);
            UpdateSettings();
        }

        private void UpdateSettings()
        {
            int valueChange = InputManager.WasKeyPressed(Keys.Right) ? 1 : InputManager.WasKeyPressed(Keys.Left) ? -1 : 0;
            _settings[_options.SelectedOption].ChangeValue(valueChange);

            if (valueChange != 0)
            {
                string sound = "magic" + _randomGenerator.Next(1, 5);
                SoundManager.Instance.PlaySound(sound);
            }

            MediaPlayer.Volume = MusicVolume.Value;
            SoundEffect.MasterVolume = AudioVolume.Value;
        }

        public void ApplySettings(int musicVolume, int audioVolume, int brightness)
        {
            MusicVolume.SetValue(musicVolume);
            AudioVolume.SetValue(audioVolume);
            Brightness.SetValue(brightness);
        }

        public int[] GetSettings()
        {
            int[] settings = new int[3];

            settings[0] = MusicVolume.IntValue;
            settings[1] = AudioVolume.IntValue;
            settings[2] = Brightness.IntValue;

            return settings;
        }
        
        public void SetOptionColours(Color selected, Color unselected) => _options.SetOptionColours(selected, unselected);

        public void Draw(SpriteBatch spriteBatch)
        {
            _options.DrawOptions(spriteBatch);
            for (int i = 0; i < 3; i++)
                _settings[i].DrawSetting(spriteBatch, _options.OptionsLayout[i] + _offset);
        }
    }
}
