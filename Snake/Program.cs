using Kriptok.Core;
using Snake.Entities;
using Kriptok.IO;
using Kriptok.Scenes;
using Kriptok.Views.Texts;
using System;
using System.Drawing;
using Kriptok;
using Kriptok.Drawing.Algebra;

namespace Snake
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
            private SnakeHead player;

            protected override void Run(SceneHandler h)
            {                
                // Pone el fondo de pantalla
                h.ScreenRegion.SetBackground(Assembly, "Resources.Background.png");

                Global.Record = MaxScore.Load();

                // Pone los textos de la puntuaci¢n y de los records
                h.Write(Global.Font, 9, 12, () => $"Puntos: {Global.Score}").LeftMiddle();
                h.Write(Global.Font, 312, 12, () => $"Record: {Global.Record}").RightMiddle();
#if DEBUG
                Config.Load<BaseConfiguration>().Mute();                
#endif
                h.PlayMusic(Assembly, "Animal.mid");

                // Crea la cabeza del gusano que maneja todo el cuerpo
                player = h.Add(new SnakeHead());
                h.Add(new AppleCreator());
            }

            protected override void OnMessage(SceneHandler h, object message)
            {
                base.OnMessage(h, message);

                if ((string)message == "Reset")
                {                
                    h.PlaySoundSync(Assembly, "Resources.Down.wav");                    
                    player.Kill();                    

                    // Apago la pantalla.
                    h.FadeOff();

                    // Comprueba si se ha superado el record y lo actualiza                    
                    Global.Record = MaxScore.CheckAndSave(Global.Score);

                    // Reinicia las variable de puntos y longitud de cola
                    Global.Score = 0;
                    Global.SnakeLength = 8;
                    
                    // Vuelvo a iniciar la pantalla.
                    h.FadeOn();

                    // Crea el nuevo gusano.
                    player = h.Add(new SnakeHead());
                }
            }
        }
    }
}
