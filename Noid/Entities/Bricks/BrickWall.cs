using Kriptok.Audio;
using Kriptok.Entities.Base;
using Kriptok.Views.Sprites;

namespace Noid.Entities
{
    class BrickWall : Brick
    {
        private readonly IndexedSpriteView view;
        private ISoundHandler boundSound;

        public BrickWall(int x, int y) 
            : base(x, y, new IndexedSpriteView(typeof(BrickWall).Assembly, GetBrickName(8), 1, 3))
        {
            this.view = (IndexedSpriteView)View;
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);

            boundSound = h.Audio.GetWaveHandler(Sounds.BoundSound);
        }

        internal override void Hit()
        {
            if (view.Graph == 2)
            {
                Global.Score += 15;
                Sounds.TaikoDrum.Play(90, 127);
                Die();
                CheckLastBrick();
            }
            else
            {                
                boundSound.Play();
                view.Graph++;
            }
        }

        internal override bool CanBeDestroyed() => base.CanBeDestroyed() && view.Graph == 0;
    }
}
