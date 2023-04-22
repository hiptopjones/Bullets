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
        private WindowManager WindowManager { get; set; }
        private TimeManager TimeManager { get; set; }
        private SceneManager SceneManager { get; set; }

        public bool IsRunning => WindowManager.IsOpen;

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

            WindowManager = new WindowManager(GameSettings.GameName, GameSettings.WindowWidth, GameSettings.WindowHeight);
            WindowManager.KeyPressed += InputManager.OnKeyPressed;
            WindowManager.KeyReleased += InputManager.OnKeyReleased;
            ServiceLocator.Instance.ProvideService(WindowManager);

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
            WindowManager.ProcessEvents();

            if (InputManager.IsKeyPressed(InputManager.Key.Escape))
            {
                WindowManager.Close();
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
            WindowManager.BeginDraw();

            SceneManager.Draw(WindowManager);

            WindowManager.EndDraw();
        }
    }
}
