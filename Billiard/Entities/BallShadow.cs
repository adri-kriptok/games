using Kriptok.Extensions;
using Billiard.Entities.Base;
using Kriptok.Entities.Base;
using Kriptok.Views.Primitives;
using System.Drawing;

namespace Billiard.Entities
{
    /// <summary>
    ///  Proceso sombra
    ///  Muestra las sombras de la bola y el palo
    /// </summary>
    public class BallShadow : ShadowBase<EllipseView>
    {
        public BallShadow(EntityBase owner) : base(owner, new EllipseView(27, 27, Color.Black.SetAlpha(128)))
        {
        }
    }
}
