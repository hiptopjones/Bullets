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
            float xSpeed = 0;
            if (InputManager.IsKeyPressed(Key.A))
            {
                xSpeed = -Speed;
            }
            else if (InputManager.IsKeyPressed(Key.D))
            {
                xSpeed = Speed;
            }

            float ySpeed = 0;
            if (InputManager.IsKeyPressed(Key.W))
            {
                ySpeed = -Speed;
            }
            if (InputManager.IsKeyPressed(Key.S))
            {
                ySpeed = Speed;
            }

            Owner.Transform.Position += new Vector2f(xSpeed, ySpeed) * deltaTime;
        }
    }
}
