using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullets
{
    class GraphicsManager
    {
        public event EventHandler<KeyEventArgs> KeyPressed;
        public event EventHandler<KeyEventArgs> KeyReleased;

        public bool IsOpen => RenderWindow.IsOpen;

        public string Name { get; set; }
        public uint Width { get; set; }
        public uint Height { get; set; }

        private VideoMode VideoMode { get; }
        private RenderWindow RenderWindow { get; }

        public GraphicsManager(string windowName, uint windowWidth, uint windowHeight)
        {
            Width = windowWidth;
            Height = windowHeight;
            VideoMode = new VideoMode(Width, Height);

            Name = windowName;
            RenderWindow = new RenderWindow(VideoMode, Name, Styles.Titlebar);
            RenderWindow.SetVerticalSyncEnabled(true);

            RenderWindow.Closed += OnClosed;
            RenderWindow.KeyPressed += OnKeyPressed;
            RenderWindow.KeyReleased += OnKeyReleased;
        }

        public void ProcessEvents()
        {
            RenderWindow.DispatchEvents();
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
            // Draw any debug graphics on top of everything before displaying the scene
            Debug.Draw(this);

            RenderWindow.Display();
        }

        public void Close()
        {
            RenderWindow.Close();
        }

        private void OnClosed(object sender, EventArgs e)
        {
            RenderWindow.Close();
        }

        private void OnKeyPressed(object sender, KeyEventArgs e)
        {
            KeyPressed?.Invoke(sender, e);
        }

        private void OnKeyReleased(object sender, KeyEventArgs e)
        {
            KeyReleased?.Invoke(sender, e);
        }
    }
}
