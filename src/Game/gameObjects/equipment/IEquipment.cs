namespace Clouds.Game
{
    interface IEquipment
    {
        void SetOwner(Ship ship);

        void Update(ShipControlState controlState, float elapsedTime);
        void Draw();
    }
}