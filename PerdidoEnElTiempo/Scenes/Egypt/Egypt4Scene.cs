using Kriptok.Common;
using Kriptok.Entities.Base;
using Kriptok.Scenes;
using Kriptok.Views.Sprites;
using Kriptok.Views.Videos;
using PerdidoEnElTiempo.Scenes.Base;
using System.Drawing;

namespace PerdidoEnElTiempo.Scenes
{
    internal class Egypt4Scene : VideoSceneBase
    {
        protected override void Run(SceneHandler h)
        {
            PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Egypt.A25.FLI"));
            PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Egypt.A24.FLI"), false);

            h.StartSingleMenu(Global.MenuFont, menu =>
            {
                menu.Location = Global.MenuPlace;

                menu.CloseOnSelection = true;
                menu.OnCursorMove((from, to) => h.PlayCursorMoveSound());

                menu.Add("Sales por la otra puerta.", () =>
                {
                    h.PlayMenuOKSound();
                    h.Set(new Egypt5Scene());
                });
                
                menu.Add("Coges la esfera de oro.", () =>
                {
                    h.PlayMenuOKSound();

                    PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Egypt.A26.FLI"));
                    PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Egypt.A27.FLI"), false);
                    h.Wait(250);
                    h.FadeOff();
                    h.Set(new GameOverScene(3));
                });
            });
        }
    }
}