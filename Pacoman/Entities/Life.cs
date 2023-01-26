using Kriptok.Entities.Base;
using Kriptok.Views.Sprites;

namespace Pacoman.Entities
{
    public class Life : EntityBase<SpriteView>
    {
        public Life(int x) : base(new SpriteView(typeof(Life).Assembly, "Images.Player.png", 52, 0, 26, 26))
        {
            Location.X = x;
            Location.Y = 14;       // Elige la coordenada vertical
        }

        protected override void OnFrame()
        {            
        }
    }
}
