using Kriptok.Drawing;
using Billiard.Entities.Base;
using Kriptok.Views.Sprites;

namespace Billiard.Entities
{
    /// <summary>
    ///  Proceso sombra
    ///  Muestra las sombras de la bola y el palo
    /// </summary>
    public class StickShadow : ShadowBase<SpriteView>
    {        
        public StickShadow(Stick owner) : base(owner, new SpriteView(typeof(StickShadow).Assembly, "Assets.Stick.png"))
        {
            View.Center = owner.View.Center;

            // Lo pone transparente.
            View.Transform = new ColorTransform()
            {
                R = new ColorVector(0f, 0f, 0f),
                G = new ColorVector(0f, 0f, 0f),
                B = new ColorVector(0f, 0f, 0f),
                A = 0.5f
            };
        }
    }
}
