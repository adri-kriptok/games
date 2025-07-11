using Kriptok.Extensions;
using Kriptok.Entities.Base;
using Kriptok.Views.Sprites;

namespace Kriptok.Games.BlastemUp.Enemies.Shots
{
    public class ShotRocket : EnemyShotBase<SpriteView>
    {
        public ShotRocket(float x, float y, float angle)
            : base(new SpriteView(typeof(ShotRocket).Assembly, "Shots.Rocket.png"), x, y, angle)
        {
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);
        
            Location.X -= 32;
            Location.Y += 6;

            h.Audio.GetWaveHandler("Assets.Sounds.Rocket.wav").Play();
        }

        internal override void Advance()
        {
            Location.X -= 24;
        }

        protected override void OnFrame()
        {
            base.OnFrame();

            if (IsAlive())
            {
                Add(new Smoke(this));
            }
        }
    }

    public class Smoke : EntityBase<SpriteView>
    {
        public Smoke(EntityBase father) 
            : base(new SpriteView(typeof(Smoke).Assembly, "Assets.Images.Shots.Smoke.png"))
        {
            // Coordenadas iniciales
            Location.X = father.Location.X.Round() + 12 + Rand.Next(0, 4);    
            Location.Y = father.Location.Y.Round();

            Location.Z = 2;

            View.ScaleX = 1f;
            View.ScaleY = 1f;
        }

        protected override void OnFrame()
        {
            View.ScaleX -= 0.2f + Rand.NextF(0f, 0.09f);
            View.ScaleY = View.Scale.X;

            if (View.ScaleX < 0.01f)
            {
                Die();
                return;
            }
        }
    }
}
