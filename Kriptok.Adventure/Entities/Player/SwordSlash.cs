using Kriptok.Adventure.Entities.Base;
using Kriptok.Adventure.Entities.Monsters;
using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions;
using Kriptok.Extensions;
using Kriptok.Helpers;
using Kriptok.Regions.Context.Base;
using Kriptok.Views.Base;
using Kriptok.Views.Primitives;
using Kriptok.Views.Sprites;
using System;
using System.Collections.Concurrent;
using System.Drawing;

namespace Kriptok.Adventure.Entities.Player
{
    internal class SwordSlash : EntityBase<SwordSlash.SwordSlashView>
    {
        /// <summary>
        /// Cantidad de pixels que "empuja" el espadazo.
        /// </summary>
        private const float StandarSlashPush = 30f;

        private static readonly int[] anim = new int[] { 0, 1, 2, 3, 3 };
        private readonly LinaBase owner;
        private bool visible;
        private bool triggerEvent = false;

        public SwordSlash(LinaBase lina) : base(new SwordSlash.SwordSlashView())
        {
            this.owner = lina;

            this.Radius = 24;
        }

        protected override void OnFrame()
        {
            const float pi4 = 4f / MathHelper.PIF;
            const float pi1_5 = MathHelper.QuarterPIF * 1.5f;

            Angle = owner.Angle;
            Location = owner.Location;
            
            if (triggerEvent)
            {
                var loc = owner.Location.XY();

                // Obtengo el vector de la dirección en la que está mirando el jugador.
                var dirVector = PolarVector.NewVector(
                    (((owner.Angle.Z * pi4).Round()) >> 1) * MathHelper.HalfPIF, 1f);
                foreach (var e in Radius2DCollisions<ISlashable>())
                {
                    // Obtengo el vector desde el jugador hacia el enemigo.
                    var push = e.GetLocation2D().Minus(loc);

                    // Y me fijo si está dentro de un arco de 3pi / 8 
                    if (Vector2F.AngleBetween(dirVector, push) < pi1_5)
                    {                   
                        e.Slash(push.Normalized().Scale(StandarSlashPush));
                    }
                }
            }
        }

        internal void Hide() => visible = false;

        internal void Show() => visible = true;

        internal void Update(int g, bool trigger)
        {
            View.Graph = anim[g];
            triggerEvent = trigger;
        }

        internal class SwordSlashView : DirectionalSpriteView
        {
            private static readonly int[,] matrix = new int[,]
            {
                { 00, 01, 02, 03 },
                { 04, 05, 06, 07 },
                { 04, 05, 06, 07 },
                { 04, 05, 06, 07 },
                { 08, 09, 10, 11 },
                { 12, 13, 14, 15 },
                { 12, 13, 14, 15 },
                { 12, 13, 14, 15 },
            };

            private static readonly PointF p0 = new PointF(11f / 24f, (26f + 02f) / 32f);
            private static readonly PointF p1 = new PointF(00f / 24f, (26f + 00f) / 32f);
            private static readonly PointF p2 = new PointF(12f / 24f, (26f - 16f) / 32f);
            private static readonly PointF p3 = new PointF(24f / 24f, (26f + 00f) / 32f);
            private SwordSlash owner;

            public SwordSlashView() : base(typeof(SwordSlashView).Assembly, "Lina.Sword0.png", 4, 4, matrix)
            {
                SetCenter(00, p0); SetCenter(01, p0); SetCenter(02, p0); SetCenter(03, p0);
                SetCenter(04, p1); SetCenter(05, p1); SetCenter(06, p1); SetCenter(07, p1);
                SetCenter(08, p2); SetCenter(09, p2); SetCenter(10, p2); SetCenter(11, p2);
                SetCenter(12, p3); SetCenter(13, p3); SetCenter(14, p3); SetCenter(15, p3);
            }

            public override void SetOwner(IViewOwner obj)
            {
                base.SetOwner(obj);
                this.owner = (SwordSlash)obj;
            }

            public override void RenderOn(IRenderContext context)
            {
                if (owner.visible)
                {
                    base.RenderOn(context);
                }
            }

            protected override float GetPriority(IProjector context, int xGraph)
            {
                if (owner.visible)
                {
                    if (xGraph == 0)
                    {
                        return base.GetPriority(context, xGraph) - 0.01f;
                    }
                    else
                    {
                        return base.GetPriority(context, xGraph) + 0.01f;
                    }
                }
                return 0f;
            }
        }

        // private class SlashHitbox : EntityBase//<PolygonView>
        // {            
        //     private bool ready = false;
        // 
        //     public SlashHitbox(SwordSlash owner) //: base(new SlashHitboxView())
        //     {                
        //         Location = owner.Location;
        //         Angle.X = owner.Angle.Z;
        //     }
        // 
        //     protected override void OnStart(EntityStartHandler h)
        //     {
        //         base.OnStart(h);                
        //     }
        // 
        //     protected override void OnFrame()
        //     {
        //         if (ready)
        //         {
        //             Die();
        //         }
        //         else
        //         {
        //             ready = true;
        //         }
        //     }
        // 
        //     //private class SlashHitboxView : PolygonView
        //     //{
        //     //    private static readonly PointF[] vertices = new PointF[]
        //     //    {
        //     //        new PointF(0f, 1f),
        //     //        new PointF(1f, 0f),
        //     //        new PointF((float)Math.Sqrt(2), 1f),
        //     //        new PointF(1f, 2f),
        //     //    };
        //     //
        //     //    public SlashHitboxView() : base(vertices, Color.Red)
        //     //    {
        //     //        Center = new PointF(0f, 0.5f);
        //     //        Scale = new PointF(24f, 24f);
        //     //    }
        //     //}
        // }
    }
}
