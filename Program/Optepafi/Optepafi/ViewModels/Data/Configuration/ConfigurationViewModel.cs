using System;
using System.Collections.Generic;
using System.Linq;
using Optepafi.Models.MapRepreMan.Configurations;
using Optepafi.Models.Utils;
using Optepafi.Models.Utils.Configurations;
using Optepafi.ModelViews.Converters2Vm.ConfigItems;

namespace Optepafi.ViewModels.Data.Configuration;

//TODO: comment
public class ConfigurationViewModel : WrappingDataViewModel<IConfiguration>,
    IConfigItemGenericVisitor<ConfigItemViewModel?>

{
    protected override IConfiguration Data => Configuration;
    
    public IConfiguration Configuration { get; }

    public ConfigurationViewModel(IConfiguration configuration)
    {
        Configuration = configuration;
        ConfigurationItems = Configuration.ConfigItems
            .Select(configItem => configItem.GenericVisit(this)).ToList();
    }

    public ConfigItemViewModel? AcceptGeneric<TConfigItem>(TConfigItem configItem) where TConfigItem : IConfigItem
    {
        // if (configItem is ICategoricalConfigItem<Enum> categoricalConfigItem)
            // return CategoricalConfigItem2VmConverter.Instance.ConvertToViewModel(categoricalConfigItem);
        if (ConfigItems2VmConverters.Converters.ContainsKey(typeof(TConfigItem)) && ConfigItems2VmConverters.Converters[typeof(TConfigItem)] is IConfigItem2VmConverter<TConfigItem> converter)
            return converter.ConvertToViewModel(configItem);
        //TODO: lognut ked neni pritomny konverter
        Console.WriteLine("Converter is not available for type " + typeof(TConfigItem).Name);
        return null;
    }


    public List<ConfigItemViewModel?> ConfigurationItems { get; }
}