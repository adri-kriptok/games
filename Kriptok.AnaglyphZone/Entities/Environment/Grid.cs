using Kriptok.Entities.Base;
using Kriptok.Extensions;
using Kriptok.AZ.Scenes;
using Kriptok.AZ.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kriptok.AZ.Scenes.EnclosedScene;

namespace Kriptok.AZ.Entities
{
    internal class Grid : EntityBase<GridView>
    {
        private readonly CamTargetBase cam;

        public Grid(CamTargetBase cam, int x) : base(new GridView())
        {
            this.cam = cam;
            Location.X = x * AZConsts.GridSize;            
        }       

        protected override void OnFrame()
        {
            var diff = cam.Location.X - Location.X;
            if (diff >= AZConsts.GridSize)
            {
                Location.X += AZConsts.GridSize;
            }
        }
    }
}
