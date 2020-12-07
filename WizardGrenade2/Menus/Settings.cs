using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System.Media;
using Microsoft.Xna.Framework.Audio;
using System;

namespace WizardGrenade2
{
    class Settings
    {
        private Options _settings;
        public GameSettings _gameSettings { get; private set; }
        public bool InSettings { get; set; }

        private readonly List<string> _settingNames = new List<string>()
        {
            "Music Volume",
            "Audio Volume",
            "Brightness"
        };

        public Settings(Vector2 menuFirstOptionPosition, float menuLastOptionPosition)
        {
            _settings = new Options(_settingNames, true, true);
            _settings.SetOptionLayout(menuFirstOptionPosition, menuLastOptionPosition);
            Vector2 _gameSettingsFirstPosition = new Vector2(ScreenSettings.CentreScreenWidth + 50, menuFirstOptionPosition.Y);
            _gameSettings = new GameSettings(_gameSettingsFirstPosition, menuLastOptionPosition);
        }

        public void LoadContent(ContentManager contentManager)
        {
            _settings.LoadContent(contentManager);
            _gameSettings.LoadContent(contentManager);
        }

        public void SetOptionColours(Color selected, Color unselected)
        {
            _settings.SetOptionColours(selected, unselected);
        }

        private void UpdateSettings()
        {
            if (InputManager.WasKeyPressed(Keys.Back))
                InSettings = false;

            int valueChange = InputManager.WasKeyPressed(Keys.Right) ? 1 : InputManager.WasKeyPressed(Keys.Left) ? -1 : 0;
            _gameSettings.ChangeValue(_settings.SelectedOption, valueChange);

            MediaPlayer.Volume = _gameSettings.MusicVolume;
            SoundEffect.MasterVolume = _gameSettings.AudioVolume;
        }

        public void ApplySettings(int musicVolume, int audioVolume, int brightness)
        {
            _gameSettings.SetValue(0, musicVolume);
            _gameSettings.SetValue(1, audioVolume);
            _gameSettings.SetValue(2, brightness);
            _gameSettings.Brightness = brightness / _gameSettings._gameSettingsValueMinMax[2]._valueMinMax[2];
        }

        public int[] GetSettings()
        {
            int[] settings = new int[3];

            for (int i = 0; i < 3; i++)
                settings[i] = _gameSettings._gameSettingsValueMinMax[i]._valueMinMax[0];

            return settings;
        }

        public void Update(GameTime gameTime)
        {
            _settings.Update(gameTime);
            UpdateSettings();
        }

        public float GetBrightness()
        {
            return _gameSettings.Brightness;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _settings.DrawOptions(spriteBatch);
            _gameSettings.DrawSettings(spriteBatch);
        }
    }
}
