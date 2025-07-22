using Kriptok.Asteridian.Entities.Enemies.Base;
using Kriptok.Asteridian.Entities.Player.Weapons.Base;
using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions;
using Kriptok.Entities.Collisions.Queries;
using Kriptok.Entities.Queries.Base;
using Kriptok.Extensions;
using Kriptok.Views.Primitives;
using Kriptok.Views.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kriptok.Asteridian.Entities.Player.Weapons.Front
{    
    internal class FrontLaserGun0 : FrontWeaponBase
    {
        private readonly PlayerShip player;
        private LaserAim aim;

        public FrontLaserGun0(PlayerShip player) : base(player, 45f)
        {
            this.player = player;
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);
            
            this.aim = Add(new LaserAim(player, 1));
        }

        protected override void Shoot(Vector3F playerLocation)
        {
            aim.Shoot(playerLocation);
        }        

        protected override FrontWeaponBase LevelDown(PlayerShip player) => this;

        protected override FrontWeaponBase LevelUp(PlayerShip player) => new FrontLaserGun9(player);        
    }
    
    internal class FrontLaserGun9 : FrontWeaponBase
    {
        private readonly PlayerShip player;
        private LaserAim aim;

        public FrontLaserGun9(PlayerShip player) : base(player, 45f)
        {
            this.player = player;
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);

            this.aim = Add(new LaserAim(player, 3));
        }

        protected override void Shoot(Vector3F playerLocation)
        {
            aim.Shoot(playerLocation);
        }

        // protected override void Shoot(Vector3F playerLocation)
        // {
        //     Add(new Laser(3, new Vector3F(playerLocation.X, playerLocation.Y - Sys.TimeDelta, playerLocation.Z)));            
        // }

        protected override FrontWeaponBase LevelDown(PlayerShip player) => new FrontLaserGun0(player);

        protected override FrontWeaponBase LevelUp(PlayerShip player) => this;
    }

    internal class LaserAim : EntityBase
    {
        private readonly PlayerShip owner;
        private readonly int width;
        private IMultipleCollisionQuery<EnemyBase> collisionQuery;
        private float? collisionY;

        public LaserAim(PlayerShip owner, int width)
            : base(new RectangleView(width, GlobalConsts.ScreenSize.Height, Color.Transparent)
            {
                Center = new PointF(0.5f, 1f)
            })
        {
            this.owner = owner;
            this.width = width;
        }

        /// <inheritdoc/>
        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);
            h.CollisionType = Collision2DTypeEnum.Auto;
            this.collisionQuery = h.GetCollisions2D<EnemyBase>();
        }

        /// <inheritdoc/>
        protected override void OnFrame()
        {
            Location = owner.Location;
            if (collisionQuery.ClosestCollision(out EnemyBase closer))
            {
                collisionY = closer.Location.Y;
            }
            else
            {
                collisionY = null;
            }
        }

        /// <inheritdoc/>
        public override Vector3F GetRenderLocation() => owner.GetRenderLocation();

        /// <inheritdoc/>
        public override bool IsAlive() => base.IsAlive() && owner.IsAlive();

        internal void Shoot(Vector3F playerLocation)
        {
            var locationY = playerLocation.Y - Sys.TimeDelta;
            if (collisionY.HasValue(out float val))
            {
                var height = (locationY - val).Round();
                if (height > 0)
                {
                    Add(new Laser(width, new Vector3F(playerLocation.X, locationY, playerLocation.Z), height));
                }
                else
                {
                    Add(new Laser(width, new Vector3F(playerLocation.X, locationY, playerLocation.Z)));
                }
            }
            else
            {
                Add(new Laser(width, new Vector3F(playerLocation.X, locationY, playerLocation.Z)));
            }
        }
    }

    internal class Laser : EntityBase<RectangleView>
    {
        private bool shown = false;

        public Laser(int width, Vector3F location)
            : base(new RectangleView(width, GlobalConsts.ScreenSize.Height, GetColor())
            {
                Center = new PointF(0.5f, 1f)
            })
        {
            this.Location = location;            
        }

        public Laser(int width, Vector3F location, int height)
            : base(new RectangleView(width, height, GetColor())
            {
                Center = new PointF(0.5f, 1f)
            })
        {
            this.Location = location;
        }

        private static Color GetColor() 
        {
            var rnd = Rand.Next(105, 255);
            return Color.FromArgb(rnd, 0, rnd);
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);
            
            h.CollisionType = Collision2DTypeEnum.Auto;
        }

        protected override void OnFrame()
        {
            if (shown)
            {
                Die();
                return;
            }

            shown = true;
        }
    }
}
