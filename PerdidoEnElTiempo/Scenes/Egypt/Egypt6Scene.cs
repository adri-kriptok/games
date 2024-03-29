﻿using Kriptok.Common;
using Kriptok.Entities.Base;
using Kriptok.Scenes;
using Kriptok.Views.Sprites;
using Kriptok.Views.Videos;
using PerdidoEnElTiempo.Scenes.Base;
using System.Drawing;

namespace PerdidoEnElTiempo.Scenes
{
    internal class Egypt6Scene : VideoSceneBase
    {
        protected override void Run(SceneHandler h)
        {
            Later(h);
            h.ScreenRegion.SetBackground(Assembly, "Assets.Images.choice3doors.png");
            h.FadeOn();

            // Limpio el buffer de teclas.
            h.WaitOrKeyPress(1);
            h.StartSingleMenu(Global.MenuFont, menu =>
            {
                menu.Location = Global.MenuPlace;

                menu.CloseOnSelection = true;
                menu.OnCursorMove((from, to) => h.PlayCursorMoveSound());

                menu.Add("Entrar por la izquierda.", () =>
                {
                    h.PlayMenuOKSound();
                    h.FadeOff();
                    h.Set(new Egypt4Scene());
                });

                menu.Add("Entrar por el medio.", () =>
                {
                    h.PlayMenuOKSound();
                    h.FadeOff();
                    h.Set(new Egypt3Scene());
                });

                menu.Add("Entrar por la derecha.", () =>
                {
                    h.PlayMenuOKSound();
                    h.FadeOff();
                    h.Set(new Egypt5Scene());
                });
            });
        }
    }
}