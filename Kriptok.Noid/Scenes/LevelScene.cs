using Kriptok.Audio;
using Kriptok.Maps.Tiles.Editor;
using Kriptok.Noid.Entities;
using Kriptok.Noid.Entities.Pills;
using Kriptok.Objects;
using Kriptok.Scenes;
using Kriptok.Views;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Kriptok.Noid.Scenes
{
    public enum LevelSceneMessages
    {
        /// <summary>
        /// Checkea si no quedan más ladrillos.
        /// </summary>
        CheckBricks = 0,

        /// <summary>
        /// Cuando se perdió la bola.
        /// </summary>
        Lose = 1, 

        /// <summary>
        /// Termina la demo y empieza el juego.
        /// </summary>
        StartGame = 3,

        /// <summary>
        /// Vuelve a la Intro del juego.
        /// </summary>
        BackToIntro = 4,

        /// <summary>
        /// Corta la demo y va a otro nivel.
        /// </summary>
        NextDemoLevel = 5,
    }

    public class LevelScene : SceneBase
    {
        /// <summary>
        /// Nivel a generar.
        /// </summary>
        private readonly int level;

        /// <summary>
        /// Indica si está en modo "demo".
        /// </summary>
        private readonly bool demo;

        private Racket racket;
        private Ball ball;

        public LevelScene(int level, bool demo)
        {            
            this.level = level;
            this.demo = demo;            
        }

        protected override void Run(SceneHandler h)
        {
            if (demo)
            {
                h.Add(h.ScreenRegion, new DemoText());
            }

            h.Write(Global.Font, 310, 180, () => Global.Score).RightTop();

            h.ScreenRegion.SetBackground(bg =>
            {
                bg.Blit(Assembly, $"Assets.Images.Backs.Back{((level - 1) % 4) + 1}.png", 0, 0);
                bg.Blit(Assembly, $"Assets.Images.ScoreBoard.png", 273, 0);
            });

            // Cargo el nivel.
            LoadLevel(h);
            
            // Arranco a jugar.
            Play(h);
        }

        private void Play(SceneHandler h)
        {
            CreateLives(h);

            // Objetos principales.
            racket = h.Add(new Racket(demo));
            racket.SetBall(ball = h.Add(new Ball(racket, true, demo)));

            // Prendo la pantalla.
            h.FadeOn();
        }

        private void LoadLevel(SceneHandler h)
        {
            var levelNumber = ((level - 1) % 12) + 1;
            var levelData = TileMapX.Load(Assembly, $"Assets.Levels.Level{levelNumber:00}.noidx").Data.Tiles;

            for (int y = 0; y < 14; y++)
            {
                for (int x = 0; x < 16; x++)
                {
                    var brick = levelData[y * 16 + x];
                    if (brick != 0)
                    {
                        h.Add(Brick.Create((int)brick, 16 + x * 16, 12 + y * 8));
                    }
                }
            }
        }

        private void CreateLives(SceneHandler h)
        {
            var count = Math.Min(Consts.MaxLivesOnScreen, Global.LifeCount);

            for (int i = 0; i < count; i++)
            {
                Global.Lives[i] = h.Add(new Life(i));
            }
        }

        protected override void OnMessage(SceneHandler h, object message)
        {
            base.OnMessage(h, message);

            if (message is LevelSceneMessages msg)
            {
                switch (msg)
                {
                    case LevelSceneMessages.CheckBricks:
                        if (h.FindAll<Brick>().Count(p => p.CanBeHit()) <= 0)
                        {
                            // Paso de nivel.
                            h.FadeOff();
                            h.PlaySoundSync(Assembly, "Sounds.Up.wav");
                            h.Set(new LevelScene(level + 1, demo));
                        }                        
                        break;
                    case LevelSceneMessages.Lose:
                        {
                            h.PlaySound(Assembly, "Sounds.Down.wav");
                            racket.Sleep();                            
                            h.FadeOff();
                            racket.Die();
                            h.Kill<PillBase>();
                            h.Kill<Life>();
#if !DEBUG
                            if (Global.LifeCount == 0) 
                            {
                                h.Set(new IntroScene());
                            }
                            else 
                            {
#endif
                                // Si no es debug, sigo jugando igual, aunque no tenga más vidas.
                                Global.LifeCount -= 1;
                                Play(h);
#if !DEBUG
                            }
#endif
                            break;
                        }
                    case LevelSceneMessages.StartGame:
                        if (demo)
                        {
                            // Arranco el juego.
                            h.FadeOff();
                            Global.ResetValues();
                            h.Set(new LevelScene(Consts.FirstLevel, false));
                        }
                        else
                        {
                            throw new Exception("Este mensaje sólo debería enviarse desde el modo 'demo'.");
                        }
                        break;
                    case LevelSceneMessages.NextDemoLevel:
                        {
                            h.FadeOff();
                            Global.ResetValues();
                            h.Set(new LevelScene(h.Rand(1, 12), true));
                            break;
                        }
                    case LevelSceneMessages.BackToIntro:                         
                        h.FadeOff();                            
                        h.Set(new IntroScene());                        
                        break;
                }
            }
        }
    }

    class DemoText : TextObject
    {
        /// <summary>
        /// Momento en que inició la demo.
        /// </summary>
        private readonly DateTime startedTime = DateTime.Now;

        private int counter = 1;

        protected internal DemoText()
            : base(Global.Font, "PULSA UNA TECLA PARA JUGAR")
        {
            Location.X = 136;
            Location.Y = 100;
            SetAlign(ShapeAlignEnum.Center, ShapeVerticalAlignEnum.Middle);
        }

        protected override void OnFrame()
        {
            base.OnFrame();

            if (counter++ % 20 == 0)
            {
                Location.Y = -Location.Y;
            }

            // Me fijo si tengo que terminar la demo.
            if ((DateTime.Now - startedTime).TotalSeconds > 15)
            {
                // if (Rand.Next(1, 5) == 1)
                // {

                // Vuelvo a mostrar el título, como si fuera un arcade.
                Scene.SendMessage(LevelSceneMessages.BackToIntro);
                // }
                // else
                // {
                //     Scene.SendMessage(LevelSceneMessages.NextDemoLevel);
                // }
                Die();
            }
        }
    }
}
