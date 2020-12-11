using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace WizardGrenade2
{
    public sealed class Map
    {
        private Map() {}
        private static readonly Lazy<Map> lazyMap = new Lazy<Map>(() => new Map());
        public static Map Instance { get => lazyMap.Value; }

        public bool[,] MapPixelCollisionData { get; private set; }
        private uint[] _mapPixelColourData;

        private CollisionManager _collisionManager;
        private Texture2D _mapTexture;
        private Vector2 _mapPosition = Vector2.Zero;
        private readonly string _defaultFileName = @"Maps/Map2";

        public void LoadContent(ContentManager contentManager, string fileName, bool isCollidable)
        {
            try { _mapTexture = contentManager.Load<Texture2D>(fileName); }
            catch (Exception) { _mapTexture = contentManager.Load<Texture2D>(_defaultFileName); }

            _mapPixelColourData = new uint[_mapTexture.Width * _mapTexture.Height];
            _mapTexture.GetData(_mapPixelColourData, 0, _mapPixelColourData.Length);
            MapPixelCollisionData = LoadPixelCollisionData(_mapTexture, _mapPixelColourData);

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

            for (int x = 0; x < texture.Width; ++x)
            {
                for (int y = 0; y < texture.Height; ++y)
                {
                    if (mapData[x + y * texture.Width] != 0)
                        boolArray[x, y] = true;
                }
            }
            return boolArray;
        }

        public void DeformLevel(int radius, Vector2 position)
        {
            int diameter = 2 * radius;

            for (int x = 0; x < diameter; ++x)
            {
                for (int y = 0; y < diameter; ++y)
                {
                    if (IsPointInBlastArea(radius, position, x, y))
                    {
                        _mapPixelColourData[PositionInArray(radius, position, x, y)] = 0;
                        MapPixelCollisionData[ArrayColumn(radius, position, x), ArrayRow(radius, position, y)] = false;
                    }
                }
            }
            _mapTexture.SetData(_mapPixelColourData);
        }

        private bool IsPointInBlastArea(int blastRadius, Vector2 blastPosition, int x, int y)
        {
            return Utility.isWithinCircleInSquare(blastRadius, x, y) &&
                blastPosition.X + x - blastRadius < MapPixelCollisionData.GetLength(0) - 1 &&
                blastPosition.Y + y - blastRadius < MapPixelCollisionData.GetLength(1) - 1 &&
                blastPosition.X + x - blastRadius >= 0 &&
                blastPosition.Y + y - blastRadius >= 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_mapTexture, _mapPosition, Color.White);
        }

        private int PositionInArray(int radius, Vector2 position, int x, int y) => ArrayColumn(radius, position, x) + (ArrayRow(radius, position, y) * _mapTexture.Width);
        private int ArrayColumn(int radius, Vector2 position, int x) => (int)position.X + x - radius;
        private int ArrayRow(int radius, Vector2 position, int y) => (int)position.Y + y - radius;
    }
}
