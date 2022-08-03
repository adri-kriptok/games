using Kriptok.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Snake.Entities
{
    class AppleCreator : EntityBase
    {
        protected override void OnFrame()
        {
            // Aleatoriamente se elige si se pone una manzana
            // o no y siempre si hay menos de 3 manzanas
            if (Global.Apples < 3 && Rand.Next(0, 32) == 0)
            {
                // Pone una manzana e incrementa el contador de las mismas
                var apple = Add(new Apple(Rand.Next(2, 38) * SnakeHead.Size + SnakeHead.HalfSize,
                    Rand.Next(4, 23) * SnakeHead.Size + SnakeHead.HalfSize));

                if (apple.IsAlive())
                {
                    Global.Apples++;
                }
            }
        }
    }
}
