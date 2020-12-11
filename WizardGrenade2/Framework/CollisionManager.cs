using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace WizardGrenade2
{
    public sealed class CollisionManager
    {
        private CollisionManager(){}
        private static readonly Lazy<CollisionManager> lazyCollisionManager = new Lazy<CollisionManager>(() => new CollisionManager());
        public static CollisionManager Instance { get => lazyCollisionManager.Value; }

        private bool[,] _mapCollisionData;
        private int _mapWidth;
        private int _mapHeight;

        public void InitialiseMapData()
        {
            if (Map.Instance.MapPixelCollisionData == null)
                throw new ArgumentNullException("Error: Map data is empty");

            _mapCollisionData = Map.Instance.MapPixelCollisionData;
            _mapWidth = _mapCollisionData.GetLength(0);
            _mapHeight = _mapCollisionData.GetLength(1);
        }

        public bool CheckObjectPlacement(List<Vector2> collisionPoints)
        {
            foreach (var point in collisionPoints)
            {
                if (IsPointWithinMap(point) && HasCollided(point))
                    return false;
            }
            return true;
        }

        private bool IsPointWithinMap(Vector2 point) => point.X >= 0 && point.Y >= 0 && point.X < _mapWidth - 1 && point.Y < _mapHeight - 1;
        public bool HasCollided(Vector2 collisionPoint) => _mapCollisionData[(int)collisionPoint.X, (int)collisionPoint.Y];

        public List<Vector2> CheckCollision(List<Vector2> transformedCollisionPoints)
        {
            List<Vector2> collidingPoints = new List<Vector2>();

            foreach (var point in transformedCollisionPoints)
            {
                if (IsPointWithinMap(point) && HasCollided(point))
                    collidingPoints.Add(point);
            }

            return collidingPoints;
        }

        public Vector2 ResolveCollision(List<Vector2> collidingPoints, Vector2 position, Vector2 velocity)
        {
            Vector2 normalResponseVector = SumResponseVector(collidingPoints, position);
            Vector2 reflectionVector = Mechanics.ReflectionVector(velocity, normalResponseVector);
            return reflectionVector;
        }

        private Vector2 SumResponseVector(List<Vector2> collisionPoints, Vector2 centre)
        {
            Vector2 responseVector = Vector2.Zero;
            foreach (var point in collisionPoints)
                responseVector += Vector2.Subtract(centre, point);

            return responseVector;
        }
    }
}
