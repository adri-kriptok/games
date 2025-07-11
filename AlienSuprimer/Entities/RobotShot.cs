using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions;
using Kriptok.Entities.Queries.Base;
using Kriptok.Views.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Games.Alien.Entities
{
    internal class RobotShot : ProcessBase<SpriteView>
    {
        private IQuery<bool?> outOfScreenQuery;

        public RobotShot(float x, float y) : base(new SpriteView(typeof(RobotShot).Assembly, "Assets.Images.RobotShot.png"))
        {
            Location.X = x;
            Location.Y = y;
            Location.Z = 10f;
        }

        protected override void OnStart(ProcessStartHandler h)
        {
            base.OnStart(h);

            h.CollisionType = Collision2DTypeEnum.Rectangle;

            outOfScreenQuery = h.GetOutOfScreenQuery();
            // collisionHelicopter = h.GetCollision2D<>
        }

        protected override void OnBegin()
        {            
            // Comprueba si el disparo ha tocado algun enemigo
            While(() => !outOfScreenQuery.Result.GetValueOrDefault(false), () =>
            {         
                // Si se toca a cualquier tipo de helicoptero sale del bucle
                // toque=collision(TYPE helicoptero1);
                // if (toque)
                // { 
                //     BREAK;
                //  } else {
                //     toque=collision(TYPE helicoptero2);
                //     if (toque) { 
                //         BREAK;
                //      } else {
                //         toque=collision(TYPE helicoptero3);
                //         if (toque) { 
                //             BREAK;
                //         }
                //     }
                // }

                // Mueve hacia arriba el disparo
                Location.Y-=12f;      
                Frame();
            });

            // // Si ha tocado algun enemigo, bajale la energ¡a
            // if (toque)
            //     toque.energ¡a-=300;
            //     impacto(x,y);   // Crea un proceso del tipo impacto
            // }
        }
    }
}
