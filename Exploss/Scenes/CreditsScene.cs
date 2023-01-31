using Exploss.Entities;
using Exploss.Entities.Blocks;
using Exploss.Entities.Bubbles;
using Exploss.Mapping;
using Kriptok.Div;
using Kriptok.Drawing;
using Kriptok.Entities.Base;
using Kriptok.Helpers;
using Kriptok.IO;
using Kriptok.Maps.Tiles;
using Kriptok.Scenes;
using Kriptok.Views.Texts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exploss.Scenes
{
    class CreditsScene : SceneBase
    {
        protected override void Run(SceneHandler h)
        {
            h.ScreenRegion.SetBackground(bg =>
            {
                bg.Draw(Assembly, "Assets.Images.Title.png", 0, 0, new ColorTransform()
                {
                    A = 0.7f,
                    R = new ColorVector(0f, 0f, 0.3f),
                    G = new ColorVector(0f, 0f, 0.6f),
                    B = new ColorVector(0.1f, 0.1f, 0.1f),
                });
            });

            h.Write(Global.Font1, h.ScreenRegion.Size.Width / 2, 100, "CREDITOS");

            h.Write(Global.Font2, h.ScreenRegion.Size.Width / 2, 100 + 20 + 60, "Programación");
            h.Write(Global.Font2, h.ScreenRegion.Size.Width / 2, 120 + 20 + 70, "Ismael Fernandez Bustos");
            h.Write(Global.Font2, h.ScreenRegion.Size.Width / 2, 150 + 40 + 80, "Presentación");
            h.Write(Global.Font2, h.ScreenRegion.Size.Width / 2, 170 + 40 + 90, "César Botana");
            h.Write(Global.Font2, h.ScreenRegion.Size.Width / 2, 200 + 60 + 100, "Gracias especiales a");
            h.Write(Global.Font2, h.ScreenRegion.Size.Width / 2, 220 + 60 + 110, "Ana Luis Campos");

            h.FadeOn();

            h.WaitForKeyPress();

            h.FadeOff();
            h.Set(new MenuScene());
        }        
    }
}
