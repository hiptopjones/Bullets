using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullets
{
    internal class EnemyMovementComponent : Component
    {
        public float Speed { get; set; }

        public GameObject Target { get; set; }

        public override void Update(float deltaTime)
        {
            Vector2f targetDirection = Target.Transform.Position - Owner.Transform.Position;

            Owner.Transform.Position += targetDirection.Normalize() * Speed * deltaTime;
        }
    }
}
