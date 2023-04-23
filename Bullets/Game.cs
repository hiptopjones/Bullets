using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullets
{
    internal class Game
    {
        private InputManager InputManager { get; set; }
        private ResourceManager ResourceManager { get; set; }
        private GraphicsManager GraphicsManager { get; set; }
        private TimeManager TimeManager { get; set; }
        private SceneManager SceneManager { get; set; }

        public bool IsRunning => GraphicsManager.IsOpen;

        public Game()
        {
            Initialize();
        }

        public void Initialize()
        {
            InputManager = new InputManager();
            ServiceLocator.Instance.ProvideService(InputManager);

            ResourceManager = new ResourceManager();
            ServiceLocator.Instance.ProvideService(ResourceManager);
            
            TimeManager = new TimeManager();
            ServiceLocator.Instance.ProvideService(TimeManager);
            
            SceneManager = new SceneManager();
            ServiceLocator.Instance.ProvideService(SceneManager);

            GraphicsManager = new GraphicsManager(GameSettings.GameName, GameSettings.WindowWidth, GameSettings.WindowHeight);
            GraphicsManager.KeyPressed += InputManager.OnKeyPressed;
            GraphicsManager.KeyReleased += InputManager.OnKeyReleased;
            ServiceLocator.Instance.ProvideService(GraphicsManager);

            InitializeScenes();
        }

        private void InitializeScenes()
        {
            GameScene gameScene = new GameScene();
            int gameSceneId = SceneManager.AddScene(gameScene);

            SplashScreenScene splashScene = new SplashScreenScene();
            splashScene.TransitionSceneId = gameSceneId;
            int splashSceneId = SceneManager.AddScene(splashScene);

            SceneManager.SwitchTo(splashSceneId);
        }

        public void StartFrame()
        {
            TimeManager.OnFrameStarted();
            InputManager.OnFrameStarted();
        }

        public void ProcessEvents()
        {
            GraphicsManager.ProcessEvents();

            if (InputManager.IsKeyPressed(InputManager.Key.Escape))
            {
                GraphicsManager.Close();
            }
        }

        public void Update()
        {
            float deltaTime = TimeManager.DeltaTime;

            SceneManager.Update(deltaTime);
        }

        public void LateUpdate()
        {
            float deltaTime = TimeManager.DeltaTime;

            SceneManager.LateUpdate(deltaTime);
        }

        public void Draw()
        {
            GraphicsManager.BeginDraw();

            SceneManager.Draw(GraphicsManager);

            GraphicsManager.EndDraw();
        }
    }
}
