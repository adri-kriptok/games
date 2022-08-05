using Kriptok.Entities.Base;
using Kriptok.Views.Sprites;
using System.Drawing;

namespace Galax
{
    public class Explosion : EntityBase<SpriteView>
    {
        private float scale;

        public Explosion(float x, float y, float scale) 
            : base(new SpriteView(typeof(Explosion).Assembly, "Assets.Images.Explosion.png"))
        {
            Location.X = x;
            Location.Y = y;            
            this.scale = scale;
        }

        protected override void OnFrame()
        {
            View.Scale = new PointF(scale, scale);
            
            if (scale > 0.5f && Rand.NextF() < scale / 8)
            { 
                // Crea otras explosiones                    
                Add(new Explosion(
                    Location.X += Rand.Next(-8, 8), 
                    Location.Y += Rand.Next(-8, 8), scale));
            }            

            scale -= 0.04f;

            // Continua hasta que sea demasiado pequenio
            if (scale < 0.25f)
            {
                Die();
            }
        }
    }
}

