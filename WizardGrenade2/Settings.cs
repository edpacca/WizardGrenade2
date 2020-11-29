using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

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

        private void UpdateSettings()
        {
            if (InputManager.WasKeyPressed(Keys.Back))
                InSettings = false;

            int valueChange = InputManager.WasKeyPressed(Keys.Right) ? 1 : InputManager.WasKeyPressed(Keys.Left) ? -1 : 0;
            _gameSettings.ChangeValue(_settings.SelectedOption, valueChange);

            if (InputManager.WasKeyPressed(Keys.Enter))
            {

            }
        }

        public void Update(GameTime gameTime)
        {
            _settings.Update(gameTime);
            UpdateSettings();
        }

        public int SetMusicVolume(int volume)
        {
            _gameSettings.MusicVolume = volume;
            return _gameSettings.MusicVolume;
        }

        public int SetAudioVolume(int volume)
        {
            _gameSettings.AudioVolume = volume;
            return _gameSettings.AudioVolume;
        }

        public int SetBrightness(int brightness)
        {
            _gameSettings.Brightness = brightness;
            return _gameSettings.Brightness;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _settings.DrawOptions(spriteBatch);
            _gameSettings.DrawSettings(spriteBatch);
        }
    }
}
