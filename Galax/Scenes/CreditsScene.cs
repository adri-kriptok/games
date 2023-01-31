using Kriptok.Drawing;
using Kriptok.Scenes;
using Kriptok.Views.Texts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galax.Scenes
{
    class CreditsScene : SceneBase
    {
        private static readonly SuperFont credits = SuperFont.Build(builder =>
        {
            builder.Font = new Font("Bauhaus 93", 14);
            builder.SetColor(Color.White, Color.Blue);
            builder.SetShadow(-2, 2, Color.Black);
        });

        protected override void Run(SceneHandler h)
        {
            h.ScreenRegion.SetBackground(bg =>
            {
                bg.Draw(Assembly, "TitleScreen.png", 0, 0, new ColorTransform()
                {
                    A = 1f,
                    R = new ColorVector(0.5f, 0.0f, 0.5f),
                    G = new ColorVector(0.0f, 0.0f, 0.0f),
                    B = new ColorVector(0.5f, 0.0f, 0.5f),
                });
            });

            // Imprime textos.
            h.Write(credits, 160,  30, "- CREDITOS -").CenterMiddle();
            h.Write(credits, 160,  60, "PROGRAMADOR: LUIS SUREDA").CenterMiddle();
            h.Write(credits, 160,  80, "GRAFICOS: JOSE FERNANDEZ").CenterMiddle();
            h.Write(credits, 160, 100, "SONIDOS: CARLOS ILLANA").CenterMiddle();
            h.Write(credits, 160, 120, "COPYRIGHT 1997").CenterMiddle();
            h.Write(credits, 160, 140, "DIV GAMES STUDIO").CenterMiddle();

            // Enciende la pantalla
            h.FadeOn();

            // Espero a que se presione una tecla o 10 segundos.
            h.WaitOrKeyPress(10000);

            // Apago la pantalla.
            h.FadeOff();

            // Y salgo.
            h.Exit();
        }
    }
}
