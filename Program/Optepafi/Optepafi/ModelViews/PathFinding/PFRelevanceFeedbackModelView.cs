using System;
using Optepafi.Models.Graphics.Sources;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.SearchingAlgorithmMan.SearchAlgorithms;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.UserModelMan.UserModels;

namespace Optepafi.ModelViews.PathFinding;

public abstract class PFRelevanceFeedbackModelView : ModelViewBase
{
    protected PFRelevanceFeedbackModelView(){}
}

public partial class PathFindingSessionModelView : SessionModelView
{
    private class PFRelevanceFeedbackIntraModelView : PFRelevanceFeedbackModelView
    {
        private PFSettingsIntraModelView Settings { get; }
        private PFMapRepreCreatingIntraModelView MapRepreCreating { get; }

        public PFRelevanceFeedbackIntraModelView(PFSettingsIntraModelView settings, PFMapRepreCreatingIntraModelView mapRepreCreating)
        {
            Settings = settings;
            MapRepreCreating = mapRepreCreating;
        }

        public ITemplate Template => Settings.Template ?? throw new ArgumentNullException(nameof(Settings.Template), "Template should be set before using of PFRelevanceFeedbackModelView.");
        // public IMapFormat<IMap> MapFormat => Settings.MapFormat ?? throw new ArgumentNullException(nameof(Settings.MapFormat), "Map format should be set before using of PFRelevanceFeedbackModelView.");
        public IMap Map => Settings.Map ?? throw new ArgumentNullException(nameof(Settings.Map), "Map should be chosen before using of PFRelevanceFeedbackModelView.");
        public IGroundGraphicsSource MapGraphics => Settings.MapGraphics ?? throw new ArgumentNullException( nameof(Settings.MapGraphics), "Map graphics should be aggregated before using of PFRelevanceFeedbackModelView.");
        public IMapRepre MapRepresentation => MapRepreCreating.MapRepresentation ?? throw new ArgumentNullException( nameof(MapRepreCreating.MapRepresentation), "Map representation should be created before using of PFRelevanceFeedbackModelView.");
        public IUserModel<ITemplate> UserModel => Settings.UserModel ?? throw new ArgumentNullException(nameof(Settings.UserModel), "User model should be chosen before using of PFRelevanceFeedbackModelView");
        public ISearchingAlgorithm SearchingAlgorithm => Settings.SearchingAlgorithm ?? throw new ArgumentNullException( nameof(Settings.SearchingAlgorithm), "Searching algorithm should be chosen before using of PFRelevanceFeedbackModelView");
    }
}