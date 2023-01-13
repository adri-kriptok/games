using Kriptok.Drawing.Algebra;
using Kriptok.AZ.Scenes;
using Kriptok.Views.Shapes;
using Kriptok.Views.Shapes.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kriptok.AZ.Entities.Enemies
{
    internal class Asteroid : EnemyBase
    {
        private readonly float modAngX;
        private readonly float modAngY;
        private readonly float modAngZ;

        private readonly float modLocX;
        private readonly float modLocY;
        private readonly float modLocZ;

        public Asteroid(CamTargetBase camTarget, float arcadeIncreaser) : base(camTarget, new AsteroidView())
        {
            var min = 40f * arcadeIncreaser;
            var max = 70f * arcadeIncreaser;
            View.Scale = new Vector3F(Rand.NextF(min, max), Rand.NextF(min, max), Rand.NextF(min, max));

            Life = (int)View.Scale.X / 3;

            Location.Y = Rand.NextF(-50f, 50f);
            if (Rand.Next(0, 10) == 0)
            {
                Location.Y += 200;
            }

            Location.Z = Rand.NextF(150f, AZConsts.CamHeight * 2f - 150);

            Angle.X = Location.X;
            Angle.Y = Location.Y;
            Angle.Z = Location.Z;

            modAngX = Rand.NextF(-0.05f, 0.05f);
            modAngY = Rand.NextF(-0.05f, 0.05f);
            modAngZ = Rand.NextF(-0.05f, 0.05f);

            modLocX = Rand.NextF(-arcadeIncreaser * 2f, 0f);
            modLocY = Rand.NextF(-2f, 2f);
            modLocZ = Rand.NextF(-2f, 2f);
        }

        protected override void OnFrame()
        {
            Angle.X += modAngX;
            Angle.Y += modAngY;
            Angle.Z += modAngZ;

            Location.X += modLocX;
            Location.Y += modLocY;
            Location.Z += modLocZ;

            base.OnFrame();
        }

        internal class AsteroidView : GdipShapeView
        {
            public AsteroidView() : base(WireframeIcosahedronView.GetIcosahedronVertices(), shapes =>
            {
                shapes.Add(Material.Black, Strokes.Fuchsia, 0, 2, 1);
                shapes.Add(Material.Black, Strokes.Fuchsia, 0, 3, 2);
                shapes.Add(Material.Black, Strokes.Fuchsia, 0, 4, 3);
                shapes.Add(Material.Black, Strokes.Fuchsia, 0, 5, 4);
                shapes.Add(Material.Black, Strokes.Fuchsia, 0, 1, 5);
                shapes.Add(Material.Black, Strokes.Fuchsia, 11, 6, 7);
                shapes.Add(Material.Black, Strokes.Fuchsia, 11, 7, 8);
                shapes.Add(Material.Black, Strokes.Fuchsia, 11, 8, 9);
                shapes.Add(Material.Black, Strokes.Fuchsia, 11, 9, 10);
                shapes.Add(Material.Black, Strokes.Fuchsia, 11, 10, 6);
                shapes.Add(Material.Black, Strokes.Fuchsia, 2, 8, 7);
                shapes.Add(Material.Black, Strokes.Fuchsia, 1, 7, 6);
                shapes.Add(Material.Black, Strokes.Fuchsia, 5, 6, 10);
                shapes.Add(Material.Black, Strokes.Fuchsia, 4, 10, 9);
                shapes.Add(Material.Black, Strokes.Fuchsia, 3, 9, 8);
                shapes.Add(Material.Black, Strokes.Fuchsia, 10, 4, 5);
                shapes.Add(Material.Black, Strokes.Fuchsia, 9, 3, 4);
                shapes.Add(Material.Black, Strokes.Fuchsia, 8, 2, 3);
                shapes.Add(Material.Black, Strokes.Fuchsia, 7, 1, 2);
                shapes.Add(Material.Black, Strokes.Fuchsia, 6, 5, 1);
            })
            {
                SwapAllFaces();
            }

            /// <summary>
            /// Indica si la figura es convexa. Las figuras convexas se renderizan más rápido, ya que no necesitan
            /// ordenar las caras u otra figuras que la componen antes de renderizarse en pantalla.
            /// </summary>        
            protected override bool IsConvex() => true;
        }
    }
}
