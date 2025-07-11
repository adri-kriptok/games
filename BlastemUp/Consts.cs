using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Games.BlastemUp
{
    public static class Consts
    {
        /// <summary>
        /// Nivel inicial.
        /// </summary>
        public const int StartLevel = 0;

        /// <summary>
        /// Vidas con las que inicia el jugador.
        /// </summary>
        public const int InitialLives = 3;

        /// <summary>
        /// Nivel inicial de poder de fuego del jugador.
        /// </summary>
        public static int StartingShootLevel = 0;

        /// <summary>
        /// Velocidad normal de movimiento.
        /// </summary>
        public const float ScrollDefaultSpeed = 5f;

        public const float SpeedModifier = 1f; // 1f / 3f;
        public const float SpeedModifier2 = SpeedModifier * SpeedModifier;

        public const int MaxShotType=4;      // Numero de disparo maximos
        public const int MaxLivesOnScreen=3;        // Maximo numero de vidas
        public const int max_ani=5;          // Numero de animaciones maximas
        public const int max_trayec=10;      // Numero maximos de trayectorias

        public const int PlayRegionMinY = 41;
        public const int PlayRegionHeight = 398;        
        public const int MinY = 66 - PlayRegionMinY;
        public const int MaxY = 414 - PlayRegionMinY;

    }
}
