using Kriptok.Views.Sprites;

namespace Kriptok.Noid.Entities
{
    class BrickWall : Brick
    {
        private readonly IndexedSpriteView view;

        public BrickWall(int x, int y) 
            : base(x, y, new IndexedSpriteView(typeof(BrickWall).Assembly, GetBrickName(8), 1, 3))
        {
            this.view = (IndexedSpriteView)View;
        }

        internal override void Hit()
        {
            if (view.Graph == 2)
            {
                Global.Score += 15;
                Sounds.TaikoDrum.Play(90, 127);
                Die();
            }
            else
            {
                Audio.PlayWave(Sounds.s_bordes);
                view.Graph++;
            }
        }

        internal override bool CanBeDestroyed() => base.CanBeDestroyed() && view.Graph == 0;
    }
}
