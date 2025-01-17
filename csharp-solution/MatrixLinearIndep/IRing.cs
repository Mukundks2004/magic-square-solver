namespace MatrixLinearIndep
{
	public interface IRing<T> where T : IRing<T>
	{
		T Add(T other);
		T Subtract(T other);
		T Multiply(T other);
	}
}
