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
    internal class BulletPatternEmitterComponent : Component
    {
        private GameObjectManager GameObjectManager { get; set; }
        private CoroutineManager CoroutineManager { get; set; }
        private InputManager InputManager { get; set; }

        private GameObject Player { get; set; }

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

            InputManager = ServiceLocator.Instance.GetService<InputManager>();
            if (InputManager == null)
            {
                throw new Exception($"Unable to retrieve input manager from service locator");
            }

            Player = ServiceLocator.Instance.GetService<GameObject>("Player");
            if (Player == null)
            {
                throw new Exception($"Unable to retrieve player from service locator");
            }
        }

        public override void Update(float deltaTime)
        {
            if (InputManager.IsKeyDown(Key.Num1))
            {
                CoroutineManager.StartCoroutine(LineEmitterComponent());
            }
            else if (InputManager.IsKeyDown(Key.Num2))
            {
                CoroutineManager.StartCoroutine(SpiralEmitterCoroutine());
            }
            else if (InputManager.IsKeyDown(Key.Num3))
            {
                CoroutineManager.StartCoroutine(RingEmitterCoroutine());
            }
        }

        private IEnumerator SingleEmitterComponent()
        {
            const float bulletSpeed = 250;

            GameObject bullet = CreateBullet();
            bullet.Transform.Position = Owner.Transform.Position;

            // Start the spiral angled at the player's direction
            Vector2f direction = Player.Transform.Position - Owner.Transform.Position;

            float angleDegrees = MathF.Atan2(direction.Y, direction.X) * 180 / MathF.PI;
            float angleRadians = angleDegrees * MathF.PI / 180;

            bullet.GetComponent<VelocityMovementComponent>().Velocity = new Vector2f(
                    bulletSpeed * MathF.Cos(angleRadians),
                    bulletSpeed * MathF.Sin(angleRadians));

            yield return null;
        }

        private IEnumerator LineEmitterComponent()
        {
            const float bulletSpeed = 500;
            const int bulletCount = 5;

            for (int i = 0; i < bulletCount; i++)
            {
                GameObject bullet = CreateBullet();
                bullet.Transform.Position = Owner.Transform.Position;

                // Start the spiral angled at the player's direction
                Vector2f direction = Player.Transform.Position - Owner.Transform.Position;

                float angleDegrees = MathF.Atan2(direction.Y, direction.X) * 180 / MathF.PI;
                float angleRadians = angleDegrees * MathF.PI / 180;
                Vector2f unitVelocity = new Vector2f(
                        MathF.Cos(angleRadians),
                        MathF.Sin(angleRadians));
                bullet.GetComponent<VelocityMovementComponent>().Velocity = unitVelocity * bulletSpeed;
                bullet.Transform.Position += unitVelocity * GameSettings.EnemyBulletStartRadialOffset;

                yield return new WaitForTime(TimeSpan.FromSeconds(0.2f));
            }
        }

        private IEnumerator SpiralEmitterCoroutine()
        {
            const float sweepCount = 7;
            const float sweepOffsetAngle = 45;

            const float bulletSpeed = 500;
            const int bulletCountPerSweep = 10;

            // Start the spiral angled at the player's direction
            Vector2f direction = Player.Transform.Position - Owner.Transform.Position;

            for (int j = 0; j < sweepCount; j++)
            {
                float angleDegrees = MathF.Atan2(direction.Y, direction.X) * 180 / MathF.PI + sweepOffsetAngle * j;
                float angleStep = 360 / (float)bulletCountPerSweep;

                for (int i = 0; i < bulletCountPerSweep; i++)
                {
                    GameObject bullet = CreateBullet();
                    bullet.Transform.Position = Owner.Transform.Position;

                    float angleRadians = angleDegrees * MathF.PI / 180;
                    Vector2f unitVelocity = new Vector2f(
                            MathF.Cos(angleRadians),
                            MathF.Sin(angleRadians));
                    bullet.GetComponent<VelocityMovementComponent>().Velocity = unitVelocity * bulletSpeed;
                    bullet.Transform.Position += unitVelocity * GameSettings.EnemyBulletStartRadialOffset;

                    angleDegrees += angleStep;
                }

                yield return new WaitForTime(TimeSpan.FromSeconds(0.1f));
            }
        }

        private IEnumerator RingEmitterCoroutine()
        {
            const int ringCount = 5;

            const float bulletSpeed = 500;
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
                    Vector2f unitVelocity = new Vector2f(
                            MathF.Cos(angleRadians),
                            MathF.Sin(angleRadians));
                    bullet.GetComponent<VelocityMovementComponent>().Velocity = unitVelocity * bulletSpeed;
                    bullet.Transform.Position += unitVelocity * GameSettings.EnemyBulletStartRadialOffset;

                    angleDegrees += angleStep;
                }

                yield return new WaitForTime(TimeSpan.FromSeconds(0.5f));
            }
        }

        private IEnumerator PulseRingEmitterCoroutine()
        {
            Dictionary<GameObject, Vector2f> bullets = new Dictionary<GameObject, Vector2f>();

            const float bulletSpeed = 250;
            const int bulletCount = 20;

            float angleDegrees = 0;
            float angleStep = 360 / (float)bulletCount;

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

                bullets[bullet] = unitVelocity;

                angleDegrees += angleStep;
            }

            for (float t = 0; t < 1; t += 1 / 60f)
            {
                float v = Easings.InBack(t);

                foreach (GameObject bullet in bullets.Keys)
                {
                    VelocityMovementComponent movementComponent = bullet.GetComponent<VelocityMovementComponent>();

                    Vector2f unitVelocity = bullets[bullet];
                    movementComponent.Velocity += unitVelocity * bulletSpeed * v;
                }

                yield return new WaitForFrame();
            }
        }

        private GameObject CreateBullet()
        {
            GameObject bullet = GameObjectManager.CreateGameObject(GameSettings.EnemyBulletObjectName);

            SpriteComponent spriteComponent = bullet.AddComponent<SpriteComponent>();
            spriteComponent.TextureId = (int)GameSettings.TextureId.EnemyBullet;
            spriteComponent.Origin = new Vector2f(GameSettings.EnemyBulletTextureWidth / 2, GameSettings.EnemyBulletTextureHeight / 2);

            VelocityMovementComponent movementComponent = bullet.AddComponent<VelocityMovementComponent>();

            BoxColliderComponent colliderComponent = bullet.AddComponent<BoxColliderComponent>();
            colliderComponent.SetColliderRect(GameSettings.EnemyBulletColliderRect);
            colliderComponent.SetColliderRectOffset(GameSettings.EnemyBulletColliderRectOffset);

            //DebugCollisionHandlerComponent collisionHandlerComponent = bullet.AddComponent<DebugCollisionHandlerComponent>();

            RangedDestroyComponent destroyComponent = bullet.AddComponent<RangedDestroyComponent>();
            destroyComponent.Target = Owner;
            destroyComponent.MaxDistance = GameSettings.EnemyBulletMaxDistance;

            return bullet;
        }


    }
}
