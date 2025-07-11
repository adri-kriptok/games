using Kriptok.Extensions;
using Kriptok.Scenes;
using Kriptok.Views.Texts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Games.Alien.Scenes
{
    internal class CreditsScene : SceneBase
    {
        private static readonly SuperFont font = SuperFont.Load(typeof(IntroScene).Assembly, "Assets.Fonts.Credits.fntx");

        protected override void Run(SceneHandler h)
        {
            h.ScreenRegion.SetBackground(typeof(IntroScene).Assembly, "Assets.Credits.png");

            // Escribe los créditos            
            h.Write(font, 320, 5, "ALIEN SUPRIMER").CenterTop();
            h.Write(font, 320, 80, "Programado por:").CenterTop();
            h.Write(font, 320, 120, "Daniel Munioz Santinio").CenterTop();
            h.Write(font, 320, 180, "Graficos by:").CenterTop();
            h.Write(font, 320, 220, "J. Ricardo Abella").CenterTop();
            h.Write(font, 320, 260, "Eva Astorga").CenterTop();
            h.Write(font, 320, 320, "Sonidos:").CenterTop();
            h.Write(font, 320, 360, "Carlos Illana Alejandro").CenterTop();

            h.FadeOn();
            h.WaitForKeyPress();
            h.FadeOff();
            h.Exit();
        }
    }
}
