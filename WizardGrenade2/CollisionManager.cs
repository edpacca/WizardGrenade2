using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardGrenade2
{
    class CollisionManager
    {
        private bool[,] _mapCollisionData;

        public CollisionManager(bool[,] mapCollisionData)
        {
            _mapCollisionData = mapCollisionData;
        }

        public bool HasCollided(Vector2 collisionPoint)
        {
            return _mapCollisionData[(int)collisionPoint.X, (int)collisionPoint.Y];
        }

        public Vector2 ResolveCollision(List<Vector2> collisionPoints, Vector2 velocity)
        {
            return Vector2.Zero;
        }

    }
}
