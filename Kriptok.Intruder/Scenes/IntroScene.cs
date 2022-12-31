using Kriptok.Intruder.Scenes.Maps.Map01_TheBeach;
using Kriptok.Intruder.Scenes.Missions;
using Kriptok.Scenes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Intruder.Scenes
{
    class IntroScene : SceneBase
    {
        protected override void Run(SceneHandler h)
        {                        
            h.PlayMusic(Assembly, "Assets.Music.Bass1.mid", volume: 1);
            h.FadeOff(255);
            h.ScreenRegion.SetBackground(sb => sb.BlitCentered(Assembly, "Assets.Images.Menues.Kriptok.png"));
            
            h.FadeOn();
            h.Wait(2000);
            h.FadeOff();
            
            
            h.Wait(1000);
            h.FadeOff(255);
            h.ScreenRegion.SetBackground(Assembly, "Assets.Images.Menues.Title.png");

#if DEBUG
            h.FadeOn(255);
#else
            h.FadeOn(1);
#endif

            h.StartSingleMenu(IntruderConsts.MenuFont, mb =>
            {
                mb.OnCursorMove((i, j) => h.PlayCursorMoveSound());
                mb.Location = new Point(140, 120);              

                mb.Add("Start", () =>
                {
                    h.PlayMenuOKSound();
                    h.StopMusic();
                    h.FadeOff();
                    h.Set(new Mission01_TheBeach());
                });

                mb.Add("Exit", () =>
                {
                    h.PlayMenuOKSound();
                    h.StopMusic();
                    h.FadeOff();
                    h.Exit();
                });
            });

        }
    }
}
