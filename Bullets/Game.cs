using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullets
{
    internal class Game
    {
        private GameWindow GameWindow { get; }

        public bool IsRunning => GameWindow.IsOpen;

        public Game()
        {
            GameWindow = new GameWindow("Bullets", 800, 600);
        }

        public void Update()
        {
            GameWindow.Update();
        }

        public void Draw()
        {
            GameWindow.BeginDraw();
            
            // TODO
            
            GameWindow.EndDraw();
        }
    }
}
