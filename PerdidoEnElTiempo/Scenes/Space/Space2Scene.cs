using Kriptok.Common;
using Kriptok.Scenes;
using PerdidoEnElTiempo.Scenes.Base;
using System.Drawing;

namespace PerdidoEnElTiempo.Scenes
{
    internal class Space2Scene : VideoSceneBase
    {
        protected override void Run(SceneHandler h)
        {            
            var vid = PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Space.A42.FLI"), () => h.FadeOn(), false);

            // Limpio el buffer de teclas.
            h.WaitOrKeyPress(1);
            h.StartSingleMenu(Global.MenuFont, menu =>
            {
                menu.Location = Global.MenuPlace;

                menu.CloseOnSelection = true;
                menu.OnCursorMove((from, to) => h.PlayCursorMoveSound());

                menu.Add("Retroceder.", () =>
                {
                    h.PlayMenuOKSound();
                    
                    h.FadeOff();
                    h.Set(new Space1Scene(true));
                });

                menu.Add("Seguir adelante.", () =>
                {
                    h.PlayMenuOKSound();
                    h.Wait(250);
                    vid.Kill();
                    PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Space.A43.FLI"), false);
                    GameOver(h, 0);
                });
            });
        }
    }
}