using Kriptok.Core;
using System.Drawing;
using Kriptok.Entities.Base;
using Kriptok.Views.Primitives;

namespace Pacoman.Entities
{
    /// <summary>
    /// Rectángulo negro que se encarga de opacar las pastillas parpadeantes.
    /// </summary>
    public class Blinker : EntityBase<RectangleView>
    {
        private int frameCounter = 0;

        public Blinker(float x, float y) : base(new RectangleView(20, 20, Color.Black))
        {
            Location.X = x;
            Location.Y = y;
            Location.Z = 100;
        }

        protected override void OnFrame()
        {
            if (frameCounter++ >= 4)
            {
                frameCounter = 0;
                Location.Y = -Location.Y;
            }
        }

        //protected override void OnBegin()
        //{
        //    Location.Z = 100;
        //    Loop(() =>
        //    {
        //        // Imprime el grafico
        //        View.SetSize(20f, 20f);
        //        Frame(3); // Espera

        //        // No pone ningun grafico                
        //        View.SetSize(0f, 0f);
        //        Frame(3); // Espera
        //    });
        //}
    }
}
