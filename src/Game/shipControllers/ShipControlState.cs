using Bearded.Utilities.Math;

namespace Clouds.Game
{
    struct ShipControlState
    {
        private readonly bool accelerate;
        private readonly float steer;
        private readonly GunControlGroup controlGroup;

        public static ShipControlState Idle { get { return default(ShipControlState); } }

        public ShipControlState(bool accelerate, float steer, GunControlGroup controlGroup)
        {
            this.accelerate = accelerate;
            this.steer = steer.Clamped(-1, 1);
            this.controlGroup = controlGroup;
        }

        public bool Accelerate { get { return this.accelerate; } }
        public float Steer { get { return this.steer; } }

        public bool TryingToShoot(GunControlGroup controlGroup)
        {
            return this.controlGroup.HasFlag(controlGroup);
        }
    }
}