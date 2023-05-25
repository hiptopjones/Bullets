using NLog;
using SFML.Graphics;
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
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

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
            DamageComponent damageComponent = bullet.AddComponent<DamageComponent>();

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

            KeyboardMovementComponent movementComponent = player.AddComponent<KeyboardMovementComponent>();
            movementComponent.NormalSpeed = GameSettings.PlayerNormalMovementSpeed;
            movementComponent.DashSpeed = GameSettings.PlayerDashMovementSpeed;
            movementComponent.DashMovementTime = GameSettings.PlayerDashMovementTime;
            movementComponent.DashCooldownTime = GameSettings.PlayerDashCooldownTime;

            //KeyboardLookComponent lookComponent = player.AddComponent<KeyboardLookComponent>();
            //lookComponent.LookSpeed = GameSettings.PlayerRotationSpeed;

            MouseLookComponent lookComponent = player.AddComponent<MouseLookComponent>();

            BoxColliderComponent colliderComponent = player.AddComponent<BoxColliderComponent>();
            colliderComponent.SetColliderRect(GameSettings.PlayerColliderRect);
            colliderComponent.SetColliderRectOffset(GameSettings.PlayerColliderRectOffset);
            colliderComponent.LayerId = (int)GameSettings.PlayerCollisionLayer;

            SingleBulletEmitterComponent bulletEmitterComponent = player.AddComponent<SingleBulletEmitterComponent>();

            //DebugCollisionHandlerComponent collisionHandlerComponent = player.AddComponent<DebugCollisionHandlerComponent>();
            DamageCollisionHandlerComponent collisionHandlerComponent = player.AddComponent<DamageCollisionHandlerComponent>();

            HealthBarComponent healthBarComponent = player.AddComponent<HealthBarComponent>();
            healthBarComponent.ForegroundTextureId = (int)GameSettings.TextureId.HealthBarForeground;
            healthBarComponent.BackgroundTextureId = (int)GameSettings.TextureId.HealthBarBackground;
            healthBarComponent.Offset = new Vector2f(0, -100);
            healthBarComponent.Size = new Vector2f(100, 10);

            HealthComponent healthComponent = player.AddComponent<HealthComponent>();
            healthComponent.MaxHealth = 50;
            healthComponent.HealthChanged += healthBarComponent.OnHealthChanged;

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

            //DebugCollisionHandlerComponent collisionHandlerComponent = enemy.AddComponent<DebugCollisionHandlerComponent>();
            DamageCollisionHandlerComponent collisionHandlerComponent = enemy.AddComponent<DamageCollisionHandlerComponent>();

            HealthBarComponent healthBarComponent = enemy.AddComponent<HealthBarComponent>();
            healthBarComponent.ForegroundTextureId = (int)GameSettings.TextureId.HealthBarForeground;
            healthBarComponent.BackgroundTextureId = (int)GameSettings.TextureId.HealthBarBackground;
            healthBarComponent.Offset = new Vector2f(0, -100);
            healthBarComponent.Size = new Vector2f(100, 10);

            DamageNumbersComponent damageNumbersComponent = enemy.AddComponent<DamageNumbersComponent>();
            damageNumbersComponent.FontId = (int)GameSettings.FontId.DamageNumbers;
            damageNumbersComponent.FillColor = Color.Green;
            damageNumbersComponent.OutlineColor = Color.White;
            damageNumbersComponent.OutlineThickness = 3;
            damageNumbersComponent.FontSize = GameSettings.DamageNumbersTextSize;
            damageNumbersComponent.PositionOffset = new Vector2f(0, -80);
            damageNumbersComponent.RandomOffsetRange = new Vector2f(50, 0);
            damageNumbersComponent.EffectVelocity = new Vector2f(0, -100);
            damageNumbersComponent.EffectDuration = 0.5f;

            ParticlesComponent particlesComponent = enemy.AddComponent<ParticlesComponent>();
            particlesComponent.CircleRadius = 3;
            particlesComponent.PointCount = 5;
            particlesComponent.FillColor = Color.White;
            particlesComponent.PositionOffset = new Vector2f(0, 0);
            particlesComponent.RandomOffsetRange = new Vector2f(50,  50);
            particlesComponent.EffectSpeed = 500;
            particlesComponent.EffectDuration = 0.25f;

            HealthComponent healthComponent = enemy.AddComponent<HealthComponent>();
            healthComponent.MaxHealth = 500;
            healthComponent.HealthChanged += healthBarComponent.OnHealthChanged;
            healthComponent.HealthChanged += damageNumbersComponent.OnHealthChanged;
            healthComponent.HealthChanged += particlesComponent.OnHealthChanged;

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
