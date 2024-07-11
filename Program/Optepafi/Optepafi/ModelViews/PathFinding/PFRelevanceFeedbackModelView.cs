using System;
using Optepafi.Models.GraphicsMan.Sources;
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.SearchingAlgorithmMan.SearchingAlgorithms;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.UserModelMan.UserModels;

namespace Optepafi.ModelViews.PathFinding;

/// <summary>
/// ModelView which is responsible for logic of relevance feedback mechanism.
/// In current state of application it does not contain any functionality because relevance feeback mechanism was not added to application yet.
/// It is an abstract class. The path finding session ModelView will creates its successor which wil be for this time used only as connecting inner class in path finding ModelView architecture.
/// </summary>
public abstract class PFRelevanceFeedbackModelView : ModelViewBase;

public partial class PathFindingSessionModelView
{
    /// <summary>
    /// Successor of <see cref="PFRelevanceFeedbackModelView"/> created by this session ModelView so some of its methods could be implemented by using data hidden from the outside world.
    /// For this time it just deliver hidden data from previous path finding ModelViews in hierarchy to following ones.
    /// For more information on ModelViews see <see cref="ModelViewBase"/>.
    /// </summary>
    private class PFRelevanceFeedbackIntraModelView(PFSettingsIntraModelView settings, PFMapRepreCreatingIntraModelView mapRepreCreating) : PFRelevanceFeedbackModelView
    {
        private PFSettingsIntraModelView Settings { get; } = settings;
        private PFMapRepreCreatingIntraModelView MapRepreCreating { get; } = mapRepreCreating;

        /// <summary>
        /// Template retrieved from settings ModelView. 
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when template in settings is not set. When relevance feedback ModelView is used it should be set already.</exception>
        public ITemplate Template => Settings.Template ?? throw new ArgumentNullException(nameof(Settings.Template), "Template should be set before using of PFRelevanceFeedbackModelView.");
        /// <summary>
        /// Map retrieved from settings ModelView. 
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when map in settings is not set. When relevance feedback ModelView is used it should be set already.</exception>
        public IMap Map => Settings.Map ?? throw new ArgumentNullException(nameof(Settings.Map), "Map should be chosen before using of PFRelevanceFeedbackModelView.");
        /// <summary>
        /// Map graphics retrieved from settings ModelView. 
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when map graphics in settings is not set. When relevance feedback ModelView is used it should be set already.</exception>
        public IGroundGraphicsSource MapGraphics => Settings.MapGraphics ?? throw new ArgumentNullException( nameof(Settings.MapGraphics), "Map graphics should be aggregated before using of PFRelevanceFeedbackModelView.");
        /// <summary>
        /// Map representation retrieved from map representation creating ModelView. 
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when map representation in map repre. creation ModelView is not set. When relevance feedback ModelView is used it should be set already.</exception>
        public IMapRepre MapRepresentation => MapRepreCreating.MapRepresentation ?? throw new ArgumentNullException( nameof(MapRepreCreating.MapRepresentation), "Map representation should be created before using of PFRelevanceFeedbackModelView.");
        /// <summary>
        /// User model retrieved from settings ModelView.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when user model in settings is not set. When relevance feedback ModelView is used it should be set already.</exception>
        public IUserModel<ITemplate> UserModel => Settings.UserModel ?? throw new ArgumentNullException(nameof(Settings.UserModel), "User model should be chosen before using of PFRelevanceFeedbackModelView");
        /// <summary>
        /// Searching algorithm retrieved from settings ModelView.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when searching algorithm in settings is not set. When relevance feedback ModelView is used it should be set already.</exception>
        public ISearchingAlgorithm SearchingAlgorithm => Settings.SearchingAlgorithm ?? throw new ArgumentNullException( nameof(Settings.SearchingAlgorithm), "Searching algorithm should be chosen before using of PFRelevanceFeedbackModelView");
    }
}