using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekrips.Scenes;

namespace Tekrips.Entities.Tokens
{
    class LToken : TokenBase
    {
		public LToken(BoardScene board, bool preview, int type)
			: base(board, preview, type, "Assets.CubeGray.png")
		{			
		}

		protected override void Rotate()
		{
			switch (Position)
			{
				case 0: AccommodateCubes(-1, 0, 1, 0, 1, -1, 1); break;
				case 1: AccommodateCubes(0, -1, 0, 1, 1, 1, 2); break;
				case 2: AccommodateCubes(-1, 0, 1, 0, -1, 1, 3); break;
				case 3: AccommodateCubes(-1, -1, 0, -1, 0, 1, 0); break;
			}
		}

		protected override bool CanRotate()
		{
			switch (Position)
			{
				case 0: if (BoardX >= Global.BoardWidth - 1 || AreOccupied(-1, 0, 1, 0, 1, -1)) return false; break;
				case 1: if (BoardY >= Global.BoardHeight - 1 || AreOccupied(0, -1, 0, 1, 1, 1)) return false; break;
				case 2: if (BoardX < 1 || AreOccupied(-1, 0, -1, 1, 0, 1)) return false; break;
				case 3: if (BoardY < 1 || AreOccupied(-1, -1, 0, -1, 0, 1)) return false; break;
			}
			return true;
		}
	}
}
