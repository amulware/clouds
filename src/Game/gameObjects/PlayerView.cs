using amulware.Graphics;
using Bearded.Utilities.Math;
using OpenTK;

namespace Clouds.Game
{
    class PlayerView : GameObject
    {
        private readonly IPositionable parent;

        public PlayerView(GameState game, IPositionable parent)
            : base(game)
        {
            this.parent = parent;
        }

        public override void Update(float elapsedTime)
        {

        }

        public override void Draw()
        {
            var xy = this.parent.Position;

            this.drawGrid(xy);

            SurfaceManager.Instance.ModelviewMatrix.Matrix
                = Matrix4.LookAt(
                    xy.WithZ(300), xy.WithZ(), new Vector3(0, 1, 0)
                );
        }

        private void drawGrid(Vector2 xy)
        {
            const float step = 10;
            const int lines = 20;
            const float offset = lines / 2.0f * step;
            const float lineLength = step * lines;

            var offsetX = step * (int)(xy.X / step) - offset;
            var offsetY = step * (int)(xy.Y / step) - offset;

            var geo = GeometryManager.Instance.Primitives;

            geo.Color = Color.LightGray;
            geo.LineWidth = 0.1f;

            for (int i = 0; i <= lines; i++)
            {
                var x = i * step + offsetX;
                geo.DrawLine(x, offsetY, x, offsetY + lineLength);
                var y = i * step + offsetY;
                geo.DrawLine(offsetX, y, offsetX + lineLength, y);
            }
        }
    }
}