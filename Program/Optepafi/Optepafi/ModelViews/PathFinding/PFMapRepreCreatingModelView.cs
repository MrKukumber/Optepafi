using System;
using System.Threading;
using System.Threading.Tasks;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.MapRepreMan;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.UserModelMan.UserModels;

namespace Optepafi.ModelViews.PathFinding;

public abstract class PFMapRepreCreatingModelView : ModelViewBase
{
    protected PFMapRepreCreatingModelView(){}
    
    public enum ElevDataPrerequisityResult {InOrder, ElevDataForMapNotPresent, MapNotSupportedByElevDataType}

    public abstract ElevDataPrerequisityResult CheckMapRequirementForElevData();
    public abstract Task CreateMapRepreAsync(IProgress<MapRepreCreationReport> progress, CancellationToken cancellationToken);
    public abstract void CleanMapRepre();
}
public partial class PathFindingSessionModelView : SessionModelView
{
    private class PFMapRepreCreatingIntraModelView : PFMapRepreCreatingModelView
    {
        private PFSettingsIntraModelView Settings { get; }
        public override ElevDataPrerequisityResult CheckMapRequirementForElevData()
        {
            MapManager.Instance.G
            MapRepreManager.Instance.DoesNeedElevData()
        }

        public override Task CreateMapRepreAsync(IProgress<MapRepreCreationReport> progress, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override void CleanMapRepre()
        {
            MapRepresentation = null;
        }

        public PFMapRepreCreatingIntraModelView(PFSettingsIntraModelView settings)
        {
            Settings = settings;
        }

        private ITemplate Template => Settings.Template ?? throw new ArgumentNullException( nameof(Settings.Template), "Template should be set before using PFMapRepreCreatingModelView");
        private IMap Map => Settings.Map ?? throw new  ArgumentNullException(nameof(Settings.Map), "Map should be set before using PFMapRepreCreatingModelView");
        private IElevDataType ElevDataType => Settings.ElevDataType ?? throw new ArgumentNullException( nameof(Settings.ElevDataType), "Elevation data type should be set before using PFMapRepreCreatingModelView");
        public IMapRepresentation? MapRepresentation { get; private set; }

    }
}
