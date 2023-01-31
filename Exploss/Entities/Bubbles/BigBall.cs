using Exploss.Entities.Bubbles;
using Kriptok.Drawing.Algebra;

namespace Exploss.Entities
{
    /// <summary>
    /// Este proceso genera una bola grande, si colisiona con un disparo
    /// genera dos bolas medianas.
    /// </summary>
    class BigBall : BubbleBase
    {
        public BigBall(float x, float y) : base(x, y, "Assets.Images.Bubble0.png")
        {
        }

        internal override void CreateSmallerBalls(Vector2F inc)
        {
            Add(new MediumBall(Location.X, Location.Y, inc.X, inc.Y));
            Add(new MediumBall(Location.X, Location.Y, -inc.X, -inc.Y));
        }
    }
}
