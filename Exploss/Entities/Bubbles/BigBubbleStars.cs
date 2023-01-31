using Kriptok.Div.Views.Explosions;
using Kriptok.Entities.Base;

namespace Exploss.Entities
{
    /// <summary>
    /// Este proceso genera las estrellas que indican el inicio de una bola grande.
    /// </summary>
    class BigBubbleStars : EntityBase<Explosion03View>
    {
        private int counter = 3;

        public BigBubbleStars(float x, float y) : base(new Explosion03View())
        {
            Location.X = x;
            Location.Y = y;
            Location.Z = -1f;
            View.Graph = 11;
        }

        protected override void OnFrame()
        {            
            if (--View.Graph == 6)
            {
                if (counter-- == 0)
                {
                    Add(new BigBubble(Location.X, Location.Y));
                    Die();
                }
                else
                {
                    View.Graph = 11;
                }
            }
        }
    }
}
