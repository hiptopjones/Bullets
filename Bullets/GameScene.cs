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
        private Sprite PlayerSprite { get; set; }

        private InputManager InputManager { get; set; }

        public override void OnCreate()
        {
            InputManager = ServiceLocator.Instance.GetService<InputManager>();

            ResourceManager resourceManager = ServiceLocator.Instance.GetService<ResourceManager>();
            Texture playerTexture = resourceManager.GetTexture("Player.png");

            PlayerSprite = new Sprite(playerTexture);
        }

        public override void OnActivate()
        {
        }

        public override void Update(float deltaTime)
        {
            const int moveSpeed = 500;

            Vector2f spritePosition = PlayerSprite.Position;

            int xMove = 0;
            if (InputManager.IsKeyPressed(InputManager.Key.Left))
            {
                xMove = -moveSpeed;
            }
            else if (InputManager.IsKeyPressed(InputManager.Key.Right))
            {
                xMove = moveSpeed;
            }

            int yMove = 0;
            if (InputManager.IsKeyPressed(InputManager.Key.Up))
            {
                yMove = -moveSpeed;
            }
            if (InputManager.IsKeyPressed(InputManager.Key.Down))
            {
                yMove = moveSpeed;
            }

            Vector2f frameMove = new Vector2f(xMove, yMove) * deltaTime;
            PlayerSprite.Position = new Vector2f(spritePosition.X + frameMove.X, spritePosition.Y + frameMove.Y);
        }

        public override void Draw(WindowManager window)
        {
            window.Draw(PlayerSprite);
        }
    }
}
