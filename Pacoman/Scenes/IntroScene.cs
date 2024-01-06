using Kriptok.Drawing;
using Kriptok.Extensions;
using Kriptok.Helpers;
using Kriptok.IO;
using Kriptok.Scenes;
using Pacoman.Entities;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Pacoman.Scenes
{
    public class IntroScene : SceneBase
    {
        protected override void Init(SceneInitializer init)
        {
            base.Init(init);

            // Carga la ultima maxima puntuacion            
            Global.MaxScore = MaxScore.Load();

            // Y la puntuación actual.
            Global.Score = 0;

            // Crea los procesos que contabilizan las vidas
            Global.LiveCount = 2;
        }

        protected override void Run(SceneHandler h)
        {
            h.FadeOff(byte.MaxValue);
            h.ScreenRegion.SetBackground(bg =>
            {
                bg.UsingGraphics(g => BitmapHelper.UsingBitmap(Assembly, "Assets.Images.Board.png", bmp =>
                {
                    g.Clear(Color.Black);
                    g.DrawImage(bmp, 105, 0, Alpha(0.5f));
                }));
            });

            var hardnesses = new FastBitmap8(typeof(IntroScene).Assembly, "Assets.Images.BoardHardness.png");

            h.Add(new Title(h.Write(Global.Font, 320, 320, "Pulsa una tecla para jugar").CenterBottom()));

            h.Write(Global.Font, 320, 140, "Record").CenterBottom();
            h.Write(Global.Font, 320, 170, Global.MaxScore.ToString()).CenterBottom();
            h.Write(Global.Font, 320, 430, "(c) 96 Daniel Navarro - DIV Game Studio").CenterBottom();

            GameScene.CreateBlinkers(h);

            // Inicia el sonido de entrada e imprime los texto necesarios            
            var introMusicHandler = h.PlaySoundLooping(typeof(GameScene).Assembly, "Sounds.Intro.wav");

            h.Add(new Ghost(hardnesses, 320, 177, 0, 0)).View.ScaleRGB(0.5f);
            h.Add(new Ghost(hardnesses, 290, 223, 1, 0)).View.ScaleRGB(0.5f);
            h.Add(new Ghost(hardnesses, 320, 223, 2, 0)).View.ScaleRGB(0.5f);
            h.Add(new Ghost(hardnesses, 352, 223, 3, 0)).View.ScaleRGB(0.5f);

            h.FadeOn();

            switch (h.WaitForKeyPress())
            {
                case Keys.Escape:
                    h.FadeOff();
                    h.Exit();
                    return;
                default:
                    h.FadeOff();
                    introMusicHandler.Stop();
                    h.Set(new GameScene(0));
                    return;
            }
        }

        /// <summary>
        /// Crea una instancia de <see cref="ImageAttributes"/> que sólo modifica la transparencia de la imagen.
        /// </summary>        
        public static ImageAttributes Alpha(float alpha)
        {
            ExceptionHelper.ValidateRange(alpha, 0f, 1f);

            float[][] ptsArray = GetPointsArray(1f, 0f);

            var imageAttributes = new ImageAttributes();

            imageAttributes.SetColorMatrix(new ColorMatrix(ptsArray), ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            return imageAttributes;

            float[][] GetPointsArray(float contrast, float brightness)
            {
                // create matrix that will brighten and contrast the image
                return new float[][]
                {
                    new float[] {contrast, 0, 0, 0, 0}, // scale red
                    new float[] {0, contrast, 0, 0, 0}, // scale green
                    new float[] {0, 0, contrast, 0, 0}, // scale blue
                    new float[] {0, 0, 0, alpha, 0}, // don't scale alpha
                    new float[] {brightness, brightness, brightness, 0, 1}
                };                
            }
        }
    }
}
