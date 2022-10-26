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
            var bre = PlayOrKeyPress(h, Resource.Get(Assembly, "Assets.Videos.Intro.A01.FLI"), () => h.FadeOn(), false);
            bre = PlayOrKeyPress(h, Resource.Get(Assembly, "Assets.Videos.Intro.A02.FLI"), null, bre);
            bre = PlayOrKeyPress(h, Resource.Get(Assembly, "Assets.Videos.Intro.A03.FLI"), null, bre);
            bre = PlayOrKeyPress(h, Resource.Get(Assembly, "Assets.Videos.Intro.A04.FLI"), null, bre);
            bre = PlayOrKeyPress(h, Resource.Get(Assembly, "Assets.Videos.Intro.A05.FLI"), null, bre);
            bre = PlayOrKeyPress(h, Resource.Get(Assembly, "Assets.Videos.Intro.A06.FLI"), null, bre);
                        
            var video = h.StartVideo(new FlicDecoder(Resource.Get(Assembly, "Assets.Videos.Intro.A07.FLI")));
            
            if (bre || h.WaitOrKeyPress(video) != Keys.None)
            {                
                video.GoToEnd();
            }

            h.Wait(500);

            // Limpio el buffer de teclas.
            h.WaitOrKeyPress(1);
            h.StartSingleMenu(Global.MenuFont, menu =>
            {
                menu.Location = new Point(85, 140);
                menu.OnCursorMove((from, to) => h.PlayCursorMoveSound());

                menu.Add("Jugar", () =>
                {
                    h.PlayMenuOKSound();
                    h.FadeTo(Color.White);
                    h.Set(new Dino1Scene());
                });

                menu.Add("Salir", () => 
                {
                    h.PlayMenuOKSound();
                    h.Exit();
                });                                
            });
        }

        private bool PlayOrKeyPress(SceneHandler h, Resource resource, Action beforeStart, bool bre)
        {
            if (bre)
            {
                return true;
            }
            var video = h.StartVideo(new FlicDecoder(resource));

            if (beforeStart != null)
            {
                beforeStart();
            }

            if (h.WaitOrKeyPress(video) != Keys.None)
            {
                return true;
            }
            video.Kill();
            return false;
        }
    }
}