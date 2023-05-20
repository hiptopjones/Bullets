using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullets
{
    internal class RangedDestroyComponent : Component
    {
        public GameObject Target { get; set; }

        public float MaxDistance { get; set; }

        public override void Update(float deltaTime)
        {
            float distance = Owner.Transform.Position.DistanceTo(Target.Transform.Position);
            if (distance > MaxDistance)
            {
                Owner.Destroy();
            }
        }
    }
}
