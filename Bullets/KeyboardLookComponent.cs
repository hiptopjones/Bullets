using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SFML.Window.Keyboard;

namespace Bullets
{
    internal class KeyboardLookComponent : Component
    {
        public float LookSpeed { get; set; }

        private InputManager InputManager { get; set; }

        public override void Start()
        {
            InputManager = ServiceLocator.Instance.GetService<InputManager>();
        }

        public override void Update(float deltaTime)
        {
            float lookDirection = 0;
            if (InputManager.IsKeyPressed(Key.Left))
            {
                lookDirection = -1;
            }
            else if (InputManager.IsKeyPressed(Key.Right))
            {
                lookDirection = 1;
            }

            float angleDegrees = lookDirection * LookSpeed * deltaTime;
            Owner.Transform.Rotation += angleDegrees;
        }
    }
}
