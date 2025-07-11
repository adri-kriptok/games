using Kriptok.Entities.Base;
using Kriptok.Views.Sprites;

namespace Kriptok.Games.BlastemUp.Enemies.Shots
{
    public class Shot2 : EnemyShotBase
    {
        public Shot2(float x, float y, float angle)
            : base(new SpriteView(typeof(Shot2).Assembly, "Shots.Shot2.png"), x, y, angle)
        {
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);
        
            Location.X -= 32;
            
            h.Audio.GetWaveHandler("Assets.Sounds.Laser1.wav").Play();
        }

        internal override void Advance()
        {            
            XAdvance2D(10, ShotAngle);
        }
    }
}
