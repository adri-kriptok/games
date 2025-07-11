using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Views.Sprites;

namespace Kriptok.Games.Alien.Entities
{
    internal class PlayerMissile : EntityBase<SpriteView>
    {
        private int movementCounter = 10;

        public PlayerMissile(Vector3F location) : base(new SpriteView(typeof(PlayerMissile).Assembly, "Assets.Images.PlayerMissile.png"))
        {
            Location = location;
            Location.Z += 1f;
        }

        protected override void OnFrame()
        {
            if (movementCounter <= 0)
            {
                Add(new Explosion3(Location, 1));
                Die();
            }
            else
            {
                Location.Y -= 12f;
                movementCounter--;
            }
        }
    }
}