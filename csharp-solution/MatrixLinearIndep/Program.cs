using System.Data;

namespace MatrixLinearIndep
{
	public class Program
	{
		public static Fraction[,] MakeEmptyArray(int rows, int cols)
		{
			var res = new Fraction[rows, cols];
			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < cols; j++)
				{
					res[i, j] = new Fraction();
				}
			}

			return res;
		}

		public static Fraction[] MakeEmptyVector(int rows)
		{
			var res = new Fraction[rows];
			for (int i = 0; i < rows; i++)
			{
				res[i] = new Fraction();
			}

			return res;
		}

		public static void PrintRectangularArray(Fraction[,] fracs)
		{
			int rows = fracs.GetLength(0);
			int cols = fracs.GetLength(1);

			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < cols; j++)
				{
					Console.Write(fracs[i, j] + "  ");
				}
				Console.Write(Environment.NewLine);
			}
		}

		public static void PrintRectangularArrayMaybeNull(Fraction?[,] fracs)
		{
			int rows = fracs.GetLength(0);
			int cols = fracs.GetLength(1);

			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < cols; j++)
				{
					var currentCell = fracs[i, j];
					if (currentCell == null)
					{
						Console.Write(".  ");
					}
					else
					{
						Console.Write(currentCell);
					}
					Console.Write("  ");
				}
				Console.Write(Environment.NewLine);
			}
		}

		public static Fraction[,] MakeDuplicate(Fraction[,] original)
		{
			var result = MakeEmptyArray(original.GetLength(0), original.GetLength(1));

			for (int i = 0; i < result.GetLength(0); i++)
			{
				for (int j = 0; j < result.GetLength(1); j++)
				{
					result[i, j] = original[i, j].GetCopy();
				}
			}

			return result;
		}

		public static Fraction?[,] MakeDuplicateMaybeNull(Fraction?[,] original)
		{
			var result = MakeEmptyArray(original.GetLength(0), original.GetLength(1));

			for (int i = 0; i < result.GetLength(0); i++)
			{
				for (int j = 0; j < result.GetLength(1); j++)
				{
					if (original[i, j] is not null)
					{
						result[i, j] = original[i, j].GetCopy();
					}
					else
					{
						result[i, j] = null;
					}
				}
			}

			return result;
		}

		public static void PrintVector(Fraction[] vector)
		{
			foreach (var f in vector)
			{
				Console.Write(f.ToString() + " ");
			}
			Console.WriteLine();
		}

		public static Fraction Get(Fraction[,] fracs, int row, int col)
		{
			return fracs[row, col];
		}

		public static int CountNulls<T>(T[,] array)
		{
			return array.Cast<T>().Count(element => element == null);
		}

		public static Fraction[,] AdjoinVectorToMatrix(Fraction[,] matrix, Fraction[] vector)
		{
			int matrixRows = matrix.GetLength(0);
			int matrixCols = matrix.GetLength(1);
			int vectorLength = vector.Length;

			if (vectorLength != matrixRows)
			{
				throw new ArgumentException("Bad size");
			}

			Fraction[,] resultMatrix = MakeEmptyArray(matrixRows, matrixCols + 1);
			for (int i = 0; i < matrixRows; i++)
			{
				for (int j = 0; j < matrixCols; j++)
				{
					resultMatrix[i, j] = matrix[i, j];
				}

				resultMatrix[i, matrixCols] = vector[i];
				//Console.WriteLine($"{i} {matrixCols}");
			}

			return resultMatrix;
		}

		public static Fraction[] Solve(Fraction?[,] matrix)
		{
			// Step 0) Make the matrix, with n^2 + 2 cols and 2n + 2 rows
			// Step 1) populate the matrix with the 10 columns (vars) and 8 rows (info), dont include const col
			// Step 2) reduce the COLUMNS to 7 (because we have 3 vars) and populate adjoint row,
			//	 put result in new matrx that is 7 cols and 8 rows
			// Step 3) use gaussian to remove a row, make it 7 cols and 7 rows- the process should put the matrix in
			//	RREF which essentially solves the square: specifically, it assigns each variable a singular value,
			//	which you can then 
			// Step 4) 

			// Step 0
			var size = matrix.GetLength(0);

			if (matrix.GetLength(1) != size)
			{
				throw new Exception("non square magic square");
			}

			var rawMatrix = MakeEmptyArray(2 * size + 2, size * size + 1);
			var debugMatrix = MakeEmptyArray(2 * size + 2, size * size + 1);

			// Step 1
			for (int rowInfo = 0; rowInfo < size; rowInfo++)
			{
				for (int var = 0; var < size; var++)
				{
					rawMatrix[rowInfo, size * rowInfo + var] = new Fraction(1);
					debugMatrix[rowInfo, size * rowInfo + var] = new Fraction(1);
				}
			}

			for (int colInfo = 0; colInfo < size; colInfo++)
			{
				for (int var = 0; var < size; var++)
				{
					rawMatrix[size + colInfo, colInfo + size * var] = new Fraction(1);
					debugMatrix[size + colInfo, colInfo + size * var] = new Fraction(1);
				}
			}

			// row = 6, col = 0, 4, 8
			for (int rowAndCol = 0; rowAndCol < size; rowAndCol++)
			{
				rawMatrix[2 * size, (size + 1) * rowAndCol] = new Fraction(1);
				debugMatrix[2 * size, (size + 1) * rowAndCol] = new Fraction(1);
			}

			// row = 7, col = 2, 4, 6
			for (int rowAndCol = 0; rowAndCol < size; rowAndCol++)
			{
				rawMatrix[2 * size + 1, (size - 1) * (rowAndCol + 1)] = new Fraction(1);
				debugMatrix[2 * size + 1, (size - 1) * (rowAndCol + 1)] = new Fraction(1);
			}

			for (int i = 0; i < 2 * size + 2; i++)
			{
				rawMatrix[i, size * size] = new Fraction(-1);
				debugMatrix[i, size * size] = new Fraction(-1);
			}

			//PrintRectangularArrayMaybeNull(matrix);
			//Console.WriteLine("====================");

			//PrintRectangularArrayMaybeNull(debugMatrix);
			//Console.WriteLine("====================");

			//PrintRectangularArray(rawMatrix);

			//Console.WriteLine();

			// ======================================================================
			// End phase 1, we have the raw matrix, now shorten the cols by the amt of data
			// ======================================================================

			var adjointVector = MakeEmptyVector(size * 2 + 2);

			var newColCount = CountNulls(matrix);

			var noUnnecessaryVarsMatrix = MakeEmptyArray(size * 2 + 2, newColCount + 1);

			for (int row = 0; row < 2 * size + 2; row++)
			{
				int writingCol = 0;
				for (int col = 0; col < size * size; col++)
				{
					var currentMagicSquareCell = matrix[col / 3, col % 3];
					if (currentMagicSquareCell is not null && rawMatrix[row, col].IsOne())
					{
						adjointVector[row] = adjointVector[row].Subtract(currentMagicSquareCell);
					}
					else
					{
						noUnnecessaryVarsMatrix[row, writingCol] = rawMatrix[row, col];
					}
					if (currentMagicSquareCell is null)
					{
						writingCol++;
					}
				}

				noUnnecessaryVarsMatrix[row, newColCount] = new Fraction(-1);
			}

			// ======================================================================
			// End phase 2, we have the thinner matrix, now to shorten rows
			// ======================================================================

			//PrintRectangularArray(noUnnecessaryVarsMatrix);

			//Console.WriteLine();
			//PrintVector(adjointVector);

			var adjoinedThinMatrix = AdjoinVectorToMatrix(noUnnecessaryVarsMatrix, adjointVector);

			//PrintRectangularArray(adjoinedThinMatrix);

			//Console.WriteLine("-------------------------");

			Rref(adjoinedThinMatrix);


			//PrintRectangularArray(adjoinedThinMatrix);

			Fraction[] resultVector = MakeEmptyVector(newColCount + 1);
			for (int i = 0; i < newColCount + 1; i++)
			{
				resultVector[i] = adjoinedThinMatrix[i, newColCount + 1];
			}

			return resultVector;
		}

		public static Fraction[,] Rref(Fraction[,] matrix)
		{
			int lead = 0;
			int rowCount = matrix.GetLength(0);
			int colCount = matrix.GetLength(1);

			for (int currentRowIndex = 0; currentRowIndex < rowCount; currentRowIndex++)
			{
				if (colCount <= lead)
				{
					break;
				}

				int notSureWhatThisDoesI = currentRowIndex;
				while (matrix[notSureWhatThisDoesI, lead].IsZero())
				{
					notSureWhatThisDoesI++;
					if (notSureWhatThisDoesI == rowCount)
					{
						notSureWhatThisDoesI = currentRowIndex;
						lead++;

						if (colCount == lead)
						{
							lead--;
							break;
						}
					}
				}

				for (int j = 0; j < colCount; j++)
				{
					(matrix[notSureWhatThisDoesI, j], matrix[currentRowIndex, j]) = (matrix[currentRowIndex, j], matrix[notSureWhatThisDoesI, j]);
				}

				Fraction div = matrix[currentRowIndex, lead];
				if (!div.IsZero())
				{
					for (int j = 0; j < colCount; j++)
					{
						matrix[currentRowIndex, j] = matrix[currentRowIndex, j].Divide(div);
					}
					for (int j = 0; j < rowCount; j++)
					{
						if (j != currentRowIndex)
						{
							Fraction sub = matrix[j, lead];

							for (int k = 0; k < colCount; k++)
							{
								matrix[j, k] = matrix[j, k].Subtract(sub.Multiply(matrix[currentRowIndex, k]));
							}
						}
					}
				}
				lead++;
			}

			return matrix;
		}

		public static Fraction[,] FillMagicSqureWithSols(Fraction?[,] inputWithHoles, Fraction[] answers)
		{
			var result = MakeDuplicateMaybeNull(inputWithHoles);
			int numberOfHoles = CountNulls(inputWithHoles);

			//PrintRectangularArray(result);
			//PrintRectangularArray(inputWithHoles);
			//Extra counting the square total
			if (numberOfHoles != answers.Length - 1)
			{
				throw new Exception("wrong number of answers");
			}

			int answerNo = 0;
			int size = result.GetLength(0);

			for (int i = 0; i < size * size; i++)
			{
				if (result[i / 3, i % 3] is null)
				{
					result[i / 3, i % 3] = answers[answerNo];
					answerNo++;
				}
			}

			return result;
		}
		
		public static void Main(string[] args)
		{
			//Console.WriteLine("Test");
			//var squareMat = new SquareMatrix<Fraction>(4);
			//squareMat[1, 1] = new Fraction(2, 3);
			//Solve();
			////Print(MakeEmptyArray(3, 4));
			///

			//var input = new Fraction?[,]
			//{
			//	{ new Fraction(2), new Fraction(3), null },
			//	{ null,            null,            null },
			//	{ new Fraction(5), null,            null },
			//};

			var input = new Fraction?[,]
			{
				{ new Fraction(1), null,            new Fraction(2) },
				{ null,            null,            null },
				{ new Fraction(3), null,            null },
			};

			Console.WriteLine("Input Mgc Sqr:");
			PrintRectangularArrayMaybeNull(input);
			Console.WriteLine();

			var result = Solve(input);

			Fraction[,] augmentedMatrix = new Fraction[,]
			{
				{ new Fraction(2), new Fraction(1), new Fraction(10) },
				{ new Fraction(2), new Fraction(2), new Fraction(15) },
				{ new Fraction(4), new Fraction(3), new Fraction(25) }
			};

			Fraction[,] augmentedMatrix2 = new Fraction[,]
			{
				{ new Fraction(1), new Fraction(2), new Fraction(-1), new Fraction(-4) },
				{ new Fraction(2), new Fraction(3), new Fraction(-1), new Fraction(-11) },
				{ new Fraction(-2), new Fraction(0), new Fraction(-3), new Fraction(22) },
				{ new Fraction(-4), new Fraction(0), new Fraction(-6), new Fraction(44) },
			};

			//PrintRectangularArray(augmentedMatrix2);
			//Console.WriteLine();
			//PrintRectangularArray(Rref(augmentedMatrix2));

			//PrintVector(result);



			var answerMatrix = FillMagicSqureWithSols(input, result);

			Console.WriteLine("Output Magic Sqr:");

			PrintRectangularArray(answerMatrix);

			Console.WriteLine("-----------------");
			Console.WriteLine("Done");


		}
	}
}
