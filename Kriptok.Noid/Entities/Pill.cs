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
            "Assets.Images.Pills.C_ChangeBlocks.png",  // 1
            "Assets.Images.Pills.D.png",               // 2
            "Assets.Images.Pills.E_Extend.png",        // 3
            "Assets.Images.Pills.L_Laser.png",         // 4
            "Assets.Images.Pills.M_MultiBall.png",     // 5
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
#if DEBUG || SHOWFPS
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
            switch (index)
            {
                case 0: return new Backwards_Pill(location, index);
                case 2: return new ChangeBlocks_Pill(location, index);
                case 5: return new MultiBall_Pill(location, index);
                case 9: return new T_Pill(location, index);
            }
            return new Pill(location, index);
        }
    }

    internal class Backwards_Pill : Pill
    {
        internal Backwards_Pill(Vector3F location, int index) : base(location, index)
        {
        }

        protected override void OnPick()
        {
            base.OnPick();

            Find.First<Racket>().Backwards();
        }
    }

    internal class MultiBall_Pill : Pill
    {
        internal MultiBall_Pill(Vector3F location, int index) : base(location, index)
        {
        }

        protected override void OnPick()
        {
            base.OnPick();

            Find.First<Racket>().MultiBallPillPicked();
        }
    }

    internal class ChangeBlocks_Pill : Pill
    {
        internal ChangeBlocks_Pill(Vector3F location, int index) : base(location, index)
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

    internal class T_Pill : Pill
    {
        internal T_Pill(Vector3F location, int index) : base(location, index)
        {
        }

        protected override void OnPick()
        {
            base.OnPick();

            foreach (var ball in Find.All<Ball>())
            {
                ball.SuperBallPicked();
            }
        }
    }
}
