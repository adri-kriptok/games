using Kriptok.Drawing.Algebra;
using Kriptok.Objects.Base;
using Kriptok.Objects.Collisions;
using Kriptok.Views.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Noid.Entities
{
    class Pill : ObjectBase<SpriteView>
    {
        private static readonly string[] files = new string[10]
        {
            "Assets.Images.Pills.B_Backwards.png",     // 0
            "Assets.Images.Pills.D_MultiBall.png",     // 1
            "Assets.Images.Pills.E_Extended.png",      // 2
            "Assets.Images.Pills.G_Glue.png",          // 3
            "Assets.Images.Pills.L_Laser.png",         // 4
            "Assets.Images.Pills.M_ChangeBlocks.png",  // 5
            "Assets.Images.Pills.P_Life.png",          // 6
            "Assets.Images.Pills.R_Reduce.png",        // 7
            "Assets.Images.Pills.S_Speed.png",         // 8
            "Assets.Images.Pills.T_SuperBall.png",     // 9
        };

        private bool picked = false;

        protected Pill(Vector3F location, int index) : base(new SpriteView(typeof(Pill).Assembly, files[index]))
        {
            Location = location;
            Location.Z = -10;
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
                OnPick();
                Die();
                return;
            }

            Location.Y += 2;

            if (Location.Y > 205)
            {
#if DEBUG
                OnPick();
#endif
                Die();
            }
        }

        protected virtual void OnPick()
        {            
        }

        internal void Pick()
        {
            picked = true;
        }

        public static Pill Create(Vector3F location, int index)
        {
            if (index == 5)
            {
                return new RedPill(location, index);
            }
            return new Pill(location, index);
        }
    }

    internal class RedPill : Pill
    {
        internal RedPill(Vector3F location, int index) : base(location, index)
        {
        }

        protected override void OnPick()
        {
            base.OnPick();

            foreach (var brick in Find.All<BrickSolid>())
            {
                brick.Change();
            }
        }
    }
}
