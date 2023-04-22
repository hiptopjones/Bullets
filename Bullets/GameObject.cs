using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullets
{
    internal class GameObject
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public string Name { get; set; }
        public TransformComponent Transform { get; set; }

        private List<Component> Components { get; } = new List<Component>();

        public GameObject()
        {
            Transform = AddComponent<TransformComponent>();
        }

        public void Awake()
        {
            for (int i = Components.Count - 1; i >= 0; i--)
            {
                Components[i].Awake();
            }
        }

        public void Start()
        {
            for (int i = Components.Count - 1; i >= 0; i--)
            {
                Components[i].Start();
            }
        }

        public void Update(float deltaTime)
        {
            for (int i = Components.Count - 1; i >= 0; i--)
            {
                Components[i].Update(deltaTime);
            }
        }

        public void LateUpdate(float deltaTime)
        {
            for (int i = Components.Count - 1; i >= 0; i--)
            {
                Components[i].LateUpdate(deltaTime);
            }
        }

        public void Draw(WindowManager windowManager)
        {
            for (int i = Components.Count - 1; i >= 0; i--)
            {
                Components[i].Draw(windowManager);
            }
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
