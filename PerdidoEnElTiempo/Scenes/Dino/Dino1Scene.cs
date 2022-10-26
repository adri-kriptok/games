using Kriptok.Common;
using Kriptok.Entities.Base;
using Kriptok.Scenes;
using Kriptok.Views.Sprites;
using Kriptok.Views.Videos;
using PerdidoEnElTiempo.Scenes.Base;
using System.Drawing;

namespace PerdidoEnElTiempo.Scenes
{
    internal class Dino1Scene : VideoSceneBase
    {
        protected override void Run(SceneHandler h)
        {                        
            // Si pierde, continúa desde acá.
            Global.State = 0;

            PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Dino.A08.FLI"), () => h.FadeFrom(Color.White));
            var frame = h.Add(new Frame());
            PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Dino.A09.FLI"));
            //frame.Die();
            PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Dino.A10.FLI"));

            PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Dino.A11.FLI"));
            PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Dino.A12.FLI"), false);

            // Limpio el buffer de teclas.
            h.WaitOrKeyPress(1);
            h.StartSingleMenu(Global.MenuFont, menu =>
            {
                menu.Location = Global.MenuPlace;
                menu.CloseOnSelection = true;
                menu.OnCursorMove((from, to) => h.PlayCursorMoveSound());

                menu.Add("Agacharte.", () =>
                {
                    h.PlayMenuOKSound();
                    h.Wait(250);
                    PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Dino.A13.FLI"));
                    PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Dino.A16.FLI"), false);
                    h.Set(new Dino2Scene());
                });
                menu.Add("Seguir corriendo.", () =>
                {
                    h.PlayMenuOKSound();
                    
                    h.Wait(250);
                    PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Dino.A14.FLI"));
                    PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Dino.A15.FLI"), false);
                    GameOver(h, 1);
                });
            });
        }
    }
}