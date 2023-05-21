using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullets
{
    internal class BulletCollisionHandlerComponent : CollisionHandlerComponent
    {
        public override void OnCollisionEnter(ColliderComponent other)
        {
            Owner.Destroy();
        }
    }
}
