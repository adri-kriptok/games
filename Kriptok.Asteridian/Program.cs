using Kriptok.Asteridian.Scenes;
using Kriptok.Core;
using Kriptok.Views.Texts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kriptok.Asteridian
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Engine.Start(new Level00(), s =>
            {
                //s.FullScreen();
                s.Title = "Asteridian";
                s.Mode = WindowSizeEnum.M320x200;
                //s.Mode = WindowSizeEnum.M320x200To240;
                s.CaptureMouse();
                s.OpenMidiNotePlayer();                
            });
        }
    }

    internal static class GlobalConsts
    {
        /// <summary>
        /// Tamaño de la pantalla.
        /// </summary>
        public static Size ScreenSize;

        //public const string IntroMusic = "Kryrian.Resources.Music.Menu.TheDelRe_TheMoon.mp3";

        //public const int ShadowXModifier = -30;
        //public const int ShadowYModifier = 30;

        //public static int EnemiesXMin = -500;
        //public static int EnemiesXMax = 500;
        //public static int EnemiesYMin = -100;
        //public static int EnemiesYMax = 700;

        /// <summary>
        /// Máximo tamaño que puede tener un nivel y que no se pierda precisión al pasarlo a float.
        /// </summary>
        public const int MaxLevelSize = int.MaxValue >> 8;

        public const int ShootInterval = 10;

        public static readonly SuperFont MenutFont = SuperFont.Build(builder =>
        {
            builder.Font = Fonts.Arial20BoldItalic;
            builder.SetColor(Color.Cyan, Color.White);
            builder.SetShadow(2, 2, Color.Blue);
        });

        public static class ZLevel
        {
            public const float StandarAir = -10;

            public const float EnemyInAir = -11;


            public const float Shadows = -1;

        }
    }
}
