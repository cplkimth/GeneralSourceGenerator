using System.Collections.Concurrent;

namespace GeneralSourceGenerator.GeneratorTest;

public partial class BasePerson<T> where T:class
{
    public static Task<ConcurrentBag<T>> Create1Async() 
        => Task.Run(() => Get());

    public Task<Dictionary<T, List<R>>> Create1Async<R>(int a) 
        => Task.Run(() => Get<R>(a));

    public Task<Dictionary<T, List<R>>> Create1Async<R>(Dictionary<T, List<R>> n) 
        => Task.Run(() => Get(n));

    public Task<Dictionary<T, List<(TA, TB)>>> Create1Async<TA, TB>(Dictionary<T, List<TB>> n)
        => Task.Run(() => Get<TA, TB>(n));
}