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
        public int TextureId { get; set; }
        public IntRect TextureRect { get; set; }
        public Vector2f Origin { get; set; }
        public float RotationOffset { get; set; }
        public bool IsHorizontalFlipEnabled { get; set; }

        private Sprite Sprite { get; set; } = new Sprite();

        private ResourceManager ResourceManager { get; set; }

        public override void Reset()
        {
            TextureId = 0;
            TextureRect = new IntRect();
            Origin = new Vector2f();
            RotationOffset = 0;
            IsHorizontalFlipEnabled = false;
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
            Sprite.Rotation = Owner.Transform.Rotation + RotationOffset;
            Sprite.Scale = Owner.Transform.Scale;
        }

        public override void Draw(GraphicsManager graphicsManager)
        {
            IntRect savedTextureRect = Sprite.TextureRect;

            if (IsHorizontalFlipEnabled)
            {
                IntRect flippedTextureRect = Sprite.TextureRect;
                flippedTextureRect.Left += flippedTextureRect.Width;
                flippedTextureRect.Width *= -1;

                Sprite.TextureRect = flippedTextureRect;
            }

            graphicsManager.Draw(Sprite);

            // Undo any adjustments
            Sprite.TextureRect = savedTextureRect;
        }
    }
}
