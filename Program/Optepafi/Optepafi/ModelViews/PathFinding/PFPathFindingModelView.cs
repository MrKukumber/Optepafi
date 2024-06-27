using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Optepafi.Models.Graphics;
using Optepafi.Models.Graphics.Sources;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.ReportMan;
using Optepafi.Models.ReportMan.Reports;
using Optepafi.Models.SearchingAlgorithmMan;
using Optepafi.Models.SearchingAlgorithmMan.Paths;
using Optepafi.Models.SearchingAlgorithmMan.SearchAlgorithms;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.UserModelMan.UserModels;
using Optepafi.ModelViews.PathFinding.Utils;
using Optepafi.ModelViews.Utils;
using Optepafi.ViewModels.Data.Graphics;
using Optepafi.ViewModels.Data.Reports;
using Optepafi.ViewModels.DataViewModels;

namespace Optepafi.ModelViews.PathFinding;

/// <summary>
/// ModelView which is responsible for logic of path finding mechanism in path finding session.
/// Its duties include:
/// - managing execution of searching algorithm upon created map representation by using selected user model
/// - providing in settings ModelView aggregated map graphics
/// - aggregate graphics for selected tracks
/// - aggregate reports for founded paths by searching algorithm
/// This is an abstract class. The path finding session ModelView will create its successor which will then be able to implement methods of this class by using data hidden from the outside world.
/// For more information on ModelViews see <see cref="ModelViewBase"/>.
/// </summary>
public abstract class PFPathFindingModelView : ModelViewBase
{
    protected PFPathFindingModelView(){}

    /// <summary>
    /// Asynchronously returns graphics source of provided track defined by its coordinates.
    /// Graphic source is returned after collecting of tracks graphic objects is finished for better visual properties.
    /// </summary>
    /// <param name="trackCoords">Canvas coordinates representing selected track.</param>
    /// <returns>Task with tracks graphics source.</returns>
    public abstract Task<GraphicsSourceViewModel> GetTrackGraphicsAsync(IEnumerable<CanvasCoordinate> trackCoords);
    /// <summary>
    /// Method providing maps graphics generated in settings ModelView.
    /// The copy of original ground graphics source is returned due rendering performance issues linked with returning o original instance.
    /// This method should be called on corresponding ViewModels activation.
    /// </summary>
    /// <returns>Maps ground graphic source.</returns>
    public abstract GraphicsSourceViewModel GetGroundMapGraphics();
    /// <summary>
    /// Asynchronous execution of path finding algorithm on provided track.
    /// Algorithm returns resulting path and then its report is aggregated and returned.
    /// </summary>
    /// <param name="trackPoints">Canvas coordinates of track for which path should be found.</param>
    /// <param name="searchingProgress">Instance which reports progress of searching. This instance is propagated to searching algorithm, so it could report its state.</param>
    /// <param name="cancellationToken">Cancellation token for cancelling of searching.</param>
    /// <returns></returns>
    public abstract Task<PathReportViewModel?> FindPathAsync(List<CanvasCoordinate> trackPoints, IProgress<SearchingReportViewModel> searchingProgress, CancellationToken cancellationToken);
    /// <summary>
    /// Method called on path finding sessions window closed event. It may be used for disposing of resources.
    /// </summary>
    public abstract void OnClosed();
}

public partial class PathFindingSessionModelView : SessionModelView
{
    private class PFPathFindingIntraModelView(PFRelevanceFeedbackIntraModelView relevanceFeedback) : PFPathFindingModelView
    {
        private PFRelevanceFeedbackIntraModelView RelevanceFeedback { get; } = relevanceFeedback;
        
        /// <inheritdoc cref="PFPathFindingModelView.GetGroundMapGraphics"/>
        public override GraphicsSourceViewModel GetGroundMapGraphics()
        {
            return new GraphicsSourceViewModel(CopyGroundGraphicsSource.ParallelCopy(MapGraphics));
        }
        
        /// <inheritdoc cref="PFPathFindingModelView.GetTrackGraphicsAsync"/>
        public override async Task<GraphicsSourceViewModel> GetTrackGraphicsAsync(IEnumerable<CanvasCoordinate> trackCoords)
        {
            var trackCoordinates= trackCoords.Select(trackCanvasCoord => trackCanvasCoord.ToMapCoordinate(MapGraphics.GraphicsArea.LeftBottomVertex)).ToList();
            CollectingGraphicsSource trackGraphicsSource = new CollectingGraphicsSource();
            _ = await Task.Run(() => GraphicsManager.Instance.AggregateTrackGraphicsAccordingTo(trackCoordinates, Map, trackGraphicsSource.Collector)); //TODO: log neexistujuceho aggregatoru 
            return new GraphicsSourceViewModel(trackGraphicsSource, MapGraphics);
        }

        private ISearchingExecutor? _searchingExecutor;
        
        
        /// <inheritdoc cref="PFPathFindingModelView.FindPathAsync"/>
        /// <remarks>
        /// Executes search of path by using appropriately created <see cref="ISearchingExecutor"/> filled with created map representation and selected user model.
        /// Executor is retrieved at first call of this method.
        /// </remarks>
        public override async Task<PathReportViewModel?> FindPathAsync(List<CanvasCoordinate> trackCanvasCoords, IProgress<SearchingReportViewModel> searchingViewModelProgress, CancellationToken cancellationToken)
        {
             _searchingExecutor ??= SearchingAlgorithmManager.Instance.GetExecutorOf(SearchingAlgorithm, MapRepresentation, UserModel);
             
            List<Leg> track = CanvasCoordsToLegsConverter.ConvertAccordingTo(trackCanvasCoords, MapGraphics.GraphicsArea);
            IProgress<ISearchingReport> searchingProgress = new Progress<ISearchingReport>(report =>
            {
                var reportViewModel = SearchingReportViewModel.Construct(report, MapGraphics);
                if(reportViewModel is not null)
                    searchingViewModelProgress.Report(reportViewModel);
                else { /*TODO: log absencie convertoru do VM pre dany typ reportu */ }
            });
            
            IPath foundMergedPath = await Task.Run(() => _searchingExecutor.Search(track.ToArray(), searchingProgress, cancellationToken));
            if (cancellationToken.IsCancellationRequested) return null;
            
            _ = ReportManager.Instance.AggregatePathReport(foundMergedPath, UserModel, out IPathReport? pathReport, cancellationToken); //TODO log problemu s agregovanim reportu
            return pathReport is null ? null : PathReportViewModel.Construct(pathReport, MapGraphics);
        }

        /// <inheritdoc cref="PFPathFindingModelView.FindPathAsync"/>
        /// <remarks>
        /// Disposes searching executor used in <c>FindPathAsync</c> method for path finding.
        /// </remarks>
        public override void OnClosed()
        {
            _searchingExecutor?.Dispose();
        }

        /// <summary>
        /// Map created in settings ModelView.
        /// </summary>
        public IMap Map => RelevanceFeedback.Map;
        /// <summary>
        /// Map graphics created in settings ModelView. 
        /// </summary>
        public IGroundGraphicsSource MapGraphics => RelevanceFeedback.MapGraphics;
        /// <summary>
        /// Map representation created by map repre. creating ModelView.
        /// </summary>
        public IMapRepre MapRepresentation => RelevanceFeedback.MapRepresentation;
        /// <summary>
        /// User model loaded in settings ModelView and eventually adjusted by relevance feedback ModelView.
        /// </summary>
        public IUserModel<ITemplate> UserModel => RelevanceFeedback.UserModel;
        /// <summary>
        /// Chosen searching algorithm which should be executed for path finding.
        /// </summary>
        public ISearchingAlgorithm SearchingAlgorithm => RelevanceFeedback.SearchingAlgorithm;
    }
}