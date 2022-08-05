using Kriptok.Drawing;
using Kriptok.Drawing.Algebra;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids
{
    public static class Helpers
    {
        internal static Vector3F Relocate(Vector3F location, Size size)
        {
            // Comprueba si se ha salido de la pantalla y los arregla
            if (location.X < -16) location.X += (size.Width + 32);
            if (location.Y < -16) location.Y += (size.Height + 32);
            if (location.X > (size.Width + 16)) location.X -= (size.Width + 32);
            if (location.Y > (size.Height + 16)) location.Y -= (size.Height + 32);
            return location;
        }
    }
}
