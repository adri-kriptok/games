﻿using Kriptok.Drawing;
using Kriptok.Helpers;
using Kriptok.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kriptok.Noid.Scenes
{
    public class IntroScene : SceneBase
    {
        protected override void Run(SceneHandler h)
        {
            h.ScreenRegion.SetBackground(typeof(IntroScene).Assembly, "Assets.Images.TitleScreen.png");
            h.FadeOn();

            var key = h.WaitForKeyPress();

            if (key == Keys.Escape)
            {
                h.FadeOff();
                h.ScreenRegion.ClearBackground();
                h.ScreenRegion.SetBackground(bg => bg.UsingGraphics(g =>
                {
                    bg.Draw(Assembly, "Assets.Images.TitleScreen.png", 0, 0, new ColorTransform()
                    {
                        A = 0.7f,
                        R = new ColorVector(0.5f, 0.5f, 0.3f),
                        G = new ColorVector(0.7f, 0.7f, 0.6f),
                        B = new ColorVector(0.1f, 0.1f, 0.1f),
                    });

                    h.Write(Global.CreditsFont, 160,  30, "- CREDITOS -").CenterMiddle();
                    h.Write(Global.CreditsFont, 160,  60, "PROGRAMADOR: LUIS SUREDA").CenterMiddle();
                    h.Write(Global.CreditsFont, 160,  80, "GRAFICOS: JOSE FERNANDEZ").CenterMiddle();
                    h.Write(Global.CreditsFont, 160, 100, "SONIDOS: CARLOS ILLANA").CenterMiddle();
                    h.Write(Global.CreditsFont, 160, 120, "COPYRIGHT 1997").CenterMiddle();
                    h.Write(Global.CreditsFont, 160, 140, "DIV GAMES STUDIO").CenterMiddle();

                }));
                h.FadeOn();
                h.WaitForKeyPress();
                h.FadeOff();
                h.Exit();
            }
            else
            {
                h.FadeOff();
                h.Set(new LevelScene(Consts.FirstLevel, true));
            }
        }
    }
}