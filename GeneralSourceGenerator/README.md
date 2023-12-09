# General Source Generator

## Asyncifier
create async methods from sync ones.

### usage
1. Add [Asyncify] attribute to each sync method.
2. Make containng class partial.

```
public partial class Calculator
{
    [Asyncify]
    public static int Sum(int a, int b) => a + b;
    
    [Asyncify]
    public static int Sum(int a, int b, int c) => a + b + c; // can overload
    
    [Asyncify]
    public static int ToDouble(int a) => a * 2;

    [Asyncify]
    public static int ToTriple(int a = 3) => a * 3; // can have default parameter value
    
    [Asyncify]
    public static int SumAll(params int[] array) => array.Sum(); // params keyword will be gone

    [Asyncify]
    public List<int> GetList(int a, int b) => [a + b, a - b]; // can return List<T>
}
```



Check out README.md in GitHub repository for basic usage and examples.
https://github.com/cplkimth/GeneralSourceGenerator