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
    class PillBase : ObjectBase<SpriteView>
    {
        private static readonly string[] files = new string[10]
        {
            "Assets.Images.Pills.B_Backwards.png",        // 0
            "Assets.Images.Pills.C_ChangeBlocks.png",     // 1
            "Assets.Images.Pills.D_DecreaseSpeed.png",    // 2
            "Assets.Images.Pills.E_ExtendRacket.png",     // 3
            "Assets.Images.Pills.L_Laser.png",            // 4
            "Assets.Images.Pills.M_MultiBall.png",        // 5
            "Assets.Images.Pills.P_Life.png",             // 6
            "Assets.Images.Pills.R_ReduceRacket.png",     // 7
            "Assets.Images.Pills.S_StickyRacket.png",     // 8
            "Assets.Images.Pills.T_SuperBall.png",        // 9
        };

        private bool picked = false;

        protected PillBase(Vector3F location, int index) : base(new SpriteView(typeof(PillBase).Assembly, files[index]))
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

        public static PillBase Create(Vector3F location, int index)
        {
            switch (index)
            {
                case 0: return new BackwardsPill(location, index);
                case 1: return new ChangeBlocksPill(location, index);
                case 2: return new DecreaseBallSpeedPill(location, index);
                case 3: return new ExtendRacketPill(location, index);
                case 4: return new LaserRacketPill(location, index);
                case 5: return new MultiBallPill(location, index);
                case 7: return new ReduceRacketPill(location, index);
                case 8: return new StickyRacketPill(location, index);
                case 9: return new ToughBallPill(location, index);
            }
            return new PillBase(location, index);
        }
    }
}
