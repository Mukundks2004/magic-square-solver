namespace MatrixLinearIndep
{
	public class SquareMatrix<T> : IRing<SquareMatrix<T>> where T : IField<T>, new()
	{
		public T[,] Data { get; set; }

		public SquareMatrix(int size)
		{
			if (size <= 0)
			{
				throw new ArgumentException("Invalid size");
			}

			Data = new T[size, size];
			for (var i = 0; i < size *size; i++)
			{
				Data[i / size, i % size] = new T();
			}
		}

		public T this[int row, int col]
		{
			get { return Data[row, col]; }
			set { Data[row, col] = value; }
		}

		public SquareMatrix<T> Add(SquareMatrix<T> other)
		{
			if (other.Data.GetLength(0) != Data.GetLength(0) || other.Data.GetLength(1) != Data.GetLength(1))
			{
				throw new InvalidOperationException("Inconsistent sizes");
			}

			int size = Data.GetLength(0);
			SquareMatrix<T> result = new SquareMatrix<T>(size);

			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					result[i, j] = this[i, j].Add(other[i, j]);
				}
			}

			return result;
		}

		public SquareMatrix<T> Subtract(SquareMatrix<T> other)
		{
			if (other.Data.GetLength(0) != Data.GetLength(0) || other.Data.GetLength(1) != Data.GetLength(1))
			{
				throw new InvalidOperationException("Inconsistent sizes");
			}

			int size = Data.GetLength(0);
			SquareMatrix<T> result = new SquareMatrix<T>(size);

			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					result[i, j] = this[i, j].Subtract(other[i, j]);
				}
			}

			return result;
		}

		public SquareMatrix<T> Multiply(SquareMatrix<T> other)
		{
			if (Data.GetLength(0) != other.Data.GetLength(0) || Data.GetLength(1) != other.Data.GetLength(1))
			{
				throw new InvalidOperationException("Inconsistent sizes");
			}

			int size = Data.GetLength(0);
			SquareMatrix<T> result = new SquareMatrix<T>(size);

			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					for (int k = 0; k < size; k++)
					{
						result[i, j] = result[i, j].Add(this[i, k].Multiply(other[k, j]));
					}
				}
			}

			return result;
		}

		public override string ToString()
		{
			int size = Data.GetLength(0);
			string result = "";

			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					result += Data[i, j] + "  ";
				}
				result += Environment.NewLine;
			}

			return result;
		}
	}
}
