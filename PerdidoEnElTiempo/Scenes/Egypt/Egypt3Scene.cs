using Kriptok.Common;
using Kriptok.Entities.Base;
using Kriptok.Scenes;
using Kriptok.Views.Sprites;
using Kriptok.Views.Videos;
using PerdidoEnElTiempo.Scenes.Base;
using System.Drawing;

namespace PerdidoEnElTiempo.Scenes
{
    internal class Egypt3Scene : VideoSceneBase
    {
        protected override void Run(SceneHandler h)
        {
            PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Egypt.A25.FLI"), () => h.FadeOn());
            PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Egypt.A28.FLI"));
            var vid = PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Egypt.A29.FLI"), false);
            h.FadeOff();
            vid.Kill();
            h.ScreenRegion.SetBackground(Assembly, "Assets.Images.choiceboulder.png");
            h.FadeOn();

            // Limpio el buffer de teclas.
            h.WaitOrKeyPress(1);
            h.StartSingleMenu(Global.MenuFont, menu =>
            {
                menu.Location = Global.MenuPlace;
                menu.CloseOnSelection = true;
                menu.OnCursorMove((from, to) => h.PlayCursorMoveSound());

                menu.Add("Seguir corriendo.", () =>
                {
                    h.PlayMenuOKSound();
                    h.Wait(250);
                    PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Egypt.A30.FLI"), false);
                    h.Wait(2000);                    
                    GameOver(h, 0);
                });

                menu.Add("Echarte para un lado.", () =>
                {
                    h.PlayMenuOKSound();
                    h.Wait(250);
                    PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Egypt.A31.FLI"));
                    var videoToKill = PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Egypt.A32.FLI"), false);
                    PlayTimeTravel(h, videoToKill);
                    h.Set(new Space1Scene());
                });
            });
        }
    }
}