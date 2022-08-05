using Kriptok.Helpers;
using Kriptok.Entities.Base;

namespace Asteroids.Entities
{
    /// <summary>
    /// Nave pequeña utilizada para representar las vidas del jugador.
    /// </summary>
    class Live : EntityBase<ShipView>
    {
        public Live(float x) : base(new ShipView())
        {
            Location.X = x;
            Location.Y = 16f;
            View.ScaleX = 0.75f;
            View.ScaleY = 0.75f;
        }

        protected override void OnFrame()
        {
            Angle.Z += MathHelper.PIF / 64;
        }
    }
}
