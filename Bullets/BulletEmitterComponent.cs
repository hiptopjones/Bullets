using SFML.System;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullets
{
    internal class BulletEmitterComponent : Component
    {
        private GameObjectManager GameObjectManager { get; set; }
        private CoroutineManager CoroutineManager { get; set; }

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
        }

        public override void Start()
        {
            CoroutineManager.StartCoroutine(EmitterCoroutine());
        }

        private IEnumerator EmitterCoroutine()
        {
            int i = 0;
            while (i < 20)
            {
                EmitBullet();
                yield return new WaitForTime(TimeSpan.FromSeconds(0.2f));

                i++;
            }
        }

        private void EmitBullet()
        {
            Debug.Log("Emit bullet");
            CreateBullet();
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


    }
}
