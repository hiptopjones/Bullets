using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Bullets
{
    internal class InputManager
    {
        private HashSet<Keyboard.Key> KeyPressed { get; set; } = new HashSet<Keyboard.Key>();
        private HashSet<Keyboard.Key> KeyDown { get; set; } = new HashSet<Keyboard.Key>();
        private HashSet<Keyboard.Key> KeyUp { get; set; } = new HashSet<Keyboard.Key>();

        private Vector2f _mousePosition = new Vector2f();
        public Vector2f MousePosition
        {
            get => _mousePosition;
        }

        public void OnFrameStarted()
        {
            // These start fresh on every frame
            KeyUp.Clear();
            KeyDown.Clear();
        }

        public bool IsKeyPressed(Keyboard.Key keycode)
        {
            return KeyPressed.Contains(keycode);
        }

        public bool IsKeyDown(Keyboard.Key keycode)
        {
            return KeyDown.Contains(keycode);
        }

        public bool IsKeyUp(Keyboard.Key keycode)
        {
            return KeyUp.Contains(keycode);
        }

        public void OnKeyPressed(object sender, KeyEventArgs e)
        {
            if (!KeyPressed.Contains(e.Code))
            {
                KeyPressed.Add(e.Code);
                KeyDown.Add(e.Code);
            }
        }

        public void OnKeyReleased(object sender, KeyEventArgs e)
        {
            if (KeyPressed.Contains(e.Code))
            {
                KeyPressed.Remove(e.Code);
                KeyUp.Add(e.Code);
            }
        }

        public void OnMouseMoved(object sender, MouseMoveEventArgs e)
        {
            _mousePosition = new Vector2f(e.X, e.Y);
        }
    }
}
