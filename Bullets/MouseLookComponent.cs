using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SFML.Window.Keyboard;

namespace Bullets
{
    internal class MouseLookComponent : Component
    {
        private InputManager InputManager { get; set; }

        public override void Start()
        {
            InputManager = ServiceLocator.Instance.GetService<InputManager>();
        }

        public override void Update(float deltaTime)
        {
            Vector2f mousePosition = InputManager.MousePosition;
            Vector2f lookDirection = mousePosition - Owner.Transform.Position;
            float angleDegrees = MathF.Atan2(lookDirection.Y, lookDirection.X) * 180 / MathF.PI;
            Owner.Transform.Rotation = angleDegrees;

            Debug.DrawLine(mousePosition, Owner.Transform.Position);
        }
    }
}
