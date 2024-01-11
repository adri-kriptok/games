using Kriptok.Common;
using Kriptok.Entities.Base;
using Kriptok.Scenes;
using Kriptok.Views.Sprites;
using Kriptok.Views.Videos;
using PerdidoEnElTiempo.Scenes.Base;
using System.Drawing;

namespace PerdidoEnElTiempo.Scenes
{
    internal class Space5Scene : VideoSceneBase
    {
        protected override void Run(SceneHandler h)
        {
            //PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Space.A44.FLI"));
            //PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Space.A46.FLI"));
            //PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Space.A47.FLI"));
            //PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Space.A48.FLI"));
            //var vid = PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Space.A49.FLI"), false);

            // Limpio el buffer de teclas.
            h.WaitOrKeyPress(1);
            h.StartSingleMenu(Global.MenuFont, menu =>
            {
                menu.Location = Global.MenuPlace;

                menu.CloseOnSelection = true;
                menu.OnCursorMove((from, to) => h.PlayCursorMoveSound());

                menu.Add("Lanzas un misil.", () =>
                {
                    //h.PlayMenuOKSound();
                    //h.Wait(250);
                    //vid.Kill();
                    //PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Space.A50.FLI"));
                    //PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Space.A53.FLI"));
                    //PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Space.A54.FLI"));
                    //PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Space.A52.FLI"), false);
                    //GameOver(h, 0);
                });

                menu.Add("Lanzas un anti-misil.", () =>
                {
                    //h.PlayMenuOKSound();
                    //h.Wait(250);
                    //vid.Kill();
                    //PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Space.A50.FLI"));
                    //PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Space.A51.FLI"));
                    //PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Space.A55.FLI"));
                    //PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Space.A56.FLI"));
                    var prevVideo = PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.A66.FLI"), false);
                    h.FadeOff();
                    prevVideo.Kill();
                    PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.FINAL.FLI"), () => h.FadeOn(), false);
                    h.WaitOrKeyPress(10000);
                    h.FadeOff();
                    h.Set(new InitScene());
                });
            });
        }
    }
}