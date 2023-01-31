using Kriptok.Views.Texts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tekrips
{
    class Global
    {
        public static readonly SuperFont BlueFont  = SuperFont.Build(builder =>
        {
            builder.Font = new Font("Courier New", 12, FontStyle.Bold);
            builder.SetColor(Color.White, Color.CornflowerBlue);
            builder.SetShadow(-1, 1, Color.DarkBlue);
        });

        public static readonly SuperFont GoldFont = SuperFont.Build(builder =>
        {
            builder.Font = new Font("Courier New", 12, FontStyle.Bold);
            builder.SetColor(Color.White, Color.Gold);
            builder.SetShadow(-1, 1, Color.DarkRed);
        });


		public static int InitialDescentSpeed = 60;
		public static int DescentSpeed = InitialDescentSpeed;			   
		public const int RotateTime = 10;			   
		public const int BoardWidth = 10;
		public const int BoardHeight = 23;
		public const int CubeSize = 8;			   
		public const int BoardX = 8;
		public const int BoardY = 8;
	}
}
