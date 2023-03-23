using System.Buffers;
using BenchmarkDotNet.Attributes;

[MemoryDiagnoser]
public class ArrayPoolBenchmark
{
    private int _collectionSize = 1_000_000;

    [Benchmark]
    public void StandardArray()
    {
        var array = new int[_collectionSize];
        for (int i = 0; i < _collectionSize; i++)
            array[i] = i;
        
    }

    [Benchmark]
    public void SharedPoolWithoutClearingArray()
    {
        var sharedPool = ArrayPool<int>.Shared;
        var array = sharedPool.Rent(_collectionSize);
        for (int i = 0; i < _collectionSize; i++)
            array[i] = i;
        sharedPool.Return(array, false);
    }

    [Benchmark]
    public void SharedPoolWithClearingArray()
    {
        var sharedPool = ArrayPool<int>.Shared;
        var array = sharedPool.Rent(_collectionSize);
        for (int i = 0; i < _collectionSize; i++)
            array[i] = i;
        sharedPool.Return(array, true);
    }

    [Benchmark]
    public void CustomPoolWithoutCaching()
    {
        var pool = ArrayPool<int>.Create();
        var array = pool.Rent(_collectionSize);
        for (int i = 0; i < _collectionSize; i++)
            array[i] = i;
        pool.Return(array, false);
    }

    private ArrayPool<int> _cachedPool = ArrayPool<int>.Create();
    [Benchmark]
    public void CustomPoolWithCaching()
    {
        var array = _cachedPool.Rent(_collectionSize);
        for (int i = 0; i < _collectionSize; i++)
            array[i] = i;
        _cachedPool.Return(array, false);
    }

    [Benchmark]
    public void List()
    {
        var list = new List<int>();
        for (int i = 0; i < _collectionSize; i++)
            list.Add(i);
    }

    [Benchmark]
    public void ListWithSize()
    {
        var list = new List<int>(_collectionSize);
        for (int i = 0; i < _collectionSize; i++)
            list.Add(i);
    }
}