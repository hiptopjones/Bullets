using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullets
{
    internal class DebugCollisionHandlerComponent : CollisionHandlerComponent
    {
        private ColliderComponent ColliderComponent { get; set; }

        public override void Awake()
        {
            ColliderComponent = Owner.GetComponent<ColliderComponent>();
        }

        public override void OnCollisionStay(ColliderComponent other)
        {
            string collisionKey = Utilities.MakeKey(ColliderComponent.Owner.Id, other.Owner.Id);

            //FloatRect thisRect = ColliderComponent.GetBoundingBox();
            //Debug.DrawRect(thisRect, Color.Red);
            //Debug.DrawLine(
            //    new[] {
            //        new Vector2f(thisRect.Left, thisRect.Top + thisRect.Height),
            //        new Vector2f(thisRect.Left + thisRect.Width, thisRect.Top)
            //    }, Color.Red);

            //Debug.DrawText(collisionKey, new Vector2f(thisRect.Left, thisRect.Top + thisRect.Height));

            //FloatRect otherRect = other.GetBoundingBox();
            //Debug.DrawRect(other.GetBoundingBox(), Color.Red);
            //Debug.DrawLine(
            //    new[] {
            //        new Vector2f(otherRect.Left, otherRect.Top + otherRect.Height),
            //        new Vector2f(otherRect.Left + otherRect.Width, otherRect.Top)
            //    }, Color.Red);
            //Debug.DrawText(collisionKey, new Vector2f(otherRect.Left, otherRect.Top + otherRect.Height));
        }
    }
}
