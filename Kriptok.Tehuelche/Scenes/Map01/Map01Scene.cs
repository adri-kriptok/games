using Kriptok.Common;
using Kriptok.Drawing.Algebra;
using Kriptok.Helpers;
using Kriptok.Mapping.Terrains;
using Kriptok.Mapping.VoxelSpace;
using Kriptok.Regions.Pseudo3D.VoxelSpace;
using Kriptok.Tehuelche.Enemies;
using Kriptok.Tehuelche.Regions;
using Kriptok.Tehuelche.Scenes.Base;
using System.Drawing;

namespace Kriptok.Tehuelche.Scenes.Map01
{
    internal class Map01Scene : LevelSceneBasePseudo3D
    {
        internal override Vector2F GetInitialLocation() => new Vector2F(810f, 1560f);        

        protected override ByteTerrainData GetTerrain() => new ByteTerrainData(Assembly, $"{GetType().Namespace}.Terrain.png");

        protected override Resource GetTexture() => Resource.Get(Assembly, $"{GetType().Namespace}.Texture.png");

        protected override void Run(LevelBuilder builder)
        {
            ((TehuelcheMapRegionPseudo3DBase)builder.Terrain).SetFog(320, 512, Color.FromArgb(96, 96, 128));

            InstallEnemyBase(builder, 2125, 3305);
            InstallEnemyBase(builder, 3525, 3000);
            InstallEnemyBase(builder, 801, 2525);
            InstallEnemyBase(builder, 2325, 581);
        }

        private void InstallEnemyBase(LevelBuilder builder, int x, int y)
        {
            builder.Add(new Tent(builder, x, y, 0f));
            builder.Add(new Tent(builder, x, y, MathHelper.DegreesToRadians(120)));
            builder.Add(new Tent(builder, x, y, MathHelper.DegreesToRadians(240)));

            builder.Add(new Tank(builder, x, y, MathHelper.DegreesToRadians(60)));
            builder.Add(new Tank(builder, x, y, MathHelper.DegreesToRadians(180)));
            builder.Add(new Tank(builder, x, y, MathHelper.DegreesToRadians(300)));
        }

        internal override TehuelcheMapRegionPseudo3DBase CreateRegion(Rectangle rect, VoxelTerrain terrain)
        {
            return new Map01Region(rect, terrain);
        }

        private class Map01Region : TehuelcheMapRegionPseudo3DBase
        {
            public Map01Region(Rectangle region, VoxelTerrain voxelTerrain) : base(region, voxelTerrain, false)
            {
            }

            protected override Resource GetBackgroundResource() => Resource.Get(typeof(Map01Scene).Assembly, "Assets.Images.Skies.Sky00.png");                        
        }
    }
}
