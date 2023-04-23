using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullets
{
    internal sealed class GameObject
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private static int nextGameObjectId = 0;

        public int Id { get; private set; }
        public string Name { get; set; }
        public TransformComponent Transform { get; private set; }

        public bool IsAlive { get; private set; } = true;

        private List<Component> Components { get; } = new List<Component>();

        public GameObject()
        {
            Id = nextGameObjectId++;
            Name = $"Object{Id}";

            Transform = AddComponent<TransformComponent>();
        }

        // Called exactly once when the object is initialized
        public void Awake()
        {
            for (int i = Components.Count - 1; i >= 0; i--)
            {
                Components[i].Awake();
            }
        }

        // Called exactly once when the object is enabled
        public void Start()
        {
            for (int i = Components.Count - 1; i >= 0; i--)
            {
                Components[i].Start();
            }
        }

        // Called every frame
        public void Update(float deltaTime)
        {
            for (int i = Components.Count - 1; i >= 0; i--)
            {
                Components[i].Update(deltaTime);
            }
        }

        // Called every frame (after all Update() calls complete)
        public void LateUpdate(float deltaTime)
        {
            for (int i = Components.Count - 1; i >= 0; i--)
            {
                Components[i].LateUpdate(deltaTime);
            }
        }

        // Called every frame
        public void Draw(WindowManager windowManager)
        {
            for (int i = Components.Count - 1; i >= 0; i--)
            {
                Components[i].Draw(windowManager);
            }
        }

        public void Destroy()
        {
            IsAlive = false;
        }

        public T AddComponent<T>() where T : Component, new()
        {
            T component = Components.OfType<T>().SingleOrDefault();
            if (component == null)
            {
                component = new T();
                component.Owner = this;

                Components.Add(component);
            }
            else
            {
                Logger.Info($"Component of type {typeof(T).Name} already exists on object '{Name}'");
            }

            return component;
        }

        public T GetComponent<T>() where T : Component
        {
            T component = Components.OfType<T>().SingleOrDefault();
            if (component == null)
            {
                Logger.Info($"Component of type {typeof(T).Name} does not exist on object '{Name}'");
            }

            return component;
        }
    }
}
