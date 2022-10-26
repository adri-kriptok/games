using Kriptok.Common;
using Kriptok.Entities.Base;
using Kriptok.Scenes;
using Kriptok.Views.Sprites;
using Kriptok.Views.Videos;
using PerdidoEnElTiempo.Scenes.Base;
using System.Drawing;

namespace PerdidoEnElTiempo.Scenes
{
    internal class Egypt1Scene : VideoSceneBase
    {
        protected override void Run(SceneHandler h)
        {
            // Si pierde, continúa desde acá.
            Global.State = 1;

            PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Egypt.A21.FLI"), () => h.FadeFrom(Color.White));
            PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Egypt.A22.FLI"), false);
            
            h.Wait(250);

            // Limpio el buffer de teclas.
            h.WaitOrKeyPress(1);
            h.StartSingleMenu(Global.MenuFont, menu =>
            {
                menu.Location = Global.MenuPlace;

                menu.CloseOnSelection = true;
                menu.OnCursorMove((from, to) => h.PlayCursorMoveSound());

                menu.Add("Ir a la gran pirámide.", () =>
                {
                    h.PlayMenuOKSound();

                    h.Wait(250);
                    h.FadeOff();
                    h.Set(new Egypt6Scene());
                });
                
                menu.Add("Descansar bajo la palmera.", () =>
                {
                    h.PlayMenuOKSound();
                
                    h.Wait(250);
                    h.FadeOff();
                    PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Egypt.A23.FLI"), () => h.FadeOn(), false);                    
                    GameOver(h, 2);
                });

                menu.Add("Ir a la pirámide pequeña.", () =>
                {
                    h.PlayMenuOKSound();

                    h.Wait(250);
                    h.FadeOff();
                    h.Set(new Egypt2Scene());
                });
            });
        }
    }
}