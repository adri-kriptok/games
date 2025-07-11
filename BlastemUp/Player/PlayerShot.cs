using Kriptok.Core;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions;
using Kriptok.Entities.Queries;
using Kriptok.Entities.Queries.Base;
using Kriptok.Views.Sprites;

namespace Kriptok.Games.BlastemUp.Player
{
    public class PlayerShot : EntityBase<SpriteView>
    {
        private readonly float angle;

        /// <summary>
        /// Consulta para saber si salió de la pantalla.
        /// </summary>
        private IQuery<bool?> outOfScreen;

        public PlayerShot(float x, float y, float angle)
            : base(new SpriteView(typeof(PlayerShot).Assembly, "Shots.PlayerShot.png"))
        {
            Location.X = x + 12;
            Location.Y = y;
            this.angle = angle;

            Location.Z = 1;
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);
            h.CollisionType = Collision2DTypeEnum.Rectangle;
            this.outOfScreen = h.GetOutOfScreenQuery();
        }

        protected override void OnFrame()
        {
            if (outOfScreen.Result.GetValueOrDefault(false))
            {
                Die();
                return;
            }

            XAdvance2D(36f, angle);
        }
    }
}
