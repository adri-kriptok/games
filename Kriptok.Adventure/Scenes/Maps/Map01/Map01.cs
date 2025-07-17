using Kriptok.Adventure.Scenes.Base;
using Kriptok.Common;
using Kriptok.Scenes;
using Kriptok.Sdk.RM2000.Tilesets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Adventure.Scenes.Maps.Map01
{
    class Map01 : ScrollMapSceneBase<InnerTileset>
    {
        public Map01() : base(Resource.Get(typeof(Map01).Assembly, $"{typeof(Map01).Namespace}.Map.mapx"))
        {
        }

        protected override void Run(ScrollMapHandler handler)
        {            
        }
    }
}
