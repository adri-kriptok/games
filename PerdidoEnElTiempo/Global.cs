using Kriptok.Views.Texts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerdidoEnElTiempo
{
    class Global
    {
        public static SuperFont MenuFont = new SuperFont(new Font("Arial", 10), Color.White, Color.LightGoldenrodYellow)
            .SetShadow(-1, 1, Color.DarkRed);

        public static SuperFont DangerFont = new SuperFont(new Font("Arial", 14, FontStyle.Bold), Color.Red, Color.OrangeRed)
            .SetBorder(Color.Orange, 1);

        /// <summary>
        /// Ubicación donde debe aparecer el menú de opciones.
        /// </summary>
        public static readonly Point MenuPlace = new Point(30, 150);

        /// <summary>
        /// Indica cuánto tiempo queda hasta la autodestrucción de la estación espacial.
        /// </summary>
        public static int AutoDestructionTimer = 0;

        /// <summary>
        /// Indica el estado del juego en el que estás cuando continuás.
        /// </summary>
        public static int State = 0;
    }
}
