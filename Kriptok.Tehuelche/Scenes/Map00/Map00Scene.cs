using Kriptok.Common;
using Kriptok.Drawing.Algebra;
using Kriptok.Helpers;
using Kriptok.Mapping.Terrains;
using Kriptok.Mapping.VoxelSpace;
using Kriptok.Regions.Ambient.Base;
using Kriptok.Regions.Pseudo3D.VoxelSpace;
using Kriptok.Tehuelche.Enemies;
using Kriptok.Tehuelche.Entities.Enemies;
using Kriptok.Tehuelche.Regions;
using Kriptok.Tehuelche.Scenes.Base;
using Kriptok.Tehuelche.Scenes.Map01;
using Kriptok.Views.Base;
using System.Collections.Generic;
using System.Drawing;

namespace Kriptok.Tehuelche.Scenes.Map00
{
    internal class Map00Scene : LevelSceneBasePseudo3D
    {
        protected override ByteTerrainData GetTerrain() => new ByteTerrainData(Assembly, $"{GetType().Namespace}.Terrain.png");

        protected override Resource GetTexture() => Resource.Get(Assembly, $"{GetType().Namespace}.Texture.png");

        protected override void Run(LevelBuilder builder)
        {         
            ((TehuelcheMapRegionPseudo3DBase)builder.Terrain).SetFog(256, 768, Color.FromArgb(96, 96, 128));

            builder.Add(new Battleship(builder, 1900, 2200));
        }

        internal override Vector2F GetInitialLocation() => Vector2F.Empty;

        internal override TehuelcheMapRegionPseudo3DBase CreateRegion(Rectangle rect, VoxelTerrain terrain)
        {
            return new Map00Map(rect, terrain);
        }

        private class Map00Map : ReflectiveTehuelcheMapRegionPseudo3DBase
        {
            public Map00Map(Rectangle region, VoxelTerrain voxelTerrain) : base(region, voxelTerrain)
            {

            }

            protected override Resource GetBackgroundResource() => Resource.Get(typeof(Map00Scene).Assembly, "Assets.Images.Skies.Sky00.png");

            protected override void RenderColumn(uint color, int x, int minY, ushort maxY)
            {
                if (color != 0xFF0000E9) base.RenderColumn(color, x, minY, maxY);                
            }

            protected override void RenderColumn(uint color, int x, int minY, ushort maxY, IColorTransformation filter)
            {
                if (color != 0xFF0000E9) base.RenderColumn(color, x, minY, maxY, filter);                
            }
        }        
    }
}
