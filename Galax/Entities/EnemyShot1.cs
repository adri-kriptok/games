using Kriptok.Views.Sprites;

namespace Galax.Entities
{
    public class EnemyShot1 : EnemyShotBase<SpriteView>
    {
        public EnemyShot1(float x, float y) 
            : base(new SpriteView(typeof(EnemyShot0).Assembly, "Assets.Images.EnemyShot1.png"), x, y)
        {
        }
    }
}
