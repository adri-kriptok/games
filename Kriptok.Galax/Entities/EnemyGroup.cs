using Kriptok.Entities.Base;
using Galax.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galax.Entities
{
    /// <summary>
    /// Controlador de la flota de enemigos.
    /// </summary>
    class EnemyGroup : EntityBase
    {
        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);

            Global.SquadCurrentX = Consts.SquadStartX;
        }

        protected override void OnFrame()
        {
            // Bucle de nivel                    
            if (Global.MovementDirection == 0)
            {
                // Mueve el escuadron segun la direccion
                Global.SquadCurrentX -= Consts.SquadronSpeed;
            }
            else
            {
                Global.SquadCurrentX += Consts.SquadronSpeed;
            }

            if (Global.ChangeSquadDirectionFlag)
            {
                // Cambia la direccion
                Global.MovementDirection = Global.MovementDirection ^ 1;
                Global.ChangeSquadDirectionFlag = false;
            }

            if (Find.All<EnemyBase>().Count() == 0 &&
                Find.All<EnemyShotBase>().Count() == 0 &&
                Find.All<Explosion>().Count() == 0)
            {
                Scene.SendMessage(LevelMessagesEnum.NextLevel);
                Die();
                return;
            }

            if (Input.Escape())
            {
                // Aborta el juego.                                             
                Global.Lives = 0;
                Scene.SendMessage(LevelMessagesEnum.LoseLife);
                Die();
            }        
        }
    }
}
