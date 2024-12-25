using System;
using System.Collections.Immutable;
using System.Linq;
using Optepafi.Models.Utils.Configurations;
using Optepafi.Views.Utils;

namespace Optepafi.Models.MapRepreMan.Configurations;

//TODO: comment + add configs if necessary
public class CompleteNetIntertwiningMapRepreConfiguration(int defaultStandardEdgeLength, float defaultMinBoundaryEdgeRatio, int indexOfDefaultNetType) : IConfiguration
{

    public readonly BoundedIntValueConfigItem standardEdgeLength = new BoundedIntValueConfigItem("Standard edge length", defaultStandardEdgeLength, "micrometers", 500, 20_000);
    public readonly BoundedFloatValueConfigItem minBoundaryEdgeRatio = new BoundedFloatValueConfigItem("Ratio of minimal boundary edge length to standard one", defaultMinBoundaryEdgeRatio, "between 0 and 1", 0.1f,  0.9f);
    public readonly CategoricalConfigItem typeOfNet = new CategoricalConfigItem("Type of net", [NetTypesEnumeration.Triangular], indexOfDefaultNetType);
    // public readonly CategoricalConfigItem<NetTypesEnumeration> typeOfNet = new CategoricalConfigItem<NetTypesEnumeration>("Type of net", NetTypesEnumeration.GetValuesAsUnderlyingType<NetTypesEnumeration>().OfType<NetTypesEnumeration>() .ToArray(), indexOfDefaultNetType);
    public ImmutableList<IConfigItem> ConfigItems => [standardEdgeLength, minBoundaryEdgeRatio, typeOfNet];
    public enum NetTypesEnumeration { Triangular = 0 }
    
    public IConfiguration DeepCopy()
    {
        return new CompleteNetIntertwiningMapRepreConfiguration(standardEdgeLength.Value, minBoundaryEdgeRatio.Value, typeOfNet.IndexOfSelectedValue);
    }
}