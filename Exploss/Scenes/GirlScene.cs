using Exploss.Entities;
using Exploss.Entities.Blocks;
using Exploss.Entities.Bubbles;
using Exploss.Mapping;
using Kriptok.Div;
using Kriptok.Entities.Base;
using Kriptok.IO;
using Kriptok.Mapping.Tiles;
using Kriptok.Scenes;
using Kriptok.Views.Texts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exploss.Scenes
{
    class GirlScene : SceneBase
    {
        private readonly int level;

        public GirlScene(int level)
        {
            this.level = level;
        }

        protected override void Run(SceneHandler h)
        {
            h.ScreenRegion.SetBackground(bg =>
            {
                bg.BlitCentered(Assembly, $"Assets.Images.Girls.g{level:00}.png");
            });
            h.FadeOn();
            h.Wait(2000);
            h.WaitForKeyPress();
            if (level >= 10)
            {
                h.FadeOff();
                h.ScreenRegion.ClearBackground();
                h.Write(Global.Font1, h.ScreenRegion.Size.Width / 2, h.ScreenRegion.Size.Height / 2,
                    "FIN DEL JUEGO").CenterMiddle();

                h.Write(Global.Font2, h.ScreenRegion.Size.Width / 2, h.ScreenRegion.Size.Height / 2 + 50,
                    $"Puntaje Total: {Global.Score}").CenterMiddle();
                
                h.FadeOn();
                h.PlaySoundSync(DivResources.Sound("MusEfect.MUSICA3.WAV"));
                h.WaitForKeyPress();

                // Actualizo el record, si corresponde.
                Global.ScoreRecord = MaxScore.CheckAndSave(Global.Score);

                h.FadeOff();
                h.Set(new MenuScene());
            }
            else
            {
                h.WaitForKeyPress();
                h.FadeOff();
                h.Set(new GameScene(level));
            }
        }
    }
}
