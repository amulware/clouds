namespace Clouds.Game
{
    interface IShipController
    {
        ShipControlState Control(float elapsedTime);
        void SetShip(Ship ship);
    }
}