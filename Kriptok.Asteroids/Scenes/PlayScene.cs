using Kriptok.Scenes;
using Kriptok.Extensions;
using Kriptok.Asteroids.Objects;
using Kriptok.IO;
using System.Drawing;
using System.Windows.Forms;
using System;

namespace Kriptok.Asteroids
{
    /// <summary>
    /// Mensajes que puede entender esta actividad.
    /// </summary>
    enum PlaySceneMessages
    {
        /// <summary>
        /// Cuando pasaste de nivel.
        /// </summary>
        Win = 0,
        
        /// <summary>
        /// Cuando murió el protagonista.
        /// </summary>
        Dead = 1,

        /// <summary>
        /// Si se presionó escape para salir al título.
        /// </summary>
        Escape = 2
    }

    class PlayScene : SceneBase
    {
        /// <summary>
        /// Nivel actual del juego.
        /// </summary>
        private readonly int level;

        public PlayScene(int level)
        {
            this.level = level;
        }

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

            // Escribo los textos de pantalla.
            TitleScene.WriteTexts(h);
            h.Write(Global.GreenFont, 0, h.ScreenRegion.Size.Height, $"Nivel {level}").LeftBottom();

            // Inicia la nave.
            h.Add(new Ship());

            // Inicia los asteroides, crea los procesos tipo asteroide.
            for (var i = 0; i < 2 + level; i++)
            {
                h.Add(new Asteroid(level, 3));                        
            }

            // Inicia los gráficos de las vidas.
            for (int i = 0; i < Math.Min(3, Global.LivesCount); i++)
            {
                Global.Lives[i] = h.Add(new Live(i * 32 + 16f));
            }
            
            // Prendo la pantalla.
            h.FadeOn(48);
        }

        private void UpdateRecord()
        {
            Global.Record = MaxScore.CheckAndSave(Global.Score);
        }

        private void ClearShots(SceneHandler h)
        {
            var livingShot = h.FindFirst<Shot>();

            // Elimina los disparos actuales
            h.While(() => livingShot != null, () =>
            {
                livingShot.Die();
                livingShot = h.FindFirst<Shot>();
            });
        }

        protected override void OnMessage(SceneHandler h, object message)
        {
             base.OnMessage(h, message);

            if (message is PlaySceneMessages msg)
            {
                switch (msg)
                {
                    case PlaySceneMessages.Win:
                        {
                            // Apaga pantalla.
                            h.FadeOff();

                            // Cambia de nivel.                    
                            h.Set(new PlayScene(level + 1));
                        }
                        break;
                    case PlaySceneMessages.Dead:
                        {
                            // Dejo pasar un frame para que se agreguen las piezas de la nave.
                            h.WaitFrame();

                            // Espera hasta que desaparecen todas las piezas.
                            h.WaitWhile(() => h.FindFirst<Piece>() != null);

                            // Se borra un gráfico de las vidas.
                            Global.Lives[--Global.LivesCount].Die();

                            // También los asteroides
                            foreach (var item in h.FindAll<Asteroid>())
                            {
                                item.ResetLocation();
                            }

                            // Y los disparos
                            ClearShots(h);
                            Global.Dead = false;

                            // Si no tienes vidas
                            if (Global.LivesCount == 0)
                            {
                                // Acaba el juego
                                h.FadeOff();
                                UpdateRecord();
                                h.Set(new TitleScene());
                            }
                            else
                            {
                                h.FadeOff();

                                // Enciende la pantalla
                                h.FadeOn();

                                // Crea nave protagonista
                                h.Add(new Ship());
                            }
                        }
                        break;
                    case PlaySceneMessages.Escape:
                        {                            
                            h.FadeOff();
                            h.Set(new TitleScene());
                        }
                        break;
                }
            }
        }
    }
}
