namespace MatrixLinearIndep
{
	public interface IField<T> : IRing<T> where T : IField<T>
	{
		T Divide (T value);
	}
}
