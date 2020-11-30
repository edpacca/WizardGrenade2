using Microsoft.Xna.Framework;

namespace WizardGrenade2
{
    public class Camera
    {
        public float Zoom { get; set; }
        public Matrix Transform { get => Transformation(); }
        public Vector2 Position { get; set; }

        public Camera()
        {
            Zoom = 1f;
            Position = Vector2.Zero;
        }

        private Matrix Transformation()
        {
            return 
                Matrix.CreateTranslation(new Vector3(-Position, 0)) *
                Matrix.CreateScale(Zoom) *
                Matrix.CreateTranslation(ScreenSettings.RESOLUTION_WIDTH / 2, ScreenSettings.RESOLUTION_HEIGHT / 2, 1);
        }
    }
}
