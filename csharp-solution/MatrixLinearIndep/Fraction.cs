namespace MatrixLinearIndep
{
	public class Fraction : IField<Fraction>
	{
		public int Numerator { get; set; }
		public int Denominator { get; set; }

		public Fraction()
		{
			Numerator = 0;
			Denominator = 1;
		}

		public Fraction(int num)
		{
			Numerator = num;
			Denominator = 1;
		}

		public Fraction(int numerator, int denominator)
		{
			if (denominator == 0)
			{
				throw new ArgumentException("Denominator is zero");
			}

			Numerator = numerator;
			Denominator = denominator;
		}

		private void Simplify()
		{
			int gcd = GCD(Numerator, Denominator);
			Numerator /= gcd;
			Denominator /= gcd;
		}

		private static int GCD(int a, int b)
		{
			while (b != 0)
			{
				int temp = b;
				b = a % b;
				a = temp;
			}
			return a;
		}

		public Fraction Add(Fraction other)
		{
			int commonDenominator = this.Denominator * other.Denominator;
			int numeratorResult = (this.Numerator * other.Denominator) + (other.Numerator * this.Denominator);

			return new Fraction(numeratorResult, commonDenominator);
		}

		public Fraction Subtract(Fraction other)
		{
			int commonDenominator = this.Denominator * other.Denominator;
			int numeratorResult = (this.Numerator * other.Denominator) - (other.Numerator * this.Denominator);

			return new Fraction(numeratorResult, commonDenominator);
		}

		public Fraction Multiply(Fraction other)
		{
			int numeratorResult = this.Numerator * other.Numerator;
			int denominatorResult = this.Denominator * other.Denominator;

			return new Fraction(numeratorResult, denominatorResult);
		}

		public Fraction Divide(Fraction other)
		{
			if (other.Numerator == 0)
			{
				throw new DivideByZeroException("Div error");
			}

			int numeratorResult = Numerator * other.Denominator;
			int denominatorResult = Denominator * other.Numerator;

			return new Fraction(numeratorResult, denominatorResult);
		}

		public bool IsOne()
		{
			return Numerator == Denominator;
		}

		public bool IsZero()
		{
			return Numerator == 0;
		}

		public override string ToString()
		{
			Simplify();
			return $"{Numerator}/{Denominator}";
		}

		public Fraction GetCopy()
		{
			return new Fraction(Numerator, Denominator);
		}
	}
}
