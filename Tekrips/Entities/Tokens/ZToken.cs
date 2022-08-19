using Tekrips.Scenes;

namespace Tekrips.Entities.Tokens
{
    class ZToken : TokenBase
    {
		public ZToken(BoardScene board, bool preview, int type)
			:base(board, preview, type, "Assets.CubeGreen.png")
		{			
		}

		protected override void Rotate()
		{
			if (Position == 0)
			{
				AccommodateCubes(1, 0, 0, -1, -1, -1, 1);
			}
			else
			{
				AccommodateCubes(1, -1, 1, 0, 0, 1, 0);
			}
		}

		protected override bool CanRotate()
		{
			if (Position == 0)
			{
				if (BoardX < 1 || AreOccupied(1, 0, 0, -1, -1, -1))
				{
					return false;
				}
			}
			else
			{
				if (BoardY >= Global.BoardHeight - 1 || AreOccupied(1, -1, 1, 0, 0, 1))
				{
					return false;
				}
			}
			return true;
		}
	}
}
