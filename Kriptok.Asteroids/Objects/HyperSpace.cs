using Kriptok.Objects.Base;
using System.Drawing;
using Kriptok.Views.Primitives;
using Kriptok.Drawing;
using Kriptok.Drawing.Algebra;

namespace Kriptok.Asteroids.Objects
{
    public class HyperSpace : ObjectBase<EllipseView>
    {
        public HyperSpace(Vector3F location) : base(new EllipseView(46, 46, null, Color.White, 3f))
        {
            Location = location;
        }

        protected override void OnStart(ObjectStartHandler h)
        {
            base.OnStart(h);
            
            // Realiza el sonido
            Audio.PlaySound(Assembly, "FX33.WAV");
        }

        protected override void OnFrame()
        {
            // Repite hasta que desaparezca
            if (View.Scale.X > 0)
            {
                // Hace que el gráfico sea más peque¤o
                View.ScaleX -= 0.05f;
                View.ScaleY -= 0.05f;
            }
            else
            {
                Die();
            }
        }
    }
}
