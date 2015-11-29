using OpenTK;

namespace Clouds.Game
{
    struct Ray
    {
        private readonly Vector2 position;
        private readonly Vector2 vDelta;

        public Ray(Vector2 position, Vector2 vDelta)
        {
            this.position = position;
            this.vDelta = vDelta;
        }

        public Vector2 Position { get { return this.position; } }
        public Vector2 VDelta { get { return this.vDelta; } }
    }
}