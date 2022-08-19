using Kriptok.Entities.Base;
using Kriptok.Views.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tekrips.Entities
{
    class Cube : EntityBase<SpriteView>
    {
		public Cube(string resourceName) : base(new SpriteView(typeof(Cube).Assembly, resourceName))
		{			
		}

		public int BoardX;
		public int BoardY;

        protected override void OnFrame()
        {            
        }

        public void SetX(int x)
		{
			BoardX = x;
			Location.X = BoardX * Global.CubeSize + Global.BoardX + Global.CubeSize / 2;
		}

		public void SetY(int y)
		{
			BoardY = y;
			Location.Y = BoardY * Global.CubeSize + Global.BoardY + Global.CubeSize / 2;
		}

		public void SetBoardCoords(int x, int y)
		{
			SetX(x);
			SetY(y);
		}

		public void Descend()
		{
			MoveY(1);
		}

		public void MoveX(int direction)
		{
			SetX(BoardX + direction);
		}

		public void MoveY(int direction)
		{
			SetY(BoardY + direction);
		}
	}
}
