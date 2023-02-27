using Kriptok.IO;
using Kriptok.Maps.Tiles;
using Kriptok.Scenes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Exploss.Program;

namespace Exploss.Scenes
{
    class MenuScene : SceneBase
    {
        protected override void Init(SceneInitializer init)
        {
            base.Init(init);

            Global.ScoreRecord = MaxScore.Load();

            Global.Score = 0;
            Global.NextLiveScore = Consts.ScoreLivesInterval;
#if DEBUG
            Global.CurrentLives = 3;
#else
            Global.CurrentLives = 3;
#endif
            Global.CurrentBombs = 4;
        }

        protected override void Run(SceneHandler h)
        {
            // Ponemos el grafico de presentacion.
            h.ScreenRegion.SetBackground(Assembly, "Assets.Images.Title.png");

            h.Write(Global.Font1, 319, 140, "SHAREWARE").CenterMiddle();

            h.Write(Global.Font2, 319, 180, $"RECORD: {Global.ScoreRecord} puntos").CenterMiddle();

            h.FadeOn();

            StartMainMenu(h, 0);
        }

        private static void StartMainMenu(SceneHandler h, int selectedOption)
        {
            h.StartSingleMenu(Global.Font3, m =>
            {
                m.Location = new Point(319, 350);
                m.SelectedOption = selectedOption;
                m.CloseOnSelection = true;
                m.OnCursorMove((from, to) => h.PlayCursorMoveSound());

                m.Add("Jugar", () =>
                {
                    h.PlayMenuOKSound();
                    h.FadeOff();
                    h.Set(new GameScene(0));
                });

                m.Add("Dificultad", () =>
                {
                    h.PlayMenuOKSound();
                    StartDifficultyMenu(h);
                });

                m.Add("Créditos", () =>
                {
                    h.PlayMenuOKSound();
                    h.FadeOff();
                    h.Set(new CreditsScene());
                });

                m.Add("Salir al DOS", () =>
                {
                    h.PlayMenuOKSound();
                    h.FadeOff();
                    h.Exit();
                });
            });
        }

        private static void StartDifficultyMenu(SceneHandler h)
        {
            h.StartSingleMenu(Global.Font3, m =>
            {
                m.CloseOnSelection = true;
                m.OnCursorMove((from, to) => h.PlayCursorMoveSound());

                m.Location = new Point(319, 350);
                m.SelectedOption = Config.Get<ExplossConfiguration>().DifficultyLevel - 1;

                m.Add("Fácil", () =>
                {
                    h.PlayMenuOKSound();
                    SaveDifficulty(1);
                    StartMainMenu(h, 1);
                });

                m.Add("Medio", () =>
                {
                    h.PlayMenuOKSound();
                    SaveDifficulty(2);
                    StartMainMenu(h, 1);
                });

                m.Add("Difícil", () =>
                {
                    h.PlayMenuOKSound();
                    SaveDifficulty(3);
                    StartMainMenu(h, 1);
                });
            });
        }

        private static void SaveDifficulty(int level)
        {
            var config = Config.Get<ExplossConfiguration>();
            config.DifficultyLevel = level;            
            config.Save();
        }
    }
}
