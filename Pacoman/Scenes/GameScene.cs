using Kriptok.Drawing;
using Kriptok.Extensions;
using Kriptok.Helpers;
using Kriptok.IO;
using Kriptok.Regions.Screen;
using Kriptok.Scenes;
using Pacoman.Entities;
using System.Drawing;

namespace Pacoman.Scenes
{
    public enum GameMessage
    {
        NextLevel = 0,
        Killed = 1,
        // EatedBlinker = 2,
        BackToIntro = 3
    }

    public class GameScene : SceneBase
    {
        private readonly int level;
        private readonly int innerLevel;
        private ScreenRegionBackground background;
        private FastBitmap8 hardnesses;

        public GameScene(int level)
        {
            this.level = level;
            this.innerLevel = (level + 1) % 11;
        }

        protected override void Run(SceneHandler h)
        {
            // Reseteo la cantidad de puntos comidos.
            Global.Balls = 0;

            background = h.ScreenRegion.SetBackground(bg =>
            {
                bg.UsingGraphics(g => BitmapHelper.UsingBitmap(Assembly, "Assets.Images.Title.png", bmp =>
                {
                    g.Clear(Color.Black);

                    bmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    g.DrawImage(bmp, 24.5f, 43.25f);

                    bmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    g.DrawImage(bmp, 553.5f, 43.25f);
                }));

                bg.Blit(Assembly, "Assets.Images.Board.png", 105, 0);
            });

            hardnesses = new FastBitmap8(typeof(Paco).Assembly, "Assets.Images.BoardHardness.png");

            // Los objetos que hacen parpadear las píldoras grandes.
            CreateBlinkers(h);

            // Creo las vidas.
            CreateLives(h);

            // Escribe la puntuacion.
            h.Write(Global.Font, 100, 0, () => Global.Score).RightTop();

            // h.While(() => Global.LiveCount >= 0, () =>
            // {
            // Encender la pantalla.
            h.FadeOn();

            ReStart(h);

            // h.While(() => player.IsAlive(), () => CheckScore(h));

            // h.FadeOff();

            // KillAll(h);
            //});
            //
            //Global.MaxScore = MaxScore.CheckAndSave(Global.Score);
            //
            //h.Set(new IntroScene());
        }

        private void ReStart(SceneHandler h)
        {
            var text0 = h.Write(Global.Font, 320, 243, "!Preparado!").CenterBottom();
            var text1 = h.Write(Global.Font, 300, 152, "Nivel").CenterBottom();
            var text2 = h.Write(Global.Font, 376, 152, (level + 1).ToString()).CenterBottom();

            // Inicia el sonido de entrada e imprime los texto necesarios            
            h.PlaySound(typeof(GameScene).Assembly, "Sounds.Start.wav");

            // Espera
            h.WaitFrames(150);

            // Borra los textos.
            text0.Die();
            text1.Die();
            text2.Die();

            // Crea a pacoman y a los fantasma
            var player = h.Add(new Paco(background, hardnesses, level));
            h.Add(new Ghost(hardnesses, 320, 177, 0, level));
            h.Add(new Ghost(hardnesses, 290, 223, 1, level));
            h.Add(new Ghost(hardnesses, 320, 223, 2, level));
            h.Add(new Ghost(hardnesses, 352, 223, 3, level));
        }

        private static void KillAll(SceneHandler h)
        {
            h.Kill<Paco>();
            h.Kill<Ghost>();
            h.Kill<Eyes>();
            h.Kill<Fruit>();
        }

        protected override void OnMessage(SceneHandler h, object message)
        {
            base.OnMessage(h, message);

            if (message is GameMessage msg)
            {
                switch (msg)
                {
                    case GameMessage.NextLevel:
                        // Incremente el nivel (fase)                   
                        h.StopAllSounds();
                        h.Kill<Ghost>();
                        h.Kill<Eyes>();
                        h.Wait(1000);
                        h.FadeOff();
                        KillAll(h);
                        h.Set(new GameScene(level + 1));
                        return;
                    case GameMessage.Killed:
                        h.Wait(1000);

                        if (Global.LiveCount >= 0)
                        {
                            ReStart(h);
                        }
                        else
                        {
                            Global.MaxScore = MaxScore.CheckAndSave(Global.Score);
                            h.Set(new IntroScene());
                        }
                        return;
                    case GameMessage.BackToIntro:
                        {
                            h.StopMusic();
                            h.FadeOff();
                            h.Set(new IntroScene());
                            break;
                        }
                }
            }
        }

        internal static void CreateBlinkers(SceneHandler h)
        {
            // Pone los parpados de los puntos grandes
            h.Add(new Blinker(128, 56));  
            h.Add(new Blinker(512, 56));
            h.Add(new Blinker(128, 364));
            h.Add(new Blinker(512, 364));
        }

        private void CreateLives(SceneHandler h)
        {
            for (int i = 0; i < Global.LiveCount; i++)
            {
                Global.Lives[i + 1] = h.Add(new Life((i * 26) + 552));
            }            
        }
    }
}
