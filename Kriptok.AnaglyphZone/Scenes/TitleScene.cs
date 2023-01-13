using Kriptok.Audio;
using Kriptok.AZ;
using Kriptok.AZ.Entities;
using Kriptok.AZ.Entities.Enemies;
using Kriptok.AZ.Regions;
using Kriptok.AZ.Scenes;
using Kriptok.Div;
using Kriptok.Entities;
using Kriptok.Entities.Base;
using Kriptok.Extensions;
using Kriptok.Helpers;
using Kriptok.IO;
using Kriptok.Regions.Pseudo3D.Cameras;
using Kriptok.Scenes;
using Kriptok.Vector3D.Extensions;
using Kriptok.Views;
using Kriptok.Views.Shapes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Kriptok.AZ.Entities.PlayerShip;

namespace Kriptok.Vector3D.Scenes
{
    internal class TitleScene : SceneBase
    {
        protected override void Run(SceneHandler h)
        {
            // Arranca la música tecno.
            h.StopMusic();
            h.PlayMusic(new PlayMusicOptions(DivResources.Music("Tecno.xm"))
            {
                Loop = true
            });

            Global.Score = 0;

            var region = h.StartPseudo3D(new StarsRegion(h.ScreenRegion.Rectangle)
            {
                DistanceModifier = 5f
            });
            region.Ambience.SetLightSource(1f, 0f, 0f);
            region.Ambience.AbsoluteLight = true;

            region.SetCamera(new TitleCam(h.Add(region, new TitleShip())));

            h.Add(region, new Title());
            h.Add(new PressKeyMessage());

            h.WriteScoreboard();

            h.FadeOn();

            if (h.WaitForKeyPress() == Keys.Escape)
            {
                h.FadeOff();
                h.Exit();
            }
            else
            {
                h.PlayMenuOKSound();
                h.FadeOff();
                h.Set(new StarsScene());
            }
        }

        private class Title : EntityBase<WireframeTextView>
        {
            private float ang = 0f;

            public Title() : base(new WireframeTextView(AZConsts.DefaultFont, "ANAGLYPH ZONE", false, Strokes.Fuchsia)
            {
                ScaleX = 5f,
                ScaleY = 2f,
                ScaleZ = 2f
            })
            {
                Location.Z = 100;                
                Angle.Z = -MathHelper.HalfPIF;
            }

            protected override void OnFrame()
            {
                var modif = MathHelper.SinF(ang += 0.02f);
                var modif2 = MathHelper.CosF(ang);
                //Angle.Z = -MathHelper.HalfPIF + modif*0.5f;
                Location.Y = modif2 * 50f;
            }
        }

        private class TitleCam : Pseudo3DCustomizerCamera
        {
            public TitleCam(IPseudo3DTarget target) : base(target, 100f)
            {
                Height = 50f;
            }

            public override float GetDirection() => -MathHelper.HalfPIF;
        }       

        private class TitleShip : EntityBase<PlayerShipView>
        {
            public TitleShip() : base(new PlayerShipView())
            {
                Location.Y = 100f;
                Location.Z = 32.5f;
                View.ScaleX *= 0.5f;
                View.ScaleY *= 0.5f;
                View.ScaleZ *= 0.5f;
            }

            protected override void OnFrame()
            {
                Angle.Z += 0.01f;
                //Angle.X += 0.01f;
                Angle.Y = (float)Math.Sin(Angle.Z);
            }
        }

        public class PressKeyMessage : TextEntity
        {
            private int counter = 0;
            private bool visible = true;

            protected internal PressKeyMessage() : base(AZConsts.TextFont, "PRESS ANY KEY TO PLAY")
            {
                SetAlign(ShapeAlignEnum.Center, ShapeVerticalAlignEnum.Middle);
            }

            protected override void OnStart(EntityStartHandler h)
            {
                base.OnStart(h);

                Location.X = h.RegionSize.Width / 2;
                Location.Y = h.RegionSize.Height * 9f / 10f;
            }

            protected override void OnFrame()
            {
                base.OnFrame();

                if (counter++ % 20 == 0)
                {
                    if (visible)
                    {
                        View.ScaleX = 0f;
                    }
                    else
                    {
                        View.ScaleX = 1f;
                    }

                    visible = !visible;
                }
            }
        }
    }
}
