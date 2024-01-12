using Kriptok.Entities.Base;
using Fostiator;
using Kriptok.Views.Div;

namespace Fostiator.Entities
{
    internal class Dust : EntityBase<DivFileXView>
    {        
        public Dust(float x, float y) : base(Global2.FileX.GetNewView())
        {
            Location.X = x;
            Location.Y = y;

            // Lo hace transparente
            View.Alpha = 0.5f;

            // Lo pone por delante del munieco
            Location.Z = -1;

            View.Graph = 2;
        }

        protected override void OnFrame()
        {
            if (View.Graph == 12)
            {
                Die();
            }
            else
            {
                View.Graph++;
            }
        }
    }
}

