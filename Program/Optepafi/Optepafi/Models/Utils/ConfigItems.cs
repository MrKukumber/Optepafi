using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;

namespace Optepafi.Models.Utils;

/// <summary>
/// Interface to be implemented by every configuration item type. Represents item which configures some application process.
///
/// Configuration items are collected in some <c>IConfiguration</c> successor type object. Each process can then define its own collection of configuration items.
/// </summary>
public interface IConfigItem
{
    public TOut GenericVisit<TOut>(IConfigItemGenericVisitor<TOut> genericVisitor);

    public IConfigItem DeepCopy();
}

public interface ICategoricalConfigItem<out TEnum> : IConfigItem where TEnum : Enum
{
    string Name { get; }
    TEnum[] AllValues { get; }
    int IndexOfSelectedValue { get; set; } 
}

/// <summary>
/// Represents configuration item with categorical values. It is used for setting of categorical values.
///
/// It contains list of usable values and currently selected value.
/// Should be implemented using enum.
/// For more information about configuration items see <see cref="IConfigItem"/>.
/// </summary>
/// <param name="name">Name of configuration item.</param>
/// <param name="allValues">Usable values for this configuration item.</param>
/// <param name="indexOfSelectedValue">Index in allValues collection of currently selected value.</param>
public class CategoricalConfigItem<TEnum>(string name, TEnum[] allValues, int indexOfSelectedValue) : ICategoricalConfigItem<TEnum> where TEnum : Enum
{
    public string Name => name;
    public TEnum[] AllValues => allValues;
    
    private int _indexOfSelectedValue = indexOfSelectedValue;
    public int IndexOfSelectedValue
    {
        get => _indexOfSelectedValue;
        set
        {
            if (value >= AllValues.Length)
                _indexOfSelectedValue = AllValues.Length - 1;
            else if (value < 0)
                _indexOfSelectedValue = 0;
            else _indexOfSelectedValue = value;
        }
    }

    public IConfigItem DeepCopy()
    {
        return new CategoricalConfigItem<TEnum>(Name, AllValues, IndexOfSelectedValue);
    }
    public TOut GenericVisit<TOut>(IConfigItemGenericVisitor<TOut> genericVisitor)
    {
        return genericVisitor.AcceptGeneric(this);
    }
}

/// <summary>
/// Represents single int value configuration item.
///
/// For more information about configuration items see <see cref="IConfigItem"/>.
/// </summary>
/// <param name="name">Name of configuration item.</param>
/// <param name="value">Current value of this config. item.</param>
/// <param name="unit">Unit of values in this config. item.</param>
public class IntValueConfigItem(string name, int value, string? unit) : IConfigItem
{
    public string Name => name;
    public int Value { get; set; } = value;
    public string? Unit => unit;
    
    public IConfigItem DeepCopy()
    {
        return new IntValueConfigItem(Name, Value, Unit);
    }
    public TOut GenericVisit<TOut>(IConfigItemGenericVisitor<TOut> genericVisitor)
    {
        return genericVisitor.AcceptGeneric(this);
    }
}
/// <summary>
/// Represents single float value configuration item.
///
/// For more information about configuration items see <see cref="IConfigItem"/>.
/// </summary>
/// <param name="name">Name of configuration item.</param>
/// <param name="value">Current value of this config. item.</param>
/// <param name="unit">Unit of values in this config. item.</param>
public class FloatValueConfigItem(string name, float value, string? unit) : IConfigItem
{
     public string Name => name;
    public float Value { get; set; } = value;
    public string? Unit => unit;
        
    public IConfigItem DeepCopy()
    {
        return new FloatValueConfigItem(Name, Value, Unit);
    }
    public TOut GenericVisit<TOut>(IConfigItemGenericVisitor<TOut> genericVisitor)
    {
        return genericVisitor.AcceptGeneric(this);
    }
}

/// <summary>
/// Represents configuration item, which holds value color value.
/// 
/// For more information about configuration items see <see cref="IConfigItem"/>.
/// </summary>
/// <param name="name">Name of configuration item.</param>
/// <param name="value">Current value of this config. item.</param>
public class ColorConfigItem(string name, Color value) : IConfigItem
{
    public string Name => name;
    public Color Value { get; set; } = value;
    
    public IConfigItem DeepCopy()
    {
        return new ColorConfigItem(Name, Value);
    }
    public TOut GenericVisit<TOut>(IConfigItemGenericVisitor<TOut> genericVisitor)
    {
        return genericVisitor.AcceptGeneric(this);
    }
}

/// <summary>
/// Represets single integer value configuration item, whose value is bounded.
/// 
/// For more information about configuration items see <see cref="IConfigItem"/>.
/// </summary>
/// <param name="name">Name of configuration item.</param>
/// <param name="value">Current value of this item.</param>
/// <param name="unit">Unit of values in this item. In case of colors no unit is needed.</param>
/// <param name="min">Minimal value, that can be assigned to v this item.</param>
/// <param name="max">Maximal value, that can be assigned to v this item.</param>
public class BoundedIntValueConfigItem(string name, int value, string? unit, int min, int max)
    : IConfigItem
{
    public string Name => name;
    public int Value { get; set; } = value;
    public string? Unit => unit;
    public int Min => min;
    public int Max => max;
    
    public IConfigItem DeepCopy()
    {
        return new BoundedIntValueConfigItem(Name, Value, Unit, Min, Max);
    }
    public TOut GenericVisit<TOut>(IConfigItemGenericVisitor<TOut> genericVisitor)
    {
        return genericVisitor.AcceptGeneric(this);
    }
}

/// <summary>
/// Represets single float value configuration item, whose value is bounded.
/// 
/// For more information about configuration items see <see cref="IConfigItem"/>.
/// </summary>
/// <param name="name">Name of configuration item.</param>
/// <param name="value">Current value of this item.</param>
/// <param name="unit">Unit of values in this item. In case of colors no unit is needed.</param>
/// <param name="min">Minimal value, that can be assigned to v this item.</param>
/// <param name="max">Maximal value, that can be assigned to v this item.</param>
public class BoundedFloatValueConfigItem(string name, float value, string? unit, float min, float max)
    : IConfigItem
{
    public string Name => name;
    public float Value { get; set; } = value;
    public string? Unit => unit;
    public float Min => min;
    public float Max => max;
    
    public IConfigItem DeepCopy()
    {
        return new BoundedFloatValueConfigItem(Name, Value, Unit, Min, Max);
    }
    public TOut GenericVisit<TOut>(IConfigItemGenericVisitor<TOut> genericVisitor)
    {
        return genericVisitor.AcceptGeneric(this);
    }
    
}











