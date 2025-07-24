using Kriptok.Asteridian.Entities.Enemies.Base;
using Kriptok.Asteridian.Helpers;
using Kriptok.Drawing.Algebra;
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
        private const float defaultHealth = 35f;
        private readonly float rot;
        private readonly Vector2F speed;
        private readonly int size;

        public Asteroid(float x, float speed) : this(x, speed, 3)
        {
        }

        private Asteroid(float x, float speed, int size) : base(defaultHealth * 3f / size, new AsteroidView(size))
        {
            this.size = size;
            Location.X = x;
            rot = (Rand.NextF() - 0.5f) * 0.1f;
            this.speed = new Vector2F(0f, speed);
        }

        private Asteroid(Vector3F location, Vector2F speed, float rotation, int size) : base(defaultHealth * 3f / size, new AsteroidView(size))
        {
            this.size = size;
            Location = location;
            rot = rotation;
            this.speed = speed;
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
            Location.X += speed.X * Sys.TimeDelta;
            Location.Y += speed.Y * Sys.TimeDelta;
        }

        internal override void StartOnTop(float y)
        {            
            var height = ((AsteroidView)View).Height;
            
            Location.Y = y + height * ((AsteroidView)View).Center.Y - height;
        }

        protected override void OnDying()
        {
            base.OnDying();

            if (size < 5)
            {                
                var mod = 0.15f / size;
                var v0 = new Vector2F(speed.X - mod, speed.Y + 0.025f);
                var v1 = new Vector2F(speed.X + mod, speed.Y + 0.025f);
                Add(new Asteroid(Location, v0, rot * +2f, size + 1));
                Add(new Asteroid(Location, v1, rot * -2f, size + 1));
            }
        }

        private class AsteroidView : PolygonView
        {            
            public AsteroidView(int size) : base(GetPoints(size), new FillConfig(Color.DarkSlateGray), Strokes.Get(Color.Gray))
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

                return ViewHelper.Translate(list);                
            }
        }
    }
}
