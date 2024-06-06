using System.Collections.Generic;

namespace Optepafi.Utils;

public interface ICollector
{
    public void Add<TObject>(TObject element);
    public void AddRange<TObject>(IEnumerable<TObject> elements);
}