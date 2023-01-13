using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Extensions;
using Kriptok.Helpers;
using Kriptok.Regions.Context.Base;
using Kriptok.Views;
using Kriptok.Views.Base;
using Kriptok.Views.Primitives;
using Kriptok.Views.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kriptok.Intruder.Entities.Hud.BarBase;

namespace Kriptok.Intruder.Entities.Hud
{
    internal abstract class BarBase : EntityBase<BarView>
    {
        /// <summary>
        /// Máximo valor que pueden tener las barras.
        /// </summary>
        public const float MaxValue = 2000f;

        public BarBase(LinearGradientBrush brush) : base(new BarView(brush)
        {
            Center = new PointF(0f, 0f)
        })
        {
            Location.X = 100f;
        }

        protected override void OnFrame()
        {
        }

        /// <summary>
        /// Obtiene el valor para representar en la barra.
        /// </summary>                        
        protected abstract float GetValue();

        internal class BarView : RectangleView
        {
            private readonly static Pen shadowPen = Pens.Black;
            private readonly static Brush shadowBrush = Brushes.Black;

            internal const float Width = 50f;
            internal const float Height = 6f;
            internal const float Height1 = Height - 1f;

            private const float width3 = Width + 3f;
            private const float height2 = Height + 2f;


            private readonly LinearGradientBrush brush;
            private BarBase owner;
            private float locationX1;
            private float locationY1;
            private float locationX2;
            private float locationY2;
            private float locationX3;
            private float locationY3;

            public BarView(LinearGradientBrush brush) : base(width3, height2, null, Strokes.White)
            {
                this.brush = brush;
            }

            public override void SetOwner(IViewOwner entity)
            {
                base.SetOwner(entity);
                this.owner = (BarBase)entity;
            }

            internal void PrecalculateValues()
            {
                locationX1 = owner.Location.X + 1f;
                locationY1 = owner.Location.Y + 1f;
                locationX2 = owner.Location.X + 2f;
                locationY2 = owner.Location.Y + 2f;
                locationX3 = owner.Location.X + 3f;
                locationY3 = owner.Location.Y + 3f;
            }

            public override void Render(IRenderContext context, Vector2F location, float rotation)
            {
                // Sombra del contenedor.
                context.Graphics.DrawRectangle(shadowPen, locationX1, locationY1, Rectangle.Width, Rectangle.Height);

                var val = (owner.GetValue().Clamp(0f, 1f) * Width).Floor();

                context.Graphics.FillRectangle(shadowBrush, locationX3, locationY3, val, Height1);

                context.Graphics.FillRectangle(brush, locationX2, locationY2, val, Height1);

                base.Render(context, location, rotation);
            }
        }
    }

    internal class EnergyBar : BarBase
    {
        private readonly Player player;
        private static readonly LinearGradientBrush brush
            = new LinearGradientBrush(new RectangleF(0f, 0f,
                BarView.Width, BarView.Height), ColorHelper.Green, Color.Green, 90f);

        public EnergyBar(Player player) : base(brush)
        {
            this.player = player;
            Location.Y = IntruderConsts.EnergyBarHeight;

            View.PrecalculateValues();
        }

        protected override float GetValue() => player.GetEnergy();
    }

    internal class LifeBar : BarBase
    {
        private readonly Player player;

        private static readonly LinearGradientBrush brush
          = new LinearGradientBrush(new RectangleF(0f, 0f,
              BarView.Width, BarView.Height), Color.Red, Color.DarkRed, 90f);

        public LifeBar(Player player) : base(brush)
        {
            this.player = player;
            Location.Y = IntruderConsts.LifeBarHeight;

            View.PrecalculateValues();
        }

        protected override float GetValue() => player.GetLife();
    }
}
