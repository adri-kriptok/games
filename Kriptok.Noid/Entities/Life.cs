using Kriptok.Entities.Base;
using Kriptok.Views.Sprites;

namespace Noid.Scenes
{
    internal class Life : EntityBase
    {        
        public Life(int index) : base(new SpriteView(typeof(Life).Assembly, "Assets.Images.Life.png"))
        {
            Location.X = 296;
            Location.Y = 16 + 11 * index;
        }

        protected override void OnFrame()
        {            
        }
    }
}
