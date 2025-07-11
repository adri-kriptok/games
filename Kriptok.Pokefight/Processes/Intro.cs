//using Kriptok.Common;
//using Kriptok.Drawing;
//using Kriptok.Main;
//using Kriptok.Objects.Base;
//using Kriptok.Modes.Scroll;
//using Kriptok.Views.Sprites;
//using System;
//using System.Drawing;
//using System.Drawing.Drawing2D;

//namespace Kriptok.Pokefight.Processes
//{
//    class Intro : ProcessBase<SpriteView>
//    {
//        public Intro() : base(new SpriteView(typeof(Intro).Assembly, "Resources.Images.pokefight2.png"))
//        {
//            Location.X = Region.Size.Width / 2;
//            Location.Y = Region.Size.Height / 2;
//        }

//        protected override void OnBegin()
//        {
//            Audio.PlayMusic(Assembly, "Music.FallenAngel.S3M", true, 24000);

//            WaitOrScanCode(1000);

//            // SetView(new SpriteView(Assembly, "Resources.Images.Kriptok.png"));
//            FadeOn(16);

//            WaitOrScanCode(3000);

//            FadeOff(16);

//            //SetView(null);

//            WaitOrScanCode(1000);

//            //Region.StartScroll(this, new LavaView(Region)      
//            //{
//            //    Priority = - 1000,
//            //    Antialias = true
//            //});

//            // SetView(new SpriteView(Assembly, "Resources.Images.pokefight2.png"));

//            FadeOn(255);

//            While(() => !Input.KeyPressed(), () =>
//            {
//                //Region.Scroll.Location.X -= 1;
//                Region.Scroll.Location.Y += 2;
                
//                Frame();
//            });
            
//            Add(new MainMenu());

//            Loop(() =>
//            {
//                //Region.Scroll.Location.X -= 1;
//                Region.Scroll.Location.Y += 2;

//                Frame();
//            });
//        }

//        //protected override bool UseRegionModeCoordinateSystem => true;

//        //private class LavaView : GdipBrushScanlineScrollLayer
//        //{
//        //    private float angle = 0f;

//        //    public LavaView(ScreenRegion region) : base(
//        //        new Resource(typeof(LavaView).Assembly, "Resources.Images.Lava.png"), true, true)
//        //    {
//        //        Priority = -1000;
//        //        //Antialias = true;                
//        //    }

//        //    //protected override void Render(IRenderContext context, TextureBrush brush)
//        //    //{
//        //    //    base.Render(context, brush);

//        //    //    angle += 0.25f;
//        //    //}

//        //    protected override void OnScanline(IRenderContext context, Matrix transform, int y)
//        //    {
//        //        base.OnScanline(context, transform, y);

//        //        float sin = (float)Math.Sin(y * 0.25f + angle);
//        //        float cos = (float)Math.Cos(y * 0.25f + angle);

//        //        transform.Rotate(sin);

//        //        transform.Scale(
//        //            1f + 0.025f * cos,
//        //            1f + 0.025f * sin);

//        //        transform.Translate(3f * cos, 5f * sin);                
//        //    }
//        //}
//    }    
//}
