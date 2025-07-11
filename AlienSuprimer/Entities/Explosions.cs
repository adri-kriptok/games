using Kriptok.Common;
using Kriptok.Drawing.Algebra;
using Kriptok.Entities;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions;
using Kriptok.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Games.Alien.Entities
{
    internal class Explosion1 : SpriteAnimationTimed
    {
        public Explosion1(Vector3F location)
            : base(Resource.Get(typeof(Explosion2).Assembly, "Assets.Images.Explosion1.png"), 5, 3, 60f)
        {
            Location = location;
            Location.Z = -50f;

            Scale = new PointF(0.5f, 0.5f);
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);

            h.Audio.GetWaveHandler(Assembly, "Assets.Sounds.EXPLOSIO.WAV").Play();
        }
    }

    internal class Explosion2 : SpriteAnimationTimed
    {
        public Explosion2(Vector3F location, float scale) 
            : base(Resource.Get(typeof(Explosion2).Assembly, "Assets.Images.Explosion2.png"), 13, 3, 40f)
        {
            Location = location;
            Location.Z = -50f;

            Scale = new PointF(scale, scale);
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);

            h.Audio.GetWaveHandler(Assembly, "Assets.Sounds.EXPLOSI8.WAV").Play();
        }
    }

    internal class Explosion3 : SpriteAnimationTimed
    {
        private readonly int index;

        public Explosion3(Vector3F location, int index)
            :this(location, 1f, 0f, index)
        {

        }

        private Explosion3(Vector3F location, float scale, float angle, int index) 
            : base(Resource.Get(typeof(Explosion2).Assembly, "Assets.Images.Explosion2.png"), 13, 3, 40f)
        {
            this.index = index;
            Location = location;

            View.ScaleX = scale;
            View.ScaleY = scale;
            Angle.Z = angle;
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);
            h.Audio.GetWaveHandler(Assembly, "Assets.Sounds.EXPLOSI8.WAV").Play();

            h.CollisionType = Collision2DTypeEnum.Auto;

            if (index == 1)
            {
                Add(new Explosion3(Location.Plus(Rand.NextF(-30f, 30f), Rand.NextF(-30f, 30f), -1f), 0.3f + Rand.NextF(0.35f), Rand.NextF(MathHelper.TwoPIF), 2));
            }
            else if (index == 2)
            {
                Add(new Explosion3(Location.Plus(Rand.NextF(-30f, 30f), Rand.NextF(-30f, 30f), -1f), 0.3f + Rand.NextF(0.35f), Rand.NextF(MathHelper.TwoPIF), 3));
            }
            else if (index == 3)
            {
                Add(new Explosion3(Location.Plus(Rand.NextF(-30f, 30f), Rand.NextF(-30f, 30f), -1f), 0.6f, Rand.NextF(MathHelper.TwoPIF), 4));
            }
        }
    }
}
