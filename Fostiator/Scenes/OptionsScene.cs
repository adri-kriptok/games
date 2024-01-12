using Kriptok.Scenes;
using Kriptok.Drawing.Algebra;
using Kriptok.Extensions;
using Fostiator.Entities;
using Kriptok.Entities;
using Kriptok.Views.Gdip;
using Kriptok.Views.Texts;
using System;
using System.Drawing;
using System.Windows.Forms;
using static Fostiator.Global2;

namespace Fostiator.Scenes
{
    internal class OptionsScene : SceneBase
    {       
        /// <summary>
        /// Textos donde se guardan las opciones del combate
        /// </summary>
        private struct TextInfo
        {
            public int X { get; }
            public int Y { get; }
            public ITextEntity Text { get; set; }

            public TextInfo(int x, int y) : this()
            {
                X = x;
                Y = y;
            }
        }

        private static readonly TextInfo[] textInfo = new TextInfo[]
        {
            new TextInfo(320,470),
            new TextInfo(320,90 ),
            new TextInfo(320,120),
            new TextInfo(320,150),
            new TextInfo(100,364),
            new TextInfo(320,364),
            new TextInfo(540,364)
        };

        /// <summary>
        /// Escenario de lucha 0 / 1.
        /// </summary>
        public static int scenario;

        /// <summary>
        /// 0-Ripley, 1-Bishop, 2-Alien, 3-Nostromo
        /// </summary>
        public static int fighter1 = Rand.Next(0, 3);

        /// <summary>
        /// 0-Ripley, 1-Bishop, 2-Alien, 3-Nostromo
        /// </summary>
        public static int fighter2 = Rand.Next(0, 3);

        /// <summary>
        /// Modo de juego (ver modos[])
        /// </summary>
        public static int gameMode;

        private float movementAngle;

        /// <summary>
        /// Opcion seleccionada del menu.
        /// </summary>
        public static int currentOption;

        /// <summary>
        /// Opción anterior.
        /// </summary>
        public static int previewsOption;

        protected override void Run(SceneHandler h)
        {
            h.ScreenRegion.SetBackground(typeof(CreditsScene).Assembly, "Assets.Backgrounds.options.png");

            fighter1 = Rand.Next(0, 3);
            fighter2 = Rand.Next(0, 3);

            // Pone los textos no seleccionables
            h.Write(Font2, 320, 10, "OPCIONES").CenterTop();
            h.Write(Font2, 320, 176, "CONTRA").CenterTop();

            // Pone textos seleccionables
            textInfo[0].Text = h.Write(Font2, 320, 470, "EMPEZAR JUEGO").CenterBottom();
            textInfo[1].Text = h.Write(Font1, 320, 90, () => Modes[gameMode]).CenterTop();
            textInfo[2].Text = h.Write(Font1, 320, 120, () => Levels[DifficultyLevel]).CenterTop();
            textInfo[3].Text = h.Write(Font1, 320, 150, () => BlodLevels[BloodLevel]).CenterTop();

            // Pone los graficos de los muniecos y del escenario elegidos
            h.Add(new GenericObject(100, 256, () => 310 + fighter1, FlipEnum.FlipX));
            h.Add(new GenericObject(540, 256, () => 310 + fighter2, FlipEnum.None));
            h.Add(new GenericObject(320, 300, () => 300 + scenario, FlipEnum.None));

            // Pone los textos de los muniecos y del escenario elegido
            textInfo[4].Text = h.Write(Font1, 100, 364, () => FighterNames[fighter1]).CenterTop();
            textInfo[5].Text = h.Write(Font1, 320, 364, () => ScenarioNames[scenario]).CenterTop();
            textInfo[6].Text = h.Write(Font1, 540, 364, () => FighterNames[fighter2]).CenterTop();

            h.FadeOn();

            h.Loop(() =>
            {
                // Mueve el texto que este seleccionado en ese momento
                MoveText(currentOption);
                previewsOption = currentOption; // Guarda la opcion para no cambiar bruscamente

                // Lee las teclas                    
                if (Input.KeyPressed(Keys.Right) || Input.KeyPressed(Keys.Down))
                {
                    h.PlayCursorMoveSound();
                    currentOption++;
                }

                if (Input.KeyPressed(Keys.Left) || Input.KeyPressed(Keys.Up))
                {
                    h.PlayCursorMoveSound();
                    currentOption--;
                }

                // Se ha cambiado a otra opcion
                if (currentOption != previewsOption)
                {
                    // Espera a que se pare
                    h.While(() => Math.Abs(PolarVector.NewVector(movementAngle, 8f).X.Round()) != 0, () =>
                    {
                        movementAngle += 0.3f;
                        MoveText(previewsOption);
                    });
                }

                // Normaliza las opciones
                if (currentOption == 7) { currentOption = 0; }
                if (currentOption == -1) { currentOption = 6; }

                // Sigue moviendo el texto
                movementAngle += 0.3f;

                // Se ha elegido una opcion
                if (IntroKey())
                {
                    h.PlayMenuOKSound();

                    switch (currentOption)
                    {
                        case 0:
                            h.FadeTo(Color.White);
                            // Empieza el juego
                            h.Set(new BattleScene(scenario, fighter1, fighter2, gameMode));
                            return false;
                        case 1:
                            // Modo de juego (1/2 jugadores)
                            gameMode++;
                            if (gameMode == 4)
                            {
                                gameMode = 0;
                            }
                            break;

                        // Nivel de juego (Facil/Dificil)
                        case 2:
                            DifficultyLevel++;
                            if (DifficultyLevel == 3)
                            {
                                DifficultyLevel = 0;
                            }
                            break;

                        // Nivel de aparicion de sangre
                        case 3:
                            BloodLevel++;
                            if (BloodLevel == 3) BloodLevel = 0;
                            break;

                        // Selecciona munieco del jugador 1
                        case 4:
                            fighter1++;
                            if (fighter1 == 4) fighter1 = 0;
                            break;

                        // Selecciona el escenario
                        case 5:
                            scenario++;
                            if (scenario == 3) scenario = 0;
                            break;

                        // Selecciona munieco del jugador 2
                        case 6:
                            fighter2++;
                            if (fighter2 == 4) fighter2 = 0;
                            break;
                    }
                }

                if (Input.Escape())
                {
                    // Se ha pulsado 'escape'.
                    h.Set(new IntroScene());
                    // Y sale del bucle
                    return false;
                }

                return true;
            });
        }

        private void MoveText(int o)
        {
            textInfo[o].Text.LocationX = textInfo[o].X + PolarVector.NewVector(movementAngle, 8f).X;
        }

        private bool IntroKey()
        {
            return Input.KeyPressed(Keys.Enter)
                || Input.KeyPressed(Keys.Space)
                || Input.KeyPressed(Keys.RControlKey);
        }
    }
}