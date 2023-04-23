using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullets
{
    public class LookDirectionEventArgs : EventArgs
    {
        public Vector2f LookDirection { get; set; }
    }

    internal class KeyboardMovementComponent : Component
    {
        public float Speed { get; set; }

        public event EventHandler<LookDirectionEventArgs> LookDirectionChange;

        private Vector2f PreviousLookDirection { get; set; }

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

            // Using velocity as a proxy for the direction the player is looking
            Vector2f lookDirection = new Vector2f(xSpeed, ySpeed);
            if (lookDirection != PreviousLookDirection)
            {
                RaiseLookDirectionChangeEvent(lookDirection);
                PreviousLookDirection = lookDirection;
            }

            Owner.Transform.Position += new Vector2f(xSpeed, ySpeed) * deltaTime;
        }

        private void RaiseLookDirectionChangeEvent(Vector2f lookDirection)
        {
            LookDirectionEventArgs eventArgs = new LookDirectionEventArgs
            {
                LookDirection = lookDirection
            };

            LookDirectionChange?.Invoke(this, eventArgs);
        }
    }
}
