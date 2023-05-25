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

        // Collisions
        public enum CollisionLayer
        {
            None,
            Player,
            Enemy,
            PlayerBullet,
            EnemyBullet,
        }

        public static readonly HashSet<string> Collidables = new HashSet<string>
        {
            Utilities.MakeKey((int)CollisionLayer.Player, (int)CollisionLayer.Enemy),
            Utilities.MakeKey((int)CollisionLayer.PlayerBullet, (int)CollisionLayer.Enemy),
            Utilities.MakeKey((int)CollisionLayer.Player, (int)CollisionLayer.EnemyBullet),
        };

        // Health Bar
        public const string HealthBarBackgroundTextureFileName = "HealthBarBlack.png";
        public const string HealthBarForegroundTextureFileName = "HealthBarRed.png";
        public const int HealthBarMaxWidth = 100;

        // Damage Numbers
        public static readonly Color DamageNumbersColor = Color.Black;
        public const string DamageNumbersFontFileName = "Super Bubble.ttf";
        public const uint DamageNumbersTextSize = 20;

        // Bullets
        public const string BulletObjectName = "Bullet";

        // Player
        public const string PlayerObjectName = "Player";
        public const string PlayerTextureFileName = "Player.png";
        public static readonly Vector2f PlayerStartPosition = new Vector2f(200, 200);
        public const float PlayerNormalMovementSpeed = 500;
        public const float PlayerDashMovementSpeed = 1000;
        public const float PlayerDashMovementTime = 0.25f;
        public const float PlayerDashCooldownTime = 0.5f;
        public const float PlayerRotationSpeed = 360;
        public const float PlayerTextureWidth = 99;
        public const float PlayerTextureHeight = 100;
        public static readonly FloatRect PlayerColliderRect = new FloatRect(0, 0, PlayerTextureWidth, PlayerTextureHeight);
        public static readonly Vector2f PlayerColliderRectOffset = new Vector2f(0, 0);
        public const int PlayerCollisionLayer = (int)CollisionLayer.Player;
        public static readonly Vector2f PlayerBulletSpawnOffset = new Vector2f(PlayerTextureWidth / 2, 0);

        // Player Bullet
        public const string PlayerBulletObjectName = "Player Bullet";
        public const string PlayerBulletTextureFileName = "SmallYellowBullet.png";
        public const float PlayerBulletTextureWidth = 26;
        public const float PlayerBulletTextureHeight = 26;
        public static readonly FloatRect PlayerBulletColliderRect = new FloatRect(0, 0, PlayerBulletTextureWidth, PlayerBulletTextureHeight);
        public static readonly Vector2f PlayerBulletColliderRectOffset = new Vector2f(0, 0);
        public const float PlayerBulletMaxDistance = 1000;
        public const int PlayerBulletCollisionLayer = (int)CollisionLayer.PlayerBullet;

        // Enemy Turret
        public const string EnemyTurretObjectName = "Enemy Turret";
        public const string EnemyTurretTextureFileName = "Turret.png";
        public static readonly Vector2f EnemyTurretStartPosition = new Vector2f(WindowWidth / 2, WindowHeight / 2);
        public const int EnemyTurretTextureWidth = 100;
        public const int EnemyTurretTextureHeight = 100;
        public static readonly FloatRect EnemyTurretColliderRect = new FloatRect(0, 0, EnemyTurretTextureWidth, EnemyTurretTextureHeight);
        public static readonly Vector2f EnemyTurretColliderRectOffset = new Vector2f(0, 0);
        public const float EnemyTurretMovementSpeed = 250;
        public const int EnemyTurretCollisionLayer = (int)CollisionLayer.Enemy;

        // Enemy Bullet Pattern
        public const string EnemyBulletObjectName = "Enemy Bullet";
        public const string EnemyBulletTextureFileName = "MediumRedBullet.png";
        public const int EnemyBulletMaxCount = 100;
        public const float EnemyBulletTextureWidth = 40;
        public const float EnemyBulletTextureHeight = 40;
        public static readonly FloatRect EnemyBulletColliderRect = new FloatRect(0, 0, EnemyBulletTextureWidth, EnemyBulletTextureHeight);
        public static readonly Vector2f EnemyBulletColliderRectOffset = new Vector2f(0, 0);
        public const float EnemyBulletMaxDistance = 1000;
        public const float EnemyBulletStartRadialOffset = 75;
        public const int EnemyBulletCollisionLayer = (int)CollisionLayer.EnemyBullet;

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
            EnemyTurret,
            HealthBarBackground,
            HealthBarForeground,
        }

        public static readonly Dictionary<int, string> Textures = new Dictionary<int, string>
        {
            { (int)TextureId.SplashScreen, SplashScreenTextureFileName },
            { (int)TextureId.Player, PlayerTextureFileName },
            { (int)TextureId.EnemyBullet, EnemyBulletTextureFileName },
            { (int)TextureId.PlayerBullet, PlayerBulletTextureFileName },
            { (int)TextureId.EnemyTurret, EnemyTurretTextureFileName },
            { (int)TextureId.HealthBarBackground, HealthBarBackgroundTextureFileName },
            { (int)TextureId.HealthBarForeground, HealthBarForegroundTextureFileName },
        };

        // Fonts
        public enum FontId
        {
            Debug,
            DamageNumbers,
        }

        public static readonly Dictionary<int, string> Fonts = new Dictionary<int, string>
        {
            { (int)FontId.Debug, DebugFontFileName },
            { (int)FontId.DamageNumbers, DamageNumbersFontFileName },
        };
    }
}
