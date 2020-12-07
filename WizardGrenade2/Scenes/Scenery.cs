using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace WizardGrenade2
{
    class Scenery
    {
        private SceneryClouds _foregroundClouds;
        private SceneryClouds _backgroundClouds;
        private Sprite _moon;
        private Sprite _mainCloud;
        private Vector2 _mainCloudPosition;
        private Vector2 _moonPosition;
        private Sprite _sky;
        private float _rotation = 0f;
        private const int NUMBER_FOREGROUND_CLOUDS = 5;
        private const int FOREGROUND_CLOUD_SPEED = 30;
        private const int NUMBER_BACKGROUND_CLOUDS = 15;
        private const int BACKGROUND_CLOUD_SPEED = 15;
        private const float MOON_RADIUS = 700f;
        private const float MOON_SPEED = 0.05f;

        private Vector2 _foregroundMinPosition;
        private Vector2 _foregroundMaxPosition;
        private Vector2 _backgroundMinPosition;
        private Vector2 _backgroundMaxPosition;

        public Scenery()
        {
            _foregroundClouds = new SceneryClouds(NUMBER_FOREGROUND_CLOUDS, FOREGROUND_CLOUD_SPEED);
            _backgroundClouds = new SceneryClouds(NUMBER_BACKGROUND_CLOUDS, BACKGROUND_CLOUD_SPEED);
        }

        public void LoadContent(ContentManager contentManager)
        {

            _mainCloud = new Sprite(contentManager, "MainCloud");
            _moon = new Sprite(contentManager, "Moon");
            _moon.SpriteScale = 0.2f;
            _moon.SpriteColour = Colours.LighterGrey;
            _sky = new Sprite(contentManager, "Sky");
            float mainCloudX = ScreenSettings.CentreScreenWidth - (_mainCloud.GetSpriteRectangle().Width / 2);
            float mainCloudY = ScreenSettings.TARGET_HEIGHT - 100;
            _mainCloudPosition = new Vector2(mainCloudX, mainCloudY);

            _foregroundMinPosition = new Vector2(-300, ScreenSettings.TARGET_HEIGHT - 150);
            _foregroundMaxPosition = new Vector2(ScreenSettings.TARGET_WIDTH + 300, ScreenSettings.TARGET_HEIGHT + _mainCloud.GetSpriteRectangle().Height);
            _backgroundMinPosition = new Vector2(-300, -300);
            _backgroundMaxPosition = new Vector2(ScreenSettings.TARGET_WIDTH + 300, ScreenSettings.TARGET_HEIGHT);
            _moonPosition = Vector2.Zero;

            _foregroundClouds.SetCloudLimits(_foregroundMinPosition, _foregroundMaxPosition);
            _backgroundClouds.SetCloudLimits(_backgroundMinPosition, _backgroundMaxPosition);
            _backgroundClouds.SetCloudScaleRange(25, 125);
            _foregroundClouds.LoadContent(contentManager);
            _backgroundClouds.LoadContent(contentManager);
        }

        public void Update(GameTime gameTime)
        {
            _foregroundClouds.Update(gameTime);
            _backgroundClouds.Update(gameTime);

            _rotation += ((float)gameTime.ElapsedGameTime.TotalSeconds / 40);
            MoonOrbit(gameTime);
        }

        public void DrawForeground(SpriteBatch spriteBatch)
        {
            _mainCloud.DrawSprite(spriteBatch, _mainCloudPosition);
            _foregroundClouds.Draw(spriteBatch);
        }

        private void MoonOrbit(GameTime gameTime)
        {
            _moonPosition.X = ScreenSettings.ScreenCentre.X + ((float)Math.Sin(-gameTime.TotalGameTime.TotalSeconds * MOON_SPEED) * MOON_RADIUS);
            _moonPosition.Y = ScreenSettings.ScreenCentre.Y + ((float)Math.Cos(-gameTime.TotalGameTime.TotalSeconds * MOON_SPEED) * MOON_RADIUS);
        }

        public void DrawBackground(SpriteBatch spriteBatch)
        {
            _sky.DrawSprite(spriteBatch, ScreenSettings.ScreenCentre, _rotation);
            _moon.DrawSprite(spriteBatch, _moonPosition, _rotation * -4.5f);
            _backgroundClouds.Draw(spriteBatch);
        }
    }
}
