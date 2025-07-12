using Kriptok.Mapping.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.JazzJackRabbit.Maps
{
    /// <summary>
    /// Flags custom para JazzJackRabbit.
    /// </summary>
    public static class JazzTilesetCustomFlags
    {
        /// <summary>
        /// Indica que el tile es sólido, diagonal hacia abajo.
        /// </summary>
        public const TileFlagsEnum DiagonalDown = (TileFlagsEnum.BlockedBottomLeftTriangle);//0b1000000000000000000000000000000;

        /// <summary>
        /// Indica que el tile es sólido, diagonal hacia arriba.
        /// </summary>
        public const TileFlagsEnum DiagonalUp = (TileFlagsEnum.BlockedBottomRightTriangle);//0b0100000000000000000000000000000;

        /// <summary>
        /// Indica que de este tile no se puede ir para abajo.
        /// </summary>
        public const TileFlagsEnum BlockedToBottom = (TileFlagsEnum)0b0010000000000000000000000000000;

        /// <summary>
        /// Indica para los tiles bloqueados, que sólo se evalúe la línea del borde.
        /// </summary>
        public const TileFlagsEnum BorderBlocked   = (TileFlagsEnum)0b0001000000000000000000000000000;

        /// <summary>
        /// Indica que de este tile no se puede ir para arriba.
        /// </summary>
        public const TileFlagsEnum BlockedToTop    = (TileFlagsEnum)0b0000100000000000000000000000000;
    }
}
