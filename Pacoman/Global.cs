using Kriptok.Entities.Base;
using Kriptok.Views.Texts;
using Pacoman.Entities;
using System.Drawing;

namespace Pacoman
{
    public static class Global
    {        
        public static SuperFont Font = new SuperFont(new Font("Quartz MS", 22), Color.White).SetShadow(2, 2, Color.DarkBlue);        

        /// <summary>
        /// Tabla con el tiempo que dura el efecto de poder comer.
        /// </summary>
        public static int[] CapsuleTimes = new int[] { 300, 240, 180, 140, 120, 100, 80, 60, 40, 0 };

        /// <summary>
        /// Tabla de dificultad.
        /// </summary>
        public static int[] GhostIntellgence = new int[] { 10, 30, 50, 65, 75, 85, 90, 95, 100, 100 };

        /// <summary>
        /// Identificador global para pacoman.
        /// </summary>
        public static ProcessBase Player;

        /// <summary>
        /// Indicador de la ultima puntuacion.
        /// </summary>
        public static int PreviewsScore = 0;

        /// <summary>
        /// Puntuacion del jugador.
        /// </summary>
        public static int Score = 0; 

        /// <summary>
        /// Cantidad de puntos que comí hasta ahora.
        /// </summary>
        public static int Balls = 0;

        /// <summary>
        /// Identificadores para las vidas del marcador.
        /// </summary>
        public static Life[] Lives = new Life[10];

        /// <summary>
        /// Contador de vidas en el marcador.
        /// </summary>
        public static int LiveCount = 0;

        /// <summary>
        /// Maxima puntuacion por defecto.
        /// </summary>
        public static int MaxScore = 10000;
    }
}
