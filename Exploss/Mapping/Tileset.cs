using Kriptok.Div;
using Kriptok.Drawing;
using Kriptok.Maps.Tiles;
using Kriptok.Maps.Tiles.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exploss.Mapping
{
    public class Tileset : Tileset4Base
    {
        public Tileset() : base(43, 43)
        {
            Layer0.AddEmptyTile();

            using (var blocks = new FastBitmap(Assembly, "Assets.Images.Blocks.png"))
            {
                Layer0.AddBasicTiles(blocks, 0, 3, 0, 1);
            }
        }

        protected override void ConfigureMapEditor(MapEditorTilesetConfig config)
        {
            base.ConfigureMapEditor(config);

            config.PaletteTilesCount = 4;
            config.PaletteTilesScale = 1;
        }
    }
}
