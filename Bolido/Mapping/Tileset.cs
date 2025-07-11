using Kriptok.Drawing;
using Kriptok.Helpers;
using Kriptok.Mapping.Tiles;
using Kriptok.Mapping.Tiles.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolido.Mapping
{
    public class Tileset : Tileset4Base
    {
        public const TileFlagsEnum Water = (TileFlagsEnum)0x40000000;

        public Tileset() : base(32, 32)
        {
            BitmapHelper.UsingBitmap(Assembly, "Assets.Tileset.png", bmp =>
            {                                
                bmp.MakeTransparent(bmp.Palette.Entries[0]);

                using (var fbmp = FastBitmap.CreateFrom(bmp))
                {                    
                    Layer0.AddBasicTiles(fbmp, 0, 1, 0, 4);
                    Layer0.Add(new Map2DTileInfo(fbmp.CutTile(0, 4 * 32, 32, 32), fbmp.CutTile(0, 5 * 32, 32, 32)));
                    Layer0.AddBasicTiles(fbmp, 0, 1, 6, 8);
                }
            });

            Layer0.GetTile(0).Block();
            Layer0.GetTile(1).Block();
            Layer0.GetTile(2).Block();
            Layer0.GetTile(4).AddFlag(Water);
        }

        protected override void ConfigureMapEditor(MapEditorTilesetConfig config)
        {
            base.ConfigureMapEditor(config);
            config.PaletteTilesCount = 2;
            config.PaletteTilesScale = 4f;
        }
    }
}
