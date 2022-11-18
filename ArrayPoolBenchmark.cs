using System.Buffers;
using BenchmarkDotNet.Attributes;

[MemoryDiagnoser]
public class ArrayPoolBenchmark
{
    private int _arraySize = 1_000_000;

    [Benchmark]
    public void StandardArray()
    {
        var array = new int[_arraySize];
        for (int i = 0; i < _arraySize; i++)
            array[i] = i;
        
    }

    [Benchmark]
    public void SharedPoolWithoutClearingArray()
    {
        var sharedPool = ArrayPool<int>.Shared;
        var array = sharedPool.Rent(_arraySize);
        for (int i = 0; i < _arraySize; i++)
            array[i] = i;
        sharedPool.Return(array, false);
    }

    [Benchmark]
    public void SharedPoolWithClearingArray()
    {
        var sharedPool = ArrayPool<int>.Shared;
        var array = sharedPool.Rent(_arraySize);
        for (int i = 0; i < _arraySize; i++)
            array[i] = i;
        sharedPool.Return(array, true);
    }

    [Benchmark]
    public void SpecificPoolWithoutCaching()
    {
        var pool = ArrayPool<int>.Create();
        var array = pool.Rent(_arraySize);
        for (int i = 0; i < _arraySize; i++)
            array[i] = i;
        pool.Return(array, false);
    }

    private ArrayPool<int> _cachedPool = ArrayPool<int>.Create();
    [Benchmark]
    public void SpecificPoolWithCaching()
    {
        var array = _cachedPool.Rent(_arraySize);
        for (int i = 0; i < _arraySize; i++)
            array[i] = i;
        _cachedPool.Return(array, false);
    }

    [Benchmark]
    public void List()
    {
        var list = new List<int>();
        for (int i = 0; i < _arraySize; i++)
            list.Add(i);
    }
}