using SFML.System;
using SFML.Window;
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
    internal class BulletEmitterComponent : Component
    {
        private GameObjectManager GameObjectManager { get; set; }
        private InputManager InputManager { get; set; }

        private GameObject Player { get; set; }

        public override void Awake()
        {
            GameObjectManager = ServiceLocator.Instance.GetService<GameObjectManager>();
            if (GameObjectManager == null)
            {
                throw new Exception($"Unable to retrieve game object manager from service locator");
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
            if (InputManager.IsMouseButtonDown(Mouse.Button.Left))
            {
                SpawnBullet();
            }
        }

        private GameObject SpawnBullet()
        {
            // TODO: Check spawn rate

            const float bulletSpeed = 1000;

            GameObject bullet = CreateBullet();
            bullet.Transform.Position = Owner.Transform.Position;
            bullet.Transform.Rotation = Owner.Transform.Rotation;

            float angleDegrees = Player.Transform.Rotation;
            float angleRadians = angleDegrees * MathF.PI / 180;

            bullet.GetComponent<VelocityMovementComponent>().Velocity = new Vector2f(
                    bulletSpeed * MathF.Cos(angleRadians),
                    bulletSpeed * MathF.Sin(angleRadians));

            return bullet;
        }

        private GameObject CreateBullet()
        {
            GameObject bullet = GameObjectManager.CreateGameObject(GameSettings.PlayerBulletObjectName);

            SpriteComponent spriteComponent = bullet.AddComponent<SpriteComponent>();
            spriteComponent.TextureId = (int)GameSettings.TextureId.PlayerBullet;
            spriteComponent.Origin = new Vector2f(GameSettings.PlayerBulletTextureWidth / 2, GameSettings.PlayerBulletTextureHeight / 2);
            spriteComponent.RotationOffset = 90;

            VelocityMovementComponent movementComponent = bullet.AddComponent<VelocityMovementComponent>();

            BoxColliderComponent colliderComponent = bullet.AddComponent<BoxColliderComponent>();
            colliderComponent.SetColliderRect(GameSettings.PlayerBulletColliderRect);
            colliderComponent.SetColliderRectOffset(GameSettings.PlayerBulletColliderRectOffset);

            //DebugCollisionHandlerComponent collisionHandlerComponent = bullet.AddComponent<DebugCollisionHandlerComponent>();

            RangedDestroyComponent destroyComponent = bullet.AddComponent<RangedDestroyComponent>();
            destroyComponent.Target = Owner;
            destroyComponent.MaxDistance = GameSettings.PlayerBulletMaxDistance;

            return bullet;
        }
    }
}
