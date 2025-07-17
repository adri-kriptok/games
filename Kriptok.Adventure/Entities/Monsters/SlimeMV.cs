using Kriptok.Adventure.Entities.Player;
using Kriptok.Adventure.Extensions;
using Kriptok.Adventure.Scenes.Base;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions;
using Kriptok.Extensions;
using Kriptok.Mapping.Entities;
using Kriptok.Regions;
using Kriptok.Regions.Scroll;
using Kriptok.Sdk.RM.MV.Views.Characters.Monster;
using Kriptok.Views.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Adventure.Entities.Monsters
{
    class SlimeMV : EnemyBase<SlimeMvView>
    {
        private const float speed = 1f / 50f;

        private readonly LinaBase player;
        private int counter;

        public SlimeMV(MapEntityCreationArgs h) : base(h, new CustomSlimeView())
        {
            this.player = h.GetPlayer();
        }

        public override float GetWeight() => 3f;

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);
            Radius = 18;
            h.CollisionType = Collision2DTypeEnum.Radius;            
        }

        protected override void OnValidatingFrame()
        {
            LookAt2D(player);

            Advance2D(speed * Sys.TimeDelta);
            if (counter++ % 16 == 0)
            {
                View.SetWalkingGraph(counter >> 4);
            }
        }

        private class CustomSlimeView : SlimeMvView
        {
            public CustomSlimeView()
            {
                ReMap(0, 1, 1, 1, 2, 3, 3, 3);
                Center = new PointF(0.5f, 1f - 7f/48f);
            }
        }
    }
}
