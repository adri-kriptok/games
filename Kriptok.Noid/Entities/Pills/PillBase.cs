using Kriptok.Drawing.Algebra;
using Kriptok.Objects.Base;
using Kriptok.Objects.Collisions;
using Kriptok.Views.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Noid.Entities.Pills
{
    abstract class PillBase : ObjectBase<SpriteView>
    {
        private bool picked = false;

        protected PillBase(Vector3F location, string imageName) 
            : base(new SpriteView(typeof(PillBase).Assembly, $"Assets.Images.Pills.{imageName}"))
        {
            Location = location;
            Location.Z = -20;
        }

        protected override void OnStart(ObjectStartHandler h)
        {
            base.OnStart(h);

            h.CollisionType = Collision2DTypeEnum.Ellipse;
        }

        protected override void OnFrame()
        {
            if (picked)
            {
                Audio.PlayWave(Sounds.PillSound);
                OnPick();
                Die();
                return;
            }

            Location.Y += 2;

            if (Location.Y > 205)
            {
                Die();
            }
        }

        /// <summary>
        /// Evento a disparar cuando se toma esta píldora.
        /// </summary>
        protected abstract void OnPick();

        internal void Pick()
        {
            if (!picked) 
            {
                picked = true;
                Global.Score += 50;
            }
        }

        public static PillBase Create(Vector3F location, int index)
        {
            switch (index)
            {
                case 0: return new BackwardsPill(location);
                case 1: return new ChangeBlocksPill(location);
                case 2: return new DecreaseBallSpeedPill(location);
                case 3: return new ExtendRacketPill(location);
                case 4: return new LaserRacketPill(location);
                case 5: return new MultiBallPill(location);
                case 6: return new LifePill(location);
                case 7: return new ReduceRacketPill(location);
                case 8: return new StickyRacketPill(location);
                case 9: return new ToughBallPill(location);
            }

            throw new NotImplementedException();
        }
    }
}
