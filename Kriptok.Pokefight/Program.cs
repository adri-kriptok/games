using Kriptok.Pokefight.Scenes;
using Kriptok.Pokefight.Common;
using Kriptok.Pokefight.Processes;
using Kriptok.IO;
using Kriptok.Core;
using Kriptok.Entities.Base;
using Kriptok.Views.Texts;
using System;
using System.Drawing;

namespace Kriptok.Pokefight
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Engine.Start(new BattleScene(), s =>
            Engine.Start(new IntroScene(), p =>
            {
                //p.FullScreen();
                p.Mode = WindowSizeEnum.W340x192;
                p.Title = "Kriptok Sdk - Samples - Pokemon Fighter";
                p.ExtractMidiPlayer();
            });
        }

        public static FontFamily DefaultFontFamily =
            Fonts.GetFontFamily(typeof(Program).Assembly, "8bitoperator.ttf");

        public static readonly SuperFont DefaultFont = SuperFont.Build(builder =>
        {
            builder.Font = new Font(DefaultFontFamily, 18, FontStyle.Bold);
            builder.SetColor(Color.White, Color.LightGray);
            builder.SetShadow(1, 1, Color.DarkGray);
        });

        public static readonly SuperFont BattleNameFont = SuperFont.Build(builder =>
        {
            builder.Font = new Font(DefaultFontFamily, 12);
            builder.SetColor(Color.White, Color.LightGray);
            builder.SetShadow(1, 1, Color.FromArgb(196, 0, 0, 0));
        });
    }
}
