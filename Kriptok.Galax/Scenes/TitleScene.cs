using Kriptok.Scenes;
using Kriptok.IO;
using System.Windows.Forms;

namespace Kriptok.Galax.Scenes
{
    /// <summary>
    /// Actividad que pone el título en pantalla.
    /// </summary>
    class TitleScene : SceneBase
    {
        protected override void Init(SceneInitializer init)
        {
            base.Init(init);

#if DEBUG
            Config.Load<BaseConfiguration>().Mute();
#else
            Config.Load<BaseConfiguration>();
#endif

            Global.Score = 0;
            Global.Lives = 3;
            Global.Level = 1;
        }

        protected override void Run(SceneHandler h)
        {
#if !DEBUG
            h.PlayMusic(GetType().Assembly, "BattleEnd2.mid", false);
            h.Wait(3000);
#endif
            h.ScreenRegion.SetBackground(GetType().Assembly, "TitleScreen.png");
            h.FadeOn();
            h.Wait(1000);

            if (h.WaitForKeyPress() == Keys.Escape)
            {
                // Si se pulsa escape se sale del juego.
                h.FadeOff();
                h.Set(new CreditsScene());
            }
            else
            {
                h.FadeOff();
                h.Set(new GameLevelScene());
            }
        }
    }
}
