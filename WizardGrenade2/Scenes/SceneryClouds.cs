using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace WizardGrenade2
{
    class SceneryClouds
    {
        private int _numberOfClouds;
        private int _cloudSpeed;
        private Random _random = new Random(); 

        private List<Sprite> _clouds = new List<Sprite>();
        private List<float> _cloudScales = new List<float>();
        private int _minScalePercentage = 50;
        private int _maxScalePercentage = 100;

        private List<Vector2> _cloudPositions = new List<Vector2>();
        private Vector2 _minPosition = Vector2.Zero;
        private Vector2 _maxPosition = new Vector2(ScreenSettings.TARGET_WIDTH, ScreenSettings.TARGET_WIDTH);


        public SceneryClouds(int numberOfClouds, int cloudSpeed)
        {
            _numberOfClouds = numberOfClouds;
            _cloudSpeed = cloudSpeed;
        }

        public SceneryClouds(int numberOfClouds, int cloudSpeed, Vector2 minPosition, Vector2 maxPosition) : this (numberOfClouds, cloudSpeed)
        {
            SetCloudLimits(minPosition, maxPosition);
        }

        public SceneryClouds(int numberOfClouds, int cloudSpeed, Vector2 minPosition, Vector2 maxPosition, int minScalePercentage, int maxScalePercentage) : this(numberOfClouds, cloudSpeed, minPosition, maxPosition)
        {
            SetCloudScaleRange(minScalePercentage, maxScalePercentage);
        }

        public void SetCloudLimits(Vector2 minPosition, Vector2 maxPosition)
        {
            _minPosition = minPosition;
            _maxPosition = maxPosition;
        }

        public void SetCloudScaleRange(int minScalePercentage, int maxScalePercantage)
        {
            _minScalePercentage = minScalePercentage;
            _maxScalePercentage = maxScalePercantage;

        }

        public void LoadContent(ContentManager contentManager)
        {
            for (int i = 0; i < _numberOfClouds; i++)
            {
                int randomX = _random.Next((int)_minPosition.X, (int)_maxPosition.X);
                int randomY = _random.Next((int)_minPosition.Y, (int)_maxPosition.Y);

                _clouds.Add(new Sprite(contentManager, @"Background/Cloud"));
                _cloudPositions.Add(new Vector2(randomX, randomY));
                float randomScale = _random.Next(_minScalePercentage, _maxScalePercentage);
                _cloudScales.Add(randomScale / 100);
                _clouds[i].SpriteScale = _cloudScales[i];
            }
        }

        public void Update(GameTime gameTime)
        {
            float movement = (float)gameTime.ElapsedGameTime.TotalSeconds * _cloudSpeed;

            for (int i = 0; i < _cloudPositions.Count; i++)
            {
                _cloudPositions[i] = _cloudPositions[i].X > _maxPosition.X ?
                    new Vector2(_minPosition.X + movement * _cloudScales[i], _cloudPositions[i].Y) :
                    new Vector2(_cloudPositions[i].X + movement * _cloudScales[i], _cloudPositions[i].Y);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _numberOfClouds; i++)
                _clouds[i].DrawSprite(spriteBatch, _cloudPositions[i]);
        }
    }
}
