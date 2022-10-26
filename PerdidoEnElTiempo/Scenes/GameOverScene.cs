using Kriptok.Scenes;
using System.Drawing;

namespace PerdidoEnElTiempo.Scenes
{
    internal class GameOverScene : SceneBase
    {
        private readonly int endScene;

        public GameOverScene(int endScene)
        {
            this.endScene = endScene;
        }

        protected override void Run(SceneHandler h)
        {
            // Reseteo este valor cuando pierdo, por si tengo que volver a pasar
            // por las mismas pantallas.
            Global.AutoDestructionTimer = 0;

            if (endScene == 0)
            {
                h.ScreenRegion.SetBackground(Assembly, $"Assets.Images.gameover.png");
            }
            else
            {
                h.ScreenRegion.SetBackground(Assembly, $"Assets.Images.muerte{endScene:00}.png");
            }
            
            h.FadeOn();
            h.Wait(2000);

            h.FadeTo(Color.White);

            switch (Global.State)
            {
                case 0:
                    h.Set(new Dino1Scene());
                    break;
                case 1:
                    h.Set(new Egypt1Scene());
                    break;
                case 2:
                    h.Set(new Space1Scene());
                    break;
            }            
        }
    }
}