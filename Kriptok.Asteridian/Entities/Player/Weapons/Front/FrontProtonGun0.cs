using Kriptok.Asteridian.Entities.Player.Weapons.Base;
using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions;
using Kriptok.Entities.Queries.Base;
using Kriptok.Views.Primitives;
using Kriptok.Views.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Asteridian.Entities.Player.Weapons.Front
{
    internal class FrontProtonGun0 : FrontWeaponBase
    {       
        public FrontProtonGun0(PlayerShip player) : base(player, 45f)
        {
        }

        protected override void Shoot(Vector3F playerLocation)
        {            
            Add(new Proton(2, new Vector3F(playerLocation.X - 4f, playerLocation.Y - Sys.TimeDelta, playerLocation.Z)));
            Add(new Proton(2, new Vector3F(playerLocation.X + 4f, playerLocation.Y - Sys.TimeDelta, playerLocation.Z)));
        }

        protected override FrontWeaponBase LevelDown(PlayerShip player) => this;

        protected override FrontWeaponBase LevelUp(PlayerShip player)
        {
            return new FrontProtonGun9(player);
        }
    }

    internal class FrontProtonGun9 : FrontWeaponBase
    {
        public FrontProtonGun9(PlayerShip player) : base(player, 45f)
        {
        }

        protected override void Shoot(Vector3F playerLocation)
        {
            Add(new Proton(4, new Vector3F(playerLocation.X - 4f, playerLocation.Y - Sys.TimeDelta, playerLocation.Z)));
            Add(new Proton(4, new Vector3F(playerLocation.X + 4f, playerLocation.Y - Sys.TimeDelta, playerLocation.Z)));
        }

        protected override FrontWeaponBase LevelDown(PlayerShip player) => new FrontProtonGun0(player);

        protected override FrontWeaponBase LevelUp(PlayerShip player) => this;
    }

    internal class Proton : EntityBase<EllipseView>
    {
        private IQuery<bool?> outOfScreen;

        public Proton(int size, Vector3F location)
            : base(new EllipseView(size, size, Color.LightCyan, Color.CornflowerBlue))
        {
            this.Location = location;
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);
            this.outOfScreen = h.GetOutOfScreenQuery();

            h.CollisionType = Collision2DTypeEnum.Auto;
        }

        protected override void OnFrame()
        {
            if (outOfScreen.Result.GetValueOrDefault(false))
            {
                Die();
            }
            else
            {
                Location.Y -= Sys.TimeDelta * 1.25f;
            }
        }
    }
}
