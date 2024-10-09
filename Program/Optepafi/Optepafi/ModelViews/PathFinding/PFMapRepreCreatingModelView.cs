using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.ElevationDataMan.Distributions;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.MapRepreMan;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.MapRepreMan.MapRepres.Representatives;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.Utils;
using Optepafi.ViewModels.Data.Reports;

namespace Optepafi.ModelViews.PathFinding;

/// <summary>
/// ModelView which is responsible for logic of map representations creation in path finding session.
/// 
/// It uses <c>MapRepreManager</c> for:
/// 
/// - checking requirements for map representations creation
/// - the creation of maps representation itself
/// 
/// This is an abstract class. The path finding session ModelView will creates its successor which will then be able to implement methods of this class by using data hidden from the outside world.  
/// For more information on ModelViews see <see cref="ModelViewBase"/>.  
/// </summary>
public abstract class PFMapRepreCreatingModelView : ModelViewBase
{
    /// <summary>
    /// Enumeration of elevation data check results. It contains result for every circumstance that can occur during check.
    /// </summary>
    public enum ElevDataPrerequisiteCheckResult {InOrder, ElevDataForMapNotPresent, MapNotSupportedByElevDataDistribution, ElevDataDistributionNotSet, Cancelled}
    /// <summary>
    /// Method for elevation data requirements check.
    /// 
    /// It contains logic of this check and resolves multiple circumstances that can occur.  
    /// </summary>
    /// <param name="ct">Cancellation token for cancelling the check.</param>
    /// <returns>Result of elevation data requirements check.</returns>
    public abstract ElevDataPrerequisiteCheckResult CheckMapRequirementsForElevData(CancellationToken ct);
    /// <summary>
    /// Method for creating map representation asynchronously.
    ///
    /// Check of map representations creation prerequisites should take place before calling this method.  
    /// In other way there could be risk of thrown exceptions for invalidity of operations. Also these checks can assign some configuration that is needed for correct map creation execution.  
    /// </summary>
    /// <param name="progressInfo">Instance for reporting information about maps creation progress. This is just textual information about progress. It is not meant to be propagated any further to Model.</param>
    /// <param name="mapRepreCreationProgress">Instance reporting progress of maps creation. This progress instance is meant for propagation to <c>MapRepreManager</c> so the maps creation can be shown. </param>
    /// <param name="cancellationToken">Cancellation token for cancelling of maps creation.</param>
    /// <returns></returns>
    public abstract Task CreateMapRepreAsync(IProgress<string> progressInfo, IProgress<MapRepreConstructionReportViewModel> mapRepreCreationProgress, CancellationToken cancellationToken);
}
public partial class PathFindingSessionModelView 
{
    /// <summary>
    /// Successor of <see cref="PFMapRepreCreatingModelView"/> created by this session ModelView so some of its methods could be implemented by using data hidden from the outside world.
    /// </summary>
    private class PFMapRepreCreatingIntraModelView(PFSettingsIntraModelView settings) : PFMapRepreCreatingModelView
    {
        /// <summary>
        /// Connection to the settings ModelView.
        /// </summary>
        private PFSettingsIntraModelView Settings { get; } = settings;

        /// <summary>
        /// Indicator whether map representation should be created by use of elevation data.
        /// 
        /// This indicator is set in <c>CheckMapRequirementsForElevData</c> method.
        /// </summary>
        private bool _useElevData;
        
        /// <inheritdoc cref="PFMapRepreCreatingModelView.CheckMapRequirementsForElevData"/>
        public override ElevDataPrerequisiteCheckResult CheckMapRequirementsForElevData(CancellationToken ct)
        {
            switch (MapRepreManager.Instance.DoesNeedElevData(Template, MapFormat, MapRepreRepresentative))
            {
                case MapRepreManager.NeedsElevDataIndic.No:
                    if (ct.IsCancellationRequested) return ElevDataPrerequisiteCheckResult.Cancelled;
                    _useElevData = false;
                    return ElevDataPrerequisiteCheckResult.InOrder;
                case MapRepreManager.NeedsElevDataIndic.Yes:
                    if (ct.IsCancellationRequested) return ElevDataPrerequisiteCheckResult.Cancelled;
                    if (ElevDataDistribution is null) return ElevDataPrerequisiteCheckResult.ElevDataDistributionNotSet;
                    if (Map is IGeoLocatedMap geoLocatedMap)
                        switch (ElevDataManager.Instance.AreElevDataFromDistObtainableFor(geoLocatedMap, ElevDataDistribution, ct))
                        {
                            case ElevDataManager.ElevDataObtainability.Obtainable:
                                _useElevData = true;
                                return ElevDataPrerequisiteCheckResult.InOrder;
                            case ElevDataManager.ElevDataObtainability.ElevDataNotPresent:
                                return ElevDataPrerequisiteCheckResult.ElevDataForMapNotPresent;
                            case ElevDataManager.ElevDataObtainability.NotSupportedMap:
                                return ElevDataPrerequisiteCheckResult.MapNotSupportedByElevDataDistribution;
                            case ElevDataManager.ElevDataObtainability.Cancelled:
                                return ElevDataPrerequisiteCheckResult.Cancelled;
                            default:
                                throw new InvalidEnumArgumentException();
                        }
                    else return ElevDataPrerequisiteCheckResult.MapNotSupportedByElevDataDistribution;
                case MapRepreManager.NeedsElevDataIndic.NotNecessary:
                    if (ct.IsCancellationRequested) return ElevDataPrerequisiteCheckResult.Cancelled;
                    if (Map is IGeoLocatedMap glm && ElevDataDistribution is not null)
                        _useElevData = (ElevDataManager.Instance.AreElevDataFromDistObtainableFor(glm, ElevDataDistribution, ct)) switch
                        {
                            ElevDataManager.ElevDataObtainability.Obtainable => true,
                            _ => false
                        };
                    else _useElevData = false;
                    return ElevDataPrerequisiteCheckResult.InOrder;
                case null:
                    throw new InvalidOperationException( "Given Map`s format and Template is not usable combination for MapRepreRepresentative.");
                default:
                    throw new InvalidEnumArgumentException();
            }
        }

        /// <inheritdoc cref="PFMapRepreCreatingModelView.CreateMapRepreAsync"/>
        public override async Task CreateMapRepreAsync(IProgress<string> progressInfo, IProgress<MapRepreConstructionReportViewModel> mapRepreCreationProgress, CancellationToken cancellationToken)
        {
            IProgress<MapRepreConstructionReport> mrcProgress = new Progress<MapRepreConstructionReport>
            (report => mapRepreCreationProgress.Report(new MapRepreConstructionReportViewModel(report)));
            
            if (_useElevData)
            {
                if (Map is IGeoLocatedMap geoLocatedMap)
                {
                    progressInfo.Report("Acquiring elevation data"); //TODO: localize
                    IElevData elevData = await Task.Run(() =>
                        ElevDataManager.Instance.GetElevDataFromDistFor(geoLocatedMap, ElevDataDistribution, cancellationToken));
                    if (cancellationToken.IsCancellationRequested) return;
                    
                    progressInfo.Report("Creating map representation"); //TODO: localize
                    MapRepresentation = await Task.Run(() => 
                        MapRepreManager.Instance.CreateMapRepre(Template, geoLocatedMap, MapRepreRepresentative, elevData, MapRepresentationConfiguration, mrcProgress, cancellationToken));
                    if (cancellationToken.IsCancellationRequested) MapRepresentation = null;
                }
                else throw new InvalidOperationException("There is some error in prerequisites check method, that allows _useElevData to be set to true, when map is not even IGeoLocatedMap.");
            }
            else
            {
                progressInfo.Report("Creating map representation"); //TODO: localize
                MapRepresentation = await Task.Run(() =>
                    MapRepreManager.Instance.CreateMapRepre(Template, Map, MapRepreRepresentative, MapRepresentationConfiguration, mrcProgress, cancellationToken));
                if (cancellationToken.IsCancellationRequested) MapRepresentation = null;
            }
        }

        /// <summary>
        /// Template retrieved from settings ModelView used in map representation creation.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when template in settings is not set. When map creating ModelView is used it should be set already.</exception>
        private ITemplate Template => Settings.Template ?? throw new ArgumentNullException( nameof(Settings.Template), "Template should be set before using PFMapRepreCreatingModelView");
        /// <summary>
        /// Map format of map retrieved from settings ModelView whose representation is created.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when map format in settings is not set. When map creating ModelView is used it should be set already.</exception>
        private IMapFormat<IMap> MapFormat => Settings.MapFormat ?? throw new ArgumentNullException(nameof(Settings.Map), "Map should be set before using PFMapRepreCreatingModelView"); 
        /// <summary>
        /// Map retrieved form settings ModelView whose representation is created.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when map in settings is not set. When map creating ModelView is used it should be set already.</exception>
        private IMap Map => Settings.Map ?? throw new  ArgumentNullException(nameof(Settings.Map), "Map should be set before using PFMapRepreCreatingModelView");
        /// <summary>
        /// Map representation representative retrieved from settings ModelView. Represents map representation which should be created.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when representative in settings is not set. When map creating ModelView is used it should be set already.</exception>
        private IMapRepreRepresentative<IMapRepre> MapRepreRepresentative => Settings.MapRepreRepresentative ?? throw new ArgumentNullException( nameof(Settings.MapRepreRepresentative), "Map representation representative should be set before using PFMapRepreCreatingModelView");
        private IConfiguration MapRepresentationConfiguration => Settings.MapRepresentationConfiguration ?? throw new ArgumentNullException( nameof(Settings.MapRepresentationConfiguration), "Map representation configuration should be set before using PFMapRepreCreatingModelView");
        /// <summary>
        /// Elevation data distribution retrieved from settings ModelView. This distribution is eventually used in map representations creation.
        /// </summary>
        private IElevDataDistribution? ElevDataDistribution => Settings.ElevDataDistribution; 
        /// <summary>
        /// Reference to created map representation. Main result of this ModelView mechanism. Other ModelViews may use it for further work.
        /// </summary>
        public IMapRepre? MapRepresentation { get; private set; }
        
    }
}
