using Kriptok.Entities.Base;
using Kriptok.Views.Sprites;

namespace Kriptok.Games.BlastemUp.Enemies.Shots
{
    public class Shot5 : EnemyShotBase<IndexedSpriteView>
    {
        public Shot5(float x, float y, float angle)
            : base(new IndexedSpriteView(typeof(Shot5).Assembly, "Shots.Shot5.png", 2, 1), x, y, angle)
        {            
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);
        
            View.Graph = 0;
            Location.X -= 90;
            Location.Y -= 18;
            
            h.Audio.GetWaveHandler("Assets.Sounds.Laser1.wav").Play();
        }

        internal override void Advance()
        {            
            XAdvance2D(14, ShotAngle);

            View.Graph = View.Graph == 0 ? 1 : 0;
        }
    }
}
