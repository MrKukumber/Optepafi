using Optepafi.Models.Utils;
using Optepafi.Models.Utils.Configurations;
using Optepafi.ViewModels.Data.Configuration;

namespace Optepafi.ModelViews.Converters2Vm.ConfigItems;

public interface IConfigItem2VmConverter<TConfigItem> : IConfigItem2VmConverter where TConfigItem : IConfigItem
{
    public ConfigItemViewModel ConvertToViewModel(TConfigItem configItem);
}

public interface IConfigItem2VmConverter
{
    
}