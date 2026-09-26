using System;
using System.Globalization;
using System.Numerics;

while (true)
{
    Console.WriteLine();
    Console.WriteLine("Лаба 1. Вариант 9");
    Console.WriteLine("1 - Факториал");
    Console.WriteLine("2 - Числа Фибоначчи");
    Console.WriteLine("3 - Значение функции");
    Console.WriteLine("4 - Сумма ряда Тейлора");
    Console.WriteLine("0 - Выход");
    Console.Write("Выберите задание: ");

    if (!int.TryParse(Console.ReadLine(), out int choice))
    {
        Console.WriteLine("Ошибка: введите целое число.");
        continue;
    }

    switch (choice)
    {
        case 0: return;
        case 1: Task1(); break;
        case 2: Task2(); break;
        case 3: Task3(); break;
        case 4: Task4(); break;
        default: Console.WriteLine("Нет такого пункта."); break;
    }
}

static void Task1()
{
    Console.Write("Введите n (n >= 0): ");

    if (!int.TryParse(Console.ReadLine(), out int n) || n < 0)
    {
        Console.WriteLine("Ошибка: нужно целое неотрицательное число.");
        return;
    }

    Console.WriteLine($"{n}! = {BigFactorial(n)}");
}

static BigInteger BigFactorial(int n)
{
    BigInteger result = BigInteger.One;
    for (int i = 2; i <= n; i++)
        result *= i;
    return result;
}

static void Task2()
{
    Console.Write("Введите n (n >= 0): ");

    if (!int.TryParse(Console.ReadLine(), out int n) || n < 0)
    {
        Console.WriteLine("Ошибка: нужно целое неотрицательное число.");
        return;
    }

    Console.WriteLine(FibonacciLine(n));
}

static string FibonacciLine(int n)
{
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

// A = sin(19 / (x - 5)) * ch(e^x + e^(4x))
static void Task3()
{
    Console.Write("Введите x: ");

    if (!double.TryParse(Console.ReadLine(), NumberStyles.Float,
                         CultureInfo.InvariantCulture, out double x))
    {
        Console.WriteLine("Ошибка: введите число (разделитель — точка).");
        return;
    }

    if (x == 5)
    {
        Console.WriteLine("Ошибка: функция не определена при x = 5 (деление на ноль).");
        return;
    }

    double A = Math.Sin(19.0 / (x - 5)) * Math.Cosh(Math.Exp(x) + Math.Exp(4 * x));
    Console.WriteLine($"A = {A}");
}

// ln(1+x) = x - x^2/2 + x^3/3 - ... ,  -1 < x <= 1
static void Task4()
{
    Console.Write("Введите x (-1 < x <= 1): ");

    if (!double.TryParse(Console.ReadLine(), NumberStyles.Float,
                         CultureInfo.InvariantCulture, out double x))
    {
        Console.WriteLine("Ошибка: введите число (разделитель — точка).");
        return;
    }

    if (x <= -1 || x > 1)
    {
        Console.WriteLine("Ошибка: ряд сходится только при -1 < x <= 1.");
        return;
    }

    double sum = TaylorLn(x, 1e-6, out int count);
    double lib = Math.Log(1 + x);

    Console.WriteLine($"Сумма ряда:             {sum}");
    Console.WriteLine($"Math.Log(1 + x):        {lib}");
    Console.WriteLine($"Разница:                {Math.Abs(sum - lib)}");
    Console.WriteLine($"Просуммировано членов:  {count}");
}

static double TaylorLn(double x, double eps, out int count)
{
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
