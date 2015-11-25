using System;
using System.Collections.Generic;
using System.Linq;
using Bearded.Utilities.Input;
using OpenTK.Input;

namespace Clouds.Game
{
    class KeyboardShipController : IShipController
    {
        private readonly IAction forwardAction = KeyboardAction.FromKey(Key.W);
        private readonly IAction leftAction = KeyboardAction.FromKey(Key.A);
        private readonly IAction rightAction = KeyboardAction.FromKey(Key.D);

        private readonly List<Tuple<IAction, GunControlGroup>> shootActions =
            new List<Tuple<IAction, GunControlGroup>>
        {
            Tuple.Create(KeyboardAction.FromKey(Key.Right), GunControlGroup.Right),
            Tuple.Create(KeyboardAction.FromKey(Key.Left), GunControlGroup.Left),
            Tuple.Create(KeyboardAction.FromKey(Key.Down), GunControlGroup.Aft),
            Tuple.Create(KeyboardAction.FromKey(Key.Up), GunControlGroup.Front),
        };

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
                this.leftAction.AnalogAmount - this.rightAction.AnalogAmount,
                this.getGunControls()
                );
        }

        private GunControlGroup getGunControls()
        {
            return this.shootActions
                .Where(t => t.Item1.Active)
                .Aggregate(GunControlGroup.None, (c, t) => c | t.Item2);
        }

        public void SetShip(Ship ship)
        {
            this.ship = ship;
        }
    }
}