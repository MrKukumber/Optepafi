using System.Threading;
using Optepafi.Models.ReportMan;
using Optepafi.Models.ReportMan.Aggregators;
using Optepafi.Models.SearchingAlgorithmMan.Paths;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModels;

namespace Optepafi.Models.ReportSubMan.Aggregators;

public interface IPathReportAggregator<in TPath, TVertexAttributes, TEdgeAttributes> : IReportAggregator
    where TPath : IPath<TVertexAttributes, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
{
    IPathReport AggregateReport(TPath path, IComputingUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> userModel, CancellationToken? cancellationToken);
}

// public interface IPathReportAggregator<TPath> : IReportAggregator
    // where TPath : IPath
// {
    // IPathReport AggregateReport(TPath path);
// }