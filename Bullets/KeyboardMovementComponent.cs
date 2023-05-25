using SFML.System;
using SFML.Window;
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
        public float NormalSpeed { get; set; }

        public float DashSpeed { get; set; }
        public float DashMovementTime { get; set; }
        public float DashCooldownTime { get; set; }

        private InputManager InputManager { get; set; }

        private Vector2f Direction { get; set; }

        private bool IsDashMovementActive { get; set; }
        private float DashMovementRemaining { get; set; }
        private bool IsDashCooldownActive { get; set; }
        private float DashCooldownRemaining { get; set; }

        public override void Start()
        {
            InputManager = ServiceLocator.Instance.GetService<InputManager>();
        }

        public override void Update(float deltaTime)
        {
            if (IsDashMovementActive)
            {
                DashMovementRemaining -= deltaTime;
                if (DashMovementRemaining <= 0)
                {
                    IsDashMovementActive = false;

                    IsDashCooldownActive = true;
                    DashCooldownRemaining = DashCooldownTime;
                }
            }
            else if (IsDashCooldownActive)
            {
                DashCooldownRemaining -= deltaTime;
                if (DashCooldownRemaining <= 0)
                {
                    IsDashCooldownActive = false;
                }
            }
            else if (InputManager.IsMouseButtonDown(Mouse.Button.Right))
            {
                IsDashMovementActive = true;
                DashMovementRemaining = DashMovementTime;
            }

            if (!IsDashMovementActive)
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
                Direction = new Vector2f(xDirection, yDirection).Normalize();
            }

            // Calculate new position
            float speed = IsDashMovementActive ? DashSpeed : NormalSpeed;
            Owner.Transform.Position += Direction * speed * deltaTime;
        }
    }
}
