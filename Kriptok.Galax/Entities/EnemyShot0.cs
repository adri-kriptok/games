using Kriptok.Views.Sprites;

namespace Kriptok.Galax.Entities
{
    public class EnemyShot0 : EnemyShotBase<IndexedSpriteView>
    {
        public EnemyShot0(float x, float y) 
            : base(new IndexedSpriteView(typeof(EnemyShot0).Assembly, "Assets.Images.EnemyShot0.png", 2, 1), x, y)
        {
        }

        protected override void Animate()
        {            
            if (View.Graph == 0)
            {
                View.Graph = 1;
            }
            else
            {
                View.Graph = 0;
            }            
        }
    }
}
