using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullets
{
    internal class CollisionInfo
    {
        public ColliderComponent Collider1 { get; set; }
        public ColliderComponent Collider2 { get; set; }
    }

    internal abstract class ColliderComponent : Component
    {
        public int LayerId { get; set; }

        public abstract bool Intersects(ColliderComponent other, out CollisionInfo collisionInfo);

        public abstract FloatRect GetBoundingBox();
    }
}
