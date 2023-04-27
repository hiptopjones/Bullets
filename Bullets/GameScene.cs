using NLog;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
        protected GameObjectManager GameObjectManager { get; set; }

        private Queue<GameObject> Bullets { get; set; } = new Queue<GameObject>();

        public override void OnCreate()
        {
            GameObjectManager = new GameObjectManager();
            ServiceLocator.Instance.ProvideService(GameObjectManager);

            GameObject player = CreatePlayer();

            GameObject emitter = CreateBulletEmitter();
        }

        private GameObject CreatePlayer()
        {
            GameObject player = GameObjectManager.CreateGameObject(GameSettings.PlayerObjectName);
            player.Transform.Position = GameSettings.PlayerStartPosition;

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

        private GameObject CreateBulletEmitter()
        {
            GameObject bulletEmitter = GameObjectManager.CreateGameObject("Bullet Emitter");
            bulletEmitter.Transform.Position = GameSettings.BulletEmitterStartPosition;

            BulletEmitterComponent bulletEmitterComponent = bulletEmitter.AddComponent<BulletEmitterComponent>();

            return bulletEmitter;
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
