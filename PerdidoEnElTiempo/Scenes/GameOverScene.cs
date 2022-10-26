using Kriptok.Scenes;

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
        }
    }
}