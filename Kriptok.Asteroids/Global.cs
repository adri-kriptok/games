using Kriptok.Asteroids.Objects;
using Kriptok.Helpers;
using Kriptok.Views.Texts;
using System.Drawing;

namespace Kriptok.Asteroids
{
    static class Global
    {
        public static Font Font = new Font("OCR A", 18);

        /// <summary>
        /// Fuente principal a utilizar.
        /// </summary>
        public static SuperFont GreenFont = new SuperFont(Font, ColorHelper.Green);

        public static SuperFont CyanFont = new SuperFont(Font, Color.Cyan);

        public static SuperFont YellowFont = new SuperFont(Font, Color.Yellow);

        /// <summary>
        /// Puntuación.
        /// </summary>
        public static int Score = 0;              

        /// <summary>
        /// Vidas restantes.
        /// </summary>
        public static int LivesCount = 3;                    

        /// <summary>
        /// Identificadores del gráfico de las vidas.
        /// </summary>
        public static Live[] Lives = new Live[3];

        /// <summary>
        /// Indica si ha muerto o no (Bandera).
        /// </summary>
        public static bool Dead = false;

        /// <summary>
        ///  Bandera de salida (true=Salir del juego).
        /// </summary>
        public static bool ExitFlag = false;


        internal static int Record;
    }
}
