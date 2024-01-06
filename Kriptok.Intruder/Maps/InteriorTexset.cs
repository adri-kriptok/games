using Kriptok.Intruder.Entities.Enemies;
using Kriptok.Mapping;
using Kriptok.Mapping.Entities;
using Kriptok.Regions.Pseudo3D.Partitioned.Wld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Intruder.Maps
{
    public class InteriorTexset : DivTexsetBase
    {
        public InteriorTexset()
        {                        
            AppendEntities(Entities);
        }

        public static void AppendEntities(EntitySet entities)
        {
            // entities.Add<Fly>(1010);
            // entities.Add<Velociraptor>(1020);
        }
    }
}
