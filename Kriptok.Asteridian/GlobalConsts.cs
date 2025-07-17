using Kriptok.Views.Texts;
using System.Drawing;

namespace Kriptok.Asteridian
{
    public static class GlobalConsts
    {
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
            
        public static float ShotBase_MinX;
        public static float ShotBase_MinY;
        public static float ShotBase_MaxX;
        public static float ShotBase_MaxY;

        public static float PlayerShip_MinX;
        public static float PlayerShip_MinModifierY;
        public static float PlayerShip_MaxX;
        public static float PlayerShip_MaxModifierY;
        public static float PlayerShip_MidX;

        public static int MouseMinX;

        public static class ZLevel
        {
            public const float StandarAir = -10;

            public const float Floor = 0;
            public const float Shadows = -1;

            public const float Layer2 = -2000;
        }
    }
}
