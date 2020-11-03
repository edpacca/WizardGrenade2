using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework.Content;

namespace WizardGrenade2
{
    class Polygon
    {
        public List<Vector2> polyPoints = new List<Vector2>();
        public List<Vector2> transformedPolyPoints = new List<Vector2>();
        private Texture2D _pixelTexture;
        private Rectangle _pixelRectangle = new Rectangle(0, 0, 1, 1);

        public Polygon(Texture2D texture, int numberOfCollisionPoints)
        {
            polyPoints = (numberOfCollisionPoints == 0) ?
                CalcRectanglePoints(texture.Width, texture.Height) :
                CalcCircleCollisionPoints((texture.Width + texture.Height) / 4, numberOfCollisionPoints);

            foreach (var point in polyPoints)
                transformedPolyPoints.Add(point);
        }

        public void LoadPolyContent(ContentManager contentManager)
        {
            _pixelTexture = contentManager.Load<Texture2D>("Pixel");
        }

        public void UpdateCollisionPoints(Vector2 position, float rotation)
        {
            for (int i = 0; i < transformedPolyPoints.Count; i++)
            {
                transformedPolyPoints[i] = Vector2.Transform(polyPoints[i], Matrix.CreateRotationZ(rotation)) + position;
            }
        }

        public static List<Vector2> CalcRectanglePoints(float width, float height)
        {
            List<Vector2> relativePoints = new List<Vector2>
            {
                new Vector2(0 - width / 2, (0 - height / 2)),
                new Vector2(0 + width / 2, (0 - height / 2)),
                new Vector2(0 + width / 2, (0 + height / 2)),
                new Vector2(0 - width / 2, (0 + height / 2)),
            };

            return relativePoints;
        }

        public static List<Vector2> CalcCircleCollisionPoints(float radius, int numberOfPoints)
        {
            List<Vector2> relativePoints = new List<Vector2>();
            float deltaTheta = (float)(Mechanics.TAO / numberOfPoints);

            for (float theta = 0; theta <= Mechanics.TAO - deltaTheta; theta += deltaTheta)
                relativePoints.Add(Mechanics.VectorComponents(radius, theta));

            return relativePoints;
        }

        public void DrawCollisionPoints(SpriteBatch spriteBatch, Vector2 position)
        {
            // Draw collision points as single pixel
            //foreach (var point in polyPoints)
            //    spriteBatch.Draw(_pixelTexture, point + position, _pixelRectangle, Color.White);

            foreach (var point in transformedPolyPoints)
                spriteBatch.Draw(_pixelTexture, point, _pixelRectangle, Color.Aqua);

            // Draw point in centre
            spriteBatch.Draw(_pixelTexture, position, _pixelRectangle, Color.Magenta);
        }
    }
}
