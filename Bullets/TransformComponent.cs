using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullets
{
    internal class TransformComponent : Component
    {
        public Vector2f Position { get; set; } = new Vector2f();
        public float Rotation { get; set; }
        public Vector2f Scale { get; set; } = new Vector2f(1, 1);
    }
}
