using Kriptok.Adventure.Entities.Player;
using Kriptok.Adventure.Scenes.Base;
using Kriptok.Drawing.Algebra;
using Kriptok.Mapping.Entities;
using Kriptok.Regions;
using Kriptok.Regions.Scroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Adventure.Extensions
{
    internal static class MapEntityCreationArgsExtensions
    {
        public static ILocationValidator GetLocationValidator(this MapEntityCreationArgs args, ITileScrollEntity entity)
        {
            return GetHandler(args).GetLocationValidator(entity);
        }

        private static ScrollMapHandler GetHandler(this MapEntityCreationArgs args)
        {
            return args.Get<ScrollMapHandler>("handler");
        }

        public static void SetHandler(this MapEntityCreationArgs args, ScrollMapHandler handler)
        {
            args.Set("handler", handler);
        }

        public static LinaBase GetPlayer(this MapEntityCreationArgs args)
        {
            return args.Get<ScrollMapHandler>("handler").Player;
        }

        /// <summary>
        /// Obtiene la ubicación en el mapa de la entidad.
        /// </summary>        
        public static Vector2F GetMapLocation(this MapEntityCreationArgs args)
        {
            // var handler = GetHandler(args);
            return new Vector2F(args.EntityX.X * 16.5f, args.EntityX.Y * 16.5f);
        }
    }
}
