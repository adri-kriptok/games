using Kriptok.Scenes;
using Kriptok.Views.Texts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Games.BlastemUp.Scenes
{
    class CreditsScene : SceneBase
    {
        /// <summary>
        /// Fuente para mostrar los créditos.
        /// </summary>
        private static readonly SuperFont creditsFont = SuperFont.Build(builder =>
        {
            builder.Font =new Font("Bauhaus 93", 32);
            builder.SetColor(Color.Red, Color.Yellow);
            builder.SetShadow(1, 1, Color.Orange);
        });            

        protected override void Run(SceneHandler h)
        {
            h.ScreenRegion.SetBackground(Assembly, "Assets.Images.Backgrounds.Credits.png");            

            // Escribe los créditos
            h.Write(creditsFont,  40,   0, "Programadores:").LeftTop();
            h.Write(creditsFont, 100,  40, "Manuel Cabanias").LeftTop();
            h.Write(creditsFont, 100,  80, "Luis Sureda").LeftTop();
            h.Write(creditsFont,  40, 300, "Graficos:").LeftTop();
            h.Write(creditsFont, 100, 340, "José Fernandez").LeftTop();
            h.Write(creditsFont,  40, 390, "Musica:").LeftTop();
            h.Write(creditsFont, 100, 430, "Moises Díaz Toledano").LeftTop();

            h.FadeOn();

            // Espera a pulsar una tecla
            h.WaitForKeyPress();

            h.FadeOff();

            h.Set(new TitleScreenScene());
        }
    }
}
