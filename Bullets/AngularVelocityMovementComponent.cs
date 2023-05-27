using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullets
{
    internal class AngularVelocityMovementComponent : Component
    {
        public float AngularVelocity { get; set; }
        public GameObject OrbitalCenter { get; set; }
        public float Radius { get; set; }

        public override void Update(float deltaTime)
        {
            if (AngularVelocity == 0)
            {
                return;
            }

            Vector2f currentDirection = Owner.Transform.Position - OrbitalCenter.Transform.Position;
            float currentAngleDegrees = MathF.Atan2(currentDirection.Y, currentDirection.X) * 180 / MathF.PI;

            float targetAngleDegrees = currentAngleDegrees + AngularVelocity * deltaTime;
            float targetAngleRadians = targetAngleDegrees * MathF.PI / 180;
            Vector2f targetDirection = new Vector2f(MathF.Cos(targetAngleRadians), MathF.Sin(targetAngleRadians));

            Owner.Transform.Position = OrbitalCenter.Transform.Position + targetDirection * Radius; 
        }

        public override void Reset()
        {
            AngularVelocity = 0;
        }
    }
}
