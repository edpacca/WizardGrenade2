namespace WizardGrenade2
{
    public class GameObjectParameters
    {
        public readonly string fileName;
        public readonly int framesH;
        public readonly int framesV;
        public readonly float mass;
        public bool canRotate;
        public readonly int numberOfCollisionPoints;
        public readonly float dampingFactor;

        public GameObjectParameters(){}

        // Constructor for single frame Sprite
        public GameObjectParameters(string setFileName, float setMass, bool setCanRotate, int setNumberOfCollisionPoints, float setDampingFactor)
        {
            fileName = setFileName;
            mass = setMass;
            canRotate = setCanRotate;
            numberOfCollisionPoints = setNumberOfCollisionPoints;
            dampingFactor = setDampingFactor;
        }

        // Constructor with animation frames;
        public GameObjectParameters(string setFileName, float setMass, bool setCanRotate, int setNumberOfCollisionPoints, float setDampingFactor, int setFramesH, int setFramesV)
            : this (setFileName, setMass, setCanRotate, setNumberOfCollisionPoints, setDampingFactor)
        {
            framesH = setFramesH;
            framesV = setFramesV;
        }
    }

}
