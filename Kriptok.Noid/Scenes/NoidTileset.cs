using Kriptok.Drawing;
using Kriptok.Helpers;
using Kriptok.Maps.Editor;
using Kriptok.Maps.Tiles;
using Kriptok.Maps.Tiles.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Noid.Scenes
{
    public class NoidTileset : Tileset4Base
    {
        public NoidTileset() : base(16, 8)
        {
            // Agrego un tile vacío.
            Layer0.AddEmptyTile();

            // Agrego los ladrillos.
            foreach (var i in IntHelper.Iota(1, 9))
            {
                using (var bmp = new FastBitmap(Assembly, $"Assets.Images.Blocks.Block{i}.png"))
                {
                    Layer0.AddBasicTiles(bmp, 0, 1, 0, 1);
                    //Layer0.Add(new Map2DTileInfo(bmp));
                }
            }
        }

        protected override void ConfigureMapEditor(MapEditorTilesetConfig config)
        {
            base.ConfigureMapEditor(config);

            config.PaletteTilesScale = 6;
            config.PaletteTilesCount = 3;
        }
    }
}
