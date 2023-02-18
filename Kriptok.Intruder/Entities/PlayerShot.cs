using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions.Queries;
using Kriptok.Entities.Queries.Base;
using Kriptok.Intruder.Entities.Enemies;
using Kriptok.Regions.Partitioned;
using Kriptok.Views.Base;
using Kriptok.Views.Shapes;
using Kriptok.Views.Shapes.Base;
using Kriptok.Views.Shapes.Vertices;
using System;

namespace Kriptok.Intruder.Entities
{
    partial class Player
    {
#if DEBUG
        private class PlayerShot : EntityBase<GdipShapeView>
#else
        private class PlayerShot : EntityBase
#endif
        {            
            private readonly IPartitionedPseudo3DRegion map;
            private IMultipleCollisionQuery<IEnemy> enemyCollisions;

            public PlayerShot(Player player, IPartitionedPseudo3DRegion map) 
#if DEBUG
                : base(new SegmentView())
#endif
            {
                this.map = map;
                // this.player = player;
                Angle = player.Angle;
                Location = player.Location;
                Location.Z = Location.Z + player.GetHeight();
            }

            protected override void OnStart(EntityStartHandler h)
            {
                base.OnStart(h);

                // Le establezo el tipo de colisión que debe usar.
                h.SetHitScanCollision(map);                

                // Y le indico que busque colisiones con enemigos.
                this.enemyCollisions = h.GetCollisions3D<IEnemy>();
            }

            protected override void OnFrame()
            {
                if (enemyCollisions.CloserCollision(out IEnemy enemy))
                {
#if DEBUG || SHOWFPS
                    enemy.Pointed();
#endif
                    enemy.Hit(Rand.Next(20, 30));
                }

                // Esta entidad sólo existe para chequear colisiones.
                // Luego muere.
                Die();
            }
        }

#if DEBUG
        private class SegmentView : GdipShapeView
        {
            public SegmentView() : base(new VertexBase[2]
            {
                new Vertex3(0f, 0f, 0f),
                new Vertex3(0f, 0f, 1000f),
            }, shapes =>
            {
                shapes.Add(Strokes.Blue, 0, 1);
            })
            {
                

                //Scale = new Vector3F(100);
            }
            // public SegmentView() : base(Material.Blue)
            // {
            //     Scale = new Vector3F(100);
            // }
        }   // 
#endif
    }
}
