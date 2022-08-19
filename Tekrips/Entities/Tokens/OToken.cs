using Tekrips.Scenes;

namespace Tekrips.Entities.Tokens
{
    class OToken : TokenBase
    {
		public OToken(BoardScene board, bool preview, int type)
			: base(board, preview, type, "Assets.CubeBlue.png")
		{
			if (preview == true)
			{
				this.BoardX += 1;
			}

			AccommodateCubes(-1, -1, 0, -1, -1, 0, 0);
		}

		protected override void Rotate()
		{

		}

		protected override bool CanRotate()
		{
			return false;
		}
	}
}
