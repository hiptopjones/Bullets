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
        public const string PLAYER_TEXTURE_FILE_NAME = "Player.png";
        public const float PLAYER_MOVEMENT_SPEED = 500;

        private GameObject Player { get; set; }

        public override void OnCreate()
        {
            Player = new GameObject
            {
                Name = "Player"
            };

            SpriteComponent spriteComponent = Player.AddComponent<SpriteComponent>();
            spriteComponent.TextureFileName = PLAYER_TEXTURE_FILE_NAME;

            KeyboardMovementComponent movementComponent = Player.AddComponent<KeyboardMovementComponent>();
            movementComponent.Speed = PLAYER_MOVEMENT_SPEED;
        }

        public override void OnActivate()
        {
            Player.Awake();
            Player.Start();
        }

        public override void Update(float deltaTime)
        {
            Player.Update(deltaTime);
        }

        public override void LateUpdate(float deltaTime)
        {
            Player.LateUpdate(deltaTime);
        }

        public override void Draw(WindowManager windowManager)
        {
            Player.Draw(windowManager);
        }
    }
}
