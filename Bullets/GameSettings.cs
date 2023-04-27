using SFML.Graphics;
using SFML.System;
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

        // Window
        public const int WindowWidth = 1600;
        public const int WindowHeight = 1200;
        public static readonly Color WindowClearColor = Color.White;

        // Stats
        public const int StatsFpsSampleCount = 100;

        // Debug
        public static readonly Color DebugDefaultColor = Color.Magenta;
        public const string DebugFontFileName = "unispace rg.ttf";

        // Splash
        public const string SplashScreenTextureFileName = "Splash.png";
        public const float SplashScreenTransitionDelaySeconds = 2;

        // Player
        public const string PlayerObjectName = "Player";
        public const string PlayerTextureFileName = "Player.png";
        public static readonly Vector2f PlayerStartPosition = new Vector2f(200, 200);
        public const float PlayerMovementSpeed = 500;
        public const float PlayerTextureWidth = 200;
        public const float PlayerTextureHeight = 244;
        public static readonly FloatRect PlayerColliderRect = new FloatRect(0, 0, PlayerTextureWidth, PlayerTextureHeight);
        public static readonly Vector2f PlayerColliderRectOffset = new Vector2f(0, 0);

        // Bullet
        public const string BulletObjectName = "Bullet";
        public const string BulletTextureFileName = "Bullet.png";
        public const int BulletMaxCount = 100;
        public const float BulletTextureWidth = 40;
        public const float BulletTextureHeight = 40;
        public static readonly FloatRect BulletColliderRect = new FloatRect(0, 0, BulletTextureWidth, BulletTextureHeight);
        public static readonly Vector2f BulletColliderRectOffset = new Vector2f(0, 0);

        // Bullet Emitter
        public static readonly Vector2f BulletEmitterStartPosition = new Vector2f(WindowWidth / 2, WindowHeight / 2);

        // Resources
        public const string ResourcesDirectoryName = "Assets";
        public const string TexturesDirectoryName = "Textures";
        public const string FontsDirectoryName = "Fonts";

        // Textures
        public enum TextureId
        {
            SplashScreen,
            Player,
            Bullet
        }

        public static readonly Dictionary<int, string> Textures = new Dictionary<int, string>
        {
            { (int)TextureId.SplashScreen, SplashScreenTextureFileName },
            { (int)TextureId.Player, PlayerTextureFileName },
            { (int)TextureId.Bullet, BulletTextureFileName },
        };

        // Fonts
        public enum FontId
        {
            Debug
        }

        public static readonly Dictionary<int, string> Fonts = new Dictionary<int, string>
        {
            { (int)FontId.Debug, DebugFontFileName },
        };
    }
}
