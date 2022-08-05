using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions;
using Kriptok.Entities.Queries;
using Kriptok.Entities.Queries.Base;
using Kriptok.Views.Sprites;

namespace Kriptok.Galax
{
    public class PlayerMisile : EntityBase<IndexedSpriteView>
    {
        private IQuery<bool> outOfScreen;

        /// <summary>
        /// Bandera del misil.
        /// </summary>
        public static bool Flag;

        public PlayerMisile(float x, float y) 
            : base(new IndexedSpriteView(typeof(PlayerMisile).Assembly, "Assets.Images.PlayerShot.png", 2, 1))
        {
            Location.X = x;
            Location.Y = y;
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);

            View.Graph = 0;
            h.CollisionType = Collision2DTypeEnum.Auto;
            outOfScreen = h.GetOutOfScreenQuery();

            Flag = true;
        }

        protected override void OnFrame()
        {
            if (outOfScreen.Result)
            {
                KillMisile();
                return;
            }

            Location.Y -= 8f;

            if (View.Graph == 0)
            {
                // Cambia el grafico (entre 14 y 15)
                View.Graph = 1;
            }
            else
            {
                View.Graph = 0;
            }
        }

        internal void KillMisile()
        {
            Flag = false;
            Die();
        }
    }
}

