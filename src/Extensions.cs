using OpenTK;

namespace Clouds
{
    static class Extensions
    {
        public static Vector2 Transform(this Matrix2 matrix, Vector2 vector)
        {
            return new Vector2(
                Vector2.Dot(matrix.Row0, vector),
                Vector2.Dot(matrix.Row1, vector)
                );
        }
    }
}