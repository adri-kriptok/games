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
            var prevVideo = PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Dino.A17.FLI"), false);

            // Limpio el buffer de teclas.
            h.WaitOrKeyPress(1);
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
                    prevVideo.Kill();
                    PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Dino.A19.FLI"), false);                    
                    GameOver(h, 4);
                });

                menu.Add("Quedarte quieto.", () =>
                {
                    h.PlayMenuOKSound();

                    h.Wait(250);
                    prevVideo.Kill();
                    
                    var videoTokill = PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Dino.A18.FLI"), false);
                    
                    PlayTimeTravel(h, videoTokill);

                    h.Set(new Egypt1Scene());
                });
            });
        }
    }
}