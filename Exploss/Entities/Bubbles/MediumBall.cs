using Exploss.Entities.Bubbles;
using Kriptok.Drawing.Algebra;

namespace Exploss.Entities
{
    /// <summary>
    /// Este proceso genera una bola mediana, si colisiona con un disparo
    /// genera dos bolas pequenias       
    /// </summary>
    class MediumBall : BubbleBase
    {
        public MediumBall(float x, float y, float xInc, float yInc) 
            : base(x, y, "Assets.Images.Bubble1.png", xInc, yInc)
        {                        
        }

        internal override void CreateSmallerBalls(Vector2F inc)
        {
            Add(new SmallBall(Location.X, Location.Y, inc.X, inc.Y));
            Add(new SmallBall(Location.X, Location.Y, -inc.X, -inc.Y));
        }
    }
}