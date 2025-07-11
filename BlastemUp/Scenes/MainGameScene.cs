using Kriptok.Scenes;
using Kriptok.Drawing.Algebra;
using Kriptok.Games.BlastemUp.Enemies;
using Kriptok.Games.BlastemUp.Maps;
using Kriptok.Games.BlastemUp.Player;
using Kriptok.Core;
using Kriptok.Regions.Scroll;
using Kriptok.Regions.Scroll.Base;
using Kriptok.Entities.Base;
using Kriptok.Views.Texts;
using System;
using System.Drawing;
using System.Linq;

namespace Kriptok.Games.BlastemUp.Scenes
{
    class MainGameScene : SceneBase
    {
        /// <summary>
        /// Contador para generar una pausa entre grupos de enemigos.
        /// </summary>
        private int noGroupTimer;

        protected override void Run(SceneHandler h)
        {
            var playArea = new Rectangle(0, Consts.PlayRegionMinY, 640, Consts.PlayRegionHeight);

            h.ScreenRegion.SetBackground(bg =>
            {
                bg.Blit(GetType().Assembly, "Assets.Images.Misc.Hud0.png", 0, 0);
                bg.Blit(GetType().Assembly, "Assets.Images.Misc.Hud1.png", 0, 440);
            });

            var subScreen = h.StartSubScreen(playArea);
            var factory = h.Add(new LetterFactory());

            var layer = new CurvedBFBackgroundScrollLayer(playArea);
            var scroll = h.StartScroll(playArea, layer);
            scroll.SetTarget(new Rail(layer.Size.Width));

            scroll.Priority = 1;
            subScreen.Priority = 2;

            Global.LifeCount = Consts.InitialLives; //                    Inicializa el numero de vidas
            Global.Score = 0;              //                      y también la puntuacion
            
            // Crea la nave del jugador
            Global.PlayerShip = h.Add(subScreen, new PlayerShip(factory)); 

            // Imprime el tipo de disparo seleccionado
            // Tipo de disparo en el marcador inferior
            h.Write(Global.GameplayFont, 600, 448, () => Global.PlayerShip.ShotType.ToString()).RightTop();

            CreateLives(h);

            for (var i = 0; i <= 4; i++)
            {
                Global.BonusLetters[i] = null;
            }

            // Escribe la puntuacion
            h.Write(Global.GameplayFont, 360, 454, () => Global.Score.ToString()).RightTop();

            // Inicializa otras variable            
            Global.CurrentGroup = 0;                     // Grupo actual
            Global.grupo_pantalla = 0;                   // Grupos en pantalla
            Global.CurrentLevel = Consts.StartLevel;     // Fase  actual
            noGroupTimer = 200;                          // Pausa entre grupos

            Global.ScrollCurrentSpeed = Consts.ScrollDefaultSpeed;

            h.FadeOn();

            h.Loop(() =>
            {
                // Enemigo final esta muerto
                if ((Global.CurrentGroup > Global.LastGroup) && (Global.grupo_pantalla == 0 ||
                    h.FindAll<EnemyBase>().Count() == 0))
                {
                    GameOver(h, "ENHORABUENA");
                    return;
                }

                // Comprueba si hay que esperar hasta que salga un grupo de naves
                if (Global.CurrentGroup <= Global.LastGroup)
                {
                    noGroupTimer++;
                }

                if (((noGroupTimer * (Global.CurrentLevel + 1) * 3 / 2) > 500) &&
                    (Global.grupo_pantalla <= (Global.CurrentLevel / 3)))
                {
                    noGroupTimer = Rand.Next(200, 450);

                    // Mira si ha cambiado de nivel
                    if ((Global.CurrentGroup % 10) == 0)
                    {
                        // Cambia de nivel
                        h.Add(new LevelTitle(++Global.CurrentLevel));

                        for (int i = 0; i <= 99; i++)
                        {
                            // Comprueba si se pulsa la tecla escape
                            if (Input.Escape())
                            {
                                GameOver(h, "FIN DE JUEGO");
                                return;
                            }

                            // h.Frame();
                        }
                    }

                    var groupIndex = 0;

                    // Buscando un hueco libre en vivos
                    h.While(() => groupIndex < Global.vivos.Length && (Global.vivos[groupIndex] > 0), () =>
                    {
                        groupIndex++;
                    });

                    // Crea un grupo
                    h.Add(subScreen, new EnemyGroup(groupIndex));

                    // Incrementa el contador de grupos
                    Global.CurrentGroup++;
                }

                // Para el movimiento de pantalla cuando se llegue al enemigo final
                if (Global.CurrentGroup > Global.LastGroup)
                {
                    Global.ScrollCurrentSpeed = 0f;
                }
                else
                {
                    Global.ScrollCurrentSpeed = Consts.ScrollDefaultSpeed;
                }

                // Mira si se ha conseguido una vida extra
                if (Global.BonusLetters.Count(p => p != null) == 5)
                {
                    // Se ha conseguido una vida extra
                    h.PlayWave(Assembly, "Assets.Sounds.ExtraLife.wav");

                    // Borra las letras del marcador
                    for (int i = 0; i <= 4; i++)
                    {
                        Global.BonusLetters[i].Die();
                        Global.BonusLetters[i] = null;
                    }

                    // Crea un escudo cuando se tenga el maximo de vidas extras
                    if (Global.LifeCount < Consts.MaxLivesOnScreen)
                    {
                        Global.Lives[Global.LifeCount] = h.Add(new Life(Global.LifeCount));
                        Global.LifeCount++;
                    }
                    else
                    {
                        Global.PlayerShip.InitShield();
                    }
                }

                // El juego se acaba si se pulsa escape o no quedan vidas
                if (Global.LifeCount < 0 || Input.Escape())
                {
                    GameOver(h, "FIN DE JUEGO");
                    return;
                }
            });

            // Hace una pequenia pausa al final
            h.WaitFrames(10);
            h.FadeOff();
            h.Set(new TitleScreenScene());
        }

        private static void GameOver(SceneHandler h, string message)
        {
            h.Write(Global.LevelFont, 320, 240, message).CenterMiddle();

            h.WaitFrames(20);
            h.FadeOff();

            h.Set(new TitleScreenScene());
        }

        private static void CreateLives(SceneHandler h)
        {
            for (var i = 0; i < Math.Min(Consts.MaxLivesOnScreen, Global.LifeCount); i++)
            {
                Global.Lives[i] = h.Add(new Life(i));   // Crea las naves pequenias (vidas)
            }
        }

        private class Rail : IScrollTarget
        {
            private readonly int width;

            public Vector2F location;

            public Rail(int width)
            {
                this.width = width;
                this.location = new Vector2F(0f, 398 / 2);
            }

            public Vector2F GetLocation2D()
            {
                // Realiza el movimiento de pantalla                
                location.X = (location.X + Global.ScrollCurrentSpeed) % width;
                return location;
            }
        }
    }

    public class LevelTitle : ProcessBase<TextView>
    {
        /// <summary>
        /// Contador.
        /// </summary>
        private int count = 5;

        public LevelTitle(int levelIndex)
            : base(new TextView(Global.LevelFont, $"NIVEL {levelIndex}"))
        {
        }

        protected override void OnBegin()
        {
            Location.X = 320;
            Repeat(() =>
            {
                Location.Y = 140;

                // Espera 4 pantallazos
                Frame(4);

                // Borra el texto
                Location.Y = -140;

                // Espera 4 pantallazos
                Frame(4);
                count--;

                // Se hace hasta 5 veces
            }, () => count < 1);
        }
    }
}
