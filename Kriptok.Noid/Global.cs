using Kriptok.Audio;
using Kriptok.Audio.Midi;
using Kriptok.Common;
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
        public const int FirstLevel = 7;

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

        internal static void ResetValues()
        {
            // Reseteo la puntuación para el modo demo, o si estoy empezando a jugar.
            Global.Score = 0;
            Global.Lives = Consts.InitialLives;
        }
    }

    public static class Sounds
    {
        // internal static Resource s_golpe = new Resource(typeof(Sounds).Assembly, "BANDA.WAV");
        internal static Resource s_golpe = new Resource(typeof(Sounds).Assembly, "BILLAR0.WAV");
        internal static Resource s_pildora = new Resource(typeof(Sounds).Assembly, "FX34.WAV");
        internal static Resource s_fuego = new Resource(typeof(Sounds).Assembly, "LASER.WAV");
        internal static Resource s_metal = new Resource(typeof(Sounds).Assembly, "METAL10.WAV");
        internal static Resource s_bordes = new Resource(typeof(Sounds).Assembly, "GOLPE.WAV"); // "PASO9.WAV");
        internal static Resource s_raqueta = new Resource(typeof(Sounds).Assembly, "GOLPE.WAV");
        
        /// <summary>
        /// Instrumento para reproducir notas cuando se golpea los ladrillos.
        /// </summary>
        internal static MidiInstrument TaikoDrum = new MidiInstrument(MidiInstrumentEnum.TaikoDrum, 0);

        // internal static Resource s_banda = new Resource(typeof(Sounds).Assembly, "BANDA.WAV");
        // internal static Resource s_billar = new Resource(typeof(Sounds).Assembly, "BILLAR0.WAV");
        // internal static Resource s_raqueta = new Resource(typeof(Sounds).Assembly, "FX35.WAV");
    }
}
