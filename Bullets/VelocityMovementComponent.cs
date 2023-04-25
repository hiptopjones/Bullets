using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Bullets
{
    internal class VelocityMovementComponent : Component
    {
        public Vector2f Velocity { get; set; }

        public override void Update(float deltaTime)
        {
            Owner.Transform.Position += Velocity * deltaTime;
        }
    }
}
