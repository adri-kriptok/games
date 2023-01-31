using Kriptok.Div.Views.Cars;
using Kriptok.Entities.Base;
using Kriptok.Views.Div;
using Kriptok.Views.Sprites;

namespace Exploss.Entities
{
    public class Lives : EntityBase<SpriteView>
    {
        public Lives() : base(new SpriteView(Car08RedView.GetResource(), 0, 60, 20, 20)
        {
            ScaleX = 2f,
            ScaleY = 2f
        })
        {            
            Location.X = 20f;
            Location.Y = 410f;
            Location.Z = -1f;
        }

        protected override void OnFrame()
        {                
        }
    }    
}