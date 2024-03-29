﻿using Kriptok.Common;
using Kriptok.Entities.Base;
using Kriptok.Scenes;
using Kriptok.Views.Sprites;
using Kriptok.Views.Videos;
using PerdidoEnElTiempo.Scenes.Base;
using System.Drawing;

namespace PerdidoEnElTiempo.Scenes
{
    internal class Egypt5Scene : VideoSceneBase
    {
        protected override void Run(SceneHandler h)
        {
            PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Egypt.A25.FLI"), () => h.FadeOn());
            var vid = PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Egypt.A33.FLI"), false);

            // Limpio el buffer de teclas.
            h.WaitOrKeyPress(1);
            h.StartSingleMenu(Global.MenuFont, menu =>
            {
                menu.Location = Global.MenuPlace;

                menu.CloseOnSelection = true;
                menu.OnCursorMove((from, to) => h.PlayCursorMoveSound());

                menu.Add("Retroceder por donde has venido.", () =>
                {
                    h.PlayMenuOKSound();
                    h.Wait(250);
                    vid.Kill();
                    PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Egypt.A34.FLI"));
                    PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Egypt.A35.FLI"));
                    PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Egypt.A36.FLI"), false);
                    h.Wait(1000);                    
                    GameOver(h, 0);
                });
                
                menu.Add("Enfrentarte al esqueleto.", () =>
                {
                    h.PlayMenuOKSound();
                    h.Wait(250);
                    vid.Kill();
                    PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Egypt.A37.FLI"));
                    var videoToKill = PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Egypt.A32.FLI"), false);
                    PlayTimeTravel(h, videoToKill);
                    h.Set(new Space1Scene());
                });
            });
        }
    }
}