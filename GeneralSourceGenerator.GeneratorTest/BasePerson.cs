using System.Collections.Concurrent;

namespace GeneralSourceGenerator.GeneratorTest;

[Asyncified]
public partial class BasePerson<T> where T:class
{
    private int _age;

    public int Age
    {
        get => _age;
        set => _age = value;
    }

    public static ConcurrentBag<T> Get() => default;
    
    public Dictionary<T, List<R>> Get<R>(int a) => default;
    
    public Dictionary<T, List<R>> Get<R>(Dictionary<T, List<R>> n) => default;
    
    public Dictionary<T, List<(TA, TB)>> Get<TA, TB>(Dictionary<T, List<TB>> n) => default;
}