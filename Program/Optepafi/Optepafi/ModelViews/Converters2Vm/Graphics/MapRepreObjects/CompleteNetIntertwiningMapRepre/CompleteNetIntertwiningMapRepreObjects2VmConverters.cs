using System;
using System.Collections.Generic;
using System.Linq;

namespace Optepafi.ModelViews.Converters2Vm.Graphics.MapRepreObjects.CompleteNetIntertwiningMapRepre;

public static class CompleteNetIntertwiningMapRepreObjects2VmConverters
{
    public static Dictionary<Type, IGraphicObjects2VmConverter> Converters =
        new Dictionary<Type, IGraphicObjects2VmConverter>()
            .Concat(CompleteNetIntertwElDatIndepOrient_ISOM_2017_2OmapImplObjects2VmConverters.Converters).ToDictionary();
}