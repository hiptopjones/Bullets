using System;
using SFML.Graphics;
using SFML.Window;

namespace Bullets
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            
            while (game.IsRunning)
            {
                game.Update();
                game.Draw();
            }
        }
    }
}