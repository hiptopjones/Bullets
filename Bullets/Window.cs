using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullets
{
    class Window
    {
        private RenderWindow RenderWindow { get; }

        public event EventHandler<KeyEventArgs> KeyPressed;
        public event EventHandler<KeyEventArgs> KeyReleased;

        public bool IsOpen => RenderWindow.IsOpen;

        public Window(string windowName, uint windowWidth, uint windowHeight)
        {
            VideoMode videoMode = new VideoMode(windowWidth, windowHeight);

            RenderWindow = new RenderWindow(videoMode, windowName, Styles.Titlebar);
            RenderWindow.SetVerticalSyncEnabled(true);

            RenderWindow.Closed += OnClosed;
            RenderWindow.KeyPressed += OnKeyPressed;
            RenderWindow.KeyReleased += OnKeyReleased;
        }

        public void ProcessEvents()
        {
            RenderWindow.DispatchEvents();
        }

        public void Update()
        {
        }

        public void BeginDraw()
        {
            RenderWindow.Clear(Color.White);
        }

        public void Draw(Drawable drawable)
        {
            RenderWindow.Draw(drawable);
        }

        public void EndDraw()
        {
            RenderWindow.Display();
        }

        public void Close()
        {
            RenderWindow.Close();
        }

        private void OnClosed(object? sender, EventArgs e)
        {
            RenderWindow.Close();
        }

        private void OnKeyPressed(object? sender, KeyEventArgs e)
        {
            KeyPressed?.Invoke(sender, e);
        }

        private void OnKeyReleased(object? sender, KeyEventArgs e)
        {
            KeyReleased?.Invoke(sender, e);
        }
    }
}
