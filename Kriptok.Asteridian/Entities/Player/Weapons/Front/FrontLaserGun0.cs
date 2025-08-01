using Kriptok.Asteridian.Entities.Enemies.Base;
using Kriptok.Asteridian.Entities.Player.Weapons.Base;
using Kriptok.Asteridian.Helpers;
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
        private LaserAim aim;

        public FrontLaserGun0(PlayerShip player) : base(player, 45f)
        {
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);
            
            this.aim = Add(new LaserAim(this, 1, 1f)); // 1
        }

        protected override void Shoot(Vector3F playerLocation)
        {
            aim.Shoot(playerLocation, Laser.GetColorPurple());
        }        

        protected override FrontWeaponBase LevelDown(PlayerShip player) => this;

        protected override FrontWeaponBase LevelUp(PlayerShip player) => new FrontLaserGun1(player);        
    }
    
    internal class FrontLaserGun1 : FrontWeaponBase
    {
        private LaserAim aim;

        public FrontLaserGun1(PlayerShip player) : base(player, 45f)
        {
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);

            this.aim = Add(new LaserAim(this, 3, 1f)); // 3
        }

        protected override void Shoot(Vector3F playerLocation)
        {
            aim.Shoot(playerLocation, Laser.GetColorPurple());
        }

        protected override FrontWeaponBase LevelDown(PlayerShip player) => new FrontLaserGun0(player);

        protected override FrontWeaponBase LevelUp(PlayerShip player) => new FrontLaserGun2(player);
    }

    internal class FrontLaserGun2 : FrontWeaponBase
    {
        private LaserAim aim0, aim1;

        private static readonly Vector2F v0 = new Vector2F(-3f, 0f);
        private static readonly Vector2F v1 = new Vector2F(+3f, 0f);

        public FrontLaserGun2(PlayerShip player) : base(player, 45f)
        {
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);

            this.aim0 = Add(new LaserAim(this, 2, v0, 1f)); // 2
            this.aim1 = Add(new LaserAim(this, 2, v1, 1f)); // 2
        }

        protected override void Shoot(Vector3F playerLocation)
        {
            var col = Laser.GetColorPurple();
            aim0.Shoot(playerLocation, col);
            aim1.Shoot(playerLocation, col);
        }

        protected override FrontWeaponBase LevelDown(PlayerShip player) => new FrontLaserGun1(player);

        protected override FrontWeaponBase LevelUp(PlayerShip player) => new FrontLaserGun3(player);
    }

    internal class FrontLaserGun3 : FrontWeaponBase
    {
        private LaserAim aim0, aim1, aim2;

        private static readonly Vector2F v0 = new Vector2F(-5f, +2f);
        private static readonly Vector2F v1 = new Vector2F(+0f, +0f);
        private static readonly Vector2F v2 = new Vector2F(+5f, +2f);

        public FrontLaserGun3(PlayerShip player) : base(player, 45f)
        {
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);

            this.aim0 = Add(new LaserAim(this, 1, v0, 1f)); // 1
            this.aim1 = Add(new LaserAim(this, 3, v1, 1f)); // 3
            this.aim2 = Add(new LaserAim(this, 1, v2, 1f)); // 1
        }

        protected override void Shoot(Vector3F playerLocation)
        {
            var col = Laser.GetColorPurple();// Laser.GetColorCyan();
            aim0.Shoot(playerLocation, col);
            aim1.Shoot(playerLocation, col);
            aim2.Shoot(playerLocation, col);
        }

        protected override FrontWeaponBase LevelDown(PlayerShip player) => new FrontLaserGun2(player);

        protected override FrontWeaponBase LevelUp(PlayerShip player) => new FrontLaserGun4(player);
    }

    internal class FrontLaserGun4 : FrontWeaponBase
    {
        private LaserAim aim0, aim1;

        private static readonly Vector2F v0 = new Vector2F(-4f, 0f);
        private static readonly Vector2F v1 = new Vector2F(+4f, 0f);

        public FrontLaserGun4(PlayerShip player) : base(player, 45f)
        {
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);

            this.aim0 = Add(new LaserAim(this, 3, v0, 1f)); // 3
            this.aim1 = Add(new LaserAim(this, 3, v1, 1f)); // 3
        }

        protected override void Shoot(Vector3F playerLocation)
        {
            var col = Laser.GetColorPurple();
            aim0.Shoot(playerLocation, col);
            aim1.Shoot(playerLocation, col);
        }

        protected override FrontWeaponBase LevelDown(PlayerShip player) => new FrontLaserGun3(player);

        protected override FrontWeaponBase LevelUp(PlayerShip player) => new FrontLaserGun5(player);
    }

    internal class FrontLaserGun5 : FrontWeaponBase
    {
        private LaserAim aim0, aim1, aim2;
        
        private static readonly Vector2F v0 = new Vector2F(-5f, +2f);
        private static readonly Vector2F v1 = new Vector2F(+0f, +0f);
        private static readonly Vector2F v2 = new Vector2F(+5f, +2f);

        public FrontLaserGun5(PlayerShip player) : base(player, 45f)
        {
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);

            this.aim0 = Add(new LaserAim(this, 3, v0, 1f)); // 3
            this.aim1 = Add(new LaserAim(this, 1, v1, 2f)); // 2
            this.aim2 = Add(new LaserAim(this, 3, v2, 1f)); // 3
        }

        protected override void Shoot(Vector3F playerLocation)
        {
            var col = Laser.GetColorPurple();
            aim0.Shoot(playerLocation, col);
            aim1.Shoot(playerLocation, Laser.GetColorCyan());
            aim2.Shoot(playerLocation, col);
        }

        protected override FrontWeaponBase LevelDown(PlayerShip player) => new FrontLaserGun4(player);

        protected override FrontWeaponBase LevelUp(PlayerShip player) => new FrontLaserGun6(player);
    }

    internal class FrontLaserGun6 : FrontWeaponBase
    {
        private LaserAim aim0, aim1, aim2, aim3;

        private static readonly Vector2F v0 = new Vector2F(-6f, +2f);
        private static readonly Vector2F v1 = new Vector2F(-2f, +0f);
        private static readonly Vector2F v2 = new Vector2F(+3f, +0f);
        private static readonly Vector2F v3 = new Vector2F(+6f, +2f);

        public FrontLaserGun6(PlayerShip player) : base(player, 45f)
        {
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);

            this.aim0 = Add(new LaserAim(this, 1, v0, 2f)); // 2
            this.aim1 = Add(new LaserAim(this, 4, v1, 1f)); // 4
            this.aim2 = Add(new LaserAim(this, 4, v2, 1f)); // 4
            this.aim3 = Add(new LaserAim(this, 1, v3, 2f)); // 2
        }

        protected override void Shoot(Vector3F playerLocation)
        {
            var purple = Laser.GetColorPurple();
            var cyan = Laser.GetColorCyan();
            aim0.Shoot(playerLocation, cyan);
            aim1.Shoot(playerLocation, purple);
            aim2.Shoot(playerLocation, purple);
            aim3.Shoot(playerLocation, cyan);
        }

        protected override FrontWeaponBase LevelDown(PlayerShip player) => new FrontLaserGun5(player);

        protected override FrontWeaponBase LevelUp(PlayerShip player) => new FrontLaserGun7(player);
    }

    internal class FrontLaserGun7 : FrontWeaponBase
    {
        private LaserAim aim0, aim1, aim2;

        private static readonly Vector2F v0 = new Vector2F(-5f, +2f);
        private static readonly Vector2F v1 = new Vector2F(+0f, +0f);
        private static readonly Vector2F v2 = new Vector2F(+5f, +2f);

        public FrontLaserGun7(PlayerShip player) : base(player, 45f)
        {
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);

            this.aim0 = Add(new LaserAim(this, 4, v0, 1f)); // 4
            this.aim1 = Add(new LaserAim(this, 4, v1, 2f)); // 8
            this.aim2 = Add(new LaserAim(this, 4, v2, 1f)); // 4          
        }

        protected override void Shoot(Vector3F playerLocation)
        {
            var purple = Laser.GetColorPurple();
            var cyan = Laser.GetColorCyan();
            aim0.Shoot(playerLocation, purple);
            aim1.Shoot(playerLocation, Laser.GetColorCyan());
            aim2.Shoot(playerLocation, purple);
        }

        protected override FrontWeaponBase LevelDown(PlayerShip player) => new FrontLaserGun6(player);

        protected override FrontWeaponBase LevelUp(PlayerShip player) => new FrontLaserGun8(player);
    }

    internal class FrontLaserGun8 : FrontWeaponBase
    {
        private LaserAim aim0, aim1, aim2, aim3, aim4;

        private static readonly Vector2F v0 = new Vector2F(-8f, +2f);
        private static readonly Vector2F v1 = new Vector2F(-3f, +0f);
        private static readonly Vector2F v2 = new Vector2F(+0f, +0f);
        private static readonly Vector2F v3 = new Vector2F(+4f, +0f);
        private static readonly Vector2F v4 = new Vector2F(+9f, +2f);

        public FrontLaserGun8(PlayerShip player) : base(player, 45f)
        {
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);

            this.aim0 = Add(new LaserAim(this, 4, v0, 1f)); // 4
            this.aim1 = Add(new LaserAim(this, 4, v1, 2f)); // 8
            this.aim4 = Add(new LaserAim(this, 1, v2, 1f)); // 1
            this.aim2 = Add(new LaserAim(this, 4, v3, 2f)); // 8
            this.aim3 = Add(new LaserAim(this, 4, v4, 1f)); // 4
        }

        protected override void Shoot(Vector3F playerLocation)
        {
            var purple = Laser.GetColorPurple();
            var cyan = Laser.GetColorCyan();
            aim0.Shoot(playerLocation, purple);
            aim1.Shoot(playerLocation, cyan);
            aim4.Shoot(playerLocation, purple);
            aim2.Shoot(playerLocation, cyan);
            aim3.Shoot(playerLocation, purple);
        }

        protected override FrontWeaponBase LevelDown(PlayerShip player) => new FrontLaserGun7(player);

        protected override FrontWeaponBase LevelUp(PlayerShip player) => new FrontLaserGun9(player);
    }

    internal class FrontLaserGun9 : FrontWeaponBase
    {
        private LaserAim aim0, aim1, aim2, aim3, aim4;

        private static readonly Vector2F v0 = new Vector2F(-10f, +2f);
        private static readonly Vector2F v1 = new Vector2F(-05f, +0f);
        private static readonly Vector2F v2 = new Vector2F(+00f, +0f);
        private static readonly Vector2F v3 = new Vector2F(+05f, +0f);
        private static readonly Vector2F v4 = new Vector2F(+10f, +2f);

        public FrontLaserGun9(PlayerShip player) : base(player, 45f)
        {
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);

            this.aim0 = Add(new LaserAim(this, 4, v0, 1f)); // 4
            this.aim1 = Add(new LaserAim(this, 4, v1, 2f)); // 8
            this.aim4 = Add(new LaserAim(this, 4, v2, 2f)); // 8
            this.aim2 = Add(new LaserAim(this, 4, v3, 2f)); // 8
            this.aim3 = Add(new LaserAim(this, 4, v4, 1f)); // 4
        }

        protected override void Shoot(Vector3F playerLocation)
        {
            var purple = Laser.GetColorPurple();
            var cyan = Laser.GetColorCyan();
            aim0.Shoot(playerLocation, purple);
            aim1.Shoot(playerLocation, cyan);
            aim4.Shoot(playerLocation, cyan);
            aim2.Shoot(playerLocation, cyan);
            aim3.Shoot(playerLocation, purple);
        }

        protected override FrontWeaponBase LevelDown(PlayerShip player) => new FrontLaserGun8(player);

        protected override FrontWeaponBase LevelUp(PlayerShip player) => this;
    }

    internal class LaserAim : EntityBase
    {
        private readonly FrontWeaponBase owner;
        private readonly int width;
        private readonly Vector2F location;
        private readonly float damageMultiplier;
        private IMultipleCollisionQuery<EnemyBase> collisionQuery;
        private float? collisionY;

        public LaserAim(FrontWeaponBase owner, int width, float damageMultiplier)
            : this(owner, width, Vector2F.Empty, damageMultiplier)
        {
        }

        public LaserAim(FrontWeaponBase owner, int width, Vector2F location, float damageMultiplier)
            : base(new RectangleView(width, GlobalConsts.ScreenSize.Height, Color.Transparent)
            {
                Center = new PointF(0.5f, 1f)
            })
        {
            this.owner = owner;
            this.width = width;
            this.location = location;
            this.damageMultiplier = damageMultiplier;
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
            // Location = owner.Location.PlusXY(locationX, -Sys.TimeDelta, 0f);

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
        public override Vector3F GetRenderLocation() => owner.GetRenderLocation().PlusXY(location);

        /// <inheritdoc/>
        public override bool IsAlive() => base.IsAlive() && owner.IsAlive();

        internal void Shoot(Vector3F playerLocation, Color color)
        {
            var locationY = playerLocation.Y + location.Y - PlayerShotBaseBase.VerticalDistance;
            var locX = playerLocation.X + location.X;

            if (collisionY.HasValue(out float val))
            {
                var height = (locationY - val).Round();
                if (height > 0)
                {
                    Add(new Laser(width, damageMultiplier, new Vector3F(locX, locationY, playerLocation.Z), height, color));
                }
                else
                {
                    Add(new Laser(width, damageMultiplier, new Vector3F(locX, locationY, playerLocation.Z), color));
                }
            }
            else
            {
                Add(new Laser(width, damageMultiplier, new Vector3F(locX, locationY, playerLocation.Z), color));
            }
        }
    }

    /// <summary>
    /// Laser no hereda de <see cref="PlayerShotBase"/> ya que no necesita verificar si "salió de la pantalla"
    /// para saber cuándo morir.
    /// </summary>
    internal class Laser : PlayerShotBaseBase
    {
        private readonly float damage;
        private bool shown = false;

        public Laser(int width, float multiplier, Vector3F location, Color color)
            :this (width, multiplier, location, GlobalConsts.ScreenSize.Height, color)            
        {            
        }

        public Laser(int width, float multiplier, Vector3F location, int height, Color color)
            : base(new RectangleView(width, height, color)
            {
                Center = new PointF(0.5f, 1f)
            })
        {
            this.damage = width * multiplier;
            this.Location = location;
        }

        protected override void Frame()
        {
            if (shown)
            {
                Die();
                return;
            }

            shown = true;
        }

        protected override float GetDamage() => damage;

        internal static Color GetColorPurple()
        {
            var rnd = Rand.Next(200, 255);
            return Color.FromArgb(rnd, 0, rnd);
        }

        internal static Color GetColorCyan()
        {
            var rnd = Rand.Next(200, 255);
            return Color.FromArgb(0, rnd, rnd);
        }
    }
}
