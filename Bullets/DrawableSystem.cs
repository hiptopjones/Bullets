using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullets
{
    internal class DrawableSystem
    {
        private List<DrawableComponent> DrawableComponents { get; } = new List<DrawableComponent>();

        public void ProcessAdditions(IEnumerable<GameObject> newGameObjects)
        {
            DrawableComponents.AddRange(newGameObjects
                .Select(x => x.GetComponent<DrawableComponent>())
                .Where(x => x != null));
        }

        public void ProcessRemovals()
        {
            // Remove any objects from consideration that are dead
            Utilities.DeleteWithSwapAndPop(DrawableComponents, x => !x.Owner.IsAlive);
        }

        public void Draw(GraphicsManager graphicsManager)
        {
            // TODO: Manage sort order and/or layer
            foreach (DrawableComponent drawableComponent in DrawableComponents)
            {
                drawableComponent.Draw(graphicsManager);
            }
        }
    }
}
