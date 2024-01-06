using Kriptok.Extensions;
using Kriptok.Entities.Base;
using Kriptok.Views.Primitives;
using System.Drawing;

namespace Billiard.Entities
{
    /// <summary>
    /// Proceso efecto
    /// Muestra la posicion del toque
    /// </summary>
    public class Effect : EntityBase<EllipseView>
    {
        public const int StartX = 604;
        public const int StartY = 452;

        public Effect() : base(new EllipseView(11, 11, Color.Black))
        {            
            // Coge las coordenadas de la posicion de toque
            Location.X = Effect.StartX;
            Location.Y = Effect.StartY;
        }

        protected override void OnFrame()
        {
        }
    }
}
