using Kriptok.Extensions;
using Kriptok.Views.Texts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Noid
{
    static class Consts
    {
        /// <summary>
        /// Nivel en el que iniciar.
        /// </summary>
        public const int FirstLevel = 1;

        /// <summary>
        /// Cantidad de vidas iniciales.
        /// </summary>
        public const int InitialLives = 3;

        /// <summary>
        /// Mayor cantidad de vidas que se pueden ver en pantalla.
        /// </summary>
        public const int MaxLivesOnScreen = 3;
    }

    public static class Global
    {
        public static SuperFont Font = new SuperFont(new Font("Arial", 7), Color.White, Color.CornflowerBlue)
            .SetShadow(1, 1, Color.Black.SetAlpha(192));

        public static SuperFont CreditsFont = new SuperFont(new Font("Arial", 10, FontStyle.Bold), Color.LightGoldenrodYellow)
            .SetBorder(Color.Orange.SetAlpha(127), 2)
            .SetShadow(3, 3, Color.Black.SetAlpha(192));

        /// <summary>
        /// Puntaje actual del jugador.
        /// </summary>
        public static int Score;

        /// <summary>
        /// Vidas que tiene el jugador actualmente.
        /// </summary>
        public static int Lives;
    }
}
