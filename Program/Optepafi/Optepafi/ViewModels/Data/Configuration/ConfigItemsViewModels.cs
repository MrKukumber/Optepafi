using System;
using Optepafi.Models.Utils.Configurations;
using ReactiveUI;
using Color = System.Drawing.Color;

namespace Optepafi.ViewModels.Data.Configuration;

    //TODO: comment
public class CategoricalConfigItemViewModel : ConfigItemViewModel
{
    public CategoricalConfigItem ConfigItem { get; }
    // public ICategoricalConfigItem<Enum> ConfigItem { get; }
    public CategoricalConfigItemViewModel(CategoricalConfigItem configItem)
    {
        ConfigItem = configItem;
        _indexOfSelectedValue = configItem.IndexOfSelectedValue;
    }
    // public CategoricalConfigItemViewModel(ICategoricalConfigItem<Enum> configItem)
    // {
        // ConfigItem = configItem;
        // _indexOfSelectedValue = configItem.IndexOfSelectedValue;
    // }

    public string Name => ConfigItem.Name;
    public Enum[] AllValues => ConfigItem.AllValues;

    private int _indexOfSelectedValue;
    public int IndexOfSelectedValue
    {
        get => _indexOfSelectedValue;
        set
        {
            ConfigItem.IndexOfSelectedValue = value;
            this.RaiseAndSetIfChanged(ref _indexOfSelectedValue, value);
        }
    }
}

public class IntValueConfigItemViewModel : ConfigItemViewModel
{
    private IntValueConfigItem _configItem;
    public IntValueConfigItem ConfigItem => _configItem;
    public IntValueConfigItemViewModel(IntValueConfigItem configItem)
    {
        _configItem = configItem;
        _value = ConfigItem.Value;
    }

    public string Name => ConfigItem.Name;
    private int _value;
    public int Value
    {
        get => _value;
        set
        {
            _configItem.Value = value;
            this.RaiseAndSetIfChanged(ref _value, value);
        }
    }
    public string? Unit => ConfigItem.Unit;

}
public class FloatValueConfigItemViewModel : ConfigItemViewModel
{
    
    private FloatValueConfigItem _configItem;
    public FloatValueConfigItem ConfigItem => _configItem;
    public FloatValueConfigItemViewModel(FloatValueConfigItem configItem)
    {
        _configItem = configItem;
        _value = ConfigItem.Value;
    }

    public string Name => ConfigItem.Name;
    private float _value;
    public float Value
    {
        get => _value;
        set
        {
            _configItem.Value = value;
            this.RaiseAndSetIfChanged(ref _value, value);
        }

    }
    public string? Unit => ConfigItem.Unit;

}

public class ColorConfigItemViewModel : ConfigItemViewModel
{

    public ColorConfigItem ConfigItem { get; }
    public ColorConfigItemViewModel(ColorConfigItem configItem)
    {
        ConfigItem = configItem;
        _value = new Avalonia.Media.Color(ConfigItem.Value.A, ConfigItem.Value.R, ConfigItem.Value.G, ConfigItem.Value.B);
    }

    public string Name => ConfigItem.Name;
    private Avalonia.Media.Color _value;
    public Avalonia.Media.Color Value
    {
        get => _value;
        set
        {
            ConfigItem.Value = Color.FromArgb(value.A, value.R, value.G, value.B);
            this.RaiseAndSetIfChanged(ref _value, value);
        }

    }
}

public class BoundedIntValueConfigItemViewModel : ConfigItemViewModel
{
    public BoundedIntValueConfigItem ConfigItem { get; }
    public BoundedIntValueConfigItemViewModel(BoundedIntValueConfigItem configItem)
    {
        ConfigItem = configItem;
        _value = ConfigItem.Value;
    }

    public string Name => ConfigItem.Name;
    
    private int _value;
    public int Value
    {
        get => _value;
        set
        {
            ConfigItem.Value = value;
            this.RaiseAndSetIfChanged(ref _value, value);
        }
    }

    public string? Unit => ConfigItem.Unit;
    public int Min => ConfigItem.Min;
    public int Max => ConfigItem.Max;
}
public class BoundedFloatValueConfigItemViewModel : ConfigItemViewModel
{
    public BoundedFloatValueConfigItem ConfigItem { get; }
    public BoundedFloatValueConfigItemViewModel(BoundedFloatValueConfigItem configItem)
    {
        ConfigItem = configItem;
        _value = ConfigItem.Value;
    }

    public string Name => ConfigItem.Name;

    private float _value;
    public float Value
    {
        get => _value;
        set
        {
            ConfigItem.Value = value;
            this.RaiseAndSetIfChanged(ref _value, value);
        }
    }

    public string? Unit => ConfigItem.Unit;
    public float Min => ConfigItem.Min;
    public float Max => ConfigItem.Max;
}
