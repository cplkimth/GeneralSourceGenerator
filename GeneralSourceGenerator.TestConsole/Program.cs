using System.ComponentModel;

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

    }

    [Asyncify]
    static int Func1(int number) => number * 1;
    //
    [Asyncify]
    internal int Func3(int number) => number * 2;

    [Asyncify]
    static List<int> Func2(List<int> numbers) => [2, 4];

    [Asyncify]
    static List<int> Func4(List<int> numbers) => [2, 5];
}