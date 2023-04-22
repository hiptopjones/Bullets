using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static SFML.Window.Keyboard;

namespace Bullets
{
    internal class InputManager
    {
        public enum Key
        {
            None = 0,
            Left = 1,
            Right = 2,
            Up = 3,
            Down = 4,
            Escape = 5,
        }

        private HashSet<Keyboard.Key> KeyPressed { get; set; } = new HashSet<Keyboard.Key>();
        private HashSet<Keyboard.Key> KeyDown { get; set; } = new HashSet<Keyboard.Key>();
        private HashSet<Keyboard.Key> KeyUp { get; set; } = new HashSet<Keyboard.Key>();

        public void OnFrameStarted()
        {
            // These start fresh on every frame
            KeyUp.Clear();
            KeyDown.Clear();
        }

        public bool IsKeyPressed(Key keycode)
        {
            switch (keycode)
            {
                case Key.Left:
                    return KeyPressed.Contains(Keyboard.Key.Left) || KeyPressed.Contains(Keyboard.Key.A);
                case Key.Right:
                    return KeyPressed.Contains(Keyboard.Key.Right) || KeyPressed.Contains(Keyboard.Key.D);
                case Key.Up:
                    return KeyPressed.Contains(Keyboard.Key.Up) || KeyPressed.Contains(Keyboard.Key.W);
                case Key.Down:
                    return KeyPressed.Contains(Keyboard.Key.Down) || KeyPressed.Contains(Keyboard.Key.S);
                case Key.Escape:
                    return KeyPressed.Contains(Keyboard.Key.Escape);
            }

            return false;
        }

        public bool IsKeyDown(Key keycode)
        {
            switch (keycode)
            {
                case Key.Left:
                    return KeyDown.Contains(Keyboard.Key.Left) || KeyDown.Contains(Keyboard.Key.A);
                case Key.Right:
                    return KeyDown.Contains(Keyboard.Key.Right) || KeyDown.Contains(Keyboard.Key.D);
                case Key.Up:
                    return KeyDown.Contains(Keyboard.Key.Up) || KeyDown.Contains(Keyboard.Key.W);
                case Key.Down:
                    return KeyDown.Contains(Keyboard.Key.Down) || KeyDown.Contains(Keyboard.Key.S);
                case Key.Escape:
                    return KeyDown.Contains(Keyboard.Key.Escape);
            }

            return false;
        }

        public bool IsKeyUp(Key keycode)
        {
            switch (keycode)
            {
                case Key.Left:
                    return KeyUp.Contains(Keyboard.Key.Left) || KeyUp.Contains(Keyboard.Key.A);
                case Key.Right:
                    return KeyUp.Contains(Keyboard.Key.Right) || KeyUp.Contains(Keyboard.Key.D);
                case Key.Up:
                    return KeyUp.Contains(Keyboard.Key.Up) || KeyUp.Contains(Keyboard.Key.W);
                case Key.Down:
                    return KeyUp.Contains(Keyboard.Key.Down) || KeyUp.Contains(Keyboard.Key.S);
                case Key.Escape:
                    return KeyUp.Contains(Keyboard.Key.Escape);
            }

            return false;
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
    }
}
