using Kriptok.Entities.Base;
using System.Drawing;
using Kriptok.Views.Primitives;
using Kriptok.Drawing;
using Kriptok.Drawing.Algebra;

namespace Asteroids.Entities
{
    public class HyperSpace : EntityBase<EllipseView>
    {
        public HyperSpace(Vector3F location) : base(new EllipseView(46, 46, null, Color.White, 3f))
        {
            Location = location;
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);
            
            // Realiza el sonido            
            h.Audio.GetSoundHandler("FX33.WAV").Play();
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
