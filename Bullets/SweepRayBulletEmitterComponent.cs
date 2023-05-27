using NLog;
using SFML.System;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static SFML.Window.Keyboard;

namespace Bullets
{
    internal class SweepRayBulletEmitterComponent : Component
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public GameObject Target { get; set; }

        public float PatternInterval { get; set; }

        private TimeManager TimeManager { get; set; }
        private GameObjectPool BulletObjectPool { get; set; }
        private CoroutineManager CoroutineManager { get; set; }

        private Random Random { get; set; } = new Random();

        private float NextPatternTime { get; set; }

        public override void Awake()
        {
            TimeManager = ServiceLocator.Instance.GetService<TimeManager>();
            if (TimeManager == null)
            {
                throw new Exception($"Unable to retrieve time manager from service locator");
            }

            BulletObjectPool = ServiceLocator.Instance.GetService<GameObjectPool>("BulletObjectPool");
            if (BulletObjectPool == null)
            {
                throw new Exception($"Unable to retrieve bullet object pool from service locator");
            }

            CoroutineManager = ServiceLocator.Instance.GetService<CoroutineManager>();
            if (CoroutineManager == null)
            {
                throw new Exception($"Unable to retrieve coroutine manager from service locator");
            }
        }

        public override void Update(float deltaTime)
        {
            if (NextPatternTime == 0)
            {
                NextPatternTime = TimeManager.TotalTime + PatternInterval;
            }

            if (TimeManager.TotalTime > NextPatternTime)
            {
                CoroutineManager.StartCoroutine(SpawnBulletPattern());
                NextPatternTime = TimeManager.TotalTime + PatternInterval;
            }
        }

        private IEnumerator SpawnBulletPattern()
        {
            const int bulletCount = 10;
            float linearBulletSpeed = 300;
            float angularBulletSpeed = 360;
            angularBulletSpeed *= MathF.Sign(Random.NextSingle() - 0.5f);

            TelegraphIntent();

            yield return new WaitForTime(TimeSpan.FromSeconds(0.2f));


            // Angle the pattern at the target
            Vector2f direction = Target.Transform.Position - Owner.Transform.Position;
            float targetAngleDegrees = MathF.Atan2(direction.Y, direction.X) * 180 / MathF.PI;

            List<GameObject> bullets = new List<GameObject>();

            for (int i = 0; i < bulletCount; i++)
            {
                GameObject bullet = CreateBullet();
                bullet.Transform.Position = Owner.Transform.Position;

                float angleRadians = targetAngleDegrees * MathF.PI / 180;
                Vector2f normalizedDirection = new Vector2f(
                        MathF.Cos(angleRadians),
                        MathF.Sin(angleRadians));
                bullet.GetComponent<VelocityMovementComponent>().Velocity = normalizedDirection * linearBulletSpeed * i;
                bullet.Transform.Position += normalizedDirection * GameSettings.EnemyBulletStartRadialOffset;

                // Ensure the bullets always overlap in the expected order
                bullet.GetComponent<SpriteComponent>().SortingOrder = i;

                // Add to the list to be enabled next frame
                bullets.Add(bullet);
            }

            yield return new WaitForFrame();

            // Ensure all bullets start on the same frame
            // Some may be delayed due to difference between reusing pooled objects and creating new objects
            foreach (GameObject bullet in bullets)
            {
                bullet.IsEnabled = true;
            }

            const int bulletSize = 50;
            yield return new WaitForTime(TimeSpan.FromSeconds(bulletSize / linearBulletSpeed));

            foreach (GameObject bullet in bullets)
            {
                bullet.GetComponent<VelocityMovementComponent>().Reset();
            }

            yield return new WaitForTime(TimeSpan.FromSeconds(0.2));

            foreach (GameObject bullet in bullets)
            {
                TimedDestroyComponent timedDestroyComponent = bullet.GetComponent<TimedDestroyComponent>();
                timedDestroyComponent.Reset();
                timedDestroyComponent.TimeToLive = 1f;

                AngularVelocityMovementComponent angularMovementComponent = bullet.GetComponent<AngularVelocityMovementComponent>();
                angularMovementComponent.AngularVelocity = angularBulletSpeed;
                angularMovementComponent.OrbitalCenter = Owner;
                angularMovementComponent.Radius = (Owner.Transform.Position - bullet.Transform.Position).Magnitude();
            }
        }

        private void TelegraphIntent()
        {
            Logger.Info($"Telegraph {GetType().Name}");
        }

        private GameObject CreateBullet()
        {
            GameObject bullet = BulletObjectPool.GetOrCreateObject();
            bullet.IsEnabled = false;

            SpriteComponent spriteComponent = bullet.GetComponent<SpriteComponent>();
            spriteComponent.TextureId = (int)GameSettings.TextureId.EnemyBullet;
            spriteComponent.Origin = new Vector2f(GameSettings.EnemyBulletTextureWidth / 2, GameSettings.EnemyBulletTextureHeight / 2);

            BoxColliderComponent colliderComponent = bullet.GetComponent<BoxColliderComponent>();
            colliderComponent.SetColliderRect(GameSettings.EnemyBulletColliderRect);
            colliderComponent.SetColliderRectOffset(GameSettings.EnemyBulletColliderRectOffset);
            colliderComponent.LayerId = GameSettings.EnemyBulletCollisionLayer;

            TimedDestroyComponent timedDestroyComponent = bullet.GetComponent<TimedDestroyComponent>();
            timedDestroyComponent.TimeToLive = 2f;

            DamageComponent damageComponent = bullet.GetComponent<DamageComponent>();
            damageComponent.Damage = 2;

            return bullet;
        }
    }
}
