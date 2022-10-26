using Kriptok.Common;
using Kriptok.Scenes;
using Kriptok.Views.Videos;
using PerdidoEnElTiempo.Scenes.Base;
using System.Drawing;

namespace PerdidoEnElTiempo.Scenes
{
    internal class Space1Scene : VideoSceneBase
    {
        private bool jump;

        public Space1Scene()
        {
        }

        public Space1Scene(bool jump)
        {
            this.jump = jump;
        }

        protected override void Run(SceneHandler h)
        {
            // Si pierde, continúa desde acá.
            Global.State = 2;

            if (jump)
            {
                var v = h.StartVideo(new FlicDecoder(Resource.Get(Assembly, "Assets.Videos.Space.A41.FLI")));
                v.GoToEnd();
                h.FadeOn();
            }
            else
            {
                PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Space.A38.FLI"), () => h.FadeFrom(Color.White));
                PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Space.A39.FLI"));
                PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Space.A40.FLI"));
                PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Space.A41.FLI"), false);
            }

            // Limpio el buffer de teclas.
            h.WaitOrKeyPress(1);
            h.StartSingleMenu(Global.MenuFont, menu =>
            {
                menu.Location = Global.MenuPlace;

                menu.CloseOnSelection = true;
                menu.OnCursorMove((from, to) => h.PlayCursorMoveSound());

                menu.Add("Vas por la izquierda.", () =>
                {
                    h.PlayMenuOKSound();

                    h.FadeOff();
                    h.Set(new Space2Scene());
                });

                menu.Add("Vas por la derecha.", () =>
                {
                    h.PlayMenuOKSound();
                    h.FadeOff();
                    h.Set(new Space3Scene());
                });
            });
        }
    }
}