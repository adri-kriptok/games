using Kriptok.Core;
using Kriptok.Drawing;
using Kriptok.Drawing.Algebra;
using Kriptok.Entities;
using Kriptok.Helpers;
using Kriptok.Intruder.Entities;
using Kriptok.Intruder.Entities.Enemies;
using Kriptok.Intruder.Maps;
using Kriptok.Intruder.Regions;
using Kriptok.Intruder.Scenes.Maps;
using Kriptok.Maps.Partitioned.Terrain;
using Kriptok.Regions.Base;
using Kriptok.Regions.Partitioned;
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
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Intruder.Scenes.Maps.Map01_TheBeach
{
    partial class TheBeachMapScene : MapSceneBase
    {
        protected override void Run(SceneHandler h, Rectangle rect)
        {

            var mapX = GetMap();
            var mapRegion = h.StartPseudo3D(new IslandBeachRegion(rect, mapX));

            var r = (RegionBase)mapRegion;

            r.Ambience.SetLightSource(0.2f, 1f, 0.1f);
            var player = h.Add(mapRegion, new Player(mapRegion)
            {
                Location = new Vector3F(mapX.Preview.X, mapX.Preview.Y, 0f)
            });

            var treePartitions = mapRegion.GetTreePartitions();
            foreach (var part in treePartitions)
            {
                h.Add(mapRegion, new TreeEntity(mapRegion, part.GetCenter()));
            }            
            
            var lakePartitions = mapRegion.GetLakePartitions();
            foreach (var part in lakePartitions)
            {
                h.Add(mapRegion, new Brachiosaurus(mapRegion)
                {
                    Location = part.GetCenter()
                });
            }            

            h.Add(mapRegion, new WaterfallEffect(player, mapRegion.GetWaterfallVertices()));
            h.Add(mapRegion, new BeachAmbient(player, mapRegion));

            AddHud(h, player, rect);

            h.FadeOn();
        }

        private TerrainMapX GetMap()
        {
            var resource = $"{GetType().Namespace}.Map.mqo";
            // var resource = $"{GetType().Namespace}.Map.terrx";
            
            var map = IslandBeachRegion.GetMap(Assembly, resource);

            if (Path.GetExtension(resource).Equals(".mqo"))
            {
                //// Si es MQO le agrego los objetos.
                //var treePartitions = map.Partitions
                //    .Where(p => p.TextureId.In(ExteriorVxTexset.GrassTexture, ExteriorVxTexset.Grass2Texture))
                //    .Where(p => 1f - p.GetNormal().Z < IntruderConsts.MaxSlopePlayerAbleToClimb)                    
                //    .OrderByDescending(p => p.GetArea2D())
                //    .Take(100)                    
                //    .ToArray(); 

                //foreach (var part in treePartitions)
                //{
                //    var tree = h.Add(mapRegion, new TreeEntity(mapRegion)
                //    {
                //        Location = part.GetCenter()
                //    });
                //}
            }

            return map;
        }
    }
}
