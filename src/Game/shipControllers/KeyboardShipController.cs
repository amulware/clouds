using Bearded.Utilities.Input;
using OpenTK.Input;

namespace Clouds.Game
{
    class KeyboardShipController : IShipController
    {
        private readonly IAction forwardAction = KeyboardAction.FromKey(Key.W);
        private readonly IAction leftAction = KeyboardAction.FromKey(Key.A);
        private readonly IAction rightAction = KeyboardAction.FromKey(Key.D);

        private readonly GameState game;
        private Ship ship;

        public KeyboardShipController(GameState game)
        {
            this.game = game;
        }

        public ShipControlState Control(float elapsedTime)
        {
            return new ShipControlState(
                this.forwardAction.Active,
                this.leftAction.AnalogAmount - this.rightAction.AnalogAmount
                );
        }

        public void SetShip(Ship ship)
        {
            this.ship = ship;
        }
    }
}