using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardGrenade2
{
    public class GameSettings
    {
        public float MusicVolume { get; set; }
        private ValueMinMax _musicVolumeValueMinMax;
        public float AudioVolume { get; set; }
        private ValueMinMax _audioVolumeValueMinMax;
        public float Brightness { get; set; }
        private ValueMinMax _brightnessValueMinMax;

        public List<ValueMinMax> _gameSettingsValueMinMax { get; private set; }

        public GameSettings(Vector2 firstPosition, float lastYPosition)
        {
            _gameSettingsValueMinMax = new List<ValueMinMax>();

            _musicVolumeValueMinMax = new ValueMinMax(3, 0, 6, "Lute");
            _audioVolumeValueMinMax = new ValueMinMax(3, 0, 6, "Bell");
            _brightnessValueMinMax = new ValueMinMax(3, 0, 6, "Fireball");
            _gameSettingsValueMinMax.Add(_musicVolumeValueMinMax);
            _gameSettingsValueMinMax.Add(_audioVolumeValueMinMax);
            _gameSettingsValueMinMax.Add(_brightnessValueMinMax);

            MusicVolume = (float)_musicVolumeValueMinMax._valueMinMax[0] / (float)_musicVolumeValueMinMax._valueMinMax[2];
            AudioVolume = (float)_audioVolumeValueMinMax._valueMinMax[0] / (float)_audioVolumeValueMinMax._valueMinMax[2];
            Brightness = (float)_brightnessValueMinMax._valueMinMax[0] / (float)_brightnessValueMinMax._valueMinMax[2];

            Vector2 startPosition = firstPosition;
            float interval = CalculateYInterval(firstPosition, lastYPosition);
            foreach (var setting in _gameSettingsValueMinMax)
            {
                setting.SetPositions(startPosition, new Vector2(startPosition.X + 250, startPosition.Y));
                startPosition.Y += interval;
            }
        }

        public float CalculateYInterval(Vector2 start, float end)
        {
            return (end - start.Y) / (_gameSettingsValueMinMax.Count - 1);
        }

        public void LoadContent(ContentManager contentManager)
        {
            foreach (var setting in _gameSettingsValueMinMax)
            {
                setting.LoadContent(contentManager);
                setting.SetScale(1.5f);
            }
            _gameSettingsValueMinMax[2].SetScale(4f);
        }

        public void ChangeValue(int index, int difference)
        {
            int value = _gameSettingsValueMinMax[index]._valueMinMax[0];

            _gameSettingsValueMinMax[index]._valueMinMax[0] =
                value + difference <= _gameSettingsValueMinMax[index]._valueMinMax[1] ? _gameSettingsValueMinMax[index]._valueMinMax[1] :
                value + difference >= _gameSettingsValueMinMax[index]._valueMinMax[2] ? _gameSettingsValueMinMax[index]._valueMinMax[2] :
                value + difference;

            MusicVolume = (float)_musicVolumeValueMinMax._valueMinMax[0] / (float)_musicVolumeValueMinMax._valueMinMax[2];
            AudioVolume = (float)_audioVolumeValueMinMax._valueMinMax[0] / (float)_audioVolumeValueMinMax._valueMinMax[2];
            Brightness = (float)_brightnessValueMinMax._valueMinMax[0] / (float)_brightnessValueMinMax._valueMinMax[2];
        }

        public void SetValue(int index, int value)
        {
            _gameSettingsValueMinMax[index]._valueMinMax[0] = value;
        }

        public void DrawSettings(SpriteBatch spriteBatch)
        {
            foreach (var setting in _gameSettingsValueMinMax)
                setting.DrawValue(spriteBatch, setting._valueMinMax[0]);
        }
    }
}
