using NLog;
using SFML.System;
using System.Numerics;

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
        protected GameObjectManager GameObjectManager { get; set; }
        protected GameObjectPool BulletObjectPool { get; set; }

        private Queue<GameObject> Bullets { get; set; } = new Queue<GameObject>();

        public override void OnCreate()
        {
            GameObjectManager = new GameObjectManager();
            ServiceLocator.Instance.ProvideService(GameObjectManager);

            BulletObjectPool = new GameObjectPool(CreateBullet);
            ServiceLocator.Instance.ProvideService("BulletObjectPool", BulletObjectPool);

            GameObject player = CreatePlayer();
            ServiceLocator.Instance.ProvideService("Player", player);

            GameObject enemy = CreateEnemy();
        }

        private GameObject CreateBullet()
        {
            GameObject bullet = GameObjectManager.CreateGameObject(GameSettings.BulletObjectName);

            // Add standard (empty) components
            SpriteComponent spriteComponent = bullet.AddComponent<SpriteComponent>();
            VelocityMovementComponent movementComponent = bullet.AddComponent<VelocityMovementComponent>();
            BoxColliderComponent colliderComponent = bullet.AddComponent<BoxColliderComponent>();
            //DebugCollisionHandlerComponent collisionHandlerComponent = bullet.AddComponent<DebugCollisionHandlerComponent>();
            BulletCollisionHandlerComponent collisionHandlerComponent = bullet.AddComponent<BulletCollisionHandlerComponent>();
            RangedDestroyComponent rangedDestroyComponent = bullet.AddComponent<RangedDestroyComponent>();
            TimedDestroyComponent timedDestroyComponent = bullet.AddComponent<TimedDestroyComponent>();

            bullet.OnDestroyed = BulletObjectPool.OnDestroyed;

            return bullet;
        }

        private GameObject CreatePlayer()
        {
            GameObject player = GameObjectManager.CreateGameObject(GameSettings.PlayerObjectName);
            player.Transform.Position = GameSettings.PlayerStartPosition;

            SpriteComponent spriteComponent = player.AddComponent<SpriteComponent>();
            spriteComponent.TextureId = (int)GameSettings.TextureId.Player;
            spriteComponent.Origin = new Vector2f(GameSettings.PlayerTextureWidth / 2, GameSettings.PlayerTextureHeight / 2);
            spriteComponent.RotationOffset = 90;

            KeyboardMovementComponent movementComponent = player.AddComponent<KeyboardMovementComponent>();
            movementComponent.Speed = GameSettings.PlayerMovementSpeed;

            //KeyboardLookComponent lookComponent = player.AddComponent<KeyboardLookComponent>();
            //lookComponent.LookSpeed = GameSettings.PlayerRotationSpeed;

            MouseLookComponent lookComponent = player.AddComponent<MouseLookComponent>();

            BoxColliderComponent colliderComponent = player.AddComponent<BoxColliderComponent>();
            colliderComponent.SetColliderRect(GameSettings.PlayerColliderRect);
            colliderComponent.SetColliderRectOffset(GameSettings.PlayerColliderRectOffset);
            colliderComponent.LayerId = (int)GameSettings.PlayerCollisionLayer;

            SingleBulletEmitterComponent bulletEmitterComponent = player.AddComponent<SingleBulletEmitterComponent>();

            DebugCollisionHandlerComponent collisionHandlerComponent = player.AddComponent<DebugCollisionHandlerComponent>();

            return player;
        }

        private GameObject CreateEnemy()
        {
            GameObject enemy = GameObjectManager.CreateGameObject(GameSettings.EnemyTurretObjectName);
            enemy.Transform.Position = GameSettings.EnemyTurretStartPosition;

            SpriteComponent spriteComponent = enemy.AddComponent<SpriteComponent>();
            spriteComponent.TextureId = (int)GameSettings.TextureId.EnemyTurret;
            spriteComponent.Origin = new Vector2f(GameSettings.EnemyTurretTextureWidth / 2, GameSettings.EnemyTurretTextureHeight / 2);

            GameObject player = ServiceLocator.Instance.GetService<GameObject>("Player");

            //EnemyMovementComponent movementComponent = enemy.AddComponent<EnemyMovementComponent>();
            //movementComponent.Speed = GameSettings.EnemyTurretMovementSpeed;
            //movementComponent.Target = player;

            BoxColliderComponent colliderComponent = enemy.AddComponent<BoxColliderComponent>();
            colliderComponent.SetColliderRect(GameSettings.EnemyTurretColliderRect);
            colliderComponent.SetColliderRectOffset(GameSettings.EnemyTurretColliderRectOffset);
            colliderComponent.LayerId = (int)GameSettings.EnemyTurretCollisionLayer;

            ArcPatternBulletEmitterComponent bulletEmitterComponent = enemy.AddComponent<ArcPatternBulletEmitterComponent>();
            bulletEmitterComponent.Target = player;
            bulletEmitterComponent.PatternInterval = 3;

            DebugCollisionHandlerComponent collisionHandlerComponent = enemy.AddComponent<DebugCollisionHandlerComponent>();

            return enemy;
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
