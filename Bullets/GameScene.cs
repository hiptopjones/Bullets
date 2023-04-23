using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullets
{
    public enum AnimationState
    {
        None,
        Idle,
        Walk
    }

    internal class GameScene : Scene
    {
        protected GameObjectCollection SceneGameObjects { get; } = new GameObjectCollection();

        private GameObject Player { get; set; }

        public override void OnCreate()
        {
            Player = SceneGameObjects.CreateGameObject(GameSettings.PlayerObjectName);

            SpriteComponent spriteComponent = Player.AddComponent<SpriteComponent>();
            spriteComponent.TextureId = (int)GameSettings.TextureId.Player;

            AnimationComponent animationComponent = Player.AddComponent<AnimationComponent>();
            animationComponent.AddAnimation((int)AnimationState.Idle, CreateIdleAnimation());
            animationComponent.AddAnimation((int)AnimationState.Walk, CreateWalkAnimation());
            animationComponent.SetAnimationState((int)AnimationState.Idle);

            KeyboardMovementComponent movementComponent = Player.AddComponent<KeyboardMovementComponent>();
            movementComponent.Speed = GameSettings.PlayerMovementSpeed;
            movementComponent.LookDirectionChange += animationComponent.OnLookDirectionChanged;
        }

        private Animation CreateIdleAnimation()
        {
            int idleAnimationFrameWidth = 165;
            int idleAnimationFrameHeight = 145;
            float idleAnimationDisplayTime = 0.2f;

            Animation idleAnimation = new Animation();

            idleAnimation.AddFrame(new AnimationFrame
            {
                TextureId = (int)GameSettings.TextureId.Player,
                TextureRect = new IntRect(600, 0, idleAnimationFrameWidth, idleAnimationFrameHeight),
                DisplayTime = idleAnimationDisplayTime
            });

            idleAnimation.AddFrame(new AnimationFrame
            {
                TextureId = (int)GameSettings.TextureId.Player,
                TextureRect = new IntRect(800, 0, idleAnimationFrameWidth, idleAnimationFrameHeight),
                DisplayTime = idleAnimationDisplayTime
            });

            idleAnimation.AddFrame(new AnimationFrame
            {
                TextureId = (int)GameSettings.TextureId.Player,
                TextureRect = new IntRect(0, 145, idleAnimationFrameWidth, idleAnimationFrameHeight),
                DisplayTime = idleAnimationDisplayTime
            });

            idleAnimation.AddFrame(new AnimationFrame
            {
                TextureId = (int)GameSettings.TextureId.Player,
                TextureRect = new IntRect(200, 145, idleAnimationFrameWidth, idleAnimationFrameHeight),
                DisplayTime = idleAnimationDisplayTime
            });

            return idleAnimation;
        }

        private Animation CreateWalkAnimation()
        {
            int walkAnimationFrameWidth = 165;
            int walkAnimationFrameHeight = 145;
            float walkAnimationDisplayTime = 0.15f;

            Animation walkAnimation = new Animation();

            walkAnimation.AddFrame(new AnimationFrame
            {
                TextureId = (int)GameSettings.TextureId.Player,
                TextureRect = new IntRect(600, 290, walkAnimationFrameWidth, walkAnimationFrameHeight),
                DisplayTime = walkAnimationDisplayTime
            });

            walkAnimation.AddFrame(new AnimationFrame
            {
                TextureId = (int)GameSettings.TextureId.Player,
                TextureRect = new IntRect(800, 290, walkAnimationFrameWidth, walkAnimationFrameHeight),
                DisplayTime = walkAnimationDisplayTime
            });

            walkAnimation.AddFrame(new AnimationFrame
            {
                TextureId = (int)GameSettings.TextureId.Player,
                TextureRect = new IntRect(0, 435, walkAnimationFrameWidth, walkAnimationFrameHeight),
                DisplayTime = walkAnimationDisplayTime
            });

            walkAnimation.AddFrame(new AnimationFrame
            {
                TextureId = (int)GameSettings.TextureId.Player,
                TextureRect = new IntRect(200, 435, walkAnimationFrameWidth, walkAnimationFrameHeight),
                DisplayTime = walkAnimationDisplayTime
            });

            walkAnimation.AddFrame(new AnimationFrame
            {
                TextureId = (int)GameSettings.TextureId.Player,
                TextureRect = new IntRect(400, 435, walkAnimationFrameWidth, walkAnimationFrameHeight),
                DisplayTime = walkAnimationDisplayTime
            });

            return walkAnimation;
        }


        public override void OnDestroy()
        {
            // Nothing
        }

        public override void OnActivate()
        {
            // Nothing
        }

        public override void OnDeactivate()
        {
            // Nothing
        }

        public override void Update(float deltaTime)
        {
            SceneGameObjects.Update(deltaTime);
        }

        public override void LateUpdate(float deltaTime)
        {
            SceneGameObjects.LateUpdate(deltaTime);
        }

        public override void Draw(GraphicsManager graphicsManager)
        {
            SceneGameObjects.Draw(graphicsManager);
        }
    }
}
