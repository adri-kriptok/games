using Kriptok.Scenes;
using Kriptok.Views.Texts;
using System.Drawing;

namespace Fostiator.Scenes
{
    internal class CreditsScene : SceneBase
    {
        private static readonly SuperFont creditsFont = SuperFont.Build(builder =>
        {
            builder.Font = new Font("Arial", 14);
            builder.SetColor(Color.White);
            builder.SetShadow(0, 1, Color.Gray);
        });

        protected override void Run(SceneHandler h)
        {
            // Carga la pantalla de fondo.
            h.ScreenRegion.SetBackground(typeof(CreditsScene).Assembly, "Assets.Backgrounds.credits.png");

            var texts = new string[]
            {
               "PROGRAMADOR","DANIEL NAVARRO","",
                "GRAFISTAS","JOSE FERNANDEZ","RAFAEL BARRASO","",
                "SONIDOS","ANTONIO MARCHAL","",
                "JUGABILIDAD","LUIS F. FERNANDEZ","",
                "AGRADECIMIENTOS","FERNANDO PEREZ",
                "JORGE SANCHEZ","JAVIER CARRION","",
                "FOSTIATOR","DIV GAMES STUDIO"
            };

            for (int i = 0; i <= 19; i++)
            {                
                h.Write(creditsFont, 320, 40 + i * 20, texts[i]).CenterMiddle();

            }
            
            h.FadeOn();
            h.WaitForKeyPress();
            h.FadeOff();
            h.Exit();
        }
    }
}