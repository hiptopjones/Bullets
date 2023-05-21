using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SFML.Window.Keyboard;

namespace Bullets
{
    internal class KeyboardMovementComponent : Component
    {
        public float Speed { get; set; }

        private InputManager InputManager { get; set; }

        public override void Start()
        {
            InputManager = ServiceLocator.Instance.GetService<InputManager>();
        }

        public override void Update(float deltaTime)
        {
            float xDirection = 0;
            if (InputManager.IsKeyPressed(Key.A))
            {
                xDirection = -1;
            }
            else if (InputManager.IsKeyPressed(Key.D))
            {
                xDirection = 1;
            }

            float yDirection = 0;
            if (InputManager.IsKeyPressed(Key.W))
            {
                yDirection = -1;
            }
            if (InputManager.IsKeyPressed(Key.S))
            {
                yDirection = 1;
            }

            // Normalize to avoid going faster on diagonal than along axes
            Vector2f direction = new Vector2f(xDirection, yDirection);
            Owner.Transform.Position += direction.Normalize() * Speed * deltaTime;
        }
    }
}
