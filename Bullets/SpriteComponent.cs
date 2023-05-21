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
    internal class SpriteComponent : DrawableComponent
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public int TextureId { get; set; }
        public IntRect TextureRect { get; set; }
        public Vector2f Origin { get; set; }
        
        private Sprite Sprite { get; set; } = new Sprite();

        private ResourceManager ResourceManager { get; set; }

        public override void Reset()
        {
            TextureId = 0;
            TextureRect = new IntRect();
            Origin = new Vector2f();
        }

        public override void Awake()
        {
            ResourceManager = ServiceLocator.Instance.GetService<ResourceManager>();
            if (ResourceManager == null)
            {
                throw new Exception($"Unable to retrieve resource manager from service locator");
            }
        }

        public override void LateUpdate(float deltaTime)
        {
            Sprite.Texture = ResourceManager.GetTexture(TextureId);
            Sprite.TextureRect = new IntRect(0, 0, (int)Sprite.Texture.Size.X, (int)Sprite.Texture.Size.Y);

            Sprite.Origin = Origin;

            Sprite.Position = Owner.Transform.Position;
            Sprite.Rotation = Owner.Transform.Rotation;
            Sprite.Scale = Owner.Transform.Scale;
        }

        public override void Draw(GraphicsManager graphicsManager)
        {
            graphicsManager.Draw(Sprite);
        }

        public override string ToString()
        {
            return $"[SpriteComponent] TextureId({TextureId}) TextureRect({TextureRect}) Origin({Origin}) Sprite({Sprite})";
        }
    }
}
