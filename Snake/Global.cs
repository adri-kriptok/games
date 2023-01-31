using Kriptok.Views.Texts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.Entities
{
    static class Global
    {
        public static SuperFont Font = SuperFont.Build(b =>
        {
            b.Font = new Font("Arial", 8, FontStyle.Italic | FontStyle.Bold);
            b.SetColor(Color.White);
        });

        /// <summary>
        /// Longitud de la cola del gusano.
        /// </summary>
        public static int SnakeLength = 8;

        /// <summary>
        /// Número de manzanas en pantalla.
        /// </summary>
        public static int Apples = 0;

        /// <summary>
        /// Variables para puntuación y record.
        /// </summary>
        public static int Score = 0, Record = 0;
    }
}
