using Kriptok.Common;
using Kriptok.Scenes;
using Kriptok.Views.Videos;
using PerdidoEnElTiempo.Scenes.Base;
using System.Drawing;

namespace PerdidoEnElTiempo.Scenes
{
    internal class Space6Scene : VideoSceneBase
    {
        protected override void Run(SceneHandler h)
        {            
            PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Space.A58.FLI"));
            var lastVid = PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Space.A57.FLI"), false);

            // Limpio el buffer de teclas.
            h.WaitOrKeyPress(1);
            h.StartSingleMenu(Global.MenuFont, menu =>
            {
                menu.Location = Global.MenuPlace;

                menu.CloseOnSelection = true;
                menu.OnCursorMove((from, to) => h.PlayCursorMoveSound());

                menu.Add("Retrocedes.", () =>
                {
                    h.PlayMenuOKSound();

                    lastVid.Kill();
                    PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Space.A59.FLI"));
                    h.Add(new Frame());
                    PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Space.A60.FLI"));
                    PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Space.A61.FLI"), false);
                    GameOver(h, 0);
                });

                menu.Add("Le atacas.", () =>
                {
                    h.PlayMenuOKSound();

                    lastVid.Kill();
                    PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Space.A62.FLI"));                    
                    PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Space.A63.FLI"));
                    PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Space.A64.FLI"), false);

                    h.Set(new Space7Scene());
                });
            });
        }
    }
}