using System;
using amulware.Graphics;
using Bearded.Utilities.Math;
using OpenTK;

namespace Clouds.Game
{
    class PositionedEquipment : IEquipment
    {
        protected readonly GameState game;

        private readonly Vector2 positionOffset;
        private readonly Angle directionOffset;

        private Ship owner;

        private Vector2 realPosition;
        private Direction2 realDirection;

        public Ship Owner { get { return this.owner; } }
        public Vector2 Position { get { return this.realPosition; } }
        public Direction2 Direction { get { return this.realDirection; } }

        public PositionedEquipment(GameState game, Vector2 positionOffset, Angle directionOffset)
        {
            this.game = game;
            this.positionOffset = positionOffset;
            this.directionOffset = directionOffset;
        }

        public void SetOwner(Ship ship)
        {
            if (this.owner != null)
                throw new Exception("Equipment can only be added to a single ship.");

            this.owner = ship;
        }

        public virtual void Update(ShipControlState controlState, float elapsedTime)
        {
            this.realPosition = this.owner.LocalToGlobalPosition(this.positionOffset);
            this.realDirection = this.owner.LocalToGlobalDirection(this.directionOffset);
        }

        public void Draw()
        {
            var geo = GeometryManager.Instance.Primitives;
            geo.Color = Color.Purple;
            geo.LineWidth = 0.3f;

            geo.DrawLine(this.realPosition, this.realPosition + this.realDirection.Vector);
        }
    }
}