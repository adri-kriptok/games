using Kriptok.Drawing;
using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Extensions;
using Kriptok.Helpers;
using Kriptok.Intruder.Entities;
using Kriptok.Intruder.Entities.Effects;
using Kriptok.Regions.Context.Base;
using Kriptok.Regions.Pseudo3D.Partitioned.Terrain;
using Kriptok.Views;
using Kriptok.Views.Primitives;
using Kriptok.Views.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Intruder.Scenes.Maps.Map01_TheBeach
{
    internal class WaterfallEffect : EntityBase
    {
        private readonly Vector3F middle;
        private readonly Vector3F dir1;
        private readonly Vector3F dir2;

        private readonly Player player;

        public WaterfallEffect(Player player, M9Vertex[] vertices)
        {
            this.player = player;
            var v0 = vertices[0].GetLocation();
            var v1 = vertices[1].GetLocation();
            var v2 = vertices[2].GetLocation();

            // Necesito encontrar cuál es el del medio.
            var avg = Vector3F.Average(v0, v1, v2);
            var d0 = Vector3F.GetDistance(v0, avg);
            var d1 = Vector3F.GetDistance(v1, avg);
            var d2 = Vector3F.GetDistance(v2, avg);

            if (d0 > d1) StructHelper.Swap(ref v0, ref v1);                            
            if (d0 > d2) StructHelper.Swap(ref v0, ref v2);

            avg.Y += 100f;
            v1.Y += 100f;
            v2.Y += 100f;

            this.middle = avg;            
            this.dir1 = v1.Minus(middle);
            this.dir2 = v2.Minus(middle);
        }

        protected override void OnFrame()
        {
            if (GetDistance2D(player) > 300000f)
            {
                return;
            }
            // counterToNextBubble += Sys.TimeDelta;

            // if (counterToNextBubble > 20f)
            // {
                //AddBubble();
                AddBubble();
                //counterToNextBubble = 0f;
            ///}
        }

        private void AddBubble()
        {
            var distanceToMiddle = Rand.NextF(-0.95f, 0.95f);

            var newLoc = middle
                .Plus(dir1.Scale(Math.Min(distanceToMiddle, 0f).Abs()))
                .Plus(dir2.Scale(Math.Max(distanceToMiddle, 0f).Abs()));

            Add(new WaterfallBubble(newLoc));
        }

        private class WaterfallBubble : EntityBase<HalfEllipseView>
        {
            private readonly Color baseColor;
            private int counter = 0;

            public WaterfallBubble(Vector3F location) 
                : this(location, GetRandomColor())
            {                
            }

            public WaterfallBubble(Vector3F location, Color color)
                : base(new HalfEllipseView(color)
                {
                    Center = new PointF(0.5f, 0.99f),
                    ScaleX = 5f,
                    ScaleY = 5f
                })
            {
                Location = location;
                this.baseColor = color;
            }

            private static Color GetRandomColor()
            {
                var r = byte.MaxValue - Rand.Next(1, 64);
                var g = byte.MaxValue - Rand.Next(1, 32);
                return Color.FromArgb(r, g, byte.MaxValue);
            }

            protected override void OnFrame()
            {
                var newAlpha = 255f - counter;
                if (newAlpha <= 0f)
                {
                    Die();
                    return;
                }

                counter += 8;
                
                View.ScaleX *= 1.065f;

                // if (View.ScaleX > 30f)
                // {
                //     Die();
                //     return;
                // }

                View.ScaleY = View.ScaleX;

                View.FillColor = baseColor.SetAlpha(newAlpha.ClampToByte());
            }            
        }

        private class HalfEllipseView : PathView
        {
            static readonly GraphicsPath path = InitPath();

            private static GraphicsPath InitPath()
            {
                var gp = new GraphicsPath();
                gp.AddPie(new Rectangle(0, 0, 100, 100), 180, 180);                
                return gp;
            }

            public HalfEllipseView(Color color) 
                : base(path, color)
            {                
            }

            public override void Render(IRenderContext context, Vector2F location, float rotation)
            {
                base.Render(context, location, rotation);
            }
        }
    }
}
