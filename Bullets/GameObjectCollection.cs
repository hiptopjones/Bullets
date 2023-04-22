using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullets
{
    internal class GameObjectCollection
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private List<GameObject> GameObjects { get; } = new List<GameObject>();
        
        private List<GameObject> NewGameObjects1 { get; } = new List<GameObject>();
        private List<GameObject> NewGameObjects2 { get; } = new List<GameObject>();
        private bool IsUsingFirstCollection { get; set; }
        private List<GameObject> NewGameObjects
        {
            get
            {
                return IsUsingFirstCollection ? NewGameObjects1 : NewGameObjects2;
            }
        }

        public GameObject CreateGameObject()
        {
            GameObject gameObject = new GameObject();

            NewGameObjects.Add(gameObject);
            return gameObject;
        }

        public GameObject CreateGameObject(string name)
        {
            GameObject gameObject = new GameObject
            {
                Name = name
            };

            NewGameObjects.Add(gameObject);
            return gameObject;
        }

        public void Update(float deltaTime)
        {
            ProcessRemovals();
            ProcessAdditions();

            //Logger.Info($"Objects: {GameObjects.Count}");

            foreach (GameObject gameObject in GameObjects)
            {
                gameObject.Update(deltaTime);
            }
        }

        public void LateUpdate(float deltaTime)
        {
            foreach (GameObject gameObject in GameObjects)
            {
                gameObject.LateUpdate(deltaTime);
            }
        }

        public void Draw(WindowManager windowManager)
        {
            foreach (GameObject gameObject in GameObjects)
            {
                gameObject.Draw(windowManager);
            }
        }

        public void ProcessRemovals()
        {
            int i = 0;
            while (i < GameObjects.Count)
            {
                if (GameObjects[i].IsAlive)
                {
                    i++;
                    continue;
                }

                // Swap and pop
                int lastIndex = GameObjects.Count - 1;
                GameObjects[i] = GameObjects[lastIndex];
                GameObjects.RemoveAt(lastIndex);
            }
        }

        public void ProcessAdditions()
        {
            // Using a double-buffering pattern to avoid race problems
            // if any Awake() or Start() implementations add new objects
            // (Those new objects would be processed next frame.)
            if (NewGameObjects.Any())
            {
                List<GameObject> addedGameObjects = NewGameObjects;
                IsUsingFirstCollection = !IsUsingFirstCollection;

                foreach (GameObject gameObject in addedGameObjects)
                {
                    gameObject.Awake();
                }

                foreach (GameObject gameObject in addedGameObjects)
                {
                    gameObject.Start();
                }

                GameObjects.AddRange(addedGameObjects);
                addedGameObjects.Clear();
            }
        }
    }
}
