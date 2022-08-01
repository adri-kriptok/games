using Kriptok.Views.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Noid.Entities
{
    /// <summary>
    /// Ladrillo rojo que se mueve.
    /// </summary>
    internal class MovingBrick : Brick
    {
        private float incrX = 1f;

        public MovingBrick(int x, int y) : base(6, x, y)
        {
        }

        protected override void OnFrame()
        {
            base.OnFrame();

            if(Location.X == 16f) incrX = 1;
            if(Location.X == 256f) incrX = -1; 
                  
            Location.X += incrX;

            // Comprueba que no choca con otros ladrillos.
            if (CollisionWithBrick())
            {
                incrX = -incrX;
                Location.X += incrX;            
            }            
        }

        private bool CollisionWithBrick()
        {
            var rect = GetRect();

            foreach (var b in Find.All<Brick>().Where(p => !p.Equals(this)))
            {
                if (b.GetRect().IntersectsWith(rect))
                {
                    return true;
                }
            }
            return false;
        }

        internal override void Hit()
        {
            base.Hit();

            incrX = 0;
        }
    }
}
