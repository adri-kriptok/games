using Kriptok.Common;
using Kriptok.Scenes;
using Kriptok.Views.Videos;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PerdidoEnElTiempo.Scenes
{
    internal class InitScene : SceneBase
    {
        protected override void Run(SceneHandler h)
        {
            var bre = PlayOrKeyPress(h, Resource.Get(Assembly, "Assets.Videos.Intro.A01.FLI"), false);
            bre = PlayOrKeyPress(h, Resource.Get(Assembly, "Assets.Videos.Intro.A02.FLI"), bre);
            bre = PlayOrKeyPress(h, Resource.Get(Assembly, "Assets.Videos.Intro.A03.FLI"), bre);
            bre = PlayOrKeyPress(h, Resource.Get(Assembly, "Assets.Videos.Intro.A04.FLI"), bre);
            bre = PlayOrKeyPress(h, Resource.Get(Assembly, "Assets.Videos.Intro.A05.FLI"), bre);
            bre = PlayOrKeyPress(h, Resource.Get(Assembly, "Assets.Videos.Intro.A06.FLI"), bre);
                        
            var video = h.StartVideo(new FlicDecoder(Resource.Get(Assembly, "Assets.Videos.Intro.A07.FLI")));
            // h.WaitOrKeyPress(video);
            if (bre || h.WaitOrKeyPress(video) != Keys.None)
            {                
                video.GoToEnd();
            }

            h.StartSingleMenu(Global.MenuFont, menu =>
            {
                menu.Location = new Point(85, 140);
                menu.OnCursorMove((from, to) => h.PlayCursorMoveSound());

                menu.Add("Jugar", () =>
                {
                    h.PlayMenuOKSound();
                    h.FadeOff();
                    h.Set(new Dino1Scene());
                });

                menu.Add("Salir", () => 
                {
                    h.PlayMenuOKSound();
                    h.Exit();
                });                                
            });
        }

        private bool PlayOrKeyPress(SceneHandler h, Resource resource, bool bre)
        {
            if (bre)
            {
                return true;
            }
            var video = h.StartVideo(new FlicDecoder(resource));
            if (h.WaitOrKeyPress(video) != Keys.None)
            {
                return true;
            }
            video.Kill();
            return false;
        }
    }
}