using Kriptok.Adventure.Entities.Monsters;
using Kriptok.Mapping.Entities;
using Kriptok.Mapping.Tiles;
using Kriptok.Sdk.RM2000.Tilesets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Adventure.Mapping.Tilesets
{
    public class NatureOutsideTileset : OutlineTileset
    {
        public NatureOutsideTileset()
        {
            AddTerracedHills10();
            AddPlants00();

            AddEntities(Entities);
        }

        internal static void AddEntities(EntitySet ent)
        {
            ent.Add(1000000, p => new Slime2k(p));
            ent.Add(1000001, p => new SlimeMV(p));
        }
    }
}
