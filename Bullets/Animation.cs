using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Bullets
{
    public class AnimationFrame
    {
        public int TextureId;
        public IntRect TextureRect;
        public float DisplayTime;
    }

    internal class Animation
    {
        private List<AnimationFrame> Frames { get; } = new List<AnimationFrame>();

        private int CurrentFrameIndex { get; set; }
        private float CurrentFrameTime { get; set; }

        public void AddFrame(AnimationFrame frame)
        {
            Frames.Add(frame);
        }

        public bool UpdateFrame(float deltaTime)
        {
            if (Frames.Count > 0)
            {
                CurrentFrameTime += deltaTime;
                if (CurrentFrameTime >= GetCurrentFrame().DisplayTime)
                {
                    SetCurrentFrame(CurrentFrameIndex + 1);
                    return true;
                }
            }

            return false;
        }

        public void Reset()
        {
            SetCurrentFrame(0);
        }

        public AnimationFrame GetCurrentFrame()
        {
            return Frames[CurrentFrameIndex];
        }

        private void SetCurrentFrame(int frameIndex)
        {
            CurrentFrameIndex = frameIndex % Frames.Count;
            CurrentFrameTime = 0;
        }
    }
}
