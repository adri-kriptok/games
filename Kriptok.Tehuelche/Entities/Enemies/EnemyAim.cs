using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions.Queries;
using Kriptok.Regions.Context.Base;
using Kriptok.Views.Shapes;
using Kriptok.Views.Shapes.Base;
using Kriptok.Views.Shapes.Vertices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kriptok.Tehuelche.Entities.Enemies
{
    internal class EnemyAim : EntityBase<GdipShapeView>
    {
        private readonly EnemyBase owner;
        private ISingleCollisionQuery<PlayerHelicopter> playerCollision;

        public EnemyAim(EnemyBase owner) : base(new EnemyAimView(owner))
        {
            this.owner = owner;
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);

            h.SetCollision3DViewOBB();
            this.playerCollision = h.GetCollision3D<PlayerHelicopter>();
        }

        /// <summary>
        /// Indica si está apuntando al jugador.
        /// </summary>
        public bool Target { get; private set; }

        protected override void OnFrame()
        {
            if (!owner.IsAlive())
            {
                Die();
                return;
            }
            
            if (playerCollision.OnCollision())
            {
                Target = true;
            }
            else
            {
                Target = false;
            }

            //Angle = owner.GetAimAngle();
        }

        private class EnemyAimView : GdipShapeView
        {            
            public EnemyAimView(EnemyBase owner) : base(new VertexBase[2]
            {
                new Vertex3(0f, 0f, 0f),
                new Vertex3(0f, 0f, 512f)
            }, shapes =>
            {
                var verts = shapes.GetVertices().ToArray();

                // shapes.Add(Strokes.Red, 0, 1);
                shapes.Add(new InvisibleLine(verts[0], verts[1]));

                //shapes.Add(new InvisibleFace(verts[0], verts[1], verts[2]));
                //#if DEBUG || SHOWFPS
                //                shapes.Add(transparenteMaterial, 0, 1, 2);
                //                shapes.Add(transparenteMaterial, 0, 2, 1);
                //#endif
            })
            {
                // this.owner = owner;
            }

            public override bool IsVisible3D(IRenderContext3D context) => false;                            

            public override float GetPriority()
            {
                return base.GetPriority() - 100f;
            }

            /// <summary>
            /// Vista invisible para que parezca que la mira se ajusta automáticamente.
            /// Aunque en realidad se chequea la colisión contra la figura.
            /// </summary>
            private class InvisibleLine : Line3DBase
            {
                public InvisibleLine(VertexBase v0, VertexBase v1)
                    : base(v0, v1)
                {
                }

                // public override bool IsVisible2D(IRenderContext2D context) => false;
                // public override bool IsVisible3D(IRenderContext3D context) => false;

                protected override void RenderLine(IRenderContext context, Vector2F p0, Vector2F p1)
                {
                }
            }
        }
    }
}
