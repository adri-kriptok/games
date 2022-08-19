using Kriptok.Entities.Base;
using Kriptok.Extensions;
using Kriptok.IO;
using Kriptok.Scenes;
using System;
using Tekrips.Entities;

namespace Tekrips.Scenes
{
	enum TetrisMessages
	{
		ReleaseToken = 0,
		GameOver = 1,
		Restart = 2
	}

	class BoardScene : SceneBase
	{
		public Cube[][] matrix;
		private TokenBase currentToken;
		private TokenBase previewToken;

		private int lines;
		private int points;

		private Record record;

		protected override void Run(SceneHandler h)
		{
#if DEBUG
			Config.Load<BaseConfiguration>().Mute();
#endif

			h.ScreenRegion.SetBackground(Assembly, "Assets.Background.png");

			h.PlayMusic(Assembly, "Tetris.mid");

			ReStart(h);
		}

		protected override void OnMessage(SceneHandler h, object message)
		{
			base.OnMessage(h, message);

			if (message is TetrisMessages msg)
			{
				switch (msg)
				{
					case TetrisMessages.GameOver:
						{
							currentToken.KillToken();

							h.Add(new GameOver());
							h.PlaySound(Assembly, "Assets.Failure1.wav");
							h.Wait(3000);
							h.WaitForKeyPress();

							Records.Save(lines, points);

							// Destruyo todos los objetos.
							h.Kill<EntityBase>();

							// Reinicio el juego.
							ReStart(h);
							return;
						}
					case TetrisMessages.ReleaseToken:
						{
							ReleaseToken(h);
							break;
						}
				}
			}
		}

		private void ReStart(SceneHandler h)
		{
			record = Records.Load();
			lines = 0;
			points = 0;
			Global.DescentSpeed = Global.InitialDescentSpeed;

			h.Write(Global.WhiteFont, 122, 89, () => $"Lineas: {$"           {lines}".Right(9)}").LeftMiddle();
			h.Write(Global.WhiteFont, 122, 105, () => $"Puntos: {$"           {points}".Right(9)}").LeftMiddle();
			h.Write(Global.PinkFont, 122, 145, () => $"Record").LeftMiddle();
			h.Write(Global.PinkFont, 122, 161, () => $"Lineas: {$"           {record.Lines}".Right(9)}").LeftMiddle();
			h.Write(Global.PinkFont, 122, 177, () => $"Puntos: {$"           {record.Points}".Right(9)}").LeftMiddle();

			matrix = (new Cube[Global.BoardWidth, Global.BoardHeight]).ToJaggedArray();

			previewToken = h.Add(TokenBase.NewToken(this, Rnd(), true));
			ReleaseToken(h);
		}

		/// <summary>
		/// Obtiene un número aleatorio entre 0 y 6.
		/// </summary>		
		private static int Rnd() => Rand.Next(0, 6);

		public void ReleaseToken(SceneHandler h)
		{
			int l = 0;

			currentToken = h.Add(TokenBase.NewToken(this, previewToken.Type, false));
			previewToken.KillToken();
			previewToken = h.Add(TokenBase.NewToken(this, Rnd(), true));

			for (int j = Global.BoardHeight - 1; j > 0; j--)
			{
				bool line = true;
				for (int i = 0; i < Global.BoardWidth; i++)
				{
					if (matrix[i][j] == null)
					{
						line = false;
						break;
					}
				}

				if (line)
				{
					ClearLine(h, j); j++;
					this.lines++;
					l++;
					if (this.lines % 10 == 0 && Global.DescentSpeed >= 25)
					{
						Global.DescentSpeed -= 10;
					}
				}
			}

			if (l > 0)
			{
				points += (int)Math.Pow(10, l);
			}
		}

		private void ClearLine(SceneHandler h, int line)
		{
			h.PlaySound(Assembly, "Assets.BOTON00.WAV");

			for (int i = 0; i < Global.BoardWidth; i++)
			{
				var c = matrix[i][line];
				matrix[i][line] = null;
				c.Die();
			}

			for (int j = line - 1; j > 0; j--)
			{
				for (int i = 0; i < Global.BoardWidth; i++)
				{
					if (matrix[i][j] != null)
					{
						matrix[i][j].MoveY(1);
						Occupy(matrix[i][j]);
						matrix[i][j] = null;
					}
				}
			}
		}

		public void Occupy(Cube cube)
		{
			matrix[cube.BoardX][cube.BoardY] = cube;
		}

		public bool IsOccupied(int x, int y)
		{
			return matrix[x][y] != null;
		}

		public bool AreOccupied(int x1, int y1, int x2, int y2, int x3, int y3)
		{
			return IsOccupied(x1, y1) || IsOccupied(x2, y2) || IsOccupied(x3, y3);
		}
	}
}
