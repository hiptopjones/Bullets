using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullets
{
    public static class Extensions
    {
        public static float Magnitude(this Vector2f vector)
        {
            return MathF.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        }

        public static Vector2f Normalize(this Vector2f vector)
        {
            float magnitude = vector.Magnitude();
            return new Vector2f(vector.X / magnitude, vector.Y / magnitude);
        }

        public static float DistanceTo(this Vector2f start, Vector2f end)
        {
            return (end - start).Magnitude();
        }
    }
}
