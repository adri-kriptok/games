using Kriptok.Games.Alien.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Games.Alien
{
    public static class Global
    {
        public static class Consts
        {
#if DEBUG
            public const int InitialLives = 999;
#else
            public const int InitialLives = 3;
#endif

            public const int MaxInitialHealth = 8000;

            public const int InitialRobotLocationY = 2700 + 170;//2870;
            public const int InitialCameraLocationY = 2700;
        }

        /// <summary>
        /// Cantidad de vidas actuales.
        /// </summary>
        public static int Lives = Consts.InitialLives;

        /// <summary>
        /// Identificador al proceso de piernas del robot
        /// </summary>
        internal static RobotLegs MainRobot = null;

        /// <summary>
        /// Si es el enemigo final entonces enemigo_f=0 
        /// </summary>
        public static int enemigo_f;
        /// <summary>
        /// Puntuacion
        /// </summary>
        public static int puntuacion;

        /// <summary>
        /// Seleccion en el menu principal
        /// </summary>
        public static int seleccion = 0;

        /// <summary>
        /// Identificador al fichero de graficos
        /// </summary>
        public static int fichero1;

        /// <summary>
        /// Identificador al fichero de graficos de menu principal
        /// </summary>
        public static int fichero2;

        /// <summary>
        /// Identificador a la fuente de letras para vidas y puntuacion
        /// </summary>
        public static int fuente_s;

        /// <summary>
        /// Fuente del menu principal
        /// </summary>
        public static int fuente_m;

        /// <summary>
        /// Fuente alfab‚tica del juego
        /// </summary>
        public static int fuente_j;

        /// <summary>
        /// Fuente alfab‚tica de la pantalla de cr‚ditos
        /// </summary>
        public static int fuente_c;

        /// <summary>
        /// Fuente alfab‚tica de la pantalla de carga de datos.
        /// </summary>
        public static int fuente_d;

        /// <summary>
        /// Identificador del proceso principal
        /// </summary>
        public static int id_menu;

        /// <summary>
        /// Identificador al proceso del cuerpo del robot
        /// </summary>
        public static int id_c_robot;

        /// <summary>
        /// Identificador al proceso de misiles
        /// </summary>
        public static int id_misiles;
        
        public static int contador_misiles;

        /// <summary>
        ///  Identificador a los graficos de los misiles del marcador
        /// </summary>
        public static int[] tabla_misiles = new int[30];
        /// <summary>
        /// Siguiente misil libre
        /// </summary>
        public static int misiles_libres;
        /// <summary>
        /// Energia del robot
        /// </summary>
        public static int energia_robot;

        /// <summary>
        /// Mayor valor x
        /// </summary>
        public static int max_x = 319;

        /// <summary>
        /// Mayor valor y;
        /// </summary>
        public static int max_y = 199;

        /// <summary>
        /// Frecuencia de disparo del tanque
        /// </summary>
        public static int TankShootFrequency;

        /// <summary>
        /// Frecuencia de disparo del helicoptero
        /// </summary>
        public static int HelicopterShootFrequency;

        /// <summary>
        /// Variable para controlar el final del juego
        /// </summary>
        public static int esta_muerto = 0;
    }
}
