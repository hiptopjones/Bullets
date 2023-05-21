using NLog;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Bullets
{
    internal class GameObjectPool
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private enum PoolState
        {
            Active,
            Pooled,
        }

        private Func<GameObject> GameObjectFactory { get; set; }

        private Queue<GameObject> PooledObjects = new Queue<GameObject>();
        private Dictionary<GameObject, PoolState> PooledObjectStates = new Dictionary<GameObject, PoolState>();

        public GameObjectPool(Func<GameObject> gameObjectFactory)
        {
            GameObjectFactory = gameObjectFactory;
        }

        public GameObject GetOrCreateObject()
        {
            GameObject obj = null;

            if (PooledObjects.Any())
            {
                obj = PooledObjects.Dequeue();
                obj.IsEnabled = true;
                obj.Reset();
            }
            else
            {
                obj = GameObjectFactory();
            }

            PooledObjectStates[obj] = PoolState.Active;
            return obj;
        }

        public void OnDestroyed(GameObject obj)
        {
            if (PooledObjectStates[obj] == PoolState.Pooled)
            {
                return;
            }

            obj.IsEnabled = false;
            PooledObjects.Enqueue(obj);
            PooledObjectStates[obj] = PoolState.Pooled;
        }
    }
}
