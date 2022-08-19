using Kriptok.Entities.Base;
using System.Windows.Forms;
using Tekrips.Entities.Tokens;
using Tekrips.Scenes;

namespace Tekrips.Entities
{
    abstract class TokenBase : EntityBase
    {
		private readonly Cube[] cubes;
		private readonly BoardScene board;
		private int countToDescend;
		private int countToMove;
		private int countToRotate;

		private bool previewMode;
		internal int Type;

		protected int BoardX;
		protected int BoardY;
		protected int Position;

		public TokenBase(BoardScene board, bool preview, int type, string resourceName)
		{
			this.board = board;

			// Cada pieza consta de cuatro cubos. Inicializo punteros, y los creo.
			cubes = new Cube[4];
			for (int i = 0; i < 4; i++)
			{
				cubes[i] = new Cube(resourceName);
			}

			// Coordenadas iniciales.
			if (preview == true)
			{
				BoardX = 16; BoardY = 3;
			}
			else
			{
				BoardX = 4; BoardY = 1;
			}
			Type = type;
			previewMode = preview;

			cubes[1].SetBoardCoords(BoardX, BoardY); // Cubo principal.
			Rotate();
		}

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);

			foreach (var item in cubes)
			{
				Add(item);
			}
        }

        protected override void OnFrame()
        {         
			if (previewMode) return;

			if (Input.Left())
			{
				MoveLeft();
			}
			else if (Input.Right())
			{
				MoveRight();

			}
			else if (Input.Down())
			{
				// Si apreto para abajo, baja más rápido.			
				countToDescend += (int)((Global.DescentSpeed - countToDescend) * 0.9925f);
			}
			else if (Input.KeyPressed(Keys.Space))
			{
				if (countToRotate <= 0 && CanRotate())
				{
					Rotate();
					Audio.PlaySound(typeof(TokenBase).Assembly, "Assets.Clock.wav");
					countToRotate = Global.RotateTime;
				}
			}

			// Si pasó el tiempo necesario, bajo. 
			if (countToDescend >= Global.DescentSpeed)
			{
				Descend();
				countToDescend = 0;
			}

			// Aumento el tiempo que toma hasta bajar.
			countToDescend += 1;

			// Reduzco el tiempo para volver a moverla hacia los lados.
			if (countToMove > 0) countToMove -= 1;

			// Reduzco el tiempo para volver a rotar.
			if (countToRotate > 0) countToRotate -= 1;
		}

		public void Descend()
		{
			if (!CanMove(0, 1))
			{
				Stuck();
			}
			else
			{
				BoardY += 1;
				for (int i = 0; i < 4; i++) cubes[i].Descend();
			}
		}

		private void MoveLeft()
		{
			Move(-1);
		}

		private void MoveRight()
		{
			Move(1);
		}

		private void Move(int direction)
		{
			if (CanMove(direction, 0))
			{
				BoardX += direction;

				for (int i = 0; i < 4; i++)
				{
					cubes[i].MoveX(direction);
				}

				countToMove = Global.RotateTime;
			}
		}

		public bool CanMove(int directionX, int directionY)
		{
			if (countToMove > 0 && directionX != 0) return false;

			// La condición puede parecer demasiado compleja, pero se logra una 
			// mejor performance que si se considerara cada caso en particular.
			for (int i = 0; i < 4; i++)
			{
				if (cubes[i].BoardX + directionX >= Global.BoardWidth
				 || cubes[i].BoardX + directionX < 0
				 || cubes[i].BoardY + directionY >= Global.BoardHeight
				 || board.IsOccupied(cubes[i].BoardX + directionX, cubes[i].BoardY + directionY))
				{
					return false;
				}
			}
			return true;
		}

		private void Stuck()
		{
			Audio.PlaySound(typeof(TokenBase).Assembly, "Cancel1.wav");
			for (int i = 0; i < 4; i++)
			{
				board.Occupy(cubes[i]);
				if (cubes[i].BoardY <= 1)
				{
					Scene.SendMessage(TetrisMessages.GameOver);
					return;
				}
			}

			Scene.SendMessage(TetrisMessages.ReleaseToken);				
			this.Die();
		}

		protected void AccommodateCubes(int x0, int y0, int x2, int y2, int x3, int y3, int position)
		{
			cubes[0].SetBoardCoords(BoardX + x0, BoardY + y0);
			cubes[1].SetBoardCoords(BoardX, BoardY);
			cubes[2].SetBoardCoords(BoardX + x2, BoardY + y2);
			cubes[3].SetBoardCoords(BoardX + x3, BoardY + y3);
			this.Position = position;
		}

		protected bool AreOccupied(int x1, int y1, int x2, int y2, int x3, int y3)
		{
			return board.AreOccupied(BoardX + x1, BoardY + y1, BoardX + x2, BoardY + y2, BoardX + x3, BoardY + y3);
		}

		public void KillToken()
		{
			for (int i = 0; i < 4; i++) cubes[i].Die();
			Die();
		}		

		protected abstract void Rotate();
		protected abstract bool CanRotate();

		public static TokenBase NewToken(BoardScene board, int tokenType, bool preview)
		{
			switch (tokenType)
			{
				case 0: return new IToken(board, preview, tokenType);
				case 1: return new JToken(board, preview, tokenType);
				case 2: return new LToken(board, preview, tokenType);
				case 3: return new OToken(board, preview, tokenType);
				case 4: return new SToken(board, preview, tokenType);
				case 5: return new TToken(board, preview, tokenType);
				case 6: return new ZToken(board, preview, tokenType);
			}
			return null;
		}
    }
}
