using amulware.Graphics;
using Bearded.Utilities;
using Bearded.Utilities.Math;
using OpenTK;

namespace Clouds.Game
{
    class Projectile : GameObject
    {
        private readonly Ship owner;

        private Vector2 position;
        private Vector2 velocity;
        private readonly float deathTime;

        public Projectile(GameState game, Ship owner, Vector2 position,
            Vector2 baseVelocity, Direction2 direction, float speed, float lifeTime)
            : base(game)
        {
            this.owner = owner;
            this.position = position;
            this.velocity = baseVelocity + direction.Vector * speed;
            this.deathTime = game.TimeF + lifeTime;
        }

        public override void Update(float elapsedTime)
        {
            var vDelta = this.velocity * elapsedTime;

            var ray = new Ray(this.position, vDelta);
            foreach (var ship in this.game.Ships)
            {
                if (ship == this.owner)
                {
                    continue;
                }

                HitResult result;

                if (ship.TryHit(ray, out result))
                {
                    ship.DealDamage(1);
                    Particle.Create(this.game, 10, Color.White, result.Point,
                        Direction2.Of(result.Normal), 0.3f, 10, 0.5f);
                    this.Delete();
                    return;
                }
            }

            this.position += vDelta;

            if (this.game.TimeF > this.deathTime)
            {
                Particle.Create(this.game, 10, Color.LightBlue, this.position, 3, 1);
                this.Delete();
            }
        }

        public override void Draw()
        {
            var geo = GeometryManager.Instance.Primitives;
            geo.Color = Color.Cyan;
            geo.LineWidth = 0.2f;

            var d = this.velocity.Normalized() * 0.15f;
            var p = this.position;
            geo.DrawLine(p - d, p + d);
        }
    }
}
