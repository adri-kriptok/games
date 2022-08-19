using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekrips.Scenes;

namespace Tekrips.Entities.Tokens
{
    class IToken : TokenBase
    {
		public IToken(BoardScene board, bool preview, int type)
			:base(board, preview, type, "Assets.CubeRed.png")
		{
			//super(program, board, program.graphicLibrary("tekrips").getGraph("res/graphs/cubered.bmp"), preview, type);
		}

		protected override void Rotate()
		{
			if (Position == 0)
			{
				AccommodateCubes(-1, 0, 1, 0, 2, 0, 1);
			}
			else
			{
				AccommodateCubes(0, -1, 0, 1, 0, 2, 0);
			}
		}

		protected override bool CanRotate()
		{
			if (Position == 0)
			{
				if (BoardX >= Global.BoardWidth - 2 || BoardX < 1 || AreOccupied(-1, 0, 1, 0, 2, 0)) return false;
			}
			else
			{
				if (BoardY >= Global.BoardHeight - 2 || BoardY < 1 || AreOccupied(0, -1, 0, 1, 0, 2)) return false;
			}
			return true;
		}
	}
}
