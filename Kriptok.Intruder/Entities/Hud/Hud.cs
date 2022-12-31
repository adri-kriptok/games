using Kriptok.Drawing.Algebra;
using Kriptok.Entities;
using Kriptok.Entities.Base;
using Kriptok.Views.Sprites;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Intruder.Entities.Hud
{
    internal class Hud : EntityBase<SpriteView>
    {
        public const float HudZ = 50;
        public const float WeaponZ = 100;

        private readonly Player player;
        private readonly float ammoY;

        public Hud(Player player, float ammoY, Rectangle rect) 
            : base(new SpriteView(typeof(Hud).Assembly, "Assets.Images.Hud.png")
        {
            Center = new PointF(0f, 0f)
        })
        {
            this.player = player;
            this.ammoY = ammoY;
#if DEBUG
            View.Alpha = 0.5f;
#endif
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);
            Location.X = 0;
            Location.Y = h.RegionSize.Height - IntruderConsts.HudHeight;
            Location.Z = HudZ;

            // Agrego el arma en pantalla.
            Add(player.Weapon);
            Add(new BasicObject(new SpriteView(Assembly, "Assets.Images.SnesJpItems.png", 144, 0, 24, 16))
            {
                Location = new Vector3F(24f, ammoY, 0f)
            });
            Add(new AimCross());

            Add(new LifeBar(player));
            Add(new EnergyBar(player));
        }

        protected override void OnFrame()
        {            
        }
    }
}
