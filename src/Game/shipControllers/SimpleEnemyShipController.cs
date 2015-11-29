using System.Linq;
using amulware.Graphics;
using Bearded.Utilities.Math;
using OpenTK;

namespace Clouds.Game
{
    class SimpleEnemyShipController : IShipController
    {
        private readonly GameState game;
        private Ship ship;

        private bool hasTargetOffset;
        private Vector2 targetOffset;
        private Vector2 targetNormal;

        public SimpleEnemyShipController(GameState game)
        {
            this.game = game;
        }

        public ShipControlState Control(float elapsedTime)
        {
            var target = this.game.PlayerShips.FirstOrDefault();

            if (target == null)
                return ShipControlState.Idle;

            var position = this.ship.Position;

            var differenceToTarget = target.Position - position;

            var distanceToTarget = differenceToTarget.Length;

            var speed = this.ship.Velocity.Length;

            var timeToTarget = distanceToTarget / speed;

            var targetShipInterceptPosition = target.Position + target.Velocity * timeToTarget;

            if (this.hasTargetOffset)
            {
                if ((Direction2.Of(differenceToTarget) - this.ship.Direction).MagnitudeInDegrees > 90)
                {
                    this.hasTargetOffset = false;
                }
            }

            if (!this.hasTargetOffset)
            {
                this.updateTargetOffset(target.Position);
                this.hasTargetOffset = true;
            }

            var targetPosition = targetShipInterceptPosition + this.targetOffset;

            var targetDirection = Direction2.Of(targetPosition - position);

            var velocityDirection = Direction2.Of(this.ship.Velocity);

            var compensateAngle = Angle.Clamp(targetDirection - velocityDirection, -90.Degrees(), 90.Degrees()) * 0.5f;

            targetDirection += compensateAngle;

            var directionDifference = this.getSteeringToDirection(targetDirection);

#if true
            var geo = GeometryManager.Instance.Primitives;

            geo.LineWidth = 0.2f;
            geo.Color = Color.Green.WithAlpha(0.3f).Premultiplied;
            geo.DrawCircle(targetPosition, 5, false);
            geo.DrawLine(position, targetPosition);
            geo.DrawLine(position, position + this.ship.Direction.Vector * 20);

            //var sideWays = this.ship.Direction.Vector.PerpendicularLeft;

            //geo.LineWidth = 5;
            //geo.Color = Color.Red.WithAlpha(0.1f).Premultiplied;
            //geo.DrawLine(position - sideWays * 50, position + sideWays * 50);
#endif
            var gunControlState = this.tryShoot(target);

            return new ShipControlState(true, directionDifference.Radians * 5, gunControlState);
        }

        private GunControlGroup tryShoot(Ship target)
        {
            var distanceToTarget = (this.ship.Position - target.Position).Length;

            const float projectileSpeed = 40;

            var timeToHit = distanceToTarget / projectileSpeed;

            var targetPoint = target.Position + target.Velocity * timeToHit - this.ship.Velocity * timeToHit;

            GunControlGroup groups = GunControlGroup.None;

            if (isInCone(targetPoint - this.ship.Position, this.ship.Direction + 90.Degrees(), 0.02f, 50))
            {
                groups |= GunControlGroup.Left;
            }

            if (isInCone(targetPoint - this.ship.Position, this.ship.Direction - 90.Degrees(), 0.02f, 50))
            {
                groups |= GunControlGroup.Right;
            }

            return groups;
        }

        private bool isInCone(Vector2 targetPoint, Direction2 direction, float directionVariance, float range)
        {
            if (targetPoint.LengthSquared > range.Squared())
                return false;

            var isInside = (Direction2.Of(targetPoint) - direction).MagnitudeInRadians < directionVariance;

            var geo = GeometryManager.Instance.Primitives;

            geo.LineWidth = 0.2f;
            geo.Color = (isInside ? Color.Red : Color.Yellow).WithAlpha(0.3f).Premultiplied;

            var p = this.ship.Position;
            var c0 = p + (direction + directionVariance.Radians()).Vector * range;
            var c1 = p + (direction - directionVariance.Radians()).Vector * range;

            geo.DrawLine(p, c0);
            geo.DrawLine(p, c1);
            geo.DrawLine(c1, c0);

            geo.DrawCircle(p + targetPoint, 3, false, 4);

            return isInside;
        }

        private void updateTargetOffset(Vector2 targetPosition)
        {
            var differenceToTarget = targetPosition - this.ship.Position;

            var targetOffset = differenceToTarget.Normalized().PerpendicularLeft * 40;

            var directionDifferenceLeft = this.getSteeringToPoint(targetPosition + targetOffset);
            var directionDifferenceRight = this.getSteeringToPoint(targetPosition - targetOffset);

            if (directionDifferenceLeft.Abs() < directionDifferenceRight.Abs())
            {
                this.targetOffset = targetOffset;
            }
            else
            {
                this.targetOffset = -targetOffset;
            }
            this.targetNormal = differenceToTarget;
        }

        private Angle getSteeringToPoint(Vector2 targetPosition)
        {
            var differenceToTargetPosition = targetPosition - this.ship.Position;
            var desiredDirection = Direction2.Of(differenceToTargetPosition);

            var currentDirection = this.ship.Direction;

            return desiredDirection - currentDirection;
        }
        private Angle getSteeringToDirection(Direction2 desiredDirection)
        {
            var currentDirection = this.ship.Direction;

            return desiredDirection - currentDirection;
        }

        public void SetShip(Ship ship)
        {
            this.ship = ship;
        }
    }
}