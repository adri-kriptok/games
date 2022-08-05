using Kriptok.Scenes;
using Galax.Entities;
using Kriptok.IO;
using Kriptok.Views.Texts;
using System;
using System.Drawing;

namespace Galax.Scenes
{
    enum LevelMessagesEnum
    {
        /// <summary>
        /// Pasa de nivel.
        /// </summary>
        NextLevel = 0,

        /// <summary>
        /// Pierde una vida.
        /// </summary>
        LoseLife = 1
    }

    class GameLevelScene : SceneBase
    {
        private static readonly SuperFont gameFont = new SuperFont(new Font("Bauhaus 93", 10), 
            Color.Cyan, Color.White).SetShadow(-1, 1, Color.FromArgb(0, 0, 255));

        /// <summary>
        /// Formacion inicial: 0 vac¡o, 1 tipo_nave_1, 2 tipo_nave_2, 3 tipo_nave_3
        /// </summary>
        public static readonly int[] formation = new int[]
        {
            3, 3, 3, 3, 3, 0, 0, 3, 3, 3, 3, 3,
            0, 2, 2, 2, 0, 2, 2, 0, 2, 2, 2, 0,
            2, 1, 2, 1, 0, 2, 2, 0, 1, 2, 1, 2,
            1, 1, 0, 1, 1, 0, 0, 1, 1, 0, 1, 1
        };

        protected override void Init(SceneInitializer init)
        {
            base.Init(init);
#if DEBUG
            Config.Load<BaseConfiguration>().Mute();
#else
            Config.Load<BaseConfiguration>();
#endif
        }

        protected override void Run(SceneHandler h)
        {
            Global.MaxScore = MaxScore.Load();

            h.ScreenRegion.SetBackground(GetType().Assembly, "Level.png");

            // Escribe los textos necesarios
            h.Write(gameFont,   0,  0, "PUNTUACION").LeftTop();
            h.Write(gameFont, 320,  0, "MAX-PUNTUACION").RightTop();
            h.Write(gameFont, 120,  0, "VIDAS").CenterTop();
            h.Write(gameFont, 175,  0, "NIVEL").CenterTop();
            h.Write(gameFont,   0, 12, () => Global.Score).LeftTop();
            h.Write(gameFont, 320, 12, () => Global.MaxScore).RightTop();
            h.Write(gameFont, 120, 12, () => Math.Max(Global.Lives, 0)).CenterTop();
            h.Write(gameFont, 175, 12, () => Global.Level).CenterTop();

            StartNextLevel(h);
        }

        private void StartNextLevel(SceneHandler h)
        {
            for (int i = 0; i < formation.Length; i++)
            {
                Global.EnemyType[i] = formation[i];
            }
            ContinueGame(h);
        }

        private void ContinueGame(SceneHandler h)
        {
            if (Global.Player != null)
            {
                if (Global.Player.IsAlive())
                {
                    Global.Player.Die();
                }
                Global.Player = null;
            }

            h.FadeOn();

            Global.SquadCurrentX = Consts.SquadStartX;

            // Crea los enemigos mediante un bucle
            for (var i = 0; i < formation.Length; i++)
            {
                if (Global.EnemyType[i] != 0)
                {
                    // Incrementa el numero de enemigos vivos.                        
                    h.Add(EnemyBase.Create(i));
                    h.WaitFrame();
                }
            }

            // Crea nuevos procesos para siguiente vida                    
            Global.Player = h.Add(new PlayerShip());
            h.Add(new EnemyGroup());
        }

        protected override void OnMessage(SceneHandler h, object message)
        {
            base.OnMessage(h, message);

            if (message is LevelMessagesEnum msg)
            {
                if (msg == LevelMessagesEnum.LoseLife)
                {
                    // Espera a que terminen todas las explosiones.                    
                    h.Wait(750);

                    if (Global.Lives == 0)
                    {
                        h.FadeOff();
                        MaxScore.CheckAndSave(Global.Score);
                        h.Set(new TitleScene());                        
                    }
                    else
                    {     
                        Global.Lives--;

                        // Apago la pantalla.
                        h.FadeOff();

                        // Termina procesos
                        h.Kill<EnemyGroup>();
                        h.Kill<EnemyBase>();
                        h.Kill<EnemyShotBase>();

                        ContinueGame(h);                                              
                    }
                }
                else if (msg == LevelMessagesEnum.NextLevel)
                {
                    // Sino, salió por que pasó de nivel.
                    h.FadeOff();
                    Global.Level++;                    
                    h.Set(new GameLevelScene());
                }
            }
        }
    }
}
