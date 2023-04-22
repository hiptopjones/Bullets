using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullets
{
    internal class SpriteComponent : Component
    {
        public string TextureFileName { get; set; }

        private Sprite Sprite { get; set; }

        public override void Start()
        {
            ResourceManager resourceManager = ServiceLocator.Instance.GetService<ResourceManager>();

            Texture texture = resourceManager.GetTexture(TextureFileName);
            Sprite = new Sprite(texture);
        }

        public override void LateUpdate(float deltaTime)
        {
            Sprite.Position = Owner.Transform.Position;
        }

        public override void Draw(WindowManager windowManager)
        {
            windowManager.Draw(Sprite);
        }
    }
}
