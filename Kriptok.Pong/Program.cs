using Kriptok.Scenes;
using Kriptok.Pong.Entities;
using Kriptok.IO;
using Kriptok.Core;
using Kriptok.Entities.Base;
using Kriptok.Views.Texts;
using System;
using System.Drawing;
using System.Windows.Forms;
using Kriptok.Views;
using Kriptok.Extensions;
using Kriptok.Entities;

namespace Kriptok.Pong
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Engine.Start(new InitScene(), s =>
            {
                s.FullScreen = false;
                s.Mode = WindowSizeEnum.M320x200;
                s.Title = "Pong | Kriptok";
            });
        }

        public static SuperFont GameFont = new SuperFont(new Font("Courier New", 8), Color.White)
            .SetShadow(1, 1, Color.FromArgb(64, 64, 255));        

        class InitScene : SceneBase
        {
            protected override void Run(SceneHandler h)
            {
#if DEBUG
                Config.Get<BaseConfiguration>().Mute();
#endif       
                h.PlayMusic(Assembly, "GameGirl.S3M", true);

                h.ScreenRegion.SetBackground(Assembly, "Background.png");

                var leftRacket = h.Add(new Racket(4, Keys.Q, Keys.A));
                var rightRacket = h.Add(new Racket(316, Keys.Up, Keys.Down));

                h.Add(new TextKiller(AddText(h, leftRacket)));
                h.Add(new TextKiller(AddText(h, rightRacket)));

                h.Add(new Ball(leftRacket, rightRacket));
            }

            private ITextEntity AddText(SceneHandler h, Racket racket)
            {
                var x = racket.Location.X.Round();

                var align = (x < h.ScreenRegion.Size.Width / 2) ? ShapeAlignEnum.Left : ShapeAlignEnum.Right;

                h.Write(Program.GameFont, x, 185, () => (racket.Points.ToString("000")))
                    .SetAlign(align, ShapeVerticalAlignEnum.Middle);

                var controlsMessage = (x < h.ScreenRegion.Size.Width / 2) ? "Utilizar:\n Q (arriba)\n A (abajo)" : "Utilizar:\n Cursores";

                return h.Write(Program.GameFont, x, 15, controlsMessage)
                    .SetAlign(align, ShapeVerticalAlignEnum.Top);                
            }

            private class TextKiller : EntityBase
            {
                private readonly ITextEntity textObject;
                private int counter = 0;

                public TextKiller(ITextEntity textObject)
                {
                    this.textObject = textObject;
                }

                protected override void OnFrame()
                {
                    if (counter++ >= 200)
                    {
                        textObject.Die();
                        Die();
                    }
                }
            }
        }
    }

}
