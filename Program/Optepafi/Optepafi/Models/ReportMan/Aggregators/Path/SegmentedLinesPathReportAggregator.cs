using System.Threading;
using System.Threading.Tasks;
using Optepafi.Models.GraphicsMan;
using Optepafi.Models.ReportMan.Reports;
using Optepafi.Models.ReportMan.Reports.Path;
using Optepafi.Models.SearchingAlgorithmMan.Paths.Specific;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModels.Functionalities;
using Optepafi.ModelViews.PathFinding.Utils;

namespace Optepafi.Models.ReportMan.Aggregators.Path;

//TODO: comment
public class SegmentedLinesPathReportAggregator<TVertexAttributes, TEdgeAttributes> : IPathReportAggregator<SegmentedLinesPath<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    public static SegmentedLinesPathReportAggregator<TVertexAttributes, TEdgeAttributes> Instance { get; } = new();
    private SegmentedLinesPathReportAggregator(){}

    public IPathReport AggregateReport(SegmentedLinesPath<TVertexAttributes, TEdgeAttributes> path, IComputing<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> userModel, CancellationToken? cancellationToken = null)
    {
        CollectingGraphicsSource collectingGraphicsSource = new();
        GraphicsSubManager<TVertexAttributes, TEdgeAttributes>.Instance.AggregatePathGraphics(path, userModel, collectingGraphicsSource.Collector, cancellationToken);
        return new SegmentedLinesPathReport(collectingGraphicsSource);
    }
}