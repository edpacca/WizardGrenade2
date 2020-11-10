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

            if (GetVelocity() == Vector2.Zero)
                KillProjectile();

            base.Update(gameTime, gameObjects);
        }

        public override void WizardInteraction(List<Wizard> wizards)
        {
            foreach (var wizard in wizards)
            {
                float distance = Math.Abs(Mechanics.VectorMagnitude(GetPosition() - wizard.GetPosition()));
                if (distance <= wizard.GetSpriteRectangle().Width / 2)
                {
                    wizard.AddVelocity(GetVelocity() * WeaponSettings.ARROW_KNOCKBACK_FACTOR);
                    wizard.entity.ApplyDamage((int)(Mechanics.VectorMagnitude(GetVelocity()) * WeaponSettings.ARROW_DAMAGE_FACTOR));
                    KillProjectile();
                }
            }
        }

        public override void SetToPlayerPosition(Vector2 newPosition, int activeDirection)
        {
            base.SetToPlayerPosition(newPosition + new Vector2(_arrowOffset * activeDirection, 0), 0);
        }
    }
}
