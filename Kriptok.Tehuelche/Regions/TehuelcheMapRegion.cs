using Kriptok.Common;
using Kriptok.Drawing;
using Kriptok.Mapping.VoxelSpace;
using Kriptok.Regions.Pseudo3D.Cameras;
using Kriptok.Regions.Pseudo3D.VoxelSpace;
using Kriptok.Views.Base;
using System.Collections.Generic;
using System.Drawing;

namespace Kriptok.Tehuelche.Regions
{
    internal class TehuelcheMapRegion : VoxelSpaceCurvedRegion
    {
        private readonly VoxelSpaceShearingBackground background;

        private PlayerCam camera;

        public TehuelcheMapRegion(Rectangle region, VoxelTerrain voxelTerrain, Resource background) 
            : base(region, voxelTerrain, 0.008f)
        {
            this.background = new VoxelSpaceShearingBackground(this, background);
            this.PlayArea = GetTerrainBounds(voxelTerrain);
        }        

        /// <summary>
        /// Área de juego.
        /// </summary>
        internal readonly BoundF2 PlayArea;

        /// <inheritdoc/>
        protected override VoxelYBuffer1 CreateYBuffer(Size size) => new TehuelcheYBuffer(size);        

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

        protected override void Render(VoxelSpaceContext context, IList<IRenderizable> views)
        {
            background.Render(context);            

            base.Render(context, views);

//            var horizon = context.HorizonY;
//#if DEBUG
//            context.Shearing(() =>
//            {
//                context.Graphics.DrawLine(Pens.Red, 0, context.ScreenMidPoint1, 340, context.ScreenMidPoint1);
//                context.Graphics.DrawLine(Pens.Cyan, 0, horizon, 340, horizon);
//            });
//#endif
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
    }
}
