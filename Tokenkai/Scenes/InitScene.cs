using Kriptok.Core;
using Kriptok.Div;
using Kriptok.Div.Extensions;
using Kriptok.Entities.Base;
using Kriptok.IO;
using Kriptok.Scenes;
using Kriptok.Views.Sprites;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Tokenkai.Scenes
{
    internal class InitScene : SceneBase
    {
        protected override void Run(SceneHandler h)
        {
            var config = Config.Load<BaseConfiguration>();
#if DEBUG
            config.Mute();
#endif
            h.FadeOff();

            h.PlayMusic(Assembly, "Assets.Music.Theme.s3m", true);
            h.ScreenRegion.SetBackground(Assembly, "Assets.Menu.Title.png");
            h.FadeOn();

            h.WaitForKeyOrMouse();
            h.FadeOff();
            h.ScreenRegion.SetBackground(Assembly, "Assets.Menu.Credits.png");
            h.FadeOn();
            h.Add(new ShakeEffect());
            h.WaitForKeyOrMouse();

            h.FadeTo(Color.Red, 8);
  
            h.Set(new MenuScene());
        }

        private class ShakeEffect : EntityBase<SpriteView>
        {
            public ShakeEffect() : base(new SpriteView(typeof(ShakeEffect).Assembly, "Assets.Menu.Credits.png")
            {
                Alpha = 0.5f,
                Center = new PointF(0f, 0f)
            })
            {
            }

            protected override void OnFrame()
            {
                if (Rand.Next(0, 35) == 0)
                {
                    Location.X += Rand.Next(-32, 32) * 0.5f;
                }

                if (Location.X > 0)
                {
                    Location.X -= 0.5f;
                }

                if (Location.X < 0)
                {
                    Location.X += 0.5f;
                }
            }
        }
    }
}
