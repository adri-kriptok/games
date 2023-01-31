using Kriptok.Entities.Base;
using Kriptok.Views.Sprites;

namespace Exploss.Entities
{
    /// <summary>
    /// Este proceso se usa para visualizar el grafico de la bomba como
    /// simbolo del numero de bombas que se tienen.
    /// </summary>
    public class Bombs : EntityBase<SpriteView>
    {
        public Bombs() : base(new SpriteView(typeof(Bombs).Assembly, "Assets.Images.Bombs.png"))
        {
            Location.X = 20;
            Location.Y = 450;
            Location.Z = -1;
        }

        protected override void OnFrame()
        {            
        }
    }
}
