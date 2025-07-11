using Kriptok.Entities.Base;
using Kriptok.Helpers;
using Kriptok.Scenes;
using Kriptok.Views.Sprites;
using Kriptok.Views.Texts;
using System.Drawing;
using System.Windows.Forms;

namespace Kriptok.Games.Alien.Scenes
{
    public class MainMenuScene : SceneBase
    {
        private static readonly SuperFont MenuFont = SuperFont.Load(typeof(IntroScene).Assembly, "Assets.Fonts.Menu.fntx");

        protected override void Run(SceneHandler h)
        {
            h.ScreenRegion.SetBackground(typeof(IntroScene).Assembly, "Assets.TitleScreen.png");
            h.FadeFrom(Color.White, 8);

            var title = h.Add(new RotatingTitle());

            for (int i = 500; i >= 110; i -= 5)
            {
                title.View.ScaleX = i / 100f;
                title.View.ScaleY = title.View.ScaleX;
                title.AngleZ = title.AngleZ + MathHelper.PIF / 40f;

                if (Input.KeyPressed(Keys.Escape))
                {
                    break;
                }
                h.WaitFrame();
            }
            
            title.View.ScaleX = 1f;
            title.View.ScaleY = title.View.ScaleX;            
            title.Angle.Z = 0f;
            
            h.PlaySound(Assembly, "Assets.Sounds.GOLPE20.WAV");
            h.Wait(1000);

            for (int i = 0; i < 75; i++)
            {
                if (i == 30)
                {                    
                    h.PlaySound(Assembly, "Assets.Sounds.ALIEN.WAV");
                }

                if (Input.Key(Keys.Escape))
                {
                    break;
                }

                h.WaitFrame();
            }

            h.Wait(1000);

            for (int y = 240; y > 130; y-= 10)
            {
                title.Location.Y = y;
                h.WaitFrame();
            }

            h.StartSingleMenu(MenuFont, m =>
            {
                m.Location = new Point(260, 350);
                m.OnCursorMove((from, to) =>
                {
                    h.PlayCursorMoveSound();
                });

                m.Add("Jugar", () =>
                {
                    h.Set(new LevelScene());
                    h.PlayMenuOKSound();
                    h.FadeOff();
                });

                m.Add("Salir", () =>
                {
                    h.PlayMenuOKSound();
                    h.FadeOff();
                    h.Set(new CreditsScene());                    
                });
            });
        }        

        private class RotatingTitle : EntityBase<SpriteView>
        {            
            public RotatingTitle() : base(new SpriteView(typeof(RotatingTitle).Assembly, "TitleBig.png"))
            {                
            }

            protected override void OnStart(EntityStartHandler h)
            {
                base.OnStart(h);
            
                Location.X = h.RegionSize.Width * 0.5f;
                Location.Y = h.RegionSize.Height * 0.5f;
            }

            protected override void OnFrame()
            {                
            }
        }
    }
}
