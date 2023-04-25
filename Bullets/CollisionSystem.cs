using NLog;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Bullets
{
    internal class CollisionSystem
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private List<ColliderComponent> ColliderComponents { get; } = new List<ColliderComponent>();

        private Quadtree<ColliderComponent> CollisionTree { get; set; }

        private Dictionary<string, KeyValuePair<ColliderComponent, ColliderComponent>> ActiveCollisions { get; }
            = new Dictionary<string, KeyValuePair<ColliderComponent, ColliderComponent>>();

        public CollisionSystem()
        {
            CollisionTree = new Quadtree<ColliderComponent>
            {
                BoundingBox = new FloatRect(0, 0, 800, 600),
                MaxEntriesPerLevel = 5
            };
        }

        public void ProcessAdditions(IEnumerable<GameObject> newGameObjects)
        {
            ColliderComponents.AddRange(newGameObjects
                .Select(x => x.GetComponent<ColliderComponent>())
                .Where(x => x != null));
        }

        public void ProcessRemovals()
        {
            // Remove any objects from consideration that are dead
            Utilities.DeleteWithSwapAndPop(ColliderComponents, x => !x.Owner.IsAlive);
        }

        public void Update()
        {
            CollisionTree.Clear();

            foreach (ColliderComponent colliderComponent in ColliderComponents)
            {
                QuadtreeEntry<ColliderComponent> quadtreeEntry = new QuadtreeEntry<ColliderComponent>
                {
                    BoundingBox = colliderComponent.GetBoundingBox(),
                    Context = colliderComponent
                };

                CollisionTree.Add(quadtreeEntry);
            }

            CollisionTree.DrawDebug();

            CheckCollisions();
        }

        public void CheckCollisions()
        {
            HashSet<string> checkedCollisions = new HashSet<string>();

            foreach (ColliderComponent colliderComponent in ColliderComponents)
            {
                List<QuadtreeEntry<ColliderComponent>> candidateEntries = CollisionTree.Search(colliderComponent.GetBoundingBox());

                foreach (QuadtreeEntry< ColliderComponent> quadtreeEntry in candidateEntries)
                {
                    ColliderComponent otherColliderComponent = quadtreeEntry.Context;

                    // Don't collide with ourselves
                    if (colliderComponent == otherColliderComponent)
                    {
                        continue;
                    }

                    // Only check the collision in one direction
                    string key = Utilities.MakeKey(colliderComponent.Owner.Id, otherColliderComponent.Owner.Id);
                    if (checkedCollisions.Contains(key))
                    {
                        continue;
                    }
                    checkedCollisions.Add(key);

                    bool isColliding = colliderComponent.Intersects(otherColliderComponent, out CollisionInfo collisionInfo);
                    bool wasColliding = ActiveCollisions.ContainsKey(key);

                    if (isColliding || wasColliding)
                    {
                        if (isColliding)
                        {
                            ActiveCollisions[key] = new KeyValuePair<ColliderComponent, ColliderComponent>(colliderComponent, otherColliderComponent);
                        }
                        else if (wasColliding)
                        {
                            ActiveCollisions.Remove(key);
                        }

                        // TODO: Check that an object is enabled and alive before raising collision events to it
                        // Raise events in both directions so the collision can be handled meaningfully
                        NotifyCollisionEvent(isColliding, wasColliding, colliderComponent, otherColliderComponent);
                        NotifyCollisionEvent(isColliding, wasColliding, otherColliderComponent, colliderComponent);

                        // TODO: Note that objects that get removed from the scene may not receive an exit call and leak memory in ActionCollisions
                    }
                }
            }
        }

        private void NotifyCollisionEvent(bool isColliding, bool wasColliding, ColliderComponent sourceColliderComponent, ColliderComponent targetColliderComponent)
        {
            CollisionHandlerComponent collisionHandlerComponent = targetColliderComponent.Owner.GetComponent<CollisionHandlerComponent>();
            if (collisionHandlerComponent != null)
            {
                if (isColliding)
                {
                    if (wasColliding)
                    {
                        collisionHandlerComponent.OnCollisionStay(sourceColliderComponent);
                    }
                    else
                    {
                        collisionHandlerComponent.OnCollisionEnter(sourceColliderComponent);
                    }
                }
                else
                {
                    if (wasColliding)
                    {
                        collisionHandlerComponent.OnCollisionExit(sourceColliderComponent);
                    }
                }
            }
        }
    }
}
