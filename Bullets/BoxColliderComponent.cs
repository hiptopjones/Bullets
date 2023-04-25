using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullets
{
    internal class BoxColliderComponent : ColliderComponent
    {
        private Vector2f ColliderRectOffset { get; set; }

        // Axis-aligned bounding box (AABB)
        private FloatRect ColliderRect { get; set; }

        public override bool Intersects(ColliderComponent other, out CollisionInfo collision)
        {
            collision = null;

            BoxColliderComponent otherBoxCollider = other as BoxColliderComponent;
            if (otherBoxCollider != null)
            {
                FloatRect thisRect = GetColliderRect();
                FloatRect otherRect = otherBoxCollider.GetColliderRect();

                if (thisRect.Intersects(otherRect))
                {
                    collision = new CollisionInfo
                    {
                        Collider1 = this,
                        Collider2 = other,
                    };

                    return true;
                }
            }

            return false;
        }

        public Vector2f GetColliderRectOffset()
        {
            return ColliderRectOffset;
        }

        public void SetColliderRectOffset(Vector2f offset)
        {
            ColliderRectOffset = offset;
            UpdateColliderRectPosition();
        }

        public FloatRect GetColliderRect()
        {
            UpdateColliderRectPosition();
            return ColliderRect;
        }

        public void SetColliderRect(FloatRect colliderRect)
        {
            ColliderRect = colliderRect;
            UpdateColliderRectPosition();
        }

        // Update the collider rect based on the object's position
        private void UpdateColliderRectPosition()
        {
            Vector2f position = Owner.Transform.Position;
            FloatRect colliderRect = ColliderRect;

            colliderRect.Left = position.X - (ColliderRect.Width / 2) + ColliderRectOffset.X;
            colliderRect.Top = position.Y - (ColliderRect.Height / 2) + ColliderRectOffset.Y;

            ColliderRect = colliderRect;
        }

        public override FloatRect GetBoundingBox()
        {
            FloatRect rect = GetColliderRect();
            Debug.DrawRect(rect, Color.Red);

            return rect;
        }
    }
}
