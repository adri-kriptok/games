using Kriptok.Common;
using Kriptok.Core;
using Kriptok.Drawing.Algebra;
using Kriptok.Extensions;
using Kriptok.Pokefight.Common;
using Kriptok.IO;
using Kriptok.Regions.Context.Base;
using Kriptok.Regions.Scroll;
using Kriptok.Regions.Scroll.Base;
using Kriptok.Entities;
using Kriptok.Scenes;
using Kriptok.Views.Sprites;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Kriptok.Pokefight.Scenes
{
    class IntroScene : SceneBase
    {
        protected override void Init(SceneInitializer init)
        {
            base.Init(init);

            var config = Config.Load<Configuration>();
#if DEBUG
            config.Mute();
#endif
        }

        protected override void Run(SceneHandler h)
        {
            h.FadeOff(255);
            h.PlayMusic(Assembly, "Music.FallenAngel.S3M", true, 24000);
            h.WaitOrKeyPress(1000);
            h.ScreenRegion.SetBackground(bg => bg.BlitCentered(Assembly, "Resources.Images.Kriptok.png"));
            h.FadeOn(16);
            h.WaitOrKeyPress(3000);
            h.FadeOff(16);
            h.ScreenRegion.ClearBackground();
            h.WaitOrKeyPress(1000);

            var scroll = h.StartScroll(new LavaScrollView(h.ScreenRegion.Rectangle)
            {
                Priority = -1000
            });
            scroll.SetTarget(new IntroTarget());

            h.Add(new BasicObject(new SpriteView(Assembly, "Resources.Images.pokefight2.png"))
            {
                Location = h.ScreenRegion.Center().ToVector3(0f)
            }); 

            h.FadeOn(255);

            h.WaitForKeyPress();

            h.StartSingleMenu(Program.DefaultFont, m =>
            {
                m.Location = new Point(135, 125);

                m.Add("Pelear", () =>
                {
                    h.PlayMenuOKSound();
                    h.FadeOff();
                    h.Set(new BattleScene());
                });

                m.Add("Salir", () => 
                {
                    h.PlayMenuOKSound();
                    h.FadeOff();
                    h.Exit();
                });

                m.OnCursorMove((from, to) => h.PlayCursorMoveSound());
            });
        }

        private class LavaScrollView : GdipBrushScanlineScrollLayer
        {
            private float angle = 0f;

            public LavaScrollView(Rectangle region) : base(region,
                Resource.Get(typeof(LavaScrollView).Assembly, "Resources.Images.Lava.png"), true, true)
            {
                Priority = -1000;
                //Antialias = true;
            }

            protected override void OnRendering(IRenderContext context)
            {
                base.OnRendering(context);

                angle += 0.25f;
            }

            protected override void OnScanline(IRenderContext context, Matrix transform, int y)
            {
                base.OnScanline(context, transform, y);

                var sin = (float)Math.Sin(y * 0.25f + angle);
                var cos = (float)Math.Cos(y * 0.25f + angle);

                transform.Rotate(sin);

                transform.Scale(
                    1f + 0.025f * cos,
                    1f + 0.025f * sin);

                transform.Translate(3f * cos, 5f * sin);
            }
        }
    }

    internal class IntroTarget : IScrollTarget
    {
        private Vector2F location;
        public Vector2F GetLocation2D()
        {
            location.Y += 2;
            return location;
        }
    }
}
