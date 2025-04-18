﻿using Kriptok.Common;
using Kriptok.Helpers;
using Kriptok.Mapping.Terrains;
using Kriptok.Tehuelche.Enemies;
using Kriptok.Tehuelche.Regions;
using Kriptok.Tehuelche.Scenes.Base;
using System.Drawing;

namespace Kriptok.Tehuelche.Scenes.Map02
{
    internal class Map02Scene : LevelSceneBasePseudo3D
    {
        protected override ByteTerrainData GetTerrain() => new ByteTerrainData(Assembly, $"{GetType().Namespace}.Terrain.png");

        protected override Resource GetTexture() => Resource.Get(Assembly, $"{GetType().Namespace}.Texture.png");

        protected override Resource GetBackground() => Resource.Get(typeof(Map02Scene).Assembly, "Assets.Images.Skies.Sky00.png");

        protected override void Run(LevelBuilder builder)
        {
            ((TehuelcheMapRegionPseudo3D)builder.Terrain).SetFog(320, 512, Color.FromArgb(96, 96, 128));

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
    }
}
