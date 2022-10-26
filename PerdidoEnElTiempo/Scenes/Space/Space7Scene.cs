using Kriptok.Common;
using Kriptok.Scenes;
using Kriptok.Views.Videos;
using PerdidoEnElTiempo.Scenes.Base;
using System.Drawing;

namespace PerdidoEnElTiempo.Scenes
{
    internal class Space7Scene : VideoSceneBase
    {
        protected override void Run(SceneHandler h)
        {
            // Inicializo la autodestrucción.
            Global.AutoDestructionTimer = 10;

            var v = h.StartVideo(new FlicDecoder(Resource.Get(Assembly, "Assets.Videos.Space.A64.FLI")));
            v.GoToEnd();

            AutoDestructCheck(h);

            // Limpio el buffer de teclas.
            h.WaitOrKeyPress(1);
            h.StartSingleMenu(Global.MenuFont, menu =>
            {
                menu.Location = Global.MenuPlace;

                menu.CloseOnSelection = true;
                menu.OnCursorMove((from, to) => h.PlayCursorMoveSound());

                menu.Add("Corres hacia el puerto A33.", () =>
                {
                    h.PlayMenuOKSound();
                    h.FadeOff();
                    h.Set(new Space4Scene());
                });

                menu.Add("Esperas a ver qué pasa.", () =>
                {
                    h.PlayMenuOKSound();
                });
            });
        }
    }
}