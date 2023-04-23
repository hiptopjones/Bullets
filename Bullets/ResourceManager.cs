using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullets
{
    internal class ResourceManager
    {
        public string ResourcesDirectory { get; }
        public string TexturesDirectory { get; }

        private Dictionary<int, Texture> Textures { get; } = new Dictionary<int, Texture>();

        public ResourceManager()
        {
            ResourcesDirectory = Path.Combine(Environment.CurrentDirectory, GameSettings.ResourcesDirectoryName);
            TexturesDirectory = Path.Combine(ResourcesDirectory, GameSettings.TexturesDirectoryName);
        }

        public Texture GetTexture(int textureId)
        {
            if (!Textures.TryGetValue(textureId, out Texture texture))
            {
                if (!GameSettings.Textures.TryGetValue(textureId, out string textureFileName))
                {
                    throw new Exception($"Unable to locate a file path for texture: {textureId}");
                }

                string textureFilePath = Path.Combine(TexturesDirectory, textureFileName);
                texture = new Texture(textureFilePath);

                Textures[textureId] = texture;
            }

            return texture;
        }
    }
}