using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullets
{
    internal abstract class DrawableComponent : Component
    {
        // Lower numbers are drawn first
        public int SortingOrder { get; set; }

        public abstract void Draw(GraphicsManager graphicsManager);
    }
}
