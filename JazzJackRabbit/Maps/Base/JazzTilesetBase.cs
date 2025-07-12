using Kriptok.Mapping.Tiles.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.JazzJackRabbit.Maps.Base
{
    public abstract class JazzTilesetBase : Tileset1111Base
    {
        public JazzTilesetBase() : base(JazzConsts.TileSize, JazzConsts.TileSize)
        {
        }
    }
}
