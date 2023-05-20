﻿using SFML.Graphics;
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

        // Necessary for animations, where different states could reference different textures
        public void RefreshTexture()
        {
            Sprite.Texture = ResourceManager.GetTexture(TextureId);

            // Check if our texture rect was actually set to something valid before
            // overwriting the default one the sprite uses
            if (TextureRect.Width > 0)
            {
                Sprite.TextureRect = TextureRect;
            }
        }
    }
}
