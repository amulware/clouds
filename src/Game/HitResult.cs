using OpenTK;

namespace Clouds.Game
{
    struct HitResult
    {
        private readonly Vector2 point;
        private readonly Vector2 normal;

        public HitResult(Vector2 point, Vector2 normal)
        {
            this.point = point;
            this.normal = normal;
        }

        public Vector2 Point { get { return this.point; } }
        public Vector2 Normal { get { return this.normal; } }
    }
}