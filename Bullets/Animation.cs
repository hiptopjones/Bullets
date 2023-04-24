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
        public Action Callback; // Called when the frame is entered
    }

    internal class Animation
    {
        // Indicates if this animation loop or just plays one time
        public bool IsSingleShot { get; set; }

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
            if (frameIndex >= Frames.Count && IsSingleShot)
            {
                // Do not update the frame index if single-shot animation is complete
                CurrentFrameTime = 0;
                return;
            }

            CurrentFrameIndex = frameIndex % Frames.Count;
            CurrentFrameTime = 0;

            // Check for a callback
            AnimationFrame frame = GetCurrentFrame();
            if (frame.Callback != null)
            {
                frame.Callback();
            }
        }
    }
}
