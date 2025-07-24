using Kriptok.Drawing.Algebra;
using Kriptok.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Asteridian.Helpers
{
#warning BORRAR TODO ESTO CUANDO ACTUALICE LA VERSION
    internal static class ViewHelper
    {
        public static PointF[] Translate(IEnumerable<PointF> points)
        {            
            return Translate(points.ToArray());
        }

        internal static PointF[] Translate(PointF[] arr)
        {
            var avg = PointFHelper.GetRectangleF(arr);

            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = new PointF()
                {
                    X = arr[i].X - avg.X,
                    Y = arr[i].Y - avg.Y
                };
            }

            return arr;
        }

        public static Vector3F PlusX(this Vector3F v, float x)
        {
            return new Vector3F(v.X + x, v.Y, v.Z);
        }

        public static Vector3F PlusXY(this Vector3F v, Vector2F xy)
        {
            return new Vector3F(v.X + xy.X, v.Y + xy.Y, v.Z);
        }
    }    
}
