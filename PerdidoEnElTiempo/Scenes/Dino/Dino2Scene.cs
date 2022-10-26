using Kriptok.Common;
using Kriptok.Entities.Base;
using Kriptok.Scenes;
using Kriptok.Views.Sprites;
using Kriptok.Views.Videos;
using PerdidoEnElTiempo.Scenes.Base;
using System.Drawing;

namespace PerdidoEnElTiempo.Scenes
{
    internal class Dino2Scene : VideoSceneBase
    {
        protected override void Run(SceneHandler h)
        {
            PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Dino.A17.FLI"), false);

            h.StartSingleMenu(Global.MenuFont, menu =>
            {
                menu.Location = Global.MenuPlace;
                menu.CloseOnSelection = true;
                menu.OnCursorMove((from, to) => h.PlayCursorMoveSound());

                menu.Add("Saltar a la otra orilla.", () =>
                {
                    h.PlayMenuOKSound();
                    h.Wait(250);
                    h.Add(new Frame());
                    PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Dino.A19.FLI"), false);
                    h.FadeOff();
                    h.Set(new GameOverScene(4));
                });

                menu.Add("Quedarte quieto.", () =>
                {
                    h.PlayMenuOKSound();

                    h.Wait(250);
                    PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Dino.A18.FLI"), false);
                    
                    PlayTimeTravel(h);

                    h.Set(new Egypt1Scene());
                });
            });
        }
    }
}