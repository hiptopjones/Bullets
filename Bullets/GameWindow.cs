using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullets
{
    class GameWindow
    {
        private RenderWindow RenderWindow { get; }

        public bool IsOpen => RenderWindow.IsOpen;

        public GameWindow(string windowName, uint windowWidth, uint windowHeight)
        {
            VideoMode videoMode = new VideoMode(windowWidth, windowHeight);

            RenderWindow = new RenderWindow(videoMode, windowName, Styles.Titlebar);
            RenderWindow.SetVerticalSyncEnabled(true);

            RenderWindow.Closed += (sender, args) => RenderWindow.Close();
        }

        public void Update()
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
            RenderWindow.Display();
        }
    }
}
