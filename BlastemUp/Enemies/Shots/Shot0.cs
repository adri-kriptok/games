using Kriptok.Entities.Base;
using Kriptok.Views.Sprites;

namespace Kriptok.Games.BlastemUp.Enemies.Shots
{
    public class Shot0 : EnemyShotBase
    {
        public Shot0(float x, float y, float angle) 
            : base(new SpriteView(typeof(Shot0).Assembly, "Shots.Shot0.png"), x, y, angle)
        {
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);
            
            Location.X -= 32;
            Location.Y += 6;

            h.Audio.GetWaveHandler("Assets.Sounds.Laser1.wav").Play();
        }

        internal override void Advance()
        {            
            XAdvance2D(8, ShotAngle);
        }
    }
}
