using Kriptok.Asteridian.Entities.Enemies.Base;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions;
using Kriptok.Helpers;
using Kriptok.Views;
using Kriptok.Views.Primitives;
using Kriptok.Views.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;

namespace Kriptok.Asteridian.Entities.Enemies
{
    internal class Asteroid : EnemyBase
    {
        private readonly float rot;

        public Asteroid(float x) : base(new AsteroidView())
        {
            Location.X = x;
            rot = Rand.NextF() * 0.1f;
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);
            h.CollisionType = Collision2DTypeEnum.Auto;           
        }

        protected override void OnFrame()
        {
            base.OnFrame();

            Angle.Z += rot;
        }

        internal override void StartOnTop(float y)
        {            
            var height = ((AsteroidView)View).Height;
            
            Location.Y = y + height * ((AsteroidView)View).Center.Y - height;
        }

        private class AsteroidView : PolygonView
        {            
            public AsteroidView() : base(GetPoints(3), new FillConfig(Color.DarkSlateGray), Strokes.Get(Color.Gray))
            {
                Rounded = true;
            }

            private static PointF[] GetPoints(int graph)
            {
                var list = new List<PointF>();

                int len = (6 - graph);
                var vertices = 15;

                var angle = 0f;
                var angleInc = MathHelper.TwoPIF / vertices;

                for (int i = 0; i < vertices; i++, angle += angleInc)
                {
                    var lenF = 5f * len - 2.5f + Rand.NextF(0, 7.5f);

                    list.Add(new PointF()
                    {
                        X = ((float)Math.Cos(angle)) * lenF,
                        Y = ((float)Math.Sin(angle)) * lenF,
                    });
                }

                var arr = list.ToArray();
                var avg = PointFHelper.GetRectangleF(arr);
                
                for (int i = 0; i < arr.Length; i++)
                {
                    arr[i] = new PointF()
                    {
                        X = arr[i].X - avg.X,
                        Y = arr[i].Y - avg.Y
                    };
                }

                return arr;
            }
        }
    }
}
