﻿using Kriptok.Entities.Collisions;
using Kriptok.Entities.Base;
using Kriptok.Views.Primitives;
using System.Drawing;
using Kriptok.Views.Sprites;
using Kriptok.Entities.Collisions.Base;
using Kriptok.Entities.Collisions.Queries;

namespace Snake.Entities
{
    /// <summary>    
    /// Maneja los gráficos de las manzanas.
    /// </summary>
    class Apple : EntityBase
    {
        private ISingleCollisionQuery<SnakeSegment> segmentColl;

        public Apple(int x, int y) : base(new SpriteView(typeof(SnakeHead).Assembly, "Green.png"))
        {
            Location.X = x;
            Location.Y = y;            
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);

            h.CollisionType = Collision2DTypeEnum.Radius;
            segmentColl = h.GetCollision2D<SnakeSegment>();
        }

        protected override void OnFrame()
        {
            // Si apenas se crea está en collisión con un segmento, directamente lo elimino.
            if (segmentColl.OnCollision())
            {
                Die();
                Global.Apples--;          // Decrementa el contador de manzanas
            }
        }
    }
}
