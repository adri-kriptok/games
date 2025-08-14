using Kriptok.Asteridian.Entities.Enemies.Base;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions;
using Kriptok.Helpers;
using Kriptok.Views;
using Kriptok.Views.Base;
using Kriptok.Views.Gdip;
using Kriptok.Views.Primitives;
using Kriptok.Views.Shapes;
using Kriptok.Views.Texts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Asteridian.Entities.Enemies
{
    internal class AsciiInvader : EnemyBase
    {
        public AsciiInvader(float x) : base(100f, new SpaceInvaderView())
        {
            Location.X = x;
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);

            if (View is TextView)
            {
                Radius = 11;
                h.CollisionType = Collision2DTypeEnum.Radius;
            }
            else
            {
                h.CollisionType = Collision2DTypeEnum.Auto;
            }
        }

        protected override void OnFrame()
        {
            base.OnFrame();
            Angle.Z -= 0.1f;
        }

        internal override void StartOnTop(float y)
        {
            var height = ((GdipViewBase)View).GetRectangle().Height;

            Location.Y = y + height * ((GdipViewBase)View).Center.Y - height;
        }

        private class AsciiInvaderViewBase : TextView
        {
            public AsciiInvaderViewBase(SuperFont font, string message) : base(font, message)
            {
            }
        }

        private class SpaceInvaderView2 : AsciiInvaderViewBase
        {
            private static readonly SuperFont superFont = SuperFont.Build(fb =>
            {
                fb.SetColor(Color.FromArgb(0, 255, 0));
                //fb.Font  = new Font("Arial", 19f);
                fb.Font = new Font(GlobalConsts.EightBitFontFamily, 15f, FontStyle.Bold);                
                fb.Border = Strokes.Red;
            });

            public SpaceInvaderView2() : base(superFont, "♀")
            {
                Flip = FlipEnum.FlipY;
            }
        }

        private class SpaceInvaderView3 : AsciiInvaderViewBase
        {
            private static readonly SuperFont superFont = SuperFont.Build(fb =>
            {
                fb.SetColor(Color.FromArgb(0, 255, 0));
                fb.Font = new Font("Arial", 15f, FontStyle.Bold | FontStyle.Underline | FontStyle.Strikeout);
            });

            public SpaceInvaderView3() : base(superFont, "U")
            {
                Flip = FlipEnum.FlipY;
            }
        }

        private class SpaceInvaderView : AsciiInvaderViewBase
        {
            private static readonly SuperFont superFont = SuperFont.Build(fb =>
            {
                fb.SetColor(Color.FromArgb(0, 0, 255));
                fb.Font  = new Font("Arial", 15f);
                //fb.Font = new Font("Arial", 18f, FontStyle.Bold | FontStyle.Strikeout);
                //fb.Font = new Font("Times New Roman", 24f;
                fb.Border = Strokes.Red;
            });

            public SpaceInvaderView()
                : base(superFont, "§")
            // : base(superFont, "♠♣♦♥")
            //: base(superFont, "♂♀♪♫☼►♠♣♦♥☻☺")
            //: base(superFont, "☺☻♥♦♣♠•◘○◙♂♀♪")
            // : base(superFont, "◙")
            // : base(superFont, ((char)8).ToString())
            {
                // Flip = FlipEnum.FlipY;
            }
        }

        private class HeartAsciiView : AsciiInvaderViewBase
        {
            private static readonly SuperFont superFont = SuperFont.Build(fb =>
            {
                fb.SetColor(Color.Red, Color.DarkRed);
                fb.Font = new Font("Times New Roman", 23f);
                fb.Border = Strokes.Get(Color.Red);
            });

            public HeartAsciiView() : base(superFont, "♥")
            {
            }
        }

        private class SpadeAsciiView : AsciiInvaderViewBase
        {
            private static readonly SuperFont superFont = SuperFont.Build(fb =>
            {
                fb.SetColor(Color.DarkBlue, Color.CornflowerBlue);
                fb.Font = new Font("Times New Roman", 25f);
                fb.Border = Strokes.Get(Color.Blue);
            });

            public SpadeAsciiView() : base(superFont, "♠")
            {
                Flip = FlipEnum.FlipY;
            }
        }

        private class ClubAsciiView : AsciiInvaderViewBase
        {
            private static readonly SuperFont superFont = SuperFont.Build(fb =>
            {
                fb.SetColor(Color.Green, Color.LightGreen);
                fb.Font = new Font("Times New Roman", 24f);
                fb.Border = Strokes.Get(Color.LimeGreen);
            });

            public ClubAsciiView() : base(superFont, "♣")
            {
                Flip = FlipEnum.FlipY;
            }
        }


        private class DiamondAsciiView : RectangleView
        {
            public DiamondAsciiView() : base(16, 16, new FillConfig(Color.White, Color.Yellow, GradientDirectionEnum.Horizontal), Strokes.Get(Color.Orange))
            {                
                ReScaleX = 0.75f;
            }

            public override void SetOwner(IViewOwner entity)
            {
                base.SetOwner(entity);

                ((EntityBase)entity).Angle.Z = MathHelper.QuarterPIF;
            }
        }
    }
}
