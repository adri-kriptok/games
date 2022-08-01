using Kriptok.Objects;
using Kriptok.Objects.Base;
using Kriptok.Views;
using Kriptok.Views.Sprites;
using System.Windows.Forms;

namespace Kriptok.Pong.Objects
{
    internal class Racket : ObjectBase
    {
        internal const int Height = 32;

        private readonly int maxY;
        private readonly int minY;

        private readonly Keys downKey;
        private readonly Keys upKey;

        public Racket(int x, Keys upKey, Keys downKey)            
            :base(new SpriteView(typeof(Racket).Assembly, x < 160 ? "Racket1.png" : "Racket2.png"))
        {
            Location.X = x;
            
            minY = 20;
            maxY = 180;

            this.upKey = upKey;
            this.downKey = downKey;
        }
        
        public int Points { get; internal set; }

        protected override void OnStart(ObjectStartHandler h)
        {
            base.OnStart(h);

            Location.Y = h.RegionSize.Height / 2;
        }

        protected override void OnFrame()
        {
            if (Input.Key(downKey) || Input.Key(Keys.PageDown))
            {
                Location.Y += 5;

                if (Location.Y > maxY)
                {
                    Location.Y = maxY;
                }
            }

            if (Input.Key(upKey) || Input.Key(Keys.PageUp))
            {
                Location.Y -= 5;
                if (Location.Y < minY)
                {
                    Location.Y = minY;
                }
            }
        }
    }    
}