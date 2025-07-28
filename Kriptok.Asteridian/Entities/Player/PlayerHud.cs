using Kriptok.Asteridian.Entities.Player.Weapons.Front;
using Kriptok.Asteridian.Scenes.Base;
using Kriptok.Drawing;
using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Helpers;
using Kriptok.Regions.Context.Base;
using Kriptok.Views.Base;
using Kriptok.Views.Gdip;
using Kriptok.Views.Gdip.Base;
using Kriptok.Views.Sprites;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Asteridian.Entities.Player
{
    internal class PlayerHud : EntityBase
    {
        public PlayerHud(LevelSceneBase playerShip) : base(new HudView(playerShip))
        {
        }

        protected override void OnFrame()
        {
            
        }

        private class HudView : ViewBase, IRenderizable
        {
            private static readonly Pen fuchsiaPen = Pens.Fuchsia;
            private static readonly Pen whitePen = Pens.White;
            private static readonly Pen cyanPen = Pens.Cyan;                        

            private static readonly Font baseFont = new Font(GlobalConsts.DefaultFontFamily, 8f);

            private readonly PlayerShip playerShip;
            private readonly int relativeX;


            
            private readonly Rectangle baseRectangle;
            private readonly Rectangle generatorRect;
            private readonly Rectangle generatorBarRect;
            private readonly LinearGradientBrush generatorBrush = null;
            private readonly LinearGradientBrush fontBrush = null;

            public HudView(LevelSceneBase level)
            {
                this.playerShip = level.PlayerShip;
                this.relativeX = GlobalConsts.ScreenSize.Width - GlobalConsts.HudWidth;
                
                this.baseRectangle = new Rectangle(relativeX, 0, GlobalConsts.HudWidth-1, GlobalConsts.ScreenSize.Height-1);

                this.generatorRect = new Rectangle(relativeX + 03, 3, 15, 94);
                this.generatorBarRect = new Rectangle(generatorRect.X + 3, generatorRect.Y + 3, generatorRect.Width - 5, generatorRect.Height - 5);
                this.generatorBrush = new LinearGradientBrush(generatorBarRect, Color.Yellow, Color.Red, LinearGradientMode.Vertical);

                this.fontBrush = new LinearGradientBrush(new RectangleF(0f, 0f, 60f, 1f), Color.White, Color.Cyan, LinearGradientMode.Horizontal);
            }

            public override void AddViews(IRenderBufferContext context, IList<IRenderizable> views) => views.Add(this);            

            public float ClosestPriority(IProjector context) => 0f;

            public BoundF2 GetLastRenderBoundingBox() => throw new NotImplementedException();

            public float GetPriority(IProjector context) => 0f;

            private float brushInc = 0f;

            public void RenderOn(IRenderContext context)
            {
                var g = context.Graphics;

                // Incremento el movimiento de la brocha.
                brushInc = (brushInc + 5f) % 60f;

                g.DrawRectangle(fuchsiaPen, baseRectangle);

                int textX = relativeX + 21;
                g.DrawRectangle(cyanPen, new Rectangle(textX, 03, 35, 22));                
                g.DrawRectangle(cyanPen, new Rectangle(textX, 27, 35, 22));
                g.DrawRectangle(cyanPen, new Rectangle(textX, 51, 35, 22));
                g.DrawRectangle(cyanPen, new Rectangle(textX, 75, 35, 22));
                Write(g, 22, 03, "FRONT");
                Write(g, 22, 27, "REAR");
                Write(g, 22, 51, "L-SD");
                Write(g, 22, 75, "R-SD");


                g.DrawRectangle(whitePen, generatorRect);                               
                g.DrawRectangle(whitePen, new Rectangle(relativeX + 3 /*+ 22*/, 102, 15, 74));
                g.DrawRectangle(whitePen, new Rectangle(relativeX + 41, 102, 15, 74));
                g.FillRectangle(generatorBrush, generatorBarRect);


            }

            private void Write(Graphics g, int x, int y, string text)
            {
                // var rect = g.MeasureString(text, baseFont);

                fontBrush.ResetTransform();
                // fontBrush.ScaleTransform(rect.Width, rect.Height);
                // fontBrush.RotateTransform(180f);



                var x2 = relativeX + x;
                fontBrush.TranslateTransform(x2 + brushInc, y);
                //fontBrush.TranslateTransform(x2 + 0.2f, y);
                g.DrawString(text, baseFont, fontBrush, x2, y);
            }
        }
    }
}
