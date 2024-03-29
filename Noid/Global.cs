﻿using Kriptok.Audio;
using Kriptok.Audio.Midi;
using Kriptok.Common;
using Kriptok.Extensions;
using Noid.Scenes;
using Kriptok.Views.Texts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kriptok.Views.Shapes;

namespace Noid
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
        public static SuperFont Font = SuperFont.Build(builder =>
        {
            builder.Font = new Font("Arial", 7);
            builder.SetColor(Color.Cyan, Color.White);
            builder.SetShadow(1, 1, Color.Black.SetAlpha(192));
        });

        public static SuperFont CreditsFont = SuperFont.Build(builder =>
        {
            builder.Font = new Font("Arial", 10, FontStyle.Bold);
            builder.SetColor(Color.LightGoldenrodYellow);
            builder.Border = Strokes.Get(Color.Orange.SetAlpha(127), 2);
            builder.SetShadow(3, 3, Color.Black.SetAlpha(192));
        });

        /// <summary>
        /// Puntaje actual del jugador.
        /// </summary>
        public static int Score;

        /// <summary>
        /// Vidas que tiene el jugador actualmente.
        /// </summary>
        public static int LifeCount;

        /// <summary>
        /// Referencias a las vidas actuales.
        /// </summary>
        internal static readonly Life[] Lives = new Life[Consts.MaxLivesOnScreen];

        internal static void ResetValues()
        {
            // Reseteo la puntuación para el modo demo, o si estoy empezando a jugar.
            Global.Score = 0;
            Global.LifeCount = Consts.InitialLives;
        }
    }

    public static class Sounds
    {        
        internal static Resource PillSound = Resource.Get(typeof(Sounds).Assembly, "BUIU.WAV");
        internal static Resource LaserSound = Resource.Get(typeof(Sounds).Assembly, "LASER.WAV");
        internal static Resource MetalSound = Resource.Get(typeof(Sounds).Assembly, "METAL10.WAV");
        internal static Resource BoundSound = Resource.Get(typeof(Sounds).Assembly, "GOLPE.WAV");
        internal static Resource RacketSound = Resource.Get(typeof(Sounds).Assembly, "GOLPE.WAV");
        // internal static Resource ReleaseBallSound = new Resource(typeof(Sounds).Assembly, "BILLAR0.WAV");

        /// <summary>
        /// Instrumento para reproducir notas cuando se golpea los ladrillos.
        /// </summary>
        internal static MidiInstrument TaikoDrum = new MidiInstrument(MidiInstrumentEnum.TaikoDrum, 0);
    }
}
