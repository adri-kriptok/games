using Kriptok.Adventure.Entities.Player;
using Kriptok.Adventure.Extensions;
using Kriptok.Adventure.Scenes.Base;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions;
using Kriptok.Extensions;
using Kriptok.Mapping.Entities;
using Kriptok.Sdk.RM2000.Views.CharSet.Monster2;
using Kriptok.Views.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Adventure.Entities.Monsters
{
    class Slime2k : EnemyBase<CustomSlimeView>
    {
        private const float speed = 1f / 40f;

        private readonly LinaBase player;
        private int counter;

        public Slime2k(MapEntityCreationArgs h) : base(h, new CustomSlime2kView())
        {
            this.player = h.GetPlayer();
        }

        public override float GetWeight() => 0.75f;

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);
            Radius = 8;
            h.CollisionType = Collision2DTypeEnum.Radius;
        }

        protected override void OnValidatingFrame()
        {
            LookAt2D(player);

            Advance2D(speed * Sys.TimeDelta);

            if ((counter++ & 0xF) == 0)
            {
                View.SetWalkingGraph(counter >> 4);
                Radius = (ushort)(10 - View.Graph);
            }
        }

        private class CustomSlime2kView : CustomSlimeView
        {
            public CustomSlime2kView() : base(1, 0)
            {
                Center = new PointF(0.5f, 27f / 32f);
                ReMap(0, 1, 1, 1, 2, 3, 3, 3);
            }
        }
    }
}
