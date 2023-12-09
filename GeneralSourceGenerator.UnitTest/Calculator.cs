namespace GeneralSourceGenerator.TestConsole.UnitTest;

public partial class Calculator
{
    [Asyncify]
    public static int Sum(int a, int b) => a + b;
    
    [Asyncify]
    public static int Sum(int a, int b, int c) => a + b + c;
    
    [Asyncify]
    public static int ToDouble(int a) => a * 2;
    
    [Asyncify]
    public static int SumAll(params int[] array) => array.Sum();

    [Asyncify]
    public List<int> GetList(int a, int b) => [a + b, a - b];
}