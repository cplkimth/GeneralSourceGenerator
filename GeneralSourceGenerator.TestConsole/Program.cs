using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Data;

namespace GeneralSourceGenerator.TestConsole;

public partial class Program
{
    static async Task Main(string[] args)
    {
        // Console.WriteLine(await Func1Async(2));
        Program p = new();
        // Console.WriteLine(await p.Func3Async(4));

        // Console.WriteLine(Generator.Generate("static List<int> Func2(List<int> numbers) => [2, 4];"));
        // Console.WriteLine(Generator.Generate("internal int Func3(int number)"));

        // var numbers = await Func2Async(null);
        var numbers = await Func4Async(null);
        var sum = numbers.Sum();
        Console.WriteLine(sum);

        var r1 = await Func1Async(1, 2);
        Console.WriteLine(r1);

        Console.WriteLine(await ToDoubleAsync(2));
        Console.WriteLine(await ToTripleAsync(3));
    }

    [Asyncify]
    static int Func1(int number1) => number1 * 1;

    [Asyncify]
    private static int ToDouble(int number1) => number1 * 2;

    [Asyncify]
    private static int ToTriple(int number) => number * 3;

    [Asyncify]
    static int Func1(int number1, int number2) => number1 + number2;

    [Asyncify]
    static List<int> Func2(List<int> numbers) => [2, 4];

    [Asyncify]
    internal int Func3(int number) => number * 2;

    [Asyncify]
    public static List<int> Func4(List<int> numbers) => [2, 5];
}