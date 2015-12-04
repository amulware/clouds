using System.Collections.Generic;
using amulware.Graphics;
using Bearded.Utilities.Math;
using OpenTK;

namespace Clouds.Game
{
    class Ship : GameObject, IPositionable
    {
        private readonly IShipController controller;
        private readonly float acceleration;
        private readonly float maxHealth = 100;
        private readonly int faction;
        private readonly float inverseFriction;

        private readonly List<IEquipment> equipment = new List<IEquipment>();

        private Vector2 position;
        private Vector2 velocity;
        private Direction2 forwards;
        private Matrix2 localRotation;
        private float health = 100;

        public Vector2 Position { get { return this.position; } }
        public Vector2 Velocity { get { return this.velocity; } }
        public Direction2 Direction { get { return this.forwards; } }

        public int Faction { get { return this.faction; } }

        public Ship(GameState game, Vector2 position,
            IShipController controller, int faction = 0, float acceleration = 5,
            Direction2 direction = default(Direction2), float inverseFriction = 0.8f)
            : base(game)
        {
            this.position = position;
            this.controller = controller;
            this.faction = faction;
            this.acceleration = acceleration;
            this.forwards = direction;
            this.inverseFriction = inverseFriction;
            controller.SetShip(this);

            game.Ships.Add(this);
        }

        public void AddEquipment(IEquipment equipment)
        {
            equipment.SetOwner(this);

            this.equipment.Add(equipment);
        }

        public void DealDamage(float damage)
        {
            this.health -= damage;
        }

        public override void Update(float elapsedTime)
        {
            if (this.health < 0)
            {
                this.Delete();
            }

            var controlState = this.controller.Control(elapsedTime);

            this.updateMovement(controlState, elapsedTime);

            this.updateEquipment(controlState, elapsedTime);
        }

        private void updateMovement(ShipControlState controlState, float elapsedTime)
        {
            this.forwards += Angle.FromRadians(controlState.Steer) * elapsedTime;

            if (controlState.Accelerate)
            {
                this.velocity += this.forwards.Vector * this.acceleration * elapsedTime;
            }

            var dragFactor = Mathf.Pow(this.inverseFriction, elapsedTime);

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

            geo.Color = Color.FromHSVA(this.faction, 0.7f, 0.8f);
            geo.LineWidth = 1;

            geo.DrawCircle(this.position, 3);
            geo.DrawLine(this.position, this.position + this.forwards.Vector * 5);

            geo.Color = Color.Red;
            geo.LineWidth = 0.3f;

            geo.DrawLine(this.position, this.position + this.velocity);


            var healthBarLength = 8f;
            var healthBarStart = this.position + new Vector2(-healthBarLength / 2, -5);
            var healthPercentage = this.health / this.maxHealth;

            geo.Color = Color.Gray;
            geo.DrawRectangle(healthBarStart, new Vector2(healthBarLength, 0.5f));

            geo.Color = Color.Lerp(Color.Red, Color.Green, healthPercentage);
            geo.DrawRectangle(healthBarStart, new Vector2(healthBarLength * healthPercentage, 0.5f));

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

        public bool TryHit(Ray ray, out HitResult result)
        {
            var circle = new CollisionCircle(this.position, 3);

            float f;
            Vector2 p, n;

            if (circle.TryHit(ray.Position, ray.VDelta, out f, out p, out n))
            {
                result = new HitResult(p, n);
                return true;
            }

            result = default(HitResult);
            return false;
        }

        protected override void onDelete()
        {
            Particle.Create(this.game, 500, Color.Pink, this.position, 10, 3);
        }
    }
}