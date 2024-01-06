using Kriptok.Core;
using Kriptok.Drawing;
using Kriptok.Drawing.Algebra;
using Kriptok.Entities;
using Kriptok.Extensions;
using Kriptok.Helpers;
using Kriptok.Intruder.Entities;
using Kriptok.Intruder.Entities.Enemies;
using Kriptok.Intruder.Maps;
using Kriptok.Intruder.Regions;
using Kriptok.Intruder.Scenes.Maps;
using Kriptok.Mapping;
using Kriptok.Mapping.Partitioned.Terrain;
using Kriptok.Mapping.Partitioned.Wld;
using Kriptok.Regions.Base;
using Kriptok.Regions.Pseudo3D;
using Kriptok.Regions.Pseudo3D.Backgrounds;
using Kriptok.Regions.Pseudo3D.Partitioned.Wld;
using Kriptok.Scenes;
using Kriptok.Sdk.RM.VX;
using Kriptok.Sdk.RM.VX.Ace.Texsets;
using Kriptok.Views.Shapes;
using Kriptok.Views.Shapes.Base;
using Kriptok.Views.Sprites;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Intruder.Scenes.Maps.Map00_Test
{
    class TestMapScene : MapSceneBase
    {
        protected override void Run(SceneHandler h, Rectangle rect)
        {
            var map = WldMapX.Load(Assembly, $"{GetType().Namespace}.Map.wldx");
            var mapRegion = h.StartPseudo3DWld<InteriorTexset>(rect, map);

            var r = (Region3DBase)mapRegion;            
            r.Ambience.Set(0.2f, 1f, 0.1f, 1f);
            var player = h.Add((RegionBase)mapRegion, new Player(mapRegion)
            {
                Location = new Vector3F(600, 850, 0)
            });

            h.Add(r, new Raptor(mapRegion, player)
            {
                Location = new Vector3F(1000, 850, 0)
            });

            h.Add(r, new Raptor(mapRegion, player)
            {
                Location = new Vector3F(1500, 850, 0)
            });

            AddHud(h, player, rect);
        }
    }
}
