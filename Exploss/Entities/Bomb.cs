using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions;
using Kriptok.Views.Div;
using Kriptok.Views.Sprites;

namespace Exploss.Entities
{
    /// <summary>
    /// Bomba que al entrar en colisión con una burbuja, la destruye.
    /// </summary>
    public class Bomb : EntityBase<SpriteView>
    {
        public Bomb(float x, float y) 
            : base(new SpriteView(typeof(Bombs).Assembly, "Assets.Images.Bombs.png"))
        {
            Location.X = x;
            Location.Y = y;
            Location.Z = -1f;
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);
            h.CollisionType = Collision2DTypeEnum.Auto;
        }

        protected override void OnFrame()
        {            
        }
    }
}