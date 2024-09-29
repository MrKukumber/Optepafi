using System.Collections.Generic;
using System.Drawing;

namespace Optepafi.Models.Utils;

public abstract record ConfigItem(string Name);

public record EnumConfigItem<TValue>(string Name, List<TValue> EnumValues) : ConfigItem(Name);

public record ValueConfigItem<TValue>(string Name, TValue Value, string Unit) : ConfigItem(Name);

public record ColorConfigItem(string Name, Color Value, string Unit) : ValueConfigItem<Color>(Name, Value, Unit);

public record BoundedValueConfigItem<TValue>(string Name, TValue Value, string Unit, TValue Min, TValue Max) : ValueConfigItem<TValue>(Name, Value, Unit);






