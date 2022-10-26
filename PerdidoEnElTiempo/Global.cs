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
        public static SuperFont MenuFont = new SuperFont(new Font("Arial", 8), Color.White, Color.LightGoldenrodYellow)
            .SetShadow(-1, 1, Color.DarkRed);

        /// <summary>
        /// Ubicación donde debe aparecer el menú de opciones.
        /// </summary>
        public static readonly Point MenuPlace = new Point(30, 150);
    }
}
