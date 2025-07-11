using Kriptok.Div;
using Kriptok.Div.Extensions;
using Kriptok.Scenes;
using Kriptok.Views.Texts;
using System.Drawing;
using System.Windows.Forms;

namespace Kriptok.Games.BlastemUp.Scenes
{
    class TitleScreenScene : SceneBase
    {
        /// <summary>
        /// Fuente para el título.
        /// </summary>
        private static readonly SuperFont titleFont = SuperFont.Build(builder =>
        {
            builder.Font = new Font("Bauhaus 93", 20);
            builder.SetColor(Color.Black, Color.White);
            builder.SetShadow(1, 1, Color.DarkBlue);
        });

        protected override void Run(SceneHandler h)
        {
            h.PlayDivMusic(MusicResourceEnum.EnTuSien, true, volume: 0.5f);        

            h.FadeOff(byte.MaxValue);

            // Pone la pantalla de fondo.            
            h.ScreenRegion.SetBackground(Assembly, "Assets.Images.Backgrounds.Title.png");

            // Textos del Menu General
            h.Write(titleFont, 8, 340, "1. Jugar").LeftTop();
            h.Write(titleFont, 8, 370, "2. Creditos").LeftTop();
            h.Write(titleFont, 8, 400, "3. Salir").LeftTop();
            h.Write(titleFont, 320, 460, "(c) 1997 DIV Games Studio - Hammer Technologies").CenterMiddle();

            h.FadeOn();

            h.Loop(() =>
            {
                switch (h.WaitForKeyPress())
                {
                    case Keys.D1:
                    case Keys.Space:
                    case Keys.Enter:
                        h.FadeOff();
                        h.Set(new MainGameScene());
                        break;
                    case Keys.D2:
                        h.FadeOff();
                        h.Set(new CreditsScene());
                        break;
                    case Keys.D3:
                    case Keys.Escape:
                        h.FadeOff();
                        h.Exit();
                        break;
                }
            });
        }
    }
}
