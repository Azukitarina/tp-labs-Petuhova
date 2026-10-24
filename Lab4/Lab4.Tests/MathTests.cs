using System;
using Lab4.Core;
using Xunit;

namespace Lab4.Tests
{
    public class MathTests
    {
        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 1)]
        [InlineData(5, 120)]
        [InlineData(10, 3628800)]
        public void BigFactorial_ValidData_ReturnsExpected(int n, long excepted)
        {
            var result = MathAlgorithms.BigFactorial(n);
            Assert.Equal(excepted, (long)result);
        }
        [Fact]
        public void BigFactorial_20_ReturnsCorrectValue()
        {
            var result = MathAlgorithms.BigFactorial(20);
            Assert.Equal(2432902008176640000, (long)result);
        }
        [Fact]
        public void BigFactorial_Negative_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => MathAlgorithms.BigFactorial(-5));
        }
        [Theory]
        [InlineData(0, "0")]
        [InlineData(1, "0, 1")]
        [InlineData(5, "0, 1, 1, 2, 3, 5")]
        public void FibonacciLine_ValidData_ReturnsExpected(int n, string expected)
        {
            Assert.Equal(expected, MathAlgorithms.FibonacciLine(n));
        }
        [Fact]
        public void FibonacciLine_Negative_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => MathAlgorithms.FibonacciLine(-1));
        }
        [Fact]
        public void CalculateA_DivideByZero_ThrowsException()
        {
            Assert.Throws<DivideByZeroException>(() => MathAlgorithms.CalculateA(5));
        }
        [Fact]
        public void TaylorLn_ValidData_ReturnsCorrectValue()
        {
            double x = 0.5;
            double eps = 1e-6;
            double result = MathAlgorithms.TaylorLn(x, eps, out int count);
            Assert.Equal(Math.Log(1 + x), result, 5);
            Assert.True(count > 0);
        }
        [Theory]
        [InlineData(-1.5)]
        [InlineData(1.5)]
        public void TaylorLn_OutOfRange_ThrowsException(double x)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => MathAlgorithms.TaylorLn(x, 1e-6, out _));
        }
    }
}