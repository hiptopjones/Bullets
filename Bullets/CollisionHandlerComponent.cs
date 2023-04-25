using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullets
{
    internal class CollisionHandlerComponent : Component
    {
        public virtual void OnCollisionEnter(ColliderComponent other)
        {
        }

        public virtual void OnCollisionExit(ColliderComponent other)
        {
        }

        public virtual void OnCollisionStay(ColliderComponent other)
        {
        }
    }
}
