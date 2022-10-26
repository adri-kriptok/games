using Kriptok.Common;
using Kriptok.Entities.Base;
using Kriptok.Scenes;
using Kriptok.Views.Sprites;
using Kriptok.Views.Videos;
using PerdidoEnElTiempo.Scenes.Base;
using System.Drawing;

namespace PerdidoEnElTiempo.Scenes
{
    internal class Space4Scene : VideoSceneBase
    {
        protected override void Run(SceneHandler h)
        {
            h.ScreenRegion.SetBackground(Assembly, "Assets.Images.choiceship.png");

            var entities = AutoDestructCheck(h);

            h.FadeOn();

            // Limpio el buffer de teclas.
            h.WaitOrKeyPress(1);
            h.StartSingleMenu(Global.MenuFont, menu =>
            {
                menu.Location = Global.MenuPlace;

                menu.CloseOnSelection = true;
                menu.OnCursorMove((from, to) => h.PlayCursorMoveSound());

                menu.Add("Nave estelar \"Aguila 2\" [roja/blanca].", () =>
                {
                    h.PlayMenuOKSound();
                    h.Set(new Space5Scene());
                });

                menu.Add("Nave experimental \"W23\" [azul].", () =>
                {
                    h.PlayMenuOKSound();
                    
                    // Detengo la autodestrucción.
                    if (entities.Item1 != null)
                    {
                        entities.Item1.Die();
                        entities.Item2.Die();
                    }

                    h.Add(new Frame());
                    PlayVideo(h, Resource.Get(Assembly, "Assets.Videos.Space.A45.FLI"), false);
                    GameOver(h, 0);
                });
            });
        }
    }
}