using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if (InputManager.IsKeyPressed(InputManager.Key.Left))
            {
                xSpeed = -Speed;
            }
            else if (InputManager.IsKeyPressed(InputManager.Key.Right))
            {
                xSpeed = Speed;
            }

            float ySpeed = 0;
            if (InputManager.IsKeyPressed(InputManager.Key.Up))
            {
                ySpeed = -Speed;
            }
            if (InputManager.IsKeyPressed(InputManager.Key.Down))
            {
                ySpeed = Speed;
            }

           Owner.Transform.Position += new Vector2f(xSpeed, ySpeed) * deltaTime;
        }
    }
}
