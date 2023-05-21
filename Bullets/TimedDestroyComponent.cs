using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullets
{
    internal class TimedDestroyComponent : Component
    {
        public float TimeToLive { get; set; }

        private float ElapsedTime { get; set; }

        public override void Update(float deltaTime)
        {
            ElapsedTime += deltaTime;
            if (ElapsedTime > TimeToLive)
            {
                Owner.Destroy();
            }
        }

        public override void Reset()
        {
            ElapsedTime = 0;
        }
    }
}
