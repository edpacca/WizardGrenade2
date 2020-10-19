using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace WizardGrenade2
{
    public sealed class Map
    {
        // Singleton pattern ensures only one instance of Map is called.
        private Map()
        {
        }

        private static readonly Lazy<Map> lazy = new Lazy<Map>(() => new Map());

        public static Map Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        private readonly string _fileName = "Map2";

        private Texture2D _mapTexture;
        private Vector2 _mapPosition = Vector2.Zero;
        private uint[] _mapPixelColourData;
        private bool[,] _mapPixelCollisionData;

        public void LoadContent(ContentManager contentManager)
        {
            _mapTexture = contentManager.Load<Texture2D>(_fileName);
            _mapPixelColourData = new uint[_mapTexture.Width * _mapTexture.Height];
            _mapTexture.GetData(_mapPixelColourData, 0, _mapPixelColourData.Length);
            _mapPixelCollisionData = LoadPixelCollisionData(_mapTexture, _mapPixelColourData);
        }

        private bool[,] LoadPixelCollisionData(Texture2D texture, uint[] mapData)
        {
            if (mapData.Length != texture.Width * texture.Height)
                throw new ArgumentException("MapData must match the texture data provided");

            bool[,] boolArray = new bool[texture.Width, texture.Height];

            for (int x = 0; x < texture.Width; x++)
            {
                for (int y = 0; y < texture.Height; y++)
                {
                    if (mapData[x + y * texture.Width] != 0)
                        boolArray[x, y] = true;
                }
            }
            return boolArray;
        }

        public bool[,] GetMapPixelCollisionData()
        {
            return _mapPixelCollisionData;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_mapTexture, _mapPosition, Color.White);
        }
    }
}
