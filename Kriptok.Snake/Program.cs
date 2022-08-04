using Kriptok.Core;
using Kriptok.Snake.Entities;
using Kriptok.IO;
using Kriptok.Scenes;
using Kriptok.Views.Texts;
using System;
using System.Drawing;

namespace Kriptok.Snake
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Engine.Start(new GameScene(), s =>
            {
                s.FullScreen = false;    
                s.Mode = WindowSizeEnum.M320x200;
                s.Title = "Snake | Kriptok";
                s.TimerInterval = 45;
                s.ExtractMidiPlayer();
            });
        }

        class GameScene : SceneBase
        {
            protected override void Run(SceneHandler h)
            {                
                // Pone el fondo de pantalla
                h.ScreenRegion.SetBackground(Assembly, "Resources.Background.png");

                var superFont = new SuperFont(new Font("Arial", 8, FontStyle.Italic | FontStyle.Bold), Color.White);

                Global.Record = MaxScore.Load();

                // Pone los textos de la puntuaci¢n y de los records
                h.Write(superFont, 9, 12, () => $"Puntos: {Global.Score}").LeftMiddle();
                h.Write(superFont, 312, 12, () => $"Record: {Global.Record}").RightMiddle();
#if DEBUG
                Config.Load<BaseConfiguration>().Mute();                
#endif
                h.PlayMusic(Assembly, "Animal.mid");

                // Crea la cabeza del gusano que maneja todo el cuerpo
                h.Add(new SnakeHead());
                h.Add(new AppleCreator());
            }

            protected override void OnMessage(SceneHandler h, object message)
            {
                base.OnMessage(h, message);

                if ((string)message == "Reset")
                {
                    // Apago la pantalla.
                    h.FadeOff();

                    // Vuelvo a iniciar la pantalla.
                    h.FadeOn();

                    // Crea el nuevo gusano.
                    h.Add(new SnakeHead());
                }
            }
        }
    }
}
