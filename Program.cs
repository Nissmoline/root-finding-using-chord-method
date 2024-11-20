using System;
using System.Diagnostics;

namespace ChordOptimized
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Введите границы интервала через пробел [a,b]:");
            string[] inputs = Console.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            double a = double.Parse(inputs[0]);
            double b = double.Parse(inputs[1]);

            Console.WriteLine("Введите точность по аргументу (eps1):");
            double epsilon1 = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("Введите точность по функции (eps2):");
            double epsilon2 = Convert.ToDouble(Console.ReadLine());

            Stopwatch totalStopwatch = Stopwatch.StartNew();
            FindRoots(a, b, epsilon1, epsilon2);
            totalStopwatch.Stop();

            double totalElapsedMilliseconds = totalStopwatch.Elapsed.TotalMilliseconds;
            Console.WriteLine("\nОбщее время счета: {0} мс", totalElapsedMilliseconds);
            Console.WriteLine("\nНажмите Enter для выхода...");
            Console.ReadLine();
        }

        static void FindRoots(double a, double b, double epsilon1, double epsilon2)
        {
            double step = 0.16;
            double fa = f(a);
            for (double x = a; x < b; x += step)
            {
                double fxStep = f(x + step);
                if (fa * fxStep < 0)
                {
                    int iterations = 0;
                    int funcEvaluations = 0;
                    double convergenceParameter;
                    double error;
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    double root = ChordMethod(x, x + step, epsilon1, epsilon2, out iterations, out funcEvaluations, out convergenceParameter, out error);
                    stopwatch.Stop();
                    double elapsedMilliseconds = stopwatch.Elapsed.TotalMilliseconds;

                    if (!double.IsNaN(root))
                    {
                        Console.WriteLine("\nНайден корень: x = {0:F6}", root);
                        Console.WriteLine("Погрешность: {0:E6}", error);
                        Console.WriteLine("Значение функции f(Xi): {0:E6}", f(root));
                        Console.WriteLine("Количество итераций: {0}", iterations);
                        Console.WriteLine("Количество вычислений функции f(x): {0}", funcEvaluations);
                        Console.WriteLine("Время счета: {0} мс", elapsedMilliseconds);
                        Console.WriteLine("Параметр сходимости: {0:F6}", convergenceParameter);
                    }
                }
                fa = fxStep;
            }
        }

        static double f(double x)
        {
            if (x + 7 <= 0)
            {
                return double.NaN;
            }
            double result = 2 * Math.Log10(x + 7) - 5 * Math.Sin(x);
            return result;
        }

        static double ChordMethod(double a, double b, double epsilon1, double epsilon2, out int iterations, out int funcEvaluations, out double convergenceParameter, out double error)
        {
            funcEvaluations = 0;

            double x_fixed, x_prev;
            double f_fixed, f_prev;

            // Вычисление значения функции в концах отрезка
            double f_a = f(a); funcEvaluations++;
            double f_b = f(b); funcEvaluations++;

            // Тут я определила, какой конец будем фиксировать
            if (Math.Abs(f_a) > Math.Abs(f_b))
            {
                x_fixed = a;
                f_fixed = f_a;
                x_prev = b;
                f_prev = f_b;
            }
            else
            {
                x_fixed = b;
                f_fixed = f_b;
                x_prev = a;
                f_prev = f_a;
            }

            iterations = 0;
            convergenceParameter = double.NaN;
            error = double.MaxValue;
            double prevError = double.MaxValue;

            double x_curr = x_prev;
            double f_curr = f_prev;

            do
            {

                x_curr = x_prev - f_prev * (x_fixed - x_prev) / (f_fixed - f_prev);
                funcEvaluations++;
                f_curr = f(x_curr);

                prevError = error;
                error = Math.Abs(x_curr - x_prev);

                if (iterations > 0 && prevError != 0)
                {
                    convergenceParameter = error / prevError; // Порядок сходимости p = 1 для метода хорд
                }

                x_prev = x_curr;
                f_prev = f_curr;

                iterations++;
            } while (Math.Abs(f_curr) > epsilon2 && error > epsilon1);

            return x_curr;
        }
    }
}
