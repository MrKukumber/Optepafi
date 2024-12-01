using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using QuadTrees;
using QuadTrees.QTreePoint;
using KdTree;

namespace Optepafi.Models.MapRepreMan.Utils;

public class RadiallySearchableKdTree<TValue> : 
    IEditableRadiallySearchableDataStruct<TValue>,
    INearestNeighborsSearchableDataStructure<TValue>
{
    public delegate (int, int) ToCoordsDelegate(TValue value);
    
    private KdTree<int, TValue> _kdTree;
    private ToCoordsDelegate _toCoordsDelegate;
    private int _modificationsSinceLastBalance = 0;

    public RadiallySearchableKdTree(ToCoordsDelegate toCoordsDelegate)
    {
        _kdTree = new KdTree<int, TValue>(2, new IntMath(), AddDuplicateBehavior.Update);
        _toCoordsDelegate = toCoordsDelegate;
    }
    
    public RadiallySearchableKdTree(IEnumerable<TValue> values, ToCoordsDelegate toCoordsDelegate)
    {
        _kdTree = new KdTree<int, TValue>(2, new IntMath(), AddDuplicateBehavior.Update);
        _toCoordsDelegate = toCoordsDelegate;
        foreach (var value in values)
        {
            (int, int) coords = toCoordsDelegate(value);
            _kdTree.Add([coords.Item1, coords.Item2], value);
        }
        _kdTree.Balance();
    }
    
    public IReadOnlyList<TValue> FindInEuclideanDistanceFrom((int, int) coords, int distance) => _kdTree
        .RadialSearch([coords.Item1, coords.Item2], distance)
        .Select(x => x.Value)
        .ToList();

    public TValue[] GetNearestNeighbors((int, int) coords, int count) => _kdTree.GetNearestNeighbours([coords.Item1, coords.Item2], count).Select(x => x.Value).ToArray();
    
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TValue> GetEnumerator() => new Enumerator(_kdTree.GetEnumerator()) ;


    public void Insert(TValue value)
    {
        (int, int) coords = _toCoordsDelegate(value);
        _kdTree.Add([coords.Item1, coords.Item2], value);
        if (++_modificationsSinceLastBalance > _kdTree.Count / 2)
        {
            _kdTree.Balance();
            _modificationsSinceLastBalance = 0;
        }
    }

    public void Delete(TValue value)
    {
        (int, int) coords = _toCoordsDelegate(value);
        _kdTree.RemoveAt([coords.Item1, coords.Item2]);
        if (++_modificationsSinceLastBalance > _kdTree.Count / 2)
        {
            _kdTree.Balance();
            _modificationsSinceLastBalance = 0;
        }
    }

    private class Enumerator(IEnumerator<KdTreeNode<int, TValue>> kdTreeEnuemrator) : IEnumerator<TValue>
    {
        public bool MoveNext() => kdTreeEnuemrator.MoveNext();    

        public void Reset() => kdTreeEnuemrator.Reset();

        object? IEnumerator.Current => Current;

        public void Dispose() => kdTreeEnuemrator.Dispose();
        public TValue Current => kdTreeEnuemrator.Current.Value;
    }

    private class IntMath : ITypeMath<int>
    {
        public int Compare(int a, int b) => a.CompareTo(b);

        public int Min(int a, int b) => int.Min(a, b);

        public int Max(int a, int b) => int.Max(a, b);

        public bool AreEqual(int a, int b) => a == b;

        public bool AreEqual(int[] a, int[] b) => a == b;

        public int Add(int a, int b) => a + b;

        public int Subtract(int a, int b) => a - b;

        public int Multiply(int a, int b) => a * b;

        public int DistanceSquaredBetweenPoints(int[] a, int[] b) => a.Zip(b, (x, y) => (x - y)^2).Sum();

        public int MinValue { get; } = int.MinValue;
        public int MaxValue { get; } = int.MaxValue;
        public int Zero { get; } = 0;
        public int NegativeInfinity { get; } = int.MinValue;
        public int PositiveInfinity { get; } = int.MaxValue;
    }
}