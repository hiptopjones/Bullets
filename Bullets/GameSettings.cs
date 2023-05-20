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
        public static readonly Color WindowClearColor = new Color(30, 30, 30);

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
        public const string PlayerTextureFileName = "Player2.png";
        public static readonly Vector2f PlayerStartPosition = new Vector2f(200, 200);
        public const float PlayerMovementSpeed = 500;
        public const float PlayerRotationSpeed = 360;
        public const float PlayerTextureWidth = 100;
        public const float PlayerTextureHeight = 99;
        public static readonly FloatRect PlayerColliderRect = new FloatRect(0, 0, PlayerTextureWidth, PlayerTextureHeight);
        public static readonly Vector2f PlayerColliderRectOffset = new Vector2f(0, 0);

        // Player Bullet
        public const string PlayerBulletObjectName = "Player Bullet";
        public const string PlayerBulletTextureFileName = "Bullet.png";
        public const float PlayerBulletTextureWidth = 10;
        public const float PlayerBulletTextureHeight = 20;
        public static readonly FloatRect PlayerBulletColliderRect = new FloatRect(0, 0, PlayerBulletTextureWidth, PlayerBulletTextureHeight);
        public static readonly Vector2f PlayerBulletColliderRectOffset = new Vector2f(0, 0);
        public const float PlayerBulletMaxDistance = 1000;

        // Enemy Bullet Pattern
        public const string EnemyBulletObjectName = "Enemy Bullet";
        public const string EnemyBulletTextureFileName = "Orb.png";
        public const int EnemyBulletMaxCount = 100;
        public const float EnemyBulletTextureWidth = 40;
        public const float EnemyBulletTextureHeight = 40;
        public static readonly FloatRect EnemyBulletColliderRect = new FloatRect(0, 0, EnemyBulletTextureWidth, EnemyBulletTextureHeight);
        public static readonly Vector2f EnemyBulletColliderRectOffset = new Vector2f(0, 0);
        public const float EnemyBulletMaxDistance = 1000;
        public const float EnemyBulletStartRadialOffset = 75;

        // Enemy Turret
        public const string TurretTextureFileName = "Turret.png";
        public static readonly Vector2f TurretStartPosition = new Vector2f(WindowWidth / 2, WindowHeight / 2);
        public const int TurretTextureWidth = 100;
        public const int TurretTextureHeight = 100;

        // Resources
        public const string ResourcesDirectoryName = "Assets";
        public const string TexturesDirectoryName = "Textures";
        public const string FontsDirectoryName = "Fonts";

        // Textures
        public enum TextureId
        {
            SplashScreen,
            Player,
            PlayerBullet,
            EnemyBullet,
            Turret
        }

        public static readonly Dictionary<int, string> Textures = new Dictionary<int, string>
        {
            { (int)TextureId.SplashScreen, SplashScreenTextureFileName },
            { (int)TextureId.Player, PlayerTextureFileName },
            { (int)TextureId.EnemyBullet, EnemyBulletTextureFileName },
            { (int)TextureId.PlayerBullet, PlayerBulletTextureFileName },
            { (int)TextureId.Turret, TurretTextureFileName },
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
