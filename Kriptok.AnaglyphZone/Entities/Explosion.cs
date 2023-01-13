using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Helpers;
using Kriptok.Regions.Context.Base;
using Kriptok.Views;
using Kriptok.Views.Gdip;
using Kriptok.Views.Shapes;
using Kriptok.Views.Shapes.Base;
using Kriptok.Views.Shapes.Vertices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Kriptok.AZ.Entities.Explosion;

namespace Kriptok.AZ.Entities
{
    internal class Explosion2 : EntityBase
    {        
        public Explosion2(Vector3F location)
        {
            Location = location;
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);

            Audio.PlayWave(Assembly, "Assets.Sounds.Explosion1.wav");
        }

        protected override void OnFrame()
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    for (int k = -1; k < 2; k++)
                    {
                        Add(new Explosion(new Vector3F(Location.X + i, Location.Y + j, Location.Z + k), 2));
                    }
                }
            }

            Die();
        }
    }

    internal class Explosion : EntityBase<ExplosionView>
    {
        private readonly float incSize;
        private readonly float maxScale;

        public Explosion(Vector3F location) : this(location, 1)
        {
        }

        public Explosion(Vector3F location, float scale) : base(new ExplosionView(scale)
        {
            Scale = new Vector3F(10f * scale)
        })
        {
            incSize = 1f + (0.25f / scale);
            maxScale = 200 * scale;

            Angle.X = MathHelper.CosF(location.X);
            Angle.Y = MathHelper.CosF(location.Y);
            Angle.Z = MathHelper.CosF(location.Z);
            Location = location;
        }

        protected override void OnFrame()
        {
            View.ScaleX *= incSize;
            View.ScaleY = View.ScaleX;
            View.ScaleZ = View.ScaleX;

            if (View.ScaleX > maxScale)
            {
                Die();
            }
        }

        internal class ExplosionView : GdipShapeView
        {            
            public ExplosionView(float scale) : this(new VertexBase[2]
            {
                new Vertex3(-1f, 0f, 0f),
                new Vertex3(1f, 0f, 0f)
            }, scale * scale)
            {
            }

            public ExplosionView(VertexBase[] vs, float scale) : base(vs, shapes =>
            {
                foreach (var v in vs)
                {
                    shapes.Add(new ExplosionParticle(v, scale));
                }
            })
            {
            }

            private class ExplosionParticle : Particle<GdipRectangle>
            {
                public ExplosionParticle(VertexBase vertex, float scale) 
                    : base(new GdipRectangle(5, 5, AZConsts.Black, Strokes.Fuchsia), vertex)
                {
                    Angle = vertex.GetLocation().Y;
                    ScaleX = scale;
                    ScaleY = scale;
                }

                protected override void OnRendering(IRenderContext context, GdipRectangle view)
                {
                    base.OnRendering(context, view);
                    Angle += 0.1f;
                }
            }
        }
    }
}
