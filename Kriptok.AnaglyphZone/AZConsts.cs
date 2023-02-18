using Kriptok.IO;
using Kriptok.Views;
using Kriptok.Views.Texts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.AZ
{
    internal class AZConsts
    {
        public const float CamHeight = 300f;

        public const float GridSize = 100f;

        public const int MinGridZ = -8;
        public const int MaxGridZ = 3;
        public const int GridLength = MaxGridZ - MinGridZ;

        public static readonly FillConfig Black = new FillConfig(Color.Black);


        public static FontFamily DefaultFontFamily =
            Fonts.GetFontFamily(typeof(Program).Assembly, "8bitoperator.ttf");

        public static readonly Font DefaultFont = new Font(DefaultFontFamily, 24, FontStyle.Bold);

        public static readonly SuperFont TextFont = SuperFont.Build(builder =>
        {
            builder.Font = new Font("OCR A", 12, FontStyle.Bold);
            builder.SetColor(Color.Fuchsia);
        });
    }

    public static class Global
    {
        /// <summary>
        /// Máximo puntaje.
        /// </summary>
        public static int Record = MaxScore.Load();

        /// <summary>
        /// Puntaje actual.
        /// </summary>
        public static int Score;
    }
}
