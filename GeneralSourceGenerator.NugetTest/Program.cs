namespace GeneralSourceGenerator.NugetTest;

public partial class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine(await Func1Async(2));
        Program p = new();
        Console.WriteLine(await p.Func3Async(4));

    }

    [Asyncify]
    static int Func1(int number) => number * 1;
    
    [Asyncify]
    int Func3(int number) => number * 2;

    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    static async Task WriteAsync(ReadOnlyMemory<byte> buffer, Stream stream, 
        CancellationToken ct)
        => await stream.WriteAsync(buffer, ct).ConfigureAwait(true);

    [Zomp.SyncMethodGenerator.CreateSyncVersionAttribute]
    static void Write2(global::System.ReadOnlySpan<byte> buffer, global::System.IO.Stream stream)
        => stream.Write(buffer);
}