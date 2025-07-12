using Kriptok.JazzJackRabbit.Entities;
using Kriptok.Entities.Base;
using Kriptok.Views.Primitives;
using Kriptok.Views.Sprites;
using System.Drawing;

namespace Kriptok.JazzJackRabbit.Entities
{
    internal class Hud : EntityBase<SpriteView>
    {
        private Jazz player;

        public Hud(Jazz player, float y) : base(new SpriteView(typeof(Hud).Assembly, "Assets.Hud.png")
        {
            Center = new PointF(0f, 0f)
        })
        {
            this.player = player;

            Location.Y = y;
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);

            Add(new LifeBar());
        }

        protected override void OnFrame()
        {          
        }


        internal class LifeBar : EntityBase<RectangleView>
        {
            public LifeBar() : base(new RectangleView(63, 7, Color.FromArgb(84, 8, 220))
            {
                Center = new PointF(0f, 0f)
            })
            {
                // View.ScaleX = 1f / 64f * 0.5f;
            }

            protected override void OnStart(EntityStartHandler h)
            {
                base.OnStart(h);

                Location.X = 20f;
                Location.Y = h.RegionSize.Height - 12;                
            }

            protected override void OnFrame()
            {                
            }
        }
    }
}