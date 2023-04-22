using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullets
{
    internal static class GameSettings
    {
        public const string GameName = "Bullets";

        public const int WindowWidth = 800;
        public const int WindowHeight = 600;

        public static readonly Color WindowClearColor = Color.White;

        // Stats
        public const int StatsFpsSampleCount = 100;

        // Splash
        public const string SplashScreenTextureFileName = "Splash.png";
        public const float SplashScreenTransitionDelaySeconds = 2;

        // Player
        public const string PlayerObjectName = "Player";
        public const string PlayerTextureFileName = "Player.png";
        public const float PlayerMovementSpeed = 500;

        // Bullet
        public const string BulletTextureFileName = "Bullet.png";
        public const int BulletMaxCount = 20;

        // Resources
        public const string ResourcesDirectoryName = "Assets";
        public const string TexturesDirectoryName = "Textures";

        public enum TextureId
        {
            SplashScreen,
            Player,
            Bullet
        }

        public static readonly Dictionary<TextureId, string> Textures = new Dictionary<TextureId, string>
        {
            { TextureId.SplashScreen, SplashScreenTextureFileName },
            { TextureId.Player, PlayerTextureFileName },
            { TextureId.Bullet, BulletTextureFileName },
        };
    }
}
