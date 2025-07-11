using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tokenkai
{
    internal static class TokenkaiConsts
    {
        public const int TileSize = 6;
        public const int HalfTileSize = TileSize / 2;

        public const float InverseTileSize = 1f / TileSize;

        public static readonly Rectangle PlayableArea = new Rectangle(0, 0, 640 - 128, 480);
    }
}
