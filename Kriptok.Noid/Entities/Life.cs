using Kriptok.Objects.Base;
using Kriptok.Views.Sprites;

namespace Kriptok.Noid.Scenes
{
    internal class Life : ObjectBase
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
