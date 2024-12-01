using System.Collections.Immutable;
using System.Linq;
using Optepafi.Models.Utils.Configurations;
using Optepafi.Views.Utils;

namespace Optepafi.Models.MapRepreMan.Configurations;

//TODO: comment + add configs if necessary
public class CompleteSnappingMapRepreConfiguration(int defaultAverageEdgeLength, float defaultEdgeVariance, int indexOfDefaultNetType ) : IConfiguration
{

    public readonly BoundedIntValueConfigItem averageEdgeLength = new BoundedIntValueConfigItem("Average edge length", defaultAverageEdgeLength, "micrometers", (int)(1 / defaultEdgeVariance) + 1, 50_000);
    public readonly BoundedFloatValueConfigItem edgeVariance = new BoundedFloatValueConfigItem("Edge variance", defaultEdgeVariance, "between 0 and 1", 0, 1);
    public readonly CategoricalConfigItem<NetTypesEnumeration> typeOfNet = new CategoricalConfigItem<NetTypesEnumeration>("Type of net", NetTypesEnumeration.GetValuesAsUnderlyingType<NetTypesEnumeration>().OfType<NetTypesEnumeration>() .ToArray(), indexOfDefaultNetType);
    public ImmutableList<IConfigItem> ConfigItems => [averageEdgeLength, edgeVariance, typeOfNet];
    public enum NetTypesEnumeration { Triangular }
    
    public IConfiguration DeepCopy()
    {
        return new CompleteSnappingMapRepreConfiguration(averageEdgeLength.Value, edgeVariance.Value, typeOfNet.IndexOfSelectedValue);
    }
}