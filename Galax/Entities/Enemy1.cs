using Kriptok.Audio;
using Kriptok.Entities.Base;
using Kriptok.Views.Sprites;

namespace Galax.Entities
{
    class Enemy1 : EnemyBase<IndexedSpriteView>
    {
        private ISoundHandler sound;

        public Enemy1(int index)
            : base(index, new IndexedSpriteView(typeof(Enemy3).Assembly, "Assets.Images.Enemy1.png", 3, 2))
        {
            View.Graph = 0;            
        }

        protected override int EnemyType => 1;

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);

            sound = h.Audio.GetWaveHandler("FX1.WAV");
        }

        protected override void OnFrame()
        {
            // Animacion de enemigos tipo 1
            View.Rotate();

            base.OnFrame();
        }

        protected override void PlaySound() => sound.Play();

        protected override void Shoot(int rnd)
        {            
            if (rnd <= Global.Level)
            {
                Add(new EnemyShot0(Location.X, Location.Y + 4f));
            }
        }
    }
}
