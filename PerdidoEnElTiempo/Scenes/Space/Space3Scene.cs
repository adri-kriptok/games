using Kriptok.Common;
using Kriptok.Entities.Base;
using Kriptok.Scenes;
using Kriptok.Views.Sprites;
using Kriptok.Views.Videos;
using PerdidoEnElTiempo.Scenes.Base;
using System.Drawing;

namespace PerdidoEnElTiempo.Scenes
{
    internal class Space3Scene : VideoSceneBase
    {
        protected override void Run(SceneHandler h)
        {
            h.ScreenRegion.SetBackground(Assembly, "Assets.Images.choicespace.png");
            h.FadeOn();

            // Limpio el buffer de teclas.
            h.WaitOrKeyPress(1);
            h.StartSingleMenu(Global.MenuFont, menu =>
            {
                menu.Location = Global.MenuPlace;

                menu.CloseOnSelection = true;
                menu.OnCursorMove((from, to) => h.PlayCursorMoveSound());

                menu.Add("Izquierda: Sala de mandos.", () =>
                {
                    h.PlayMenuOKSound();
                    h.Wait(250);
                    h.Set(new Space6Scene());
                });

                menu.Add("Derecha: Puerto A33.", () =>
                {
                    h.PlayMenuOKSound();
                    h.FadeOff();
                    h.Set(new Space4Scene());
                });

                menu.Add("Volver.", () =>
                {
                    h.PlayMenuOKSound();
                    h.FadeOff();
                    h.Set(new Space1Scene(true));
                });
            });
        }
    }
}