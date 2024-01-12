using Kriptok.Scenes;
using Kriptok.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fostiator.Scenes
{
    public class IntroScene : SceneBase
    {
        protected override void Run(SceneHandler h)
        {
            h.FadeTo(Color.White, 255);
            h.PlaySound(Assembly, "Sound.INTROHIT.WAV");

            h.ScreenRegion.SetBackground(typeof(IntroScene).Assembly, "Assets.Backgrounds.intro.png");

            h.FadeFrom(Color.White, 4);

            h.WaitForKeyPress();

            h.FadeOff();

            h.ScreenRegion.SetBackground(typeof(IntroScene).Assembly, "Assets.Backgrounds.menu.png");

            h.FadeOn();

            while (true)
            {
                switch (h.WaitForKeyPress())
                {
                    case Keys.D1:
                        h.FadeOff();
                        h.Set(new OptionsScene());
                        return;
                    case Keys.D2:
                        h.FadeOff();
                        h.Set(new CreditsScene());
                        return;
                }
            }
        }
    }
}
