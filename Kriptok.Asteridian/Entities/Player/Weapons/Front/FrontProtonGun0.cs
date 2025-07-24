using Kriptok.Asteridian.Entities.Enemies.Base;
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
            var y = playerLocation.Y - Sys.TimeDelta;
            Add(new Proton(2, new Vector3F(playerLocation.X - 4f, y, playerLocation.Z), Proton.SP_0000));
            Add(new Proton(2, new Vector3F(playerLocation.X + 4f, y, playerLocation.Z), Proton.SP_0000));
        }

        protected override FrontWeaponBase LevelDown(PlayerShip player) => this;

        protected override FrontWeaponBase LevelUp(PlayerShip player) => new FrontProtonGun1(player);        
    }

    internal class FrontProtonGun1 : FrontWeaponBase
    {
        public FrontProtonGun1(PlayerShip player) : base(player, 45f)
        {
        }

        protected override void Shoot(Vector3F playerLocation)
        {
            var y = playerLocation.Y - Sys.TimeDelta;
            Add(new Proton(4, new Vector3F(playerLocation.X - 4f, y, playerLocation.Z), Proton.SP_0000));
            Add(new Proton(4, new Vector3F(playerLocation.X + 4f, y, playerLocation.Z), Proton.SP_0000));
        }

        protected override FrontWeaponBase LevelDown(PlayerShip player) => new FrontProtonGun0(player);

        protected override FrontWeaponBase LevelUp(PlayerShip player) => new FrontProtonGun2(player);        
    }

    internal class FrontProtonGun2 : FrontWeaponBase
    {
        public FrontProtonGun2(PlayerShip player) : base(player, 45f)
        {
        }

        protected override void Shoot(Vector3F playerLocation)
        {
            var y = playerLocation.Y - Sys.TimeDelta;
            var y2 = y + 1f;
            Add(new Proton(2, new Vector3F(playerLocation.X - 5f, y2, playerLocation.Z), Proton.SP_1500));
            Add(new Proton(4, new Vector3F(playerLocation.X,      y, playerLocation.Z), Proton.SP_0000));
            Add(new Proton(2, new Vector3F(playerLocation.X + 5f, y2, playerLocation.Z), Proton.SP_0015));
        }

        protected override FrontWeaponBase LevelDown(PlayerShip player) => new FrontProtonGun1(player);

        protected override FrontWeaponBase LevelUp(PlayerShip player) => new FrontProtonGun3(player);
    }

    internal class FrontProtonGun3 : FrontWeaponBase
    {
        public FrontProtonGun3(PlayerShip player) : base(player, 45f)
        {
        }

        protected override void Shoot(Vector3F playerLocation)
        {
            var y = playerLocation.Y - Sys.TimeDelta;
            var y2 = y + 1f;
            Add(new Proton(2, new Vector3F(playerLocation.X - 10f, y2, playerLocation.Z ), Proton.SP_1500));
            Add(new Proton(4, new Vector3F(playerLocation.X - 04f, y, playerLocation.Z), Proton.SP_0000));
            Add(new Proton(4, new Vector3F(playerLocation.X + 04f, y, playerLocation.Z), Proton.SP_0000));
            Add(new Proton(2, new Vector3F(playerLocation.X + 10f, y2, playerLocation.Z), Proton.SP_0015));
        }

        protected override FrontWeaponBase LevelDown(PlayerShip player) => new FrontProtonGun2(player);

        protected override FrontWeaponBase LevelUp(PlayerShip player) => new FrontProtonGun4(player);
    }

    internal class FrontProtonGun4 : FrontWeaponBase
    {
        public FrontProtonGun4(PlayerShip player) : base(player, 45f)
        {
        }

        protected override void Shoot(Vector3F playerLocation)
        {
            var y = playerLocation.Y - Sys.TimeDelta;
            Add(new Proton(2, new Vector3F(playerLocation.X - 11f, y + 3f, playerLocation.Z), Proton.SP_4000));
            Add(new Proton(4, new Vector3F(playerLocation.X - 06f, y + 1f, playerLocation.Z), Proton.SP_2000));
            Add(new Proton(4, new Vector3F(playerLocation.X - 00f, y, playerLocation.Z), Proton.SP_0000));
            Add(new Proton(4, new Vector3F(playerLocation.X + 06f, y + 1f, playerLocation.Z), Proton.SP_0020));
            Add(new Proton(2, new Vector3F(playerLocation.X + 11f, y + 3f, playerLocation.Z), Proton.SP_0040));
        }

        protected override FrontWeaponBase LevelDown(PlayerShip player) => new FrontProtonGun3(player);

        protected override FrontWeaponBase LevelUp(PlayerShip player) => this;
    }

    internal class Proton : PlayerShotBase
    {
        private readonly float damage;
        private readonly Vector2F speedVector;

        internal static readonly Vector2F SP_0000 = new Vector2F(+0.00f, -1.00f).Normalized(1.25f);
        internal static readonly Vector2F SP_2000 = new Vector2F(-0.20f, -1.25f).Normalized(1.25f);
        internal static readonly Vector2F SP_0020 = new Vector2F(+0.20f, -1.25f).Normalized(1.25f);
        internal static readonly Vector2F SP_1500 = new Vector2F(-0.15f, -1.25f).Normalized(1.25f);
        internal static readonly Vector2F SP_0015 = new Vector2F(+0.15f, -1.25f).Normalized(1.25f);
        internal static readonly Vector2F SP_4000 = new Vector2F(-0.40f, -1.25f).Normalized(1.25f);
        internal static readonly Vector2F SP_0040 = new Vector2F(+0.40f, -1.25f).Normalized(1.25f);

        public Proton(int size, Vector3F location, Vector2F speedVector)
            : base(new EllipseView(size, size, Color.LightCyan, Color.CornflowerBlue))
        {
            this.damage = size;
            this.speedVector = speedVector;
            this.Location = location;
        }

        protected override void Frame()
        {
            Location.X += Sys.TimeDelta * speedVector.X;
            Location.Y += Sys.TimeDelta * speedVector.Y;
        }

        protected override float GetDamage() => damage;
    }
}
