using Optepafi.Models.MapRepreMan.VertecesAndEdges;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.Graphics.GraphicsAggregators;

/// <summary>
/// Predecessor for all graphics aggregators. Thanks to its non-generic nature it is suited for being used as type parameter in collections of aggregators.
/// </summary>
public interface IGraphicsAggregator { }