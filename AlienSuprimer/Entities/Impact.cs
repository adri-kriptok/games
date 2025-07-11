using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Views.Sprites;

namespace Kriptok.Games.Alien.Entities
{
    internal class Impact : EntityBase<SpriteView>
    {
        public Impact(Vector3F location) : base(new SpriteView(typeof(Impact).Assembly, "Assets.Images.Impact.png"))
        {
            Location = location;
            View.ScaleX = 0.26f;
            View.ScaleY = 0.26f;
            Location.Z = -999;
        }

        protected override void OnFrame()
        {
            View.ScaleX -= 0.02f;
            View.ScaleY = View.ScaleX;

            if (View.ScaleX <= 0.02f)
            {
                Die();
            }
        }
    }
}
