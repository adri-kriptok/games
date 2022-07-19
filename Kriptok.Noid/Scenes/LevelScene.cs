using Kriptok.Noid.Entities;
using Kriptok.Helpers;
using Kriptok.Objects;
using Kriptok.Objects.Base;
using Kriptok.Scenes;
using Kriptok.Views;
using System;
using System.Linq;
using System.Windows.Forms;
using Kriptok.Maps.Editor;
using Kriptok.Maps.Tiles;

namespace Kriptok.Noid.Scenes
{
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

        /// <summary>
        /// Referencias a las vidas actuales.
        /// </summary>
        private readonly Life[] lives = new Life[Consts.MaxLivesOnScreen];

        private Racket racket;
        private Ball ball;

        public LevelScene(int level, bool demo)
        {
            if (level == Consts.FirstLevel || demo)
            {
                // Reseteo la puntuación para el modo demo, o si estoy empezando a jugar.
                Global.Score = 0;
                Global.Lives = Consts.InitialLives;
            }

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

            CreateLives(h);

            // Cargo el nivel.
            LoadLevel(h);

            // Objetos principales.
            racket = h.Add(new Racket(demo));
            racket.SetBall(ball = h.Add(new Ball(racket, true, demo)));

            // Prendo la pantalla.
            h.FadeOn();

            if (demo)
            {
                // Comprueba la pulsacion de escape
                var key = h.WaitForKeyPress();
                if (key == Keys.Escape)
                {
                    // Si esta en modo demo, sale del programa                
                    h.FadeOff();
                    h.Set(new IntroScene());
                }
                else
                {
                    // Arranco el juego.
                    h.FadeOff();
                    h.Set(new LevelScene(Consts.FirstLevel, false));
                }
            }
            else
            {
                // Espero hasta que no haya más ladrillos en pantalla.
                h.WaitWhile(() => IsPlaying(h));

                // Paso de nivel.
                h.FadeOff();
                h.Set(new LevelScene(level + 1, demo));
            }
        }

        /// <summary>
        /// Indica si está jugando.
        /// </summary>        
        private static bool IsPlaying(SceneHandler h)
        {
            return h.FindAll<Brick>().Count(p => p.CanBeDestroyed()) > 0;
        }

        private void LoadLevel(SceneHandler h)
        {
            // var levelData = ResourcesHelper.GetBytes(Assembly, $"Assets.Levels.SCREEN.{((level - 1) % 12) + 1}");
            // var levelData2 = new int[levelData.Length / 4];
            // for (int i = 0; i < levelData2.Length; i++)
            // {
            //     levelData2[i] = levelData[i * 4];
            // }

            var levelData2 = TiledMapConfig.Load(Assembly, $"Assets.Levels.Level{(((level - 1) % 12) + 1):00}.noidx").Data.Tiles;

            for (int y = 0; y < 14; y++)
            {
                for (int x = 0; x < 16; x++)
                {
                    var brick = levelData2[y * 16 + x];
                    if (brick != 0)
                    {
                        h.Add(Brick.Create((int)brick, 16 + x * 16, 12 + y * 8));
                    }
                }
            }
        }

        private void CreateLives(SceneHandler h)
        {
            var count = Math.Min(Consts.MaxLivesOnScreen, Global.Lives);

            for (int i = 0; i < count; i++)
            {
                lives[i] = h.Add(new Life(i));
            }
        }
    }

    class DemoText : TextObject
    {
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
        }
    }
}
