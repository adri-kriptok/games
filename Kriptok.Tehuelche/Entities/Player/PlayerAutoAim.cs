using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions.Queries;
using Kriptok.Extensions;
using Kriptok.Helpers;
using Kriptok.Regions.Context.Base;
using Kriptok.Regions.Full3D.Cameras;
using Kriptok.Tehuelche.Entities.Enemies;
using Kriptok.Tehuelche.Regions;
using Kriptok.Views.Shapes;
using Kriptok.Views.Shapes.Base;
using Kriptok.Views.Shapes.Vertices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace Kriptok.Tehuelche.Entities.Player
{
    internal class PlayerAutoAim : EntityBase<GdipShapeView>
    {
        private const float initialLength = 350f;
        private const float thirdPersonAngleModifier = MathHelper.QuarterPIF * 1.625f;

        private readonly PlayerHelicopter owner;
        private Blank blank;
        private IMultipleCollisionQuery<EnemyBase> enemyCollision;

        public PlayerAutoAim(PlayerHelicopter owner) : base(new PlayerAutoAimView(owner))
        {
            this.owner = owner;
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);
            h.SetCollision3DViewOBB();

            this.blank = Add(new Blank());
            this.enemyCollision = h.GetCollisions3D<EnemyBase>();
        }

        protected override void OnFrame()
        {
            if (enemyCollision.CloserCollision(out EnemyBase closer))
            {
                var distance = GetDistance3D(closer);
                View.ScaleX = distance / initialLength;
                blank.Enemy = closer;
            }
            else
            {
                View.ScaleX = 1f;
                blank.Enemy = null;
            }

            if (owner.UserFirstPersonCamera)
            {
                Angle.Y = owner.GetCameraAngle() * 0.5f - MathHelper.QuarterPIF;
                Angle.Z = owner.CameraAngle;
            }
            else
            {
                Angle.Y = owner.Angle.Y - thirdPersonAngleModifier - owner.GetCameraAngle() * 0.125f;
                Angle.Z = owner.Angle.Z;
            }
        }

        public override Vector3F GetRenderLocation()
        {
            var ownerLocation = owner.GetRenderLocation();

            if (owner.UserFirstPersonCamera)
            {
                ownerLocation.Z += 30f;
            }

            return ownerLocation;
        }

        /// <summary>
        /// Indica si tiene un enemigo en la mira.
        /// </summary>        
        internal bool LockedOnEnemy(out EnemyBase enemy)
        {
            enemy = blank.Enemy;
            return enemy != null;
        }

        private class PlayerAutoAimView : GdipShapeView
        {
            //#if DEBUG || SHOWFPS
            //            private static readonly IMaterial transparenteMaterial = Material.Get(ColorHelper.Green.SetAlpha(128));
            //#endif
            private readonly PlayerHelicopter player;

            public PlayerAutoAimView(PlayerHelicopter player) : base(new VertexBase[3]
            {
                new Vertex3(0f, 0f, 0f),
                new Vertex3(0f, 0f, initialLength),
                new Vertex3(0f, -initialLength, 0f)
            }, shapes =>
            {
                var verts = shapes.GetVertices().ToArray();
                shapes.Add(new InvisibleFace(verts[0], verts[1], verts[2]));
                //#if DEBUG || SHOWFPS
                //                shapes.Add(transparenteMaterial, 0, 1, 2);
                //                shapes.Add(transparenteMaterial, 0, 2, 1);
                //#endif
            })
            {
                this.player = player;
            }

#if DEBUG || SHOWFPS
            public override bool IsVisible3D(IRenderContext3D context)
            {
                if (player.UserFirstPersonCamera)
                {
                    return false;
                }
                return base.IsVisible3D(context);
            }
#else
            public override bool IsVisible3D(IRenderContext3D context) => false;                            
#endif

            /// <summary>
            /// Vista invisible para que parezca que la mira se ajusta automáticamente.
            /// Aunque en realidad se chequea la colisión contra la figura.
            /// </summary>
            private class InvisibleFace : GdipFace3DBase
            {
                public InvisibleFace(VertexBase v0, VertexBase v1, VertexBase v2)
                    : base(v0, v1, v2)
                {
                }

                public override bool IsVisible2D(IRenderContext2D context) => false;
                public override bool IsVisible3D(IRenderContext3D context) => false;

                public override IShape Clone(IDictionary<VertexBase, VertexBase> clonedPoints) => throw new NotImplementedException();

                public override void Render2D(IRenderContext2D context) => throw new NotImplementedException();

                public override void Render3D(IRenderContext3D context) => throw new NotImplementedException();
            }
        }
    }
}
