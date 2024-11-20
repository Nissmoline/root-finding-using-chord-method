# Root Finding Using Chord Method

This repository contains a program for solving equations with one variable using the **Chord Method**. The program identifies roots of a given function on a specified interval and outputs relevant metrics, including precision, function values at the roots, the number of iterations, and computation time.

## Problem Statement

Solving equations with one variable involves finding the value of the variable that satisfies the equation f(x) = 0. This program implements the **Chord Method** for solving such equations and provides the following functionalities:

### Input:
- A function f(x) (along with its derivatives if required for advanced methods).
- Interval [a, b].
- Tolerances $\varepsilon_1$ (precision for arguments) and $\varepsilon_1$ (precision for function values).

### Output:
- Roots \( \xi_i \).
- Function values at roots \( f(\xi_i) \).
- Number of iterations \( n \).
- Number of function evaluations.
- Computation time.
- Convergence parameter: \( \alpha = \frac{|x_{n+1} - x_n|}{|x_n - x_{n-1}|^n} \), where \( n \) is the order of convergence.

---

## Implementation Details

### Algorithm Description: Chord Method
The Chord Method iteratively approximates the root of \( f(x) = 0 \) using the formula:

\[
x_{n+1} = x_n - \frac{f(x_n)}{f(b) - f(x_n)} (b - x_n), \text{ if } x_n = a
\]
\[
x_{n+1} = x_n - \frac{f(x_n)}{f(x_n) - f(a)} (x_n - a), \text{ if } x_n = b
\]

**Steps:**
1. **Initial Points:** Select \( a \) and \( b \) such that \( f(a) \cdot f(b) < 0 \).
2. **Iterative Process:** Apply the chord method formula until the stopping criteria are met:
   - \( |x_{n+1} - x_n| < \epsilon_1 \), or
   - \( |f(x_n)| < \epsilon_2 \).
3. **Output Results:** Report the root, precision metrics, and performance statistics.

---

## Example

Given the function:

\[
f(x) = 2 \cdot \log_{10}(x + 7) - 5 \cdot \sin(x)
\]

For the interval \([-5, 1.5]\) with tolerances \( \epsilon_1 = 10^{-6} \) and \( \epsilon_2 = 10^{-6} \), the program iteratively computes the root and displays the following:

- Root \( x \).
- \( f(x) \) at the root.
- Number of iterations.
- Function evaluations.
- Convergence parameter.
- Computation time.

---

## Code Structure

### Main Function
Handles user input and initiates the root-finding process:
```csharp
static void Main()
{
    Console.WriteLine("Enter the interval [a,b]:");
    string[] inputs = Console.ReadLine().Split(' ');
    double a = double.Parse(inputs[0]);
    double b = double.Parse(inputs[1]);

    Console.WriteLine("Enter precision for arguments (eps1):");
    double epsilon1 = double.Parse(Console.ReadLine());

    Console.WriteLine("Enter precision for function values (eps2):");
    double epsilon2 = double.Parse(Console.ReadLine());

    Stopwatch stopwatch = Stopwatch.StartNew();
    FindRoots(a, b, epsilon1, epsilon2);
    stopwatch.Stop();

    Console.WriteLine($"Computation Time: {stopwatch.ElapsedMilliseconds} ms");
}
```

### Function Implementation
**Function \( f(x) \):**
```csharp
static double f(double x)
{
    return 2 * Math.Log10(x + 7) - 5 * Math.Sin(x);
}
```

**Chord Method Logic:**
```csharp
static double ChordMethod(double a, double b, double epsilon1, double epsilon2, out int iterations, ref int funcEvaluations, out double convergenceParameter)
{
    double fa = f(a), fb = f(b);
    funcEvaluations += 2;

    double x_prev = a, x_curr = a - (fa * (b - a)) / (fb - fa);
    double f_curr = f(x_curr);
    funcEvaluations++;
    iterations = 0;
    convergenceParameter = double.NaN;

    while (Math.Abs(f_curr) > epsilon2 && Math.Abs(x_curr - x_prev) > epsilon1)
    {
        iterations++;
        double x_next = x_curr - f_curr * (x_curr - x_prev) / (f_curr - f(x_prev));
        funcEvaluations++;
        convergenceParameter = Math.Abs((x_next - x_curr) / Math.Pow(Math.Abs(x_curr - x_prev), 1));
        x_prev = x_curr;
        x_curr = x_next;
        f_curr = f(x_curr);
    }
    return x_curr;
}
```

**Root Search:**
```csharp
static void FindRoots(double a, double b, double epsilon1, double epsilon2)
{
    double step = 0.5;
    for (double x = a; x < b; x += step)
    {
        if (f(x) * f(x + step) < 0)
        {
            int iterations = 0;
            int funcEvaluations = 0;
            double root = ChordMethod(x, x + step, epsilon1, epsilon2, out iterations, ref funcEvaluations, out double convergenceParameter);
            Console.WriteLine($"Root: {root:F6}, f(x): {f(root):E6}, Iterations: {iterations}, Func. Evaluations: {funcEvaluations}, Conv. Param: {convergenceParameter:E6}");
        }
    }
}
```

---

## Requirements
- **Language:** C#
- **Environment:** .NET Framework or .NET Core
- **Libraries:** `System`, `System.Diagnostics`

---

## Usage
Clone the repository and execute the program:
```bash
git clone https://github.com/Nissmoline/root-finding-using-chord-method.git
cd root-finding-using-chord-method
dotnet run
```

---

## License
This project is open-source and available under the MIT License.
