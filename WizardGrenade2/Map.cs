using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace WizardGrenade2
{
    public sealed class Map
    {
        // Singleton pattern ensures only one instance of Map is called.
        private Map(){}
        private static readonly Lazy<Map> lazyMap = new Lazy<Map>(() => new Map());
        public static Map Instance { get => lazyMap.Value; }

        private CollisionManager _collisionManager;
        private readonly string _defaultFileName = "Map2";

        private Texture2D _mapTexture;
        private Vector2 _mapPosition = Vector2.Zero;
        private uint[] _mapPixelColourData;
        private bool[,] _mapPixelCollisionData;

        public void LoadContent(ContentManager contentManager, string fileName, bool isCollidable)
        {
            try
            {
                _mapTexture = contentManager.Load<Texture2D>(fileName);
            }
            catch (Exception)
            {
                _mapTexture = contentManager.Load<Texture2D>(_defaultFileName);
            }

            _mapPixelColourData = new uint[_mapTexture.Width * _mapTexture.Height];
            _mapTexture.GetData(_mapPixelColourData, 0, _mapPixelColourData.Length);
            _mapPixelCollisionData = LoadPixelCollisionData(_mapTexture, _mapPixelColourData);

            if (isCollidable)
            {
                _collisionManager = CollisionManager.Instance;
                _collisionManager.InitialiseMapData();
            }
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

        public void DeformLevel(int blastRadius, Vector2 blastPosition)
        {
            int blastDiameter = 2 * blastRadius;

            for (int x = 0; x < blastDiameter; x++)
            {
                for (int y = 0; y < blastDiameter; y++)
                {
                    if (IsPointInBlastArea(blastRadius, blastPosition, x, y))
                    {
                        _mapPixelColourData[((int)blastPosition.X + x - blastRadius) + ((int)blastPosition.Y + y - blastRadius) * _mapTexture.Width] = 0;
                        _mapPixelCollisionData[(int)blastPosition.X + x - blastRadius, ((int)blastPosition.Y + y - blastRadius)] = false;
                    }
                }
            }
            _mapTexture.SetData(_mapPixelColourData);
        }

        private bool IsPointInBlastArea(int blastRadius, Vector2 blastPosition, int x, int y)
        {
            if (MathsExt.isWithinCircleInSquare(blastRadius, x, y) &&
                blastPosition.X + x - blastRadius < _mapPixelCollisionData.GetLength(0) - 1 &&
                blastPosition.Y + y - blastRadius < _mapPixelCollisionData.GetLength(1) - 1 &&
                blastPosition.X + x - blastRadius >= 0 &&
                blastPosition.Y + y - blastRadius >= 0)
                return true;

            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_mapTexture, _mapPosition, Color.White);
        }
    }
}
