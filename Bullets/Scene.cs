using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullets
{
    internal abstract class Scene
    {
        public abstract void OnCreate();

        public virtual void OnDestroy() { }

        public abstract void OnActivate();

        public virtual void OnDeactivate() { }

        public abstract void Update(float deltaTime);

        public abstract void Draw(WindowManager window);
    }
}
