using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Bullets
{
    internal class SpriteComponent : Component
    {
        public int TextureId { get; set; }
        public IntRect TextureRect { get; set; }

        private Sprite Sprite { get; set; } = new Sprite();

        private ResourceManager ResourceManager { get; set; }

        public bool IsHorizontalFlipEnabled { get; set; }

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
            RefreshTexture();
        }

        public override void LateUpdate(float deltaTime)
        {
            Sprite.Position = Owner.Transform.Position;
        }

        public override void Draw(WindowManager windowManager)
        {
            IntRect savedTextureRect = Sprite.TextureRect;

            if (IsHorizontalFlipEnabled)
            {
                IntRect flippedTextureRect = Sprite.TextureRect;
                flippedTextureRect.Left += flippedTextureRect.Width;
                flippedTextureRect.Width *= -1;

                Sprite.TextureRect = flippedTextureRect;
            }

            windowManager.Draw(Sprite);

            // Undo any adjustments
            Sprite.TextureRect = savedTextureRect;
        }

        // Necessary for animations, where different states could reference different textures
        public void RefreshTexture()
        {
            Sprite.Texture = ResourceManager.GetTexture(TextureId);
            Sprite.TextureRect = TextureRect;
        }
    }
}
