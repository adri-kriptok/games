using Kriptok.Views.Sprites;
using System;

namespace Galax.Entities
{
    class Enemy3 : EnemyBase<SpriteView>
    {
        protected override int EnemyType => 3;

        public Enemy3(int index) 
            : base(index, new SpriteView(typeof(Enemy3).Assembly, "Assets.Images.Enemy3.png"))
        {
        }

        protected override void PlaySound()
        {
            Audio.PlayWave(GetType().Assembly, "BUIU.WAV");            
        }

        protected override void Shoot(int rnd)
        {
            if (rnd <= Global.Level * 2)
            {
                Add(new EnemyShot1(Location.X, Location.Y + 4f));
            }
        }

        private float mod;

        protected override void OnFrame()
        {
            mod += 1f;

            View.ScaleX = 1f + (float)Math.Sin(mod) * 0.15f;
            View.ScaleY = 1f - (float)Math.Cos(mod) * 0.125f;

            base.OnFrame();
        }
    }
}
