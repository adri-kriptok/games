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
        private static readonly SuperFont credits = new SuperFont(new Font("Bauhaus 93", 14), 
            Color.White, Color.Blue).SetShadow(-2, 2, Color.Black);

        protected override void Run(SceneHandler h)
        {
            h.ScreenRegion.SetBackground(GetType().Assembly, "Credits.png");

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
