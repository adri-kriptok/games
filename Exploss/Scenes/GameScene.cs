using Exploss.Entities;
using Exploss.Entities.Blocks;
using Exploss.Entities.Bubbles;
using Exploss.Mapping;
using Kriptok.Div;
using Kriptok.Entities.Base;
using Kriptok.IO;
using Kriptok.Maps.Tiles;
using Kriptok.Scenes;
using Kriptok.Views.Texts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exploss.Scenes
{
    enum GameMessages
    {
        Clear = 0,
        PlayerDied = 1,
        BackToMenu = 2
    }

    class GameScene : SceneBase
    {
        private readonly int level;

        public GameScene(int level)
        {
            this.level = level;
            Global.BlocksBroken = 0;
        }

        protected override void Run(SceneHandler h)
        {
            h.ScreenRegion.SetBackground(back =>
            {
                using (var bmp = TileMapX.Load(Assembly, "Assets.Levels.Border.explossx")
                    .ToTiledMap<Tileset>().ToFastBitmap())
                {
                    back.Blit(bmp, 0, 0);
                }
            });

            var levelData = TileMapX.Load(Assembly, $"Assets.Levels.Map{level:00}.explossx").Data.Tiles;

            for (int y = 0; y < 7; y++)
            {
                for (int x = 0; x < 13; x++)
                {
                    var brick = levelData[y * 13 + x];
                    if (brick != 0)
                    {
                        var xf = (x + 1) * 43f;
                        var yf = (y + 1) * 43f;
                        if (brick == 2)
                        {
                            h.Add(new Obstacle(xf, yf));
                        }
                        else if (brick == 3)
                        {
                            h.Add(new GreenBlock(xf, yf));
                        }
                    }
                }
            }

            h.Write(Global.Font0, h.ScreenRegion.Size.Width / 2, 22, $"NIVEL {level + 1}");

            h.FadeOn();

            h.Add(new Lives());
            h.Add(new Bombs());

            h.Write(Global.Font0, 50, 410, () => $"x {Math.Max(0, Global.CurrentLives)}").LeftMiddle();
            h.Write(Global.Font0, 50, 450, () => $"x {Global.CurrentBombs}").LeftMiddle();
            h.Write(Global.Font2, 639, 410, $"({Consts.RequiredPercentage}%)").RightMiddle();
            h.Write(Global.Font0, 639, 450, "%").RightMiddle();
            h.Write(Global.Font0, 600, 450, () => Global.BlocksPercentage).RightMiddle();
            h.Write(Global.Font2, 320 - 30, 410, "PUNTUACION").CenterMiddle();
            h.Write(Global.Font0, 320 - 30, 450, () => Global.Score).CenterMiddle();
            h.Write(Global.Font2, 320 + 150, 410, "RECORD").CenterMiddle();
            h.Write(Global.Font0, 320 + 150, 450, () => Global.ScoreRecord).CenterMiddle();

            Restart(h);
        }

        private void Restart(SceneHandler h)
        {
            var msg = h.Add(new ReadyMessage("PREPARADO"));
            h.Wait(1000);
            msg.Die();

            h.Add(new PlayerCar());

            // Cada vez que se inicia una pantalla se crea al menos una bola grande
            h.Add(new BigBallStars(Rand.Next(108, 532), Rand.Next(108, 287)));

            // Y agrego el generador de objetos.
            h.Add(new LevelRandomizer(level));
        }

        protected override void OnMessage(SceneHandler h, object message)
        {
            base.OnMessage(h, message);

            if (message is GameMessages msg)
            {
                switch (msg)
                {
                    case GameMessages.BackToMenu:
                        h.StopAllSounds();
                        // Espero un frame para que se actualice el contador.
                        var exiting = h.Add(new ReadyMessage("SALIENDO"));
                        KillAll(h);
                        h.FadeOff();
                        exiting.Die();
                        h.Set(new MenuScene());
                        break;
                    case GameMessages.Clear:
                        h.StopAllSounds();
                        // Espero un frame para que se actualice el contador.
                        var clear = h.Add(new ReadyMessage("LISTO"));                        
                        h.WaitFrame();
                        h.PlaySoundSync(DivResources.Sound("MusEfect.MUSICA3.WAV"));
                        KillAll(h);
                        h.FadeOff();
                        clear.Die();
                        h.Set(new GirlScene(level + 1));
                        break;
                    case GameMessages.PlayerDied:
                        h.PlaySound(DivResources.Sound("MusEfect.MUSICA2.WAV"));
                        if (Global.CurrentLives == 0)
                        {
                            KillAll(h);
                            h.Add(new ReadyMessage("PERDISTE"));                            
                            h.Wait(1000);
                            h.FadeOff();
                            Global.ScoreRecord = MaxScore.CheckAndSave(Global.Score);
                            h.Set(new MenuScene());
                        }
                        else
                        {
                            h.Wait(1000);
                            h.FadeOff();
                            Global.CurrentLives--;

                            KillAll(h);
                            h.FadeOn();
                            Restart(h);
                        }
                        break;
                }
            }
        }

        private static void KillAll(SceneHandler h)
        {
            // Destruimos todos los procesos bola (grande, mediana,...)
            // que existian y todos los disparos     
            h.Kill<BigBallStars>();
            h.Kill<BubbleBase>();
            h.Kill<Shot>();
            h.Kill<LevelRandomizer>();
            h.Kill<BombPickup>();
        }

        private class ReadyMessage : EntityBase<TextView>
        {
            public ReadyMessage(string message) : base(new TextView(Global.Font1, message))
            {
                Location.Z = -99;
            }

            protected override void OnStart(EntityStartHandler h)
            {
                base.OnStart(h);
                Location.X = h.RegionSize.Width / 2f;
                Location.Y = (h.RegionSize.Height - 43 * 3) / 2f;
            }

            protected override void OnFrame()
            {
            }
        }

        private class LevelRandomizer : EntityBase
        {
            private readonly int level;

            internal LevelRandomizer(int level)
            {
                this.level = level;
            }

            protected override void OnFrame()
            {
                // Generamos una bola grande en funcion de la pantalla que
                // nos encontremos, a mayor pantalla mayor probabilidad de que
                // se cree una bola grande (mas dificultad)

                if ((Rand.Next(1, 4000 / ((level +1) * Global.DifficultyLevel)) <= 2))
                {
                    Add(new BigBallStars(Rand.Next(108, 532), Rand.Next(108, 287)));
                }

                // Generamos un proceso que si es cogido por el coche nos da
                // cuatro bombas mas en funcion de la pantalla en que estemos
                // a mayor pantalla mayor probabilidad de que genere esto

                if ((Rand.Next(1, 10000 / (level + 1)) <= 2))
                {
                    // Esto es para que no aparezcan dos pickups al mismo tiempo.
                    if (Find.All<BombPickup>().Count() <= 0)
                    {
                        Add(new BombPickup(Rand.Next(108, 532), Rand.Next(108, 287)));
                    }
                }
            }
        }
    }
}
