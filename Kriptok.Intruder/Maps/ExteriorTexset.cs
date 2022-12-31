using Kriptok.Extensions;
using Kriptok.Maps;
using Kriptok.Regions.Pseudo3D;
using Kriptok.Regions.Pseudo3D.Partitioned.Terrain;
using Kriptok.Sdk.RM.VX.Ace.Texsets;
using Kriptok.Views.Shapes;
using Kriptok.Views.Shapes.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Intruder.Maps
{
    public class ExteriorTexset : VxExteriorTexsetBase
    {
        internal const int GrassTexture = 1100200;
        internal const int Grass2Texture = 1100210;
        internal const int SandTexture  = 1200270;
        internal const int Sand2Texture = 1100230;
        internal const int DirtTexture  = 1200200;
        internal const int OceanTexture = 1200170;
        internal const int LakeTexture = 2100100;
        internal const int RiverTexture = 1100000;
        internal const int WaterFallTexture = 1200260;
        private const int BrownRocksTexture = 2101100;
        internal const int WaterFall2Texture = 2100150;

        /// <summary>
        /// Escala a aplicar a la textura de océano.
        /// </summary>
        internal const int OceanTextureScale = 8;

        public ExteriorTexset()
        {            
            InteriorTexset.AppendEntities(Entities);

            this[OceanTexture] = this[OceanTexture].WithNoShade();
            this[LakeTexture] = this[LakeTexture].WithNoShade();
            this[RiverTexture] = this[RiverTexture].WithNoShade();
            this[WaterFallTexture] = this[WaterFallTexture].WithNoShade();
            this[WaterFall2Texture] = this[WaterFall2Texture].WithNoShade();
        }

        internal static TerrainTextureConfig[] GetTextureMapping() => new TerrainTextureConfig[]
        {
            new TerrainTextureConfig(/* 0 */ GrassTexture, ShadingAlgorithmEnum.Garoud),
            new TerrainTextureConfig(/* 1 */ SandTexture,  ShadingAlgorithmEnum.Flat),
            new TerrainTextureConfig(/* 2 */ DirtTexture,  ShadingAlgorithmEnum.Garoud),
            new TerrainTextureConfig(/* 3 */ OceanTexture, ShadingAlgorithmEnum.None),
            new TerrainTextureConfig(/* 4 */ Grass2Texture /* Dark-Grass */,  ShadingAlgorithmEnum.Garoud),
            new TerrainTextureConfig(/* 5 */ LakeTexture /* Lake */,  ShadingAlgorithmEnum.None),
            new TerrainTextureConfig(/* 6 */ RiverTexture /* River */,  ShadingAlgorithmEnum.None),
            new TerrainTextureConfig(/* 7 */ WaterFallTexture /* Water Fall */,  ShadingAlgorithmEnum.None),
            new TerrainTextureConfig(/* 8 */ BrownRocksTexture /* BrownRocks */,  ShadingAlgorithmEnum.Garoud),
            new TerrainTextureConfig(/* 9 */ WaterFall2Texture /* Water Fall */,  ShadingAlgorithmEnum.None),
            new TerrainTextureConfig(/* 10 */ Sand2Texture,  ShadingAlgorithmEnum.Garoud),
            //new TerrainTextureConfig(/* 8 */ 2101100 /* Big-Rocks */),
            //new TerrainTextureConfig(/* 9 */ 2100150 /* Big-Waterfall */)
        };

        internal static float GetMaterialScale(int materialId)
        {
            var textureId = GetTextureMapping()[materialId].Id;

            if (textureId == BrownRocksTexture)
            {
                return 0.001f;
            }

            return 1f / 256; // GetScaleH(face.MaterialId);
        }

        internal static float GetTextureScaleV(int textureId)
        {            
            if (textureId == WaterFallTexture)
            {
                return 0.0005f;
            }
            else if (textureId == BrownRocksTexture)
            {
                return 0.001f;
            }
            else if (textureId == WaterFall2Texture)
            {
                return 0.01f;
            }
            else if (textureId == LakeTexture)
            {
                return 0.01f;
            }
            return 0.005f;
        }

        internal static bool InWater(int groundTextureId)
        {
            return groundTextureId.In(OceanTexture, LakeTexture, RiverTexture);
        }
    }
}
