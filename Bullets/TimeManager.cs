using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Bullets
{
    internal class TimeManager
    {
        private const int FPS_SAMPLE_COUNT = 100;

        public float TotalTime { get; private set; }
        public float DeltaTime { get; private set; }

        public float InstantFps { get; private set; }
        public float RollingFps { get; private set; }

        public float TimeScale { get; private set; } = 1;

        private Clock Clock { get; } = new Clock();

        private Queue<float> RecentDeltaTimes { get; } = new Queue<float>(FPS_SAMPLE_COUNT);

        public void OnFrameStarted()
        {
            Time time = Clock.Restart();
            DeltaTime = time.AsSeconds() * TimeScale;
            
            // TODO: Avoid in production
            CalculateFps();
        }

        private void CalculateFps()
        {
            if (RecentDeltaTimes.Count == FPS_SAMPLE_COUNT)
            {
                RecentDeltaTimes.Dequeue();
            }

            RecentDeltaTimes.Enqueue(DeltaTime);

            RollingFps = 1 / RecentDeltaTimes.Average();
            InstantFps = 1 / DeltaTime;
        }
    }
}
