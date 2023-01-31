using Exploss.Entities.Bubbles;
using Kriptok.Drawing.Algebra;

namespace Exploss.Entities
{
    /// <summary>
    /// Este proceso genera una bola pequenia, si colisiona con un disparo */
    /// el proceso es destruido   
    /// </summary>
    class SmallBubble : BubbleBase
    {
        public SmallBubble(float x, float y, float xInc, float yInc)
            : base(x, y, "Assets.Images.Bubble2.png", xInc, yInc)
        {
        }

        internal override void CreateSmallerBalls(Vector2F inc)
        {
        }
    }
}
