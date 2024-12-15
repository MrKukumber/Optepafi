using System;
using System.Collections;
using System.Collections.Generic;

namespace Optepafi.Models.MapRepreMan.Utils;

public interface IEditableRadiallySearchableDataStruct<TValue> : IRadiallySearchableDataStruct<TValue>
{
    void Insert(TValue value);
    
    void Delete(TValue value);
    
    bool TryFindAt(TValue value, out TValue foundValue);
}

public class BlankEditableRadiallySearchableDataStruct<TValue> : IEditableRadiallySearchableDataStruct<TValue>
{
    public delegate (int, int) ToCoordsDelegate(TValue value);
    public BlankEditableRadiallySearchableDataStruct(ToCoordsDelegate toCoordsDelegate) { }
    public BlankEditableRadiallySearchableDataStruct(IEnumerable<TValue> values, ToCoordsDelegate toCoordsDelegate) { }
    
    public void Insert(TValue value) { }

    public void Delete(TValue value) { }

    public IReadOnlyList<TValue> FindInEuclideanDistanceFrom((int, int) coords, int distance) => new List<TValue>();

    public bool TryFindAt(TValue value, out TValue foundItem)
    {
        foundItem = value;
        return false;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public IEnumerator<TValue> GetEnumerator() => new Enumerator();

    private class Enumerator : IEnumerator<TValue>
    {
        public bool MoveNext() => false;

        public void Reset() { }

        object? IEnumerator.Current => Current;

        public void Dispose() { }

        public TValue Current => throw new InvalidOperationException();
    }
}