using Kriptok.Views.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Tehuelche.Views
{
    /// <summary>
    /// Clase base para espacios de voxels.    
    /// </summary>
    internal class VoxelSpaceShapeViewBase : MqoMeshView
    {
        public VoxelSpaceShapeViewBase(Assembly assembly, string resourceName) 
            : base(assembly, resourceName)
        {
        }

        /// <summary>
        /// Se utiliza el vértice más cercano a la cámara en lugar del vértice del medio,
        /// para la oclusión a nivel voxel.
        /// </summary>        
        public override float GetPriority() => -GetClosestDistance();        
    }
}
