using System;
using OpenTK;

namespace Clouds.Game
{
    public struct CollisionCircle
    {
        private readonly Vector2 center;
        private readonly float radius;

        public CollisionCircle(Vector2 center, float radius)
        {
            this.center = center;
            this.radius = radius;
        }

        public bool TryHit(Vector2 start, Vector2 dir, out float rayFactor, out Vector2 point, out Vector2 normal)
        {
            float a = start.X - this.center.X;
            float b = start.Y - this.center.Y;
            float r2 = this.radius * this.radius;

            float c2 = dir.X * dir.X;
            float d2 = dir.Y * dir.Y;
            float cd = dir.X * dir.Y;

            float s = (r2 - a * a) * d2
                    + (r2 - b * b) * c2
                    + 2 * a * b * cd;


            // if s is less than 0, the solutions for f are imaginary
            // and the ray's line does not intersect the circle
            if (s >= 0)
            {
                float f = ((float)Math.Sqrt(s) + a * dir.X + b * dir.Y) / -(c2 + d2);

                if (f <= 1)
                {
                    if (f >= 0 ||
                        (a * a + b * b < r2 && !float.IsNegativeInfinity(f)))
                    {
                        rayFactor = f;
                        point = start + dir * f;
                        normal = new Vector2(a, b);
                        return true;
                    }
                }
            }
            rayFactor = 0;
            point = new Vector2();
            normal = new Vector2();
            return false;
        }

        public bool IsInside(Vector2 position)
        {
            var d = this.center - position;
            return d.LengthSquared < this.radius * this.radius;
        }
    }
}