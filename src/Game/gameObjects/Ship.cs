using amulware.Graphics;
using Bearded.Utilities.Math;
using OpenTK;

namespace Clouds.Game
{
    class Ship : GameObject, IPositionable
    {
        private readonly IShipController controller;

        private Vector2 position;
        private Vector2 velocity;
        private Direction2 forwards;

        public Vector2 Position { get { return this.position; } }

        public Ship(GameState game, IShipController controller)
            : base(game)
        {
            this.controller = controller;
            controller.SetShip(this);
        }

        public override void Update(float elapsedTime)
        {
            var controlState = this.controller.Control(elapsedTime);

            this.forwards += Angle.FromRadians(controlState.Steer) * elapsedTime;

            if (controlState.Accelerate)
            {
                this.velocity += this.forwards.Vector * 5 * elapsedTime;
            }

            var dragFactor = GameMath.Pow(0.8f, elapsedTime);

            this.velocity *= dragFactor;


            this.position += this.velocity * elapsedTime;
        }

        public override void Draw()
        {
            var geo = GeometryManager.Instance.Primitives;

            geo.Color = Color.AliceBlue;
            geo.LineWidth = 1;

            geo.DrawCircle(this.position, 3);
            geo.DrawLine(this.position, this.position + this.forwards.Vector * 5);

            geo.Color = Color.Red;
            geo.LineWidth = 0.3f;

            geo.DrawLine(this.position, this.position + this.velocity);
        }
    }
}