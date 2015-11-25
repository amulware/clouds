﻿using System.Collections.Generic;
using amulware.Graphics;
using Bearded.Utilities.Math;
using OpenTK;

namespace Clouds.Game
{
    class Ship : GameObject, IPositionable
    {
        private readonly IShipController controller;

        private readonly List<IEquipment> equipment = new List<IEquipment>();

        private Vector2 position;
        private Vector2 velocity;
        private Direction2 forwards;
        private Matrix2 localRotation;

        public Vector2 Position { get { return this.position; } }

        public Ship(GameState game, IShipController controller)
            : base(game)
        {
            this.controller = controller;
            controller.SetShip(this);
        }

        public void AddEquipment(IEquipment equipment)
        {
            equipment.SetOwner(this);

            this.equipment.Add(equipment);
        }

        public override void Update(float elapsedTime)
        {
            var controlState = this.controller.Control(elapsedTime);

            this.updateMovement(controlState, elapsedTime);

            this.updateEquipment(controlState, elapsedTime);
        }

        private void updateMovement(ShipControlState controlState, float elapsedTime)
        {
            this.forwards += Angle.FromRadians(controlState.Steer) * elapsedTime;

            if (controlState.Accelerate)
            {
                this.velocity += this.forwards.Vector * 5 * elapsedTime;
            }

            var dragFactor = GameMath.Pow(0.8f, elapsedTime);

            this.velocity *= dragFactor;

            this.position += this.velocity * elapsedTime;

            this.localRotation = Matrix2.CreateRotation(-this.forwards.Radians);
        }

        private void updateEquipment(ShipControlState controlState, float elapsedTime)
        {
            foreach (var e in this.equipment)
            {
                e.Update(controlState, elapsedTime);
            }
        }

        public override void Draw()
        {
            var geo = GeometryManager.Instance.Primitives;

            geo.Color = Color.AliceBlue;
            geo.LineWidth = 1;

            geo.DrawCircle(this.position, 3);
            geo.DrawLine(this.position, this.position + this.forwards.Vector * 5);

            geo.Color = Color.Red;
            geo.LineWidth = 0.3f;

            geo.DrawLine(this.position, this.position + this.velocity);

            this.drawEquipment();
        }

        private void drawEquipment()
        {
            foreach (var e in this.equipment)
            {
                e.Draw();
            }
        }

        public Vector2 LocalToGlobalPosition(Vector2 p)
        {
            return this.position + this.localRotation.Transform(p);
        }

        public Direction2 LocalToGlobalDirection(Angle direction)
        {
            return this.forwards + direction;
        }
    }
}