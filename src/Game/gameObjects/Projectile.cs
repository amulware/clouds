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
                    for (int i = 0; i < 10; i++)
                    {
                        new Particle(this.game, Color.White, result.Point,
                            Direction2.Of(result.Normal) + (StaticRandom.Float(-1, 1) * 0.3f).Radians(),
                            StaticRandom.Float(10), StaticRandom.Float(0.25f, 0.5f)
                            );
                    }
                    this.Delete();
                    return;
                }
            }

            this.position += vDelta;

            if (this.game.TimeF > this.deathTime)
            {
                for (int i = 0; i < 10; i++)
                {
                    new Particle(this.game, Color.LightBlue, this.position,
                        Direction2.FromRadians(StaticRandom.Float(GameMath.TwoPi)),
                        StaticRandom.Float(3), StaticRandom.Float(0.5f, 1)
                        );
                }
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
