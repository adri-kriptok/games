using Kriptok.Common;
using Kriptok.Drawing;
using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Mapping.VoxelSpace;
using Kriptok.Regions.Pseudo3D;
using Kriptok.Regions.Pseudo3D.Cameras;
using Kriptok.Regions.Pseudo3D.Mode7;
using Kriptok.Regions.Pseudo3D.Mode7.Gdip;
using Kriptok.Regions.Pseudo3D.VoxelSpace;
using Kriptok.Regions.Surfaces;
using Kriptok.Regions.VoxelSpace;
using Kriptok.Tehuelche.Entities.Enemies;
using Kriptok.Views.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Kriptok.Tehuelche.Regions
{
    internal abstract class TehuelcheMapRegionPseudo3DBase : VoxelSpaceCurvedRegion, ITerrain
    {
        private readonly IVoxelSpaceBackground background;

        private PlayerCam camera;

        public TehuelcheMapRegionPseudo3DBase(Rectangle region, VoxelTerrain voxelTerrain, bool reflection)
            : base(region, voxelTerrain, 0.008f, reflection)
        {
            this.background = CreateBackground(GetBackgroundResource());
            this.PlayArea = GetTerrainBounds(voxelTerrain);
        }

        /// <summary>
        /// Obtiene el recurso utilizado para el fondo.
        /// </summary>                
        protected abstract Resource GetBackgroundResource();

        /// <summary>
        /// Inicializa el fondo para esta region.
        /// </summary>        
        internal virtual IVoxelSpaceBackground CreateBackground(Resource background) => new VoxelSpaceShearingBackground(this, background);        

        /// <summary>
        /// Área de juego.
        /// </summary>
        internal readonly BoundF2 PlayArea;

        ///// <inheritdoc/>
        //protected override VoxelYBuffer1 CreateYBuffer(Size size) => new TehuelcheYBuffer(size, size.Height);

        /// <inheritdoc/>
        public PlayerCam SetCamera(PlayerCam camera)
        {
            this.camera = camera;
            return base.SetCamera(camera);
        }

        /// <summary>
        /// Obtiene el ángulo vertical de la cámara.
        /// </summary>        
        internal float GetCameraVerticalAngle() => camera.GetVerticalAngle();

        protected override void Render(VoxelSpaceContext context, IEnumerable<IRenderizable> views)
        {
            // ---------------------------------------------------------------------------------------
            // Filtro todo lo que esté cubierto por la neblina.
            // ---------------------------------------------------------------------------------------
            var fog = base.GetFogFilter();
            if (fog != null)
            {
                var maxDistance = -fog.GetFullEffectDistance();

                //views.RemoveWhen(p => p.GetPriority(context) <= maxDistance);
                views = views.Where(p => p.GetPriority(context) > maxDistance);
            }
            // ---------------------------------------------------------------------------------------

            background.Render(context, views.OfType<IRenderizableReflection>());
            
            base.Render(context, views.Where(p => !(p is IRenderizableReflection)));
        }

        /// <summary>
        /// Obtiene los límites del área de juego.
        /// </summary>        
        private BoundF2 GetTerrainBounds(VoxelTerrain voxelTerrain)
        {
            var b = new Bound2(voxelTerrain.Size);

            b.MinX = (int)(b.MinX * TextureScale);
            b.MinY = (int)(b.MinY * TextureScale);
            b.MaxX = (int)(b.MaxX * TextureScale);
            b.MaxY = (int)(b.MaxY * TextureScale);

            b.MinX += 384;
            b.MinY += 384;
            b.MaxX -= 384;
            b.MaxY -= 384;

            return new BoundF2(b);
        }

        public float GetHeight(Vector2F location) => base.SampleHeight(location);

        public BoundF2 GetPlayArea() => PlayArea;
    }

    internal abstract class ReflectiveTehuelcheMapRegionPseudo3DBase : TehuelcheMapRegionPseudo3DBase, ITerrain
    {        
        public ReflectiveTehuelcheMapRegionPseudo3DBase(Rectangle region, VoxelTerrain voxelTerrain)
            : base(region, voxelTerrain, true)
        {            
        }

        internal sealed override IVoxelSpaceBackground CreateBackground(Resource background)
        {
            return new ReflectiveTehuelcheMapRegionPseudo3DBackground(this, background);
        }

        //protected override void Render(VoxelSpaceContext context, IEnumerable<IRenderizable> views)
        //{
        //    base.Render(context, views);
        //}

        public class ReflectiveTehuelcheMapRegionPseudo3DBackground : ReflectiveVoxelSpaceShearingBackgroundBase
        {
            public ReflectiveTehuelcheMapRegionPseudo3DBackground(ReflectiveTehuelcheMapRegionPseudo3DBase owner, Resource background)
                : base(owner, background)
            {
            }

            protected override GdipMode7PlaneBase CreateReflectiveLayer()
            {
                //return new GdipMode7Plane(typeof(ReflectiveTehuelcheMapRegionPseudo3DBackground).Assembly, "Water.png", true);
                return new GdipMode7AnimatedPlane(
                    Resource.Get(typeof(ReflectiveTehuelcheMapRegionPseudo3DBackground).Assembly, "Water.png"), 2, 5, true);
            }
        }
    }
}
