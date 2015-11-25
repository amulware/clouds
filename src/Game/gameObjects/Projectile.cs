using amulware.Graphics;
using Bearded.Utilities.Math;
using OpenTK;

namespace Clouds.Game
{
    class Projectile : GameObject
    {
        private Vector2 position;
        private Vector2 velocity;
        private float deathTime;

        public Projectile(GameState game, Vector2 position, Direction2 direction, float speed, float lifeTime)
            : base(game)
        {
            this.position = position;
            this.velocity = direction.Vector * speed;
            this.deathTime = game.TimeF + lifeTime;
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
            geo.Color = Color.Cyan;
            geo.LineWidth = 0.2f;

            var d = this.velocity.Normalized() * 0.15f;
            var p = this.position;
            geo.DrawLine(p - d, p + d);
        }
    }
}
