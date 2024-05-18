using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Markup.Xaml.Templates;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.MapRepreMan;
using Optepafi.Models.SearchingAlgorithmMan;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.UserModelMan;
using Optepafi.Models.UserModelMan.UserModels;
using Optepafi.ViewModels.DataViewModels;

namespace Optepafi.ModelViews.ModelCreating;

public abstract class PFSettingsModelView : ModelViewBase
{
    protected PFSettingsModelView() { }

    public IReadOnlyCollection<SearchingAlgorithmViewModel>? GetUsableAlgorithms(
        TemplateViewModel? templateViewModel, MapFormatViewModel? mapFormatViewModel)
    {
        
    }

    public IReadOnlyCollection<MapFormatViewModel> GetUsableMapFormats(TemplateViewModel? templateViewModel)
    {
        
    }

    public IReadOnlyCollection<UserModelTypeViewModel> GetUsableUserModelTypes(
        TemplateViewModel? templateViewModel)
    {
        
    }

    public IEnumerable<TemplateViewModel> GetUsableTemplates(MapFormatViewModel? mapFormatViewModel)
    {
        
    }

    public MapFormatViewModel? GetCorrespondingMapFormat(string mapFileName)
    {
        
    }

    public UserModelTypeViewModel? GetCorrespondingUserModelType(string userModelFileName)
    {
        
    }

    public TemplateViewModel? GetTemplateByTypeName(string templateTypeName)
    {
        
    }

    public SearchingAlgorithmViewModel? GetSearchingAlgorithmByTypeName(string searchingAlgorithmTypeName)
    {
        
    }

    public abstract void SetElevDataType(ElevDataTypeViewModel? elevDataTypeViewModel);
    public abstract void SetTemplate(TemplateViewModel? templateViewModel);
    public abstract void SetSearchingAlgorithm(SearchingAlgorithmViewModel? searchingAlgorithmViewModel);
    public abstract Task<MapManager.MapCreationResult> LoadAndSetMapAsync(Stream stream, 
        MapFormatViewModel mapFormatViewModel, CancellationToken cancellationToken);
    public abstract Task<UserModelManager.UserModelLoadResult> LoadAndSetUserModelAsync(Stream stream,
        UserModelTypeViewModel userModelTypeViewModel, CancellationToken cancellationToken);
    
    
}

public partial class PathFindingSessionModelView : SessionModelView
{
    private class PFSettingsIntraModelView : PFSettingsModelView
    {
        public PFSettingsIntraModelView(){}
        
        public IElevDataType? ElevDataType { get; private set; }
        public ITemplate? Template { get; private set; }
        public IMap? Map { get; private set; }
        public IUserModel<ITemplate>? UserModel { get; private set; }
        public ISearchingAlgorithm? SearchingAlgorithm { get; private set; }
        public override void SetElevDataType(ElevDataTypeViewModel? elevDataTypeViewModel)
        {
            ElevDataType = elevDataTypeViewModel?.ElevDataType;
        }
        public override void SetTemplate(TemplateViewModel? templateViewModel)
        {
            Template = templateViewModel?.Template;
        }

        public override void SetSearchingAlgorithm(SearchingAlgorithmViewModel? searchingAlgorithmViewModel)
        {
            SearchingAlgorithm = searchingAlgorithmViewModel?.SearchingAlgorithm;
        }

        public override async Task<MapManager.MapCreationResult> LoadAndSetMapAsync(Stream stream, 
            MapFormatViewModel mapFormatViewModel, CancellationToken cancellationToken)
        {
            var (mapCreationResult, map) = await Task.Run(() =>
            {
                var result = MapManager.Instance.GetMapFromOf(stream, mapFormatViewModel.MapFormat, cancellationToken, out IMap? map);
                return (result, map);
            });
            if(!cancellationToken.IsCancellationRequested)
                Map = map;
            return mapCreationResult;
        }

        public override async Task<UserModelManager.UserModelLoadResult> LoadAndSetUserModelAsync(Stream stream, 
            UserModelTypeViewModel userModelTypeViewModel, CancellationToken cancellationToken)
        {
            var (userModelCreationResult, userModel) = await Task.Run(() =>
            {
                var result = UserModelManager.Instance.DeserializeUserModelFromOf(stream, userModelTypeViewModel.UserModelType, cancellationToken, out IUserModel<ITemplate>? userModel);
                return (result, userModel);
            });
            if(!cancellationToken.IsCancellationRequested)
                UserModel = userModel;
            return userModelCreationResult;
        }
    }
}