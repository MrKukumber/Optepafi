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

namespace Optepafi.ModelViews.PathFinding;

public abstract class PFPathFindingModelView : ModelViewBase
{
    protected PFPathFindingModelView(){}


    public abstract Task<GraphicsSourceViewModel> GetTrackGraphicsAsync(IEnumerable<(int leftPos, int bottomPos)> trackPoints);
    public abstract GraphicsSourceViewModel GetGroundMapGraphics();
    public abstract Task<PathReportViewModel?> FindPathAsync(List<(int leftPos, int bottomPos)> trackPoints, IProgress<SearchingReportViewModel> searchingProgress, CancellationToken cancellationToken);
    public abstract void OnClosed();
}

public partial class PathFindingSessionModelView : SessionModelView
{
    private class PFPathFindingIntraModelView : PFPathFindingModelView
    {
        private PFRelevanceFeedbackIntraModelView RelevanceFeedback { get; }
        public PFPathFindingIntraModelView(PFRelevanceFeedbackIntraModelView relevanceFeedback)
        {
            RelevanceFeedback = relevanceFeedback;
        }
        
        public override GraphicsSourceViewModel GetGroundMapGraphics()
        {
            return new GraphicsSourceViewModel(CopyGroundGraphicsSource.ParallelCopy(MapGraphics));
        }
        public override async Task<GraphicsSourceViewModel> GetTrackGraphicsAsync(IEnumerable<(int leftPos, int bottomPos)> trackPoints)
        {
            var trackCoordinates= PointsToMapCoordinatesConverter.ConvertAccordingTo(trackPoints, MapGraphics.GraphicsArea).ToList();
            CollectingGraphicsSource trackGraphicsSource = new CollectingGraphicsSource();
            _ = await Task.Run(() => GraphicsManager.Instance.AggregateTrackGraphicsAccordingTo(trackCoordinates, Map, trackGraphicsSource.Collector)); //TODO: log neexistujuceho aggregatoru 
            return new GraphicsSourceViewModel(trackGraphicsSource, MapGraphics);
        }

        private ISearchingExecutor? _searchingExecutor;
        public override async Task<PathReportViewModel?> FindPathAsync(List<(int leftPos, int bottomPos)> trackPoints, IProgress<SearchingReportViewModel> searchingViewModelProgress, CancellationToken cancellationToken)
        {
             _searchingExecutor ??= SearchingAlgorithmManager.Instance.GetExecutorOf(SearchingAlgorithm, MapRepresentation, UserModel);
             
            List<Leg> track = PointsToLegsConverter.ConvertAccordingTo(trackPoints, MapGraphics.GraphicsArea);
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

        public override void OnClosed()
        {
            _searchingExecutor?.Dispose();
        }

        // public IMapFormat<IMap> MapFormat => RelevanceFeedback.MapFormat;
        public IMap Map => RelevanceFeedback.Map;
        public IGroundGraphicsSource MapGraphics => RelevanceFeedback.MapGraphics;
        public IMapRepre MapRepresentation => RelevanceFeedback.MapRepresentation;
        public IUserModel<ITemplate> UserModel => RelevanceFeedback.UserModel;
        public ISearchingAlgorithm SearchingAlgorithm => RelevanceFeedback.SearchingAlgorithm;
    }
}