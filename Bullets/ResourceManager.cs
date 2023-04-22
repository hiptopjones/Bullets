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
        private const string ASSETS_DIRECTORY_NAME = "Assets";
        private const string TEXTURES_DIRECTORY_NAME = "Textures";

        public string ResourcesDirectory { get; }
        public string TexturesDirectory { get; }

        private Dictionary<string, Texture> Textures { get; }

        public ResourceManager()
        {
            ResourcesDirectory = Path.Combine(Environment.CurrentDirectory, ASSETS_DIRECTORY_NAME);
            TexturesDirectory = Path.Combine(ResourcesDirectory, TEXTURES_DIRECTORY_NAME);

            Textures = new Dictionary<string, Texture>();
        }

        public Texture GetTexture(string textureFileName)
        {
            if (!Textures.TryGetValue(textureFileName, out Texture? texture))
            {
                string textureFilePath = Path.Combine(TexturesDirectory, textureFileName);
                texture = new Texture(textureFilePath);

                Textures[textureFileName] = texture;
            }

            return texture;
        }
    }
}