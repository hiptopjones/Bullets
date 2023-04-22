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
    internal class Game
    {
        private Window Window { get; }

        private ResourceManager ResourceManager { get; }
        private InputManager InputManager { get; }

        private Clock Clock { get; }
        private float DeltaTime { get; set; }

        private Sprite PlayerSprite { get; set; }

        public bool IsRunning => Window.IsOpen;

        public Game()
        {
            InputManager = new InputManager();
            ResourceManager = new ResourceManager();

            Window = new Window("Bullets", 800, 600);
            Window.KeyPressed += InputManager.OnKeyPressed;
            Window.KeyReleased += InputManager.OnKeyReleased;

            Texture playerTexture = ResourceManager.GetTexture("Player.png");
            PlayerSprite = new Sprite(playerTexture);

            Clock = new Clock();
        }

        public void StartFrame()
        {
            Time time = Clock.Restart();
            DeltaTime = time.AsSeconds();

            // Need to clear state at the start of the frame
            InputManager.OnFrameStarted();
        }

        public void ProcessEvents()
        {
            Window.ProcessEvents();

            if (InputManager.IsKeyPressed(InputManager.Key.Escape))
            {
                Window.Close();
            }
        }

        public void Update()
        {
            Window.Update();

            MovePlayer();
        }

        public void Draw()
        {
            Window.BeginDraw();
            Window.Draw(PlayerSprite);
            Window.EndDraw();
        }

        private void MovePlayer()
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

            Vector2f frameMove = new Vector2f(xMove, yMove) * DeltaTime;
            PlayerSprite.Position = new Vector2f(spritePosition.X + frameMove.X, spritePosition.Y + frameMove.Y);
        }
    }
}
