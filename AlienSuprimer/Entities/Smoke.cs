using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Views.Sprites;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kriptok.Games.Alien.Entities
{
    internal class Smoke2 : EntityBase<SpriteView>
    {
        public Smoke2(float x, float y) : base(new SpriteView(typeof(Impact).Assembly, "Assets.Images.Impact.png"))
        {
            View.SetAlpha(0.5f);
            Location.X = x;
            Location.Y = y;
  
            View.ScaleX = 0.8f;
            View.ScaleY = View.ScaleX;
        }

        protected override void OnFrame()
        {
            Location.X += Rand.Next(-1, 1);
            Location.Y -= 4;            
            View.ScaleX -= 0.08f;
            View.ScaleY = View.ScaleX;

            if (View.ScaleX <= 0f)
            {
                Die();
            }
        }
    }

    internal class TrailSmoke : EntityBase<SpriteView>
    {
        public TrailSmoke(Vector3F location) : base(new SpriteView(typeof(Impact).Assembly, "Assets.Images.Impact.png"))
        {
            View.SetAlpha(0.5f);
            Location = location;
            Location.Z += 1f;

            View.ScaleX = 0.25f;
            View.ScaleY = View.ScaleX;
        }

        protected override void OnFrame()
        {
            View.ScaleX -= 0.05f;
            View.ScaleY = View.ScaleX;

            if (View.ScaleX <= 0f)
            {
                Die();
            }
        }
    }
}
