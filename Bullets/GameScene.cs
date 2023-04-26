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
                Random.NextSingle() * 800 + 400, //GameSettings.WindowWidth,
                Random.NextSingle() * 600 + 300); //GameSettings.WindowHeight);

            SpriteComponent spriteComponent = bullet.AddComponent<SpriteComponent>();
            spriteComponent.TextureId = (int)GameSettings.TextureId.Bullet;
            spriteComponent.Origin = new Vector2f(GameSettings.BulletTextureWidth / 2, GameSettings.BulletTextureHeight / 2);

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
            spriteComponent.Origin = new Vector2f(GameSettings.PlayerTextureWidth / 2, GameSettings.PlayerTextureHeight / 2);

            KeyboardMovementComponent movementComponent = player.AddComponent<KeyboardMovementComponent>();
            movementComponent.Speed = GameSettings.PlayerMovementSpeed;

            BoxColliderComponent colliderComponent = player.AddComponent<BoxColliderComponent>();
            colliderComponent.SetColliderRect(GameSettings.PlayerColliderRect);
            colliderComponent.SetColliderRectOffset(GameSettings.PlayerColliderRectOffset);

            DebugCollisionHandlerComponent collisionHandlerComponent = player.AddComponent<DebugCollisionHandlerComponent>();

            return player;
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
            if (spawnDelayTime < .5f)
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
