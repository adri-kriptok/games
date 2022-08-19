using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekrips.Scenes;

namespace Tekrips.Entities.Tokens
{
    class SToken : TokenBase
    {
		public SToken(BoardScene board, bool preview, int type)
			: base(board, preview, type, "Assets.CubeCyan.png")
		{			
		}

		protected override void Rotate()
		{
			if (Position == 0)
			{
				AccommodateCubes(-1, 0, 0, -1, 1, -1, 1);
			}
			else
			{
				AccommodateCubes(-1, -1, -1, 0, 0, 1, 0);
			}
		}

		protected override bool CanRotate()
		{
			if (Position == 0)
			{
				if (BoardX >= Global.BoardWidth - 1 || AreOccupied(-1, 0, 0, -1, 1, -1))
				{
					return false;
				}
			}
			else
			{
				if (BoardY >= Global.BoardHeight - 1 || AreOccupied(-1, -1, -1, 0, 0, 1))
				{
					return false;
				}
			}
			return true;
		}
	}
}
