using Kriptok.Helpers;
using Kriptok.IO;
using Kriptok.Objects;
using Kriptok.Objects.Base;
using Kriptok.Scenes;
using Kriptok.Views;
using Kriptok.Views.Shapes;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Kriptok.Asteroids
{
    class TitleScene : SceneBase
    {
        protected override void Init(SceneInitializer init)
        {
            base.Init(init);
#if DEBUG
            Config.Load<BaseConfiguration>().Mute();
#endif
            Global.Record = MaxScore.Load();
        }

        protected override void Run(SceneHandler h)
        {
            // Crea las estrellas del fondo de la pantalla            
            h.ScreenRegion.SetBackground(bg =>
            {
                for (var i = 0; i <= 499; i++)
                {
                    bg.Plot(
                        h.Rand(0, h.ScreenRegion.Size.Width - 1),
                        h.Rand(0, h.ScreenRegion.Size.Height - 1), Color.White);
                }
            });

            WriteTexts(h);

            // Inicia el sonido del motor de la nave            
            h.PlaySound(Assembly, "NAVE.WAV");

            // Inicia variables necesarias
            // Puntuación, Fase, Vidas
            Global.Score = 0;            
            Global.LivesCount = 3;

            // Pone el título del juego.
            var title = h.Add(new Title());

            // Pone un texto explicativo
            var text = h.Add(new PressKeyMessage());

            // Apaga pantalla
            h.FadeOn();

            // Espera hasta que se suelte cualquier tecla.
            if (h.WaitForKeyPress() == Keys.Escape)
            {
                // Si se pulso escape, sale del juego.
                // Apaga la pantalla.
                h.FadeOff();
                h.Exit();
            }
            else
            {
                h.FadeOff();
                h.Set(new PlayScene(1));
            }
        }

        internal static void WriteTexts(SceneHandler h)
        {
            var screenSize = h.ScreenRegion.Size;

            // Escribe los textos de presentaci¢n            
            h.Write(Global.GreenFont, screenSize.Width, screenSize.Height, "[←] [↑] [→] [↓]: movimiento [SPACE]: disparo [H]: hiperespacio").RightBottom();
            h.Write(Global.YellowFont, screenSize.Width, 0, () => $"Record: {Global.Record}").RightTop();
            h.Write(Global.CyanFont, screenSize.Width, 24, () => $"Puntos: {Global.Score}").RightTop();            
        }
    }

    public class Title : ObjectBase<WireframeTextView>
    {
        private float angle = 0f;

        public Title()
            : base(new WireframeTextView(GetFont(), "ASTEROIDS", false, GetStroke()))
        {
            View.Scale.X = 7.5f;
            View.Scale.Y = 7.5f;
            View.Scale.Z = 25;

            Angle.X = 0.5f;
        }

        protected override void OnStart(ObjectStartHandler h)
        {
            base.OnStart(h);

            Location.X = h.RegionSize.Width / 2;
            Location.Y = h.RegionSize.Height / 3;
        }

        private static Font GetFont()
        {
            return new Font(Global.Font, FontStyle.Bold);
        }

        private static IStroke GetStroke()
        {
            return Strokes.Get(Color.Cyan, 2f, DashStyle.Solid);
        }

        protected override void OnFrame()
        {
            angle += 0.05f;
            var mod = (float)(Math.Sin(angle)) * 0.1f;

            View.Scale.X = 7.5f + mod;
            View.Scale.Y = 7.5f + mod;
            View.Scale.Z = 25 + 5 * mod;
        }
    }

    public class PressKeyMessage : TextObject
    {
        private int counter = 0;
        private bool visible = true;

        protected internal PressKeyMessage() : base(Global.YellowFont, "PRESIONE UNA TECLA PARA JUGAR")
        {
            SetAlign(ShapeAlignEnum.Center, ShapeVerticalAlignEnum.Middle);
        }

        protected override void OnStart(ObjectStartHandler h)
        {
            base.OnStart(h);

            Location.X = h.RegionSize.Width / 2;
            Location.Y = h.RegionSize.Height * 2f / 3f;
        }

        protected override void OnFrame()
        {
            base.OnFrame();

            if (counter++ % 10 == 0)
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
