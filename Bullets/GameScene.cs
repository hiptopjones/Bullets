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
    internal class GameScene : Scene
    {
        protected GameObjectCollection SceneGameObjects { get; } = new GameObjectCollection();

        private GameObject Player { get; set; }

        private Queue<GameObject> Bullets { get; } = new Queue<GameObject>();
        private Random Random { get; } = new Random();

        public override void OnCreate()
        {
            Player = SceneGameObjects.CreateGameObject(GameSettings.PlayerObjectName);

            SpriteComponent spriteComponent = Player.AddComponent<SpriteComponent>();
            spriteComponent.TextureId = GameSettings.TextureId.Player;

            KeyboardMovementComponent movementComponent = Player.AddComponent<KeyboardMovementComponent>();
            movementComponent.Speed = GameSettings.PlayerMovementSpeed;
        }
        public override void OnDestroy()
        {
        }

        public override void OnActivate()
        {
        }

        public override void OnDeactivate()
        {
        }

        public override void Update(float deltaTime)
        {
            GameObject bullet = SceneGameObjects.CreateGameObject("Bullet");
            SpriteComponent spritComponent = bullet.AddComponent<SpriteComponent>();
            spritComponent.TextureId = GameSettings.TextureId.Bullet;

            bullet.Transform.Position = new Vector2f(Random.Next(GameSettings.WindowWidth), Random.Next(GameSettings.WindowHeight));

            Bullets.Enqueue(bullet);
            if (Bullets.Count > GameSettings.BulletMaxCount)
            {
                Bullets.Dequeue().Destroy();
            }

            SceneGameObjects.Update(deltaTime);
        }

        public override void LateUpdate(float deltaTime)
        {
            SceneGameObjects.LateUpdate(deltaTime);
        }

        public override void Draw(WindowManager windowManager)
        {
            SceneGameObjects.Draw(windowManager);
        }
    }
}
