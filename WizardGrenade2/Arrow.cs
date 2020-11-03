using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardGrenade2
{
    class Arrow : Weapon
    {
        private readonly string _fileName = "MelfsAcidArrow";
        private const int MASS = 15;
        private const int NUMBER_OF_COLLISION_POINTS = 0;
        private const float DAMPING_FACTOR = 0f;
        private const int CHARGE_POWER = 1500;
        private const float MAX_CHARGE = 0.6f;

        public void LoadContent(ContentManager contentManager)
        {
            LoadContent(contentManager, new GameObjectParameters(_fileName, MASS, true, NUMBER_OF_COLLISION_POINTS, DAMPING_FACTOR));
            SetFiringBehaviour(CHARGE_POWER, MAX_CHARGE);
        }

        

    }
}
