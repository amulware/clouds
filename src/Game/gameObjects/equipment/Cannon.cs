using amulware.Graphics;
using Bearded.Utilities;
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
            new Projectile(this.game, this.Position,
                this.Owner.Velocity,
                this.Direction + (StaticRandom.Float(-1, 1) * 0.1f).Radians(),
                40 + StaticRandom.Float(5),
                1 + StaticRandom.Float(0.3f)
                );

            for (int i = 0; i < 10; i++)
            {
                new Particle(this.game, Color.Gray, this.Position + this.Direction.Vector,
                    Direction2.FromRadians(StaticRandom.Float(GameMath.TwoPi)),
                    StaticRandom.Float(1), StaticRandom.Float(0.5f, 1)
                    );
            }

            this.dontFireUntil = this.game.TimeF + 1;
        }
    }
}