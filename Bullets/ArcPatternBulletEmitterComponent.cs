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
    internal class ArcPatternBulletEmitterComponent : Component
    {
        public GameObject Target { get; set; }

        public float PatternInterval { get; set; }

        private GameObjectPool BulletObjectPool { get; set; }
        private TimeManager TimeManager { get; set; }

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
        }

        public override void Update(float deltaTime)
        {
            if (NextPatternTime == 0)
            {
                NextPatternTime = TimeManager.TotalTime + PatternInterval;
            }

            if (TimeManager.TotalTime > NextPatternTime)
            {
                SpawnBulletPattern();
                NextPatternTime = TimeManager.TotalTime + PatternInterval;
            }
        }

        private void SpawnBulletPattern()
        {
            const float arcAngleDegrees = 120;

            const float bulletSpeed = 500;
            const int bulletCount = 20;

            // Angle the arc at the target
            Vector2f direction = Target.Transform.Position - Owner.Transform.Position;

            float targetAngleDegrees = MathF.Atan2(direction.Y, direction.X) * 180 / MathF.PI;
            float minAngleDegrees = targetAngleDegrees - arcAngleDegrees / 2;
            float angleDegrees = minAngleDegrees;
            float angleStep = arcAngleDegrees / (float)bulletCount;

            for (int i = 0; i < bulletCount; i++)
            {
                GameObject bullet = CreateBullet();
                bullet.Transform.Position = Owner.Transform.Position;

                float angleRadians = angleDegrees * MathF.PI / 180;
                Vector2f unitVelocity = new Vector2f(
                        MathF.Cos(angleRadians),
                        MathF.Sin(angleRadians));
                bullet.GetComponent<VelocityMovementComponent>().Velocity = unitVelocity * bulletSpeed;
                bullet.Transform.Position += unitVelocity * GameSettings.EnemyBulletStartRadialOffset;

                // Ensure the bullets always overlap in the expected order
                bullet.GetComponent<SpriteComponent>().SortingOrder = i;

                angleDegrees += angleStep;
            }
        }

        private GameObject CreateBullet()
        {
            GameObject bullet = BulletObjectPool.GetOrCreateObject();

            SpriteComponent spriteComponent = bullet.GetComponent<SpriteComponent>();
            spriteComponent.TextureId = (int)GameSettings.TextureId.EnemyBullet;
            spriteComponent.Origin = new Vector2f(GameSettings.EnemyBulletTextureWidth / 2, GameSettings.EnemyBulletTextureHeight / 2);

            BoxColliderComponent colliderComponent = bullet.GetComponent<BoxColliderComponent>();
            colliderComponent.SetColliderRect(GameSettings.EnemyBulletColliderRect);
            colliderComponent.SetColliderRectOffset(GameSettings.EnemyBulletColliderRectOffset);
            colliderComponent.LayerId = GameSettings.EnemyBulletCollisionLayer;

            RangedDestroyComponent rangedDestroyComponent = bullet.GetComponent<RangedDestroyComponent>();
            rangedDestroyComponent.Target = Owner;
            rangedDestroyComponent.MaxDistance = GameSettings.EnemyBulletMaxDistance;

            TimedDestroyComponent timedDestroyComponent = bullet.GetComponent<TimedDestroyComponent>();
            timedDestroyComponent.TimeToLive = 3;

            return bullet;
        }
    }
}
