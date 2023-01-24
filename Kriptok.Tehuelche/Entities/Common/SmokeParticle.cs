using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Extensions;
using Kriptok.Views.Primitives;
using System.Drawing;

namespace Kriptok.Tehuelche.Entities
{
    internal class SmokeParticle : EntityBase<RectangleView>
    {
        public SmokeParticle(Vector3F location) 
            : base(new RectangleView(1, 1, Color.Gray.SetAlpha(128)))
        {
            Location = location;

            Location.X += Rand.NextF(-1f, 1f);
            Location.Y += Rand.NextF(-1f, 1f);
            Location.Z += Rand.NextF(-1f, 1f);
        }

        protected override void OnFrame()
        {
            Location.Z += 0.1f;
            View.ScaleX *= 0.75f;

            if (View.ScaleX < 0.1f)
            {
                Die();
                return;
            }
            View.ScaleY = View.ScaleX;
        }
    }    
}