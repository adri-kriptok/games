using Kriptok.Entities.Base;
using Kriptok.Views.Sprites;

namespace Kriptok.Games.BlastemUp.Player
{
    public class Life : EntityBase<SpriteView>
    {
        private static readonly int[] locationX = new int[3] { 37, 73, 108 };

        public Life(int position) : base(new SpriteView(typeof(Life).Assembly, "Misc.Life.png"))
        {
            Location.Y = 460;
            Location.X = locationX[position];
        }

        protected override void OnFrame()
        {            
        }
    }
}
