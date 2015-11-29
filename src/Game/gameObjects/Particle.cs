using amulware.Graphics;
using Bearded.Utilities;
using Bearded.Utilities.Math;
using OpenTK;

namespace Clouds.Game
{
    class Particle : GameObject
    {
        private Vector2 position;
        private Vector2 velocity;
        private readonly float deathTime;
        private readonly Color color;

        public Particle(GameState game, Color color, Vector2 position, Direction2 direction, float speed, float lifeTime)
            : this(game, color, position, direction.Vector * speed, lifeTime)
        {
        }

        public Particle(GameState game, Color color, Vector2 position, Vector2 velocity, float lifeTime)
            : base(game)
        {
            this.color = color;
            this.position = position;
            this.velocity = velocity;
            this.deathTime = game.TimeF + lifeTime;
        }

        public static void Create(GameState game, int count, Color color, Vector2 position,
            Direction2 direction, float directionVariance, float speed, float lifeTime)
        {
            for (int i = 0; i < count; i++)
            {
                new Particle(game, color, position,
                    direction + (StaticRandom.Float(-1, 1) * directionVariance).Radians(),
                    StaticRandom.Float(speed), StaticRandom.Float(0.5f, 1) * lifeTime
                    );
            }
        }

        public static void Create(GameState game, int count, Color color, Vector2 position,
            float speed, float lifeTime)
        {
            Particle.Create(game, count, color, position, Direction2.Zero, Mathf.Pi, speed, lifeTime);
        }

        public override void Update(float elapsedTime)
        {
            this.position += this.velocity * elapsedTime;

            if (this.game.TimeF > this.deathTime)
            {
                this.Delete();
            }
        }

        public override void Draw()
        {
            var geo = GeometryManager.Instance.Primitives;
            geo.Color = this.color;
            geo.LineWidth = 0.2f;

            var d = this.velocity.Normalized() * 0.15f;
            var p = this.position;
            geo.DrawLine(p - d, p + d);
        }
    }
}
