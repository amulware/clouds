using Bearded.Utilities.Math;
using OpenTK;

namespace Clouds.Game
{
    class Cannon : PositionedEquipment
    {
        private readonly GunControlGroup controlGroup;
        private float dontFireUntil;

        public Cannon(GameState game, Vector2 positionOffset, Angle directionOffset,
                GunControlGroup controlGroup)
            : base(game, positionOffset, directionOffset)
        {
            this.controlGroup = controlGroup;
        }

        public override void Update(ShipControlState controlState, float elapsedTime)
        {
            base.Update(controlState, elapsedTime);

            if (controlState.TryingToShoot(this.controlGroup))
            {
                if (this.game.TimeF > this.dontFireUntil)
                {
                    this.shoot();
                }
            }
        }

        private void shoot()
        {
            new Projectile(this.game, this.Position, this.Direction, 20, 5);
            this.dontFireUntil = this.game.TimeF + 1;
        }
    }
}