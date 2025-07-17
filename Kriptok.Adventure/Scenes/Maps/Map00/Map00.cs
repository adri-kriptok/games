using Kriptok.Adventure.Entities.Monsters;
using Kriptok.Adventure.Mapping.Tilesets;
using Kriptok.Adventure.Scenes.Base;
using Kriptok.Common;
using Kriptok.Scenes;
using Kriptok.Sdk.RM2000.Tilesets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Adventure.Scenes.Maps.Map00
{
    class Map00 : ScrollMapSceneBase<NatureOutsideTileset>
    {
        public Map00() : base(Resource.Get(typeof(Map00).Assembly, $"{typeof(Map00).Namespace}.Map.mapx"))
        {
        }

        protected override void Run(ScrollMapHandler h)
        {
            //h.Add(new SlimeMV(h)
            //{
            //    Location = new Drawing.Algebra.Vector3F(100, 100, 0)
            //});

            //h.Add(new Slime2k(h)
            //{
            //    Location = new Drawing.Algebra.Vector3F(150, 100, 0)
            //});
        }
    }
}
