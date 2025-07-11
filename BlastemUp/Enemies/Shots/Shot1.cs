using Kriptok.Entities.Base;
using Kriptok.Views.Sprites;

namespace Kriptok.Games.BlastemUp.Enemies.Shots
{
    public class Shot1 : EnemyShotBase
    {
        public Shot1(float x, float y, float angle)
            : base(new SpriteView(typeof(Shot1).Assembly, "Shots.Shot1.png"), x, y, angle)
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
            Location.X -= 20;
        }
    }
}
