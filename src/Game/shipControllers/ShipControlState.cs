using Bearded.Utilities.Math;

namespace Clouds.Game
{
    struct ShipControlState
    {
        private readonly bool accelerate;
        private readonly float steer;

        public ShipControlState(bool accelerate, float steer)
        {
            this.accelerate = accelerate;
            this.steer = steer.Clamped(-1, 1);
        }

        public bool Accelerate { get { return this.accelerate; } }
        public float Steer { get { return this.steer; } }
    }
}