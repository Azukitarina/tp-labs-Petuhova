using System;
using System.Globalization;
using System.Numerics;

namespace Lab4.Core
{
    public static class MathAlgorithms
    {
        public static BigInteger BigFactorial(int n)
        {
            if (n < 0) throw new ArgumentOutOfRangeException(nameof(n), "n должно быть >= 0");
            BigInteger result = BigInteger.One;
            for (int i = 2; i <= n; i++)
                result *= i;
            return result;
        }
        public static string FibonacciLine(int n)
        {
            if (n < 0) throw new ArgumentOutOfRangeException(nameof(n), "n должно быть >= 0");

            string result = "";
            long a = 0, b = 1;
            for (int i = 0; i <= n; i++)
            {
                if (i > 0) result += ", ";
                result += a;
                long next = a + b;
                a = b;
                b = next;
            }
            return result;
        }
        public static double CalculateA(double x)
        {
            if (x == 5)
                throw new DivideByZeroException("Функция не определена при x = 5 (деление на ноль).");

            return Math.Sin(19.0 / (x - 5)) * Math.Cosh(Math.Exp(x) + Math.Exp(4 * x));
        }
        public static double TaylorLn(double x, double eps, out int count)
        {
            if (x <= -1 || x > 1)
                throw new ArgumentOutOfRangeException(nameof(x), "Ряд сходится только при -1 < x <= 1");

            double sum = 0.0;
            double term = x;
            int k = 1;

            while (Math.Abs(term) > eps)
            {
                sum += term;
                k++;
                term = -term * x * (k - 1) / k;
            }
            count = k - 1;
            return sum;
        }
    }
}