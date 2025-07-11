using Kriptok.Drawing.Algebra;
using Kriptok.Regions.Scroll.Base;

namespace Kriptok.Games.Alien.Regions
{
    internal class AlienScrollTarget : IScrollTarget
    {
        internal AlienScrollTarget(float x, float y)
        {
            Location = new Vector2F(x, y);
        }

        public Vector2F Location;

        public Vector2F GetLocation2D() => Location.Plus(160f, 120f);

        public int X { get => (int)Location.X; set => Location.X = value; }

        public int Y { get => (int)Location.Y; set => Location.Y = value; }
    }
}