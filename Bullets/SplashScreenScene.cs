using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullets
{
    internal class SplashScreenScene : Scene
    {
        public const string SPLASH_TEXTURE_FILE_NAME = "Splash.png";
        public const float TRANSITION_DELAY_SECONDS = 2;

        public int TransitionSceneId { get; set; }

        private Sprite Sprite { get; set; }

        private float CurrentSeconds { get; set; }

        private SceneManager SceneManager { get; set; }

        public override void OnCreate()
        {
            SceneManager = ServiceLocator.Instance.GetService<SceneManager>();
            
            ResourceManager resourceManager = ServiceLocator.Instance.GetService<ResourceManager>();

            Texture texture = resourceManager.GetTexture(SPLASH_TEXTURE_FILE_NAME);
            Sprite = new Sprite(texture);

            FloatRect spriteSize = Sprite.GetLocalBounds();
            Sprite.Origin = new Vector2f(spriteSize.Width, spriteSize.Height) * 0.5f;

            WindowManager windowManager = ServiceLocator.Instance.GetService<WindowManager>();
            Sprite.Position = new Vector2f(windowManager.Width, windowManager.Height) * 0.5f;
        }

        public override void OnActivate()
        {
            CurrentSeconds = 0;
        }

        public override void Update(float deltaTime)
        {
            CurrentSeconds += deltaTime;
            if (CurrentSeconds >= TRANSITION_DELAY_SECONDS)
            {
                SceneManager.SwitchTo(TransitionSceneId);
            }
        }

        public override void Draw(WindowManager window)
        {
            window.Draw(Sprite);
        }
    }
}
