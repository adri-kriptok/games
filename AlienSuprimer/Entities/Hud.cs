using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Extensions;
using Kriptok.Regions.Context.Base;
using Kriptok.Views;
using Kriptok.Views.Sprites;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Kriptok.Games.Alien.Entities.EnergyBar;

namespace Kriptok.Games.Alien.Entities
{
    internal class Life : EntityBase<SpriteView>
    {
        public Life() : base(new SpriteView(typeof(Robot).Assembly, "Assets.Images.Robot.png", 0, 0, 41, 28))
        {
            // Y las coordenadas del mismo            
            Location.X = 31f;
            Location.Y = 22f;

            // Grafico mas pequenio
            View.ScaleX = 1.4f;                
            View.ScaleY = 1.4f;                
        }

        protected override void OnFrame()
        {
        }
    }

    internal class EnergyBar : EntityBase<EnergyBarView>
    {
        private const int initialWide = 120;
        private const float maxBars = 30f;
        private const float divisor = maxBars / Global.Consts.MaxInitialHealth;

        public EnergyBar() : base(new EnergyBarView())
        {
            Location.X = 360;
            Location.Y = 20;
            Location.Z = -100;
        }

        protected override void OnFrame()
        {
            var wide = ((int)((Global.energia_robot * divisor).Clamp(0f, maxBars))) * 8;

            View.SetRectangle(new Rectangle(239, 0, wide + 1, 40));
        }

        internal class EnergyBarView : ClippedViewBase<SpriteView>
        {
            private Rectangle rectangle = new Rectangle(239, 0, initialWide + 1, 40);

            public EnergyBarView() : base(new SpriteView(typeof(EnergyBarView).Assembly, "Assets.Images.EnergyBar.png"))
            {
                View.ScaleX = 2f;
                View.ScaleY = 2f;
            }

            protected override void Clip(Graphics g) => g.SetClip(rectangle);

            internal void SetRectangle(Rectangle rectangle) => this.rectangle = rectangle;
        }
    }

    internal class Missiles : EntityBase<SpriteView>
    {
        public Missiles() : base(new SpriteMultipleView())
        {
            // Y las coordenadas del mismo
            Location.X = 65f * 2f;
            Location.Y = 17f + 5f;

            // Grafico mas pequenio
            View.ScaleX = 2f;
            View.ScaleY = 2f;
        }

        protected override void OnFrame()
        {
        }

        public class SpriteMultipleView : SpriteView
        {
            public SpriteMultipleView() : base(typeof(Robot).Assembly, "Assets.Images.HudMissile.png")
            {
            }

            public override void Render(IRenderContext context, Vector2F location, float rotation)
            {
                for (int i = 0; i < Global.contador_misiles; i++) 
                {
                    base.Render(context, location.Plus(i * 14f, 0f), rotation);
                }
            }
        }
    }
}
