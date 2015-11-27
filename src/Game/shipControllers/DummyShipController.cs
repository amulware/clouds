namespace Clouds.Game
{
    class DummyShipController : IShipController
    {
        public ShipControlState Control(float elapsedTime)
        {
            return ShipControlState.Idle;
        }

        public void SetShip(Ship ship)
        {
        }
    }
}