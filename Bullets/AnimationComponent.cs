using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullets
{
    internal class AnimationComponent : Component
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private const int ANIMATION_STATE_NONE = -1;

        private int CurrentAnimationState { get; set; } = ANIMATION_STATE_NONE;

        private SpriteComponent SpriteComponent { get; set; }

        private Dictionary<int, Animation> Animations { get; } = new Dictionary<int, Animation>();

        public override void Awake()
        {
            SpriteComponent = Owner.GetComponent<SpriteComponent>();
            if (SpriteComponent == null)
            {
                throw new Exception($"Unable to locate a SpriteComponent");
            }
        }

        public override void Start()
        {
            Animation animation = GetCurrentAnimation();
            if (animation == null)
            {
                return;
            }

            AnimationFrame frame = animation.GetCurrentFrame();
            UpdateSpriteComponent(frame);
        }

        public override void Update(float deltaTime)
        {
            Animation animation = GetCurrentAnimation();
            if (animation == null)
            {
                return;
            }

            bool isNextFrame = animation.UpdateFrame(deltaTime);
            if (isNextFrame)
            {
                AnimationFrame frame = animation.GetCurrentFrame();
                UpdateSpriteComponent(frame);
            }
        }

        public void AddAnimation(int animationState, Animation animation)
        {
            Animations[animationState] = animation;
        }

        public void SetAnimationState(int animationState)
        {
            //Logger.Info($"AnimationState: old {CurrentAnimationState} new {animationState}");

            if (CurrentAnimationState == animationState)
            {
                return;
            }

            CurrentAnimationState = animationState;

            Animation animation = GetCurrentAnimation();
            if (animation != null)
            {
                animation.Reset();

                AnimationFrame frame = animation.GetCurrentFrame();
                UpdateSpriteComponent(frame);
            }
        }

        private Animation GetCurrentAnimation()
        {
            if (Animations.TryGetValue(CurrentAnimationState, out Animation animation))
            {
                return animation;
            }

            return null;
        }

        private void UpdateSpriteComponent(AnimationFrame frame)
        {
            // Protect against being called too early
            if (SpriteComponent == null)
            {
                return;
            }

            SpriteComponent.TextureId = frame.TextureId;
            SpriteComponent.TextureRect = frame.TextureRect;
        }
    }
}
