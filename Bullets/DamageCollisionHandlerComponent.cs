using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullets
{
    internal class DamageCollisionHandlerComponent : CollisionHandlerComponent
    {
        public override void OnCollisionEnter(ColliderComponent other)
        {
            // TODO: Ignore if dodging
            // TODO: Ignore if recently took damage

            DamageComponent damageComponent = other.Owner.GetComponent<DamageComponent>();
            if (damageComponent == null)
            {
                return;
            }

            HealthComponent healthComponent = Owner.GetComponent<HealthComponent>();
            if (healthComponent == null)
            {
                return;
            }

            healthComponent.TakeDamage(damageComponent.Damage);
        }
    }
}
