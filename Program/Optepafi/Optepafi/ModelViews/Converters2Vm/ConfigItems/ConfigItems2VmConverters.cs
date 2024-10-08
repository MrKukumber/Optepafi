using System;
using System.Collections.Generic;
using Optepafi.Models.Utils;
using Optepafi.ViewModels.Data.Configuration;

namespace Optepafi.ModelViews.Converters2Vm.ConfigItems;

public static class ConfigItems2VmConverters
{
    public static Dictionary<Type, IConfigItem2VmConverter> Converters = new()
    {
        [typeof(ICategoricalConfigItem<Enum>)] =  CategoricalConfigItem2VmConverter.Instance,
        [typeof(IntValueConfigItem)] =  IntValueConfigItem2VmConverter.Instance,
        [typeof(FloatValueConfigItem)] =  FloatValueConfigItem2VmConverter.Instance,
        [typeof(ColorConfigItem)] =  ColorConfigItem2VmConverter.Instance,
        [typeof(BoundedIntValueConfigItem)] =  BoundedIntValueConfigItem2VmConverter.Instance,
        [typeof(BoundedFloatValueConfigItem)] =  BoundedFloatValueConfigItem2VmConverter.Instance
    };
}

public class CategoricalConfigItem2VmConverter : IConfigItem2VmConverter<ICategoricalConfigItem<Enum>>
{

    public static CategoricalConfigItem2VmConverter Instance { get; } = new();
    private CategoricalConfigItem2VmConverter(){}

    public ConfigItemViewModel ConvertToViewModel(ICategoricalConfigItem<Enum> configItem)
    {
        return new CategoricalConfigItemViewModel(configItem);
    }
}

public class IntValueConfigItem2VmConverter : IConfigItem2VmConverter<IntValueConfigItem>
{
    public static IntValueConfigItem2VmConverter Instance { get; } = new();
    private IntValueConfigItem2VmConverter(){}

    public ConfigItemViewModel ConvertToViewModel(IntValueConfigItem configItem)
    {
        return new IntValueConfigItemViewModel(configItem);
    }
}
public class FloatValueConfigItem2VmConverter : IConfigItem2VmConverter<FloatValueConfigItem>
{
    public static FloatValueConfigItem2VmConverter Instance { get; } = new();
    private FloatValueConfigItem2VmConverter(){}

    public ConfigItemViewModel ConvertToViewModel(FloatValueConfigItem configItem)
    {
        return new FloatValueConfigItemViewModel(configItem);
    }
}



public class ColorConfigItem2VmConverter : IConfigItem2VmConverter<ColorConfigItem>
{
    public static ColorConfigItem2VmConverter Instance { get; } = new();
    private ColorConfigItem2VmConverter(){}

    public ConfigItemViewModel ConvertToViewModel(ColorConfigItem configItem)
    {
        return new ColorConfigItemViewModel(configItem);
    }
}


public class BoundedIntValueConfigItem2VmConverter : IConfigItem2VmConverter<BoundedIntValueConfigItem>
{
    public static BoundedIntValueConfigItem2VmConverter Instance { get; } = new();
    private BoundedIntValueConfigItem2VmConverter(){}

    public ConfigItemViewModel ConvertToViewModel(BoundedIntValueConfigItem configItem)
    {
        return new BoundedIntValueConfigItemViewModel(configItem);
    }
}
public class BoundedFloatValueConfigItem2VmConverter : IConfigItem2VmConverter<BoundedFloatValueConfigItem>
{
    public static BoundedFloatValueConfigItem2VmConverter Instance { get; } = new();
    private BoundedFloatValueConfigItem2VmConverter(){}

    public ConfigItemViewModel ConvertToViewModel(BoundedFloatValueConfigItem configItem)
    {
        return new BoundedFloatValueConfigItemViewModel(configItem);
    }
}