using Kriptok.Common;
using Kriptok.Drawing;
using Kriptok.Drawing.Algebra;
using Kriptok.Mapping.VoxelSpace;
using Kriptok.Regions.Pseudo3D.Cameras;
using Kriptok.Regions.Pseudo3D.VoxelSpace;
using Kriptok.Regions.Scroll.Axonometric.VoxelSpace;
using Kriptok.Regions.VoxelSpace;
using Kriptok.Views.Base;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Kriptok.Tehuelche.Regions
{
    internal class TehuelcheMapRegionAxonometric : VoxelSpaceRegionAxonometric, ITerrain
    {
        public TehuelcheMapRegionAxonometric(Rectangle region, VoxelTerrain voxelTerrain)
            : base(region, voxelTerrain)
        {
            this.PlayArea = GetTerrainBounds(voxelTerrain);

            // Scale.X = (float)Math.Sqrt(2d);
            // Scale.Y = Scale.X;
            ReScale.Y = 0.5f;
        }

        /// <summary>
        /// Área de juego.
        /// </summary>
        internal readonly BoundF2 PlayArea;

        /// <inheritdoc/>
        protected override VoxelYBuffer1 CreateYBuffer(Size size) => new TehuelcheYBuffer(size, 192);

        /// <summary>
        /// Obtiene los límites del área de juego.
        /// </summary>        
        private BoundF2 GetTerrainBounds(VoxelTerrain voxelTerrain)
        {
            var b = new Bound2(voxelTerrain.Size);

            b.MinX += 384;
            b.MinY += 384;
            b.MaxX -= 384;
            b.MaxY -= 384;

            return new BoundF2(b);
        }

        public float GetHeight(Vector2F location) => base.SampleHeight(location);

        public BoundF2 GetPlayArea() => PlayArea;
    }
}
