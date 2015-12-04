using System;
using Bearded.Utilities;
using Bearded.Utilities.Math;
using OpenTK;

namespace Clouds.Game
{
    class ShipFactory
    {
        private readonly GameState game;

        public ShipFactory(GameState game)
        {
            this.game = game;
        }

        public void MakeShip(Vector2 position, int faction,
            int minCannons = 2, int maxCannons = 5, Direction2 direction = default(Direction2))
        {
            var cannons = StaticRandom.Int(minCannons, maxCannons + 1);

            var s = new Ship(this.game, position,
                new SimpleEnemyShipController(this.game), faction,
                11 - 3f / 2f * cannons, direction, Math.Min(0.4f + 0.1f * cannons, 0.9f)
                );

            for (int j = 0; j < cannons; j++)
            {
                var x2 = (j - cannons / 2f + 0.5f) / cannons * 5;
                 
                var a = Angle.FromDegrees(90);
                s.AddEquipment(new Cannon(this.game, new Vector2(x2, 2), a, GunControlGroup.Left));
                s.AddEquipment(new Cannon(this.game, new Vector2(x2, -2), -a, GunControlGroup.Right));
            }
        }
    }
}