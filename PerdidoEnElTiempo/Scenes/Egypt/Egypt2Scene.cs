using Kriptok.Common;
using Kriptok.Entities.Base;
using Kriptok.Scenes;
using Kriptok.Views.Sprites;
using Kriptok.Views.Videos;
using PerdidoEnElTiempo.Scenes.Base;
using System.Drawing;

namespace PerdidoEnElTiempo.Scenes
{
    internal class Egypt2Scene : VideoSceneBase
    {
        protected override void Run(SceneHandler h)
        {
            h.FadeOn();
            var text = h.Write(Global.MenuFont,
                h.ScreenRegion.Size.Width / 2,
                h.ScreenRegion.Size.Height / 2, "mas tarde...").CenterMiddle();
            h.Wait(2000);
            h.FadeOff();
            text.Die();
            h.ScreenRegion.SetBackground(Assembly, "Assets.Images.choice2doors.png");
            h.FadeOn();

            // Limpio el buffer de teclas.
            h.WaitOrKeyPress(1);

            h.StartSingleMenu(Global.MenuFont, menu =>
            {
                menu.Location = Global.MenuPlace;

                menu.CloseOnSelection = true;
                menu.OnCursorMove((from, to) => h.PlayCursorMoveSound());

                menu.Add("Entrar por la izquierda.", () =>
                {
                    h.PlayMenuOKSound();                   
                    h.Set(new Egypt3Scene());
                });
                
                menu.Add("Entrar por la derecha.", () =>
                {
                    h.PlayMenuOKSound();                    
                    h.Set(new Egypt4Scene());                    
                });
            });
        }
    }
}