using NLog;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Bullets
{
    internal class HealthBarComponent : DrawableComponent
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public int BackgroundTextureId { get; set; }
        public int ForegroundTextureId { get; set; }

        public Vector2f Offset { get; set; }
        public Vector2f Size { get; set; }

        private ResourceManager ResourceManager { get; set; }

        private Sprite BackgroundSprite { get; set; } = new Sprite();
        private Sprite ForegroundSprite { get; set; } = new Sprite();

        private float CurrentHealthPercent { get; set; } = 1;

        public override void Awake()
        {
            ResourceManager = ServiceLocator.Instance.GetService<ResourceManager>();
            if (ResourceManager == null)
            {
                throw new Exception($"Unable to retrieve resource manager from service locator");
            }
        }

        public override void Start()
        {
            BackgroundSprite.Texture = ResourceManager.GetTexture(BackgroundTextureId);
            ForegroundSprite.Texture = ResourceManager.GetTexture(ForegroundTextureId);

            BackgroundSprite.Scale = new Vector2f(
                Size.X / BackgroundSprite.Texture.Size.X,
                Size.Y / BackgroundSprite.Texture.Size.Y);
        }

        public override void LateUpdate(float deltaTime)
        {
            Vector2f centerOffset = new Vector2f(Size.X / 2f, 0);
            Vector2f position = Owner.Transform.Position + Offset - centerOffset;
            BackgroundSprite.Position = position;
            ForegroundSprite.Position = position;

            ForegroundSprite.Scale = new Vector2f(
                Size.X / BackgroundSprite.Texture.Size.X * CurrentHealthPercent,
                Size.Y / BackgroundSprite.Texture.Size.Y);
        }

        public override void Draw(GraphicsManager graphicsManager)
        {
            graphicsManager.Draw(BackgroundSprite);
            graphicsManager.Draw(ForegroundSprite);
        }

        public void OnHealthChanged(object sender, HealthChangeEventArgs e)
        {
            CurrentHealthPercent = e.CurrentHealth / e.MaxHealth;
        }

        public override string ToString()
        {
            return $"[HealthBarComponent] BackgroundTextureId({BackgroundTextureId}) ForegroundTextureId({ForegroundTextureId}) BackgroundSprite({BackgroundSprite}) ForegroundSprite({ForegroundSprite}) Offset({Offset})";
        }
    }
}
