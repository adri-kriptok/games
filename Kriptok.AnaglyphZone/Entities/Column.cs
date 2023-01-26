using Kriptok.Entities.Base;
using Kriptok.Extensions;
using Kriptok.AZ.Scenes;
using Kriptok.AZ.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Kriptok.AZ.Scenes.EnclosedScene;

namespace Kriptok.AZ.Entities
{
    internal class Column : EntityBase<CubeView>
    {
        private readonly CamTargetBase cam;
        private readonly float relocateDistance;

        public Column(CamTargetBase cam, int xDiff, float y, int repeatPattern) : base(new CubeView())
        {
            this.cam = cam;
                        
            Location.Y = y * AZConsts.GridSize;
            View.Scale.X = AZConsts.GridSize / 2;
            View.Scale.Y = AZConsts.GridSize / 2;
            View.Scale.Z = AZConsts.CamHeight * 2;

            Location.Z = AZConsts.CamHeight;            

            this.relocateDistance =  cam.Location.X.Abs() + AZConsts.GridLength * AZConsts.GridSize;
            Location.X = cam.Location.X + (xDiff * AZConsts.GridSize) + relocateDistance;
            
            relocateDistance += repeatPattern * AZConsts.GridLength;
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);
            h.SetCollision3DViewAABB();
        }

        protected override void OnFrame()
        {
            //if(Location.X < cam.Location.X - )
            var diff = cam.Location.X - Location.X;
            if (diff >= relocateDistance)
            {
                Location.X += relocateDistance * 2;
            }
        }
    }
}
