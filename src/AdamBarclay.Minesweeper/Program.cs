using System;

namespace AdamBarclay.Minesweeper
{
	internal static class Program
	{
		private const int BoardHeight = 8;
		private const int BoardWidth = 8;
		private const byte MaskHidden = 0x80;
		private const byte MaskMine = 0x40;
		private const byte MaskNumber = 0x0F;
		private const int NumberOfMines = 8;

		private static byte AnalyseCell(Span<byte> board, int x1, int x2, int x3, int y1, int y2, int y3)
		{
			var value = (board[y1 + x1] & Program.MaskMine) +
				(board[y1 + x2] & Program.MaskMine) +
				(board[y1 + x3] & Program.MaskMine) +
				(board[y2 + x1] & Program.MaskMine) +
				(board[y2 + x3] & Program.MaskMine) +
				(board[y3 + x1] & Program.MaskMine) +
				(board[y3 + x2] & Program.MaskMine) +
				(board[y3 + x3] & Program.MaskMine);

			return (byte)(value / Program.MaskMine);
		}

		private static byte AnalyseCorner(Span<byte> board, int x1, int x2, int y1, int y2)
		{
			var value = (board[y1 + x2] & Program.MaskMine) +
				(board[y2 + x2] & Program.MaskMine) +
				(board[y2 + x1] & Program.MaskMine);

			return (byte)(value / Program.MaskMine);
		}

		private static byte AnalyseEdge(Span<byte> board, int a1, int a2, int b1, int b2, int b3)
		{
			var value = (board[a1 + b1] & Program.MaskMine) +
				(board[a2 + b1] & Program.MaskMine) +
				(board[a2 + b2] & Program.MaskMine) +
				(board[a2 + b3] & Program.MaskMine) +
				(board[a1 + b3] & Program.MaskMine);

			return (byte)(value / Program.MaskMine);
		}

		private static void CreateBoard(Span<byte> board)
		{
			board.Fill(Program.MaskHidden);

			var random = new Random();

			for (var i = 0; i < Program.NumberOfMines; ++i)
			{
				int location;

				while ((board[location = random.Next(board.Length)] & Program.MaskMine) != 0)
				{
				}

				board[location] |= Program.MaskMine;
			}

			const int c1 = 0;
			const int c2 = 1;
			const int c3 = c1 * Program.BoardHeight;
			const int c4 = c2 * Program.BoardHeight;
			const int y1 = Program.BoardHeight - 1;
			const int y2 = Program.BoardHeight - 2;
			const int y3 = y1 * Program.BoardHeight;
			const int y4 = y2 * Program.BoardHeight;
			const int x1 = Program.BoardWidth - 1;
			const int x2 = Program.BoardWidth - 2;

			board[c3 + c1] |= Program.AnalyseCorner(board, c1, c2, c3, c4);
			board[c3 + x1] |= Program.AnalyseCorner(board, x1, x2, c3, c4);
			board[y3 + c1] |= Program.AnalyseCorner(board, c1, c2, y3, y4);
			board[y3 + x1] |= Program.AnalyseCorner(board, x1, x2, y3, y4);

			for (var x = 1; x < x1; ++x)
			{
				board[c3 + x] |= Program.AnalyseEdge(board, c3, c4, x - 1, x, x + 1);
				board[y3 + x] |= Program.AnalyseEdge(board, y3, y4, x - 1, x, x + 1);
			}

			for (var y = 1; y < y1; ++y)
			{
				var y5 = (y - 1) * Program.BoardHeight;
				var y6 = (y + 0) * Program.BoardHeight;
				var y7 = (y + 1) * Program.BoardHeight;

				board[y6 + c1] |= Program.AnalyseEdge(board, c1, c2, y5, y6, y7);
				board[y6 + x1] |= Program.AnalyseEdge(board, x1, x2, y5, y6, y7);

				for (var x = 1; x < x1; ++x)
				{
					board[y6 + x] |= Program.AnalyseCell(board, x - 1, x, x + 1, y5, y6, y7);
				}
			}
		}

		private static void GameOver(Span<byte> board, string gameOverMessage)
		{
			for (var i = 0; i < board.Length; ++i)
			{
				board[i] &= Program.MaskHidden ^ 0xFF;
			}

			Program.Render(board);

			Console.WriteLine(gameOverMessage);
			Console.WriteLine();
			Console.Write("   --Press any key to play again--");
			Console.ReadKey(true);

			Program.CreateBoard(board);
		}

		private static bool InputIsValid(char inputX, char inputY, ref int offset)
		{
			var x = inputX - 'A';
			var y = inputY - '1';

			if (x >= 0 && x < Program.BoardWidth && y >= 0 && y < Program.BoardHeight)
			{
				offset = (y * Program.BoardHeight) + x;

				return true;
			}

			return false;
		}

		private static void Main()
		{
			Console.Title = "Minesweeper";
			Console.BackgroundColor = ConsoleColor.Black;
			Console.ForegroundColor = ConsoleColor.White;

			// ReSharper disable once RedundantCast
			var board = (Span<byte>)stackalloc byte[Program.BoardWidth * Program.BoardHeight];

			Program.CreateBoard(board);

			var turn = 0;

			while (true)
			{
				Program.Render(board);

				Console.Write("   Please enter a column and row (e.g. A8): ");

				var offset = 0;

				var x = Console.ReadKey(true);

				if (x.Key == ConsoleKey.Escape)
				{
					break;
				}

				var charX = char.ToUpperInvariant(x.KeyChar);

				Console.Write(charX);

				var y = Console.ReadKey(true);

				if (y.Key == ConsoleKey.Escape)
				{
					break;
				}

				if (Program.InputIsValid(charX, y.KeyChar, ref offset) && (board[offset] & Program.MaskHidden) != 0)
				{
					board[offset] &= Program.MaskHidden ^ 0xFF;

					if ((board[offset] & Program.MaskMine) != 0)
					{
						Program.GameOver(board, "   **Sorry you hit a mine**");

						turn = 0;
					}
					else if (++turn == (Program.BoardWidth * Program.BoardHeight) - Program.NumberOfMines)
					{
						Program.GameOver(board, "   **Congratulations you win!**");

						turn = 0;
					}
				}
			}

			Program.Render(board);
			Console.Write("   Thanks for playing!");
			Console.WriteLine();
		}

		private static void Render(Span<byte> board)
		{
			Console.Clear();
			Console.WriteLine();
			Console.Write("    ");

			for (var x = 0; x < Program.BoardWidth; ++x)
			{
				Console.Write(" ");
				Console.Write((char)('A' + x));
			}

			Console.WriteLine();

			for (var y = 0; y < Program.BoardHeight; ++y)
			{
				var offsetY = y * Program.BoardHeight;

				Console.Write("   ");
				Console.Write(y + 1);

				for (var x = 0; x < Program.BoardWidth; ++x)
				{
					var cell = board[offsetY + x];

					if ((cell & Program.MaskHidden) != 0)
					{
						Console.ForegroundColor = ConsoleColor.DarkGray;
						Console.Write(" *");
					}
					else if ((cell & Program.MaskMine) != 0)
					{
						Console.ForegroundColor = ConsoleColor.Magenta;
						Console.Write(" m");
					}
					else
					{
						Console.ForegroundColor = (cell & Program.MaskNumber) switch
						{
							0 => ConsoleColor.Gray,
							1 => ConsoleColor.Blue,
							2 => ConsoleColor.Green,
							3 => ConsoleColor.Red,
							4 => ConsoleColor.DarkBlue,
							5 => ConsoleColor.DarkRed,
							6 => ConsoleColor.DarkCyan,
							7 => ConsoleColor.DarkGreen,
							8 => ConsoleColor.Cyan,
							var _ => 0
						};

						Console.Write(" ");
						Console.Write(cell & Program.MaskNumber);
					}
				}

				Console.ForegroundColor = ConsoleColor.White;

				Console.WriteLine();
			}

			Console.WriteLine();
		}
	}
}
