using Kriptok.Drawing.Algebra;
using Kriptok.Regions.Scroll.Base;
using System.Windows.Forms;
using Tokenkai.Entities;

namespace Tokenkai.Scenes
{
    internal class ScrollCam : IScrollTarget
    {
        private readonly Player player;
        private Vector2F[] locations = new Vector2F[30];

        public ScrollCam(Player player)
        {
            this.player = player;

            for (int i = 0; i < locations.Length; i++)
            {
                this.locations[i] = player.Location.XY();
            }
        }

        public Vector2F GetLocation2D()
        {
            for (int i = 0; i < locations.Length - 1; i++)
            {
                locations[i] = locations[i + 1];
            }

            locations[locations.Length - 1] = player.GetLocation2D();            

            return Vector2F.Avg(locations);
        }
    }
}