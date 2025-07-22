using Kriptok.Asteridian.Entities;
using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Regions.Context.Base;
using Kriptok.Regions.Scroll;
using Kriptok.Regions.Scroll.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Asteridian.Regions
{
    public interface IAsteridianScroll
    {
        int GetLevelHeight();
    }

    public abstract class Asteridian3DScrollBase : FixedScroll3DRegion, IAsteridianScroll
    {
        public Asteridian3DScrollBase(Rectangle region, ScrollLayerBase mainLayer) 
            : base(region, mainLayer)
        {
        }

        /// <inheritdoc/>
        public abstract int GetLevelHeight();
    }


    internal class AsteridianScrollTarget : ItemBase, IScrollTarget
    {
        private readonly float maxY;
        private readonly float minY;

        private readonly PlayerShip playerShip;

        private float previewsY = 0;
        private Vector2F location;

        public AsteridianScrollTarget(Rectangle region, PlayerShip playerShip, int levelHeight)
        {
            this.playerShip = playerShip;

            minY = region.Size.Height * 0.5f;
            maxY = levelHeight - region.Size.Height * 0.5f;

            playerShip.Location.X = location.X = region.Size.Width * 0.5f;
            playerShip.Location.Y = previewsY = location.Y = maxY;
        }

        public Vector2F GetLocation2D() => location;

        public float GetLocationY() => location.Y;
        public float GetPreviewsY() => previewsY;

        internal bool KeepMoving()
        {
            return true;
        }

        internal float Inc(float timeDelta)
        {
            previewsY = location.Y;
            location.Y -= timeDelta;
            return location.Y;
        }

        internal void SetX(float x)
        {
            location.X = x;
        }

        internal float GetStartOnTopY()
        {
            return location.Y - minY;
        }
    }
}
