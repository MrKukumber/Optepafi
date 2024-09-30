using System.Collections.Generic;
using System.Drawing;

namespace Optepafi.Models.Utils;
/// <summary>
/// Abstract predecessor for every configuration item type. Represents item which configures some application process.
///
/// Configuration items are collected in some <c>IConfiguration</c> successor type object. Each process can then define its own collection of configuration items.
/// </summary>
/// <param name="Name">Name of configuration item.</param>
public abstract record ConfigItem(string Name);

/// <summary>
/// Represents configuration item with categorical values. It is used for setting of categorical values.
///
/// It contains list of usable values and currently selected value.
/// Should be implemented using enum.
/// For more information about configuration items see <see cref="ConfigItem"/>.
/// </summary>
/// <param name="Name">Name of configuration item.</param>
/// <param name="AllValues">Usable values for this configuration item.</param>
/// <param name="CurrentValue">Currently selected value.</param>
/// <typeparam name="TValue">Type of values in this item.</typeparam>
public record CategoricalConfigItem<TValue>(string Name, TValue[] AllValues, TValue CurrentValue) : ConfigItem(Name);

/// <summary>
/// Represents single value configuration item.
///
/// For more information about configuration items see <see cref="ConfigItem"/>.
/// </summary>
/// <param name="Name">Name of configuration item.</param>
/// <param name="Value">Current value of this config. item.</param>
/// <param name="Unit">Unit of values in this config. item.</param>
/// <typeparam name="TValue">Type of value in this config. item.</typeparam>
public record ValueConfigItem<TValue>(string Name, TValue Value, string? Unit) : ConfigItem(Name);

/// <summary>
/// Represents configuration item, which holds value color value.
/// 
/// For more information about configuration items see <see cref="ConfigItem"/>.
/// </summary>
/// <param name="Name">Name of configuration item.</param>
/// <param name="Value">Current value of this config. item.</param>
/// <param name="Unit">Unit of values in this config. item. In case of colors no unit is needed.</param>
public record ColorConfigItem(string Name, Color Value, string Unit) : ValueConfigItem<Color>(Name, Value, Unit);

/// <summary>
/// Represets single value configuration item, whose value is bounded.
/// 
/// For more information about configuration items see <see cref="ConfigItem"/>.
/// </summary>
/// <param name="Name">Name of configuration item.</param>
/// <param name="Value">Current value of this item.</param>
/// <param name="Unit">Unit of values in this item. In case of colors no unit is needed.</param>
/// <param name="Min">Minimal value, that can be assigned to v this item.</param>
/// <param name="Max">Maximal value, that can be assigned to v this item.</param>
/// <typeparam name="TValue">Type of value in this config. item.</typeparam>
public record BoundedValueConfigItem<TValue>(string Name, TValue Value, string Unit, TValue Min, TValue Max) : ValueConfigItem<TValue>(Name, Value, Unit);






