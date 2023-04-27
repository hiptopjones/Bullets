using SFML.System;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Bullets
{
    internal class BulletEmitterComponent : Component
    {
        private GameObjectManager GameObjectManager { get; set; }
        private CoroutineManager CoroutineManager { get; set; }

        private GameObject Player { get; set; }

        private Random Random { get; set; } = new Random();

        public override void Awake()
        {
            GameObjectManager = ServiceLocator.Instance.GetService<GameObjectManager>();
            if (GameObjectManager == null)
            {
                throw new Exception($"Unable to retrieve game object manager from service locator");
            }

            CoroutineManager = ServiceLocator.Instance.GetService<CoroutineManager>();
            if (CoroutineManager == null)
            {
                throw new Exception($"Unable to retrieve coroutine manager from service locator");
            }

            Player = ServiceLocator.Instance.GetService<GameObject>("Player");
            if (Player == null)
            {
                throw new Exception($"Unable to retrieve player from service locator");
            }
        }

        public override void Start()
        {
            CoroutineManager.StartCoroutine(RingEmitterCoroutine());
        }

        public override void Update(float deltaTime)
        {
            Debug.DrawLine(Player.Transform.Position, Owner.Transform.Position);
        }

        private IEnumerator LineEmitterCoroutine()
        {
            const float bulletSpeed = 250;
            const int bulletCount = 20;

            for (int i = 0; i < bulletCount; i++)
            {
                GameObject bullet = CreateBullet();
                bullet.Transform.Position = Owner.Transform.Position;

                // Start the spiral angled at the player's direction
                Vector2f direction = Player.Transform.Position - Owner.Transform.Position;

                float angleDegrees = MathF.Atan2(direction.Y, direction.X) * 180 / MathF.PI;
                float angleRadians = angleDegrees * MathF.PI / 180;

                bullet.GetComponent<VelocityMovementComponent>().Velocity = new Vector2f(
                        bulletSpeed * MathF.Cos(angleRadians),
                        bulletSpeed * MathF.Sin(angleRadians));

                yield return new WaitForTime(TimeSpan.FromSeconds(0.2f));
            }
        }

        private IEnumerator SpiralEmitterCoroutine()
        {
            const float sweepCount = 3;

            const float bulletSpeed = 250;
            const int bulletCountPerSweep = 20;

            // Start the spiral angled at the player's direction
            Vector2f direction = Player.Transform.Position - Owner.Transform.Position;

            float angleDegrees = MathF.Atan2(direction.Y, direction.X) * 180 / MathF.PI;
            float angleStep = 360 / (float)bulletCountPerSweep;

            for (int i = 0; i < bulletCountPerSweep * sweepCount; i++)
            {
                GameObject bullet = CreateBullet();
                bullet.Transform.Position = Owner.Transform.Position;

                float angleRadians = angleDegrees * MathF.PI / 180;
                bullet.GetComponent<VelocityMovementComponent>().Velocity = new Vector2f(
                        bulletSpeed * MathF.Cos(angleRadians),
                        bulletSpeed * MathF.Sin(angleRadians));

                angleDegrees += angleStep;

                yield return new WaitForTime(TimeSpan.FromSeconds(0.05f));
            }
        }

        private IEnumerator RingEmitterCoroutine()
        {
            const int ringCount = 5;

            const float bulletSpeed = 250;
            const int bulletCount = 20;

            float angleDegrees = 0;
            float angleStep = 360 / (float)bulletCount;

            for (int j = 0; j < ringCount; j++)
            {
                for (int i = 0; i < bulletCount; i++)
                {
                    GameObject bullet = CreateBullet();
                    bullet.Transform.Position = Owner.Transform.Position;

                    float angleRadians = angleDegrees * MathF.PI / 180;
                    bullet.GetComponent<VelocityMovementComponent>().Velocity = new Vector2f(
                            bulletSpeed * MathF.Cos(angleRadians),
                            bulletSpeed * MathF.Sin(angleRadians));

                    angleDegrees += angleStep;
                }

                yield return new WaitForTime(TimeSpan.FromSeconds(0.5f));
            }
        }

        private GameObject CreateBullet()
        {
            GameObject bullet = GameObjectManager.CreateGameObject(GameSettings.BulletObjectName);

            SpriteComponent spriteComponent = bullet.AddComponent<SpriteComponent>();
            spriteComponent.TextureId = (int)GameSettings.TextureId.Bullet;
            spriteComponent.Origin = new Vector2f(GameSettings.BulletTextureWidth / 2, GameSettings.BulletTextureHeight / 2);

            VelocityMovementComponent movementComponent = bullet.AddComponent<VelocityMovementComponent>();

            BoxColliderComponent colliderComponent = bullet.AddComponent<BoxColliderComponent>();
            colliderComponent.SetColliderRect(GameSettings.BulletColliderRect);
            colliderComponent.SetColliderRectOffset(GameSettings.BulletColliderRectOffset);

            DebugCollisionHandlerComponent collisionHandlerComponent = bullet.AddComponent<DebugCollisionHandlerComponent>();

            return bullet;
        }


    }
}
