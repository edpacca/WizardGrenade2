namespace WizardGrenade2
{
    public class GameObjectParameters
    {
        public bool CanRotate { get; set; }
        public readonly string fileName;
        public readonly int framesH;
        public readonly int framesV;
        public readonly float mass;
        public readonly int numberOfCollisionPoints;
        public readonly float dampingFactor;

        public GameObjectParameters(){}

        // Constructor for single frame Sprite
        public GameObjectParameters(string setFileName, float setMass, bool canRotate, int setNumberOfCollisionPoints, float setDampingFactor)
        {
            fileName = setFileName;
            mass = setMass;
            CanRotate = canRotate;
            numberOfCollisionPoints = setNumberOfCollisionPoints;
            dampingFactor = setDampingFactor;
        }

        // Constructor with animation frames;
        public GameObjectParameters(string setFileName, float setMass, bool canRotate, int setNumberOfCollisionPoints, float setDampingFactor, int setFramesH, int setFramesV)
            : this (setFileName, setMass, canRotate, setNumberOfCollisionPoints, setDampingFactor)
        {
            framesH = setFramesH;
            framesV = setFramesV;
        }
    }

}
