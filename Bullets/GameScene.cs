using NLog;
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
        protected GameObjectManager GameObjectManager { get; } = new GameObjectManager();

        private Queue<GameObject> Bullets { get; set; } = new Queue<GameObject>();
        private Random Random { get; set; } = new Random();

        public override void OnCreate()
        {
            GameObject player = CreatePlayer();
        }

        private GameObject CreateBullet()
        {
            GameObject bullet = GameObjectManager.CreateGameObject(GameSettings.BulletObjectName);

            bullet.Transform.Position = new Vector2f(
                Random.NextSingle() * GameSettings.WindowWidth,
                Random.NextSingle() * GameSettings.WindowHeight);

            SpriteComponent spriteComponent = bullet.AddComponent<SpriteComponent>();
            spriteComponent.TextureId = (int)GameSettings.TextureId.Bullet; 

            VelocityMovementComponent movementComponent = bullet.AddComponent<VelocityMovementComponent>();
            movementComponent.Velocity = new Vector2f(
                (Random.NextSingle() - 0.5f) * 100,
                (Random.NextSingle() - 0.5f) * 100);

            BoxColliderComponent colliderComponent = bullet.AddComponent<BoxColliderComponent>();
            colliderComponent.SetColliderRect(GameSettings.BulletColliderRect);
            colliderComponent.SetColliderRectOffset(GameSettings.BulletColliderRectOffset);

            DebugCollisionHandlerComponent collisionHandlerComponent = bullet.AddComponent<DebugCollisionHandlerComponent>();

            return bullet;
        }

        private GameObject CreatePlayer()
        {
            GameObject player = GameObjectManager.CreateGameObject(GameSettings.PlayerObjectName);

            SpriteComponent spriteComponent = player.AddComponent<SpriteComponent>();
            spriteComponent.TextureId = (int)GameSettings.TextureId.Player;

            AnimationComponent animationComponent = player.AddComponent<AnimationComponent>();
            animationComponent.AddAnimation((int)AnimationState.Idle, CreateIdleAnimation());
            animationComponent.AddAnimation((int)AnimationState.Walk, CreateWalkAnimation());
            animationComponent.SetAnimationState((int)AnimationState.Idle);

            KeyboardMovementComponent movementComponent = player.AddComponent<KeyboardMovementComponent>();
            movementComponent.Speed = GameSettings.PlayerMovementSpeed;
            movementComponent.LookDirectionChange += animationComponent.OnLookDirectionChanged;

            BoxColliderComponent colliderComponent = player.AddComponent<BoxColliderComponent>();
            colliderComponent.SetColliderRect(GameSettings.PlayerColliderRect);
            colliderComponent.SetColliderRectOffset(GameSettings.PlayerColliderRectOffset);

            DebugCollisionHandlerComponent collisionHandlerComponent = player.AddComponent<DebugCollisionHandlerComponent>();

            return player;
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

        private float spawnDelayTime;

        public override void Update(float deltaTime)
        {
            if (spawnDelayTime < 0.5f)
            {
                spawnDelayTime += deltaTime;
            }
            else
            {
                spawnDelayTime = 0;

                GameObject bullet = CreateBullet();

                Bullets.Enqueue(bullet);
                if (Bullets.Count >= GameSettings.BulletMaxCount)
                {
                    Bullets.Dequeue().Destroy();
                }
            }

            GameObjectManager.Update(deltaTime);
        }

        public override void LateUpdate(float deltaTime)
        {
            GameObjectManager.LateUpdate(deltaTime);
        }

        public override void Draw(GraphicsManager graphicsManager)
        {
            GameObjectManager.Draw(graphicsManager);
        }
    }
}
