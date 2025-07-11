using Kriptok.Games.BlastemUp.Player;
using Kriptok.Core;
using Kriptok.Entities.Base;
using System;

namespace Kriptok.Games.BlastemUp.Enemies
{
    public class EnemyGroup : ProcessBase
    {
        private int n_enemigos_g;           // Numero de enemigos en el grupo
        private int contador_enemigos = 0;    // Contador de enemigos en el grupo
        private int pausa_contador = 1;       // Contador de tiempo de espera entre enemigos
        private int tipo_enem = 0;            // Tipo de enemigo
        private int trayectoria1;           // Trayectoria
        private int pausa_enemigo;          // Pausa entre enemigo
        private int agresividad = 0;          // Agresividad

        /// <summary>
        /// Número de grupo.
        /// </summary>
        private readonly int groupId;

        public EnemyGroup(int n_grupo)
        {
            this.groupId = Math.Min(n_grupo,Global.vivos.Length -1);
        }

        protected override void OnBegin()
        {
            Global.grupo_pantalla++;       // Este es de otro grupo
            Global.vivos[groupId] = 0;       // Numero de enemigos vivos en el grupo
            Global.no_bonus[groupId] = 0;    // Pon bonus para el grupo
            if (Global.CurrentGroup == Global.LastGroup)     // Enemigo final
            {
                tipo_enem = 5;
                trayectoria1 = 0;
                n_enemigos_g = 0;
                agresividad = 30;
            }
            else
            {
                // Numero de enemigos
                n_enemigos_g = Rand.Next(2, 5);

                // 5 es el enemigo final
                tipo_enem = Rand.Next(0, 4);          

                // Cuando el enemigo es una mina.
                if (tipo_enem == 4)               
                {                    
                    // Haz mas minas
                    n_enemigos_g += 3;            
                }
                else
                {
                    trayectoria1 = Rand.Next(2, Consts.max_trayec);    // Pon la trayectoria
                    
                    // Pone la agresividad
                    agresividad = Rand.Next(0, Math.Max(5 - Global.grupo_pantalla, 1)) + Global.CurrentLevel;
                }

                pausa_enemigo = Rand.Next(5, 15);
            }

            Repeat(() =>
            {
                pausa_contador--;               // Bucle de retardo
                if (pausa_contador <= 0)
                {
                    // Crea un enemigo
                    Add(EnemyBase.New(tipo_enem, trayectoria1, agresividad, groupId));
                    pausa_contador = pausa_enemigo * 3;
                    contador_enemigos++;
                }

                Frame();
            }, () => contador_enemigos > n_enemigos_g);
            
            // Repite hasta que cree todos los enemigos necesarios
            Repeat(() =>
            {
                Frame();
            }, () => Global.vivos[groupId] == 0);       // Espera al ultimo enemigo

            Frame();

            if (IsAlive())
            {
                if (Global.no_bonus[groupId] == 0)       // Crea los bonus
                {
                    if (Rand.Next(0, 2) == 0)
                    {
                        Add(new Bonus(Global.bonus_x, Global.bonus_y, 7));
                    }
                    else
                    {
                        Add(new Bonus(Global.bonus_x, Global.bonus_y, Rand.Next(1, 7)));
                    }
                }

                // Cambia de grupo.
                Global.grupo_pantalla--;
            }
        }
    }
}
