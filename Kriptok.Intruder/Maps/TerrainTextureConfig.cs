using Kriptok.Regions.Pseudo3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Intruder.Maps
{
    /// <summary>
    /// Estructura para configurar las texturas de los terrenos.
    /// </summary>
    internal struct TerrainTextureConfig
    {
        public TerrainTextureConfig(int id, ShadingAlgorithmEnum shading) : this()
        {
            Id = id;
            Shading = shading;
        }

        public int Id { get; }

        public ShadingAlgorithmEnum Shading { get; }
    }
}
