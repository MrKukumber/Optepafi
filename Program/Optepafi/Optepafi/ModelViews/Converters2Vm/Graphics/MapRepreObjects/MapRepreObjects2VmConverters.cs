using System;
using System.Collections.Generic;
using System.Linq;

namespace Optepafi.ModelViews.Converters2Vm.Graphics.MapRepreObjects;

public static class MapRepreObjects2VmConverters
{
    public static Dictionary<Type, IGraphicObjects2VmConverter> Converters =
        new Dictionary<Type, IGraphicObjects2VmConverter>()
            .Concat(CompleteNetIntertwiningMapRepreObjects2VmConverters.Converters).ToDictionary();

}