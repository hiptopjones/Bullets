﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullets
{
    internal class DrawableSystem
    {
        private List<GameObject> GameObjects { get; } = new List<GameObject>();

        public void ProcessAdditions(IEnumerable<GameObject> newGameObjects)
        {
            GameObjects.AddRange(newGameObjects);
        }

        public void ProcessRemovals()
        {
            // Remove any objects from consideration that are dead
            Utilities.DeleteWithSwapAndPop(GameObjects, x => !x.IsAlive);
        }

        public void Draw(GraphicsManager graphicsManager)
        {
            // TODO: Manage this separately so that Draw() is as fast as possible
            IEnumerable<DrawableComponent> drawableComponents = GameObjects.Select(x => x.GetComponent<DrawableComponent>());

            foreach (DrawableComponent drawableComponent in drawableComponents)
            {
                drawableComponent.Draw(graphicsManager);
            }
        }
    }
}
