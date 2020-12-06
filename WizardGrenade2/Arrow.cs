using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;

namespace WizardGrenade2
{
    class Arrow : Weapon
    {
        private int _arrowOffset;

        public Arrow()
        {
            ChargingSoundFile = "arrowDraw";
            MovingSoundFile = "arrowShot";
            LoadWeaponObject(WeaponSettings.ARROW_GAMEOBJECT, WeaponSettings.ARROW_POWER, WeaponSettings.ARROW_MAX_CHARGE_TIME);
        }

        public override void LoadContent (ContentManager contentManager)
        {
            _arrowOffset = (WeaponManager.Instance.WizardSpriteRectangle.Width / 2) + 1;
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime, List<Wizard> gameObjects)
        {
            WizardInteraction(gameObjects);

            if (IsMoving && Velocity == Vector2.Zero)
                KillProjectile("stone1");

            base.Update(gameTime, gameObjects);
        }

        public override void WizardInteraction(List<Wizard> wizards)
        {
            foreach (var wizard in wizards)
            {
                float distance = Math.Abs(Mechanics.VectorMagnitude(Position - wizard.Position));
                if (IsMoving && distance <= wizard.GetSpriteRectangle().Width / 2)
                {
                    wizard.AddVelocity(Velocity * WeaponSettings.ARROW_KNOCKBACK_FACTOR);
                    wizard.entity.ApplyDamage((int)(Mechanics.VectorMagnitude(Velocity) * WeaponSettings.ARROW_DAMAGE_FACTOR));
                    KillProjectile("wizardHit1");
                }
            }
        }

        public override void SetToPlayerPosition(Vector2 newPosition, int activeDirection, float crosshairAngle)
        {
            Vector2 offset = Mechanics.VectorComponents(_arrowOffset, crosshairAngle);
            base.SetToPlayerPosition(newPosition + offset, 0, crosshairAngle);
        }
    }
}
