namespace GeneralSourceGenerator.TestConsole.UnitTest;

public partial class Calculator2
{
    [Asyncify]
    public static int Sum(int a, int b) => a + b;
    
    [Asyncify]
    public static int Sum(int a, int b, int c) => a + b + c;
    
    [Asyncify]
    public static int ToDouble(int a) => a * 2;
    
    [Asyncify]
    public static int ToTriple(int a = 3) => a * 3;
    
    [Asyncify]
    public static int SumAll(params int[] array) => array.Sum();

    [Asyncify]
    public List<int> GetList(int a, int b) => [a + b, a - b];
}