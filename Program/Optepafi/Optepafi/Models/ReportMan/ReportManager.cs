using System.Threading;
using Optepafi.Models.ReportMan.Aggregators;
using Optepafi.Models.ReportMan.Reports;
using Optepafi.Models.SearchingAlgorithmMan.Paths;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.UserModelMan.UserModels;
using Optepafi.Models.UserModelMan.UserModels.Functionalities;

namespace Optepafi.Models.ReportMan;

/// <summary>
/// Singleton class used for managing of reports aggregation.
/// 
/// It is main channel between report aggregation mechanisms and applications logic (ModelView/ViewModel).  
/// It provides supporting methods for correct callings of report aggregators.  
/// Additionally there is also <see cref="ReportSubManager{TVertexAttributes,TEdgeAttributes}"/>  singleton class which is intended to provide report aggregation services for other managers and constructs in Model.  
///
/// Report creation is done by accepting object for which report can and will be created and filling it with information extracted from object. These extraction can include graphics aggregation which is then included in report.  
/// In process of aggregation there is usually need for user models support. Provided objects to be aggregated often bears vertex and edge attributes so it is sometimes necessary for user models to be able to process these attributes and provide usable values.  
/// User models should at leas be ale to retrieve positions from attributes and return them to aggregators, so they could at least provide the basic information in created reports.  
/// Each method is dedicated for aggregation of specific type of objects.  
/// </summary>
public class ReportManager : 
    ITemplateGenericVisitor<(ReportManager.AggregationResult, IPathReport?), (IPath, IUserModel<ITemplate>, CancellationToken?)>,
    IPathGenericVisitor<(ReportManager.AggregationResult, IPathReport?), (IUserModel<ITemplate>, CancellationToken?)>

{
    public static ReportManager Instance { get; } = new();
    private ReportManager(){}
    
    /// <summary>
    /// Indicates result of aggregation. Mainly it indicates whether some received source does not satisfy usability conditions.
    /// </summary>
    public enum AggregationResult {Aggregated, NonGenericPath, NotUsableUserModel, NoUsableAggregator}
    
    /// <summary>
    /// Method for aggregation of paths report. It accepts path which aggregation is based on.
    /// 
    /// It also requests user model, that can be used for computing of some values for aggregator.No specific functionality is forced on provided user model. More about usage of user models in <see cref="IPathReportAggregator{TPath,TVertexAttributes,TEdgeAttributes}"/>.  
    /// Implementation of method uses "generic visitor pattern" at first on template associated with provided user model and then on path itself.  
    /// The check of type usability of provided instances is performed along side of visiting.  
    /// When everything is satisfied, path report aggregating method <see cref="ReportSubManager{TVertexAttributes,TEdgeAttributes}.AggregatePathReport{TPath}"/> of report sub-manager is called and it secures all remaining actions.  
    /// </summary>
    /// <param name="path">Path which report is to be aggregated.</param>
    /// <param name="userModel">User model that can be used for aggregation of report.</param>
    /// <param name="pathReport">Out parameter for returning of created path report when successful.</param>
    /// <param name="cancellationToken">Cancellation token for cancellation of aggregation.</param>
    /// <returns></returns>
    public AggregationResult AggregatePathReport(
        IPath path, IUserModel<ITemplate> userModel, out IPathReport? pathReport, CancellationToken? cancellationToken = null)
    {
        var (aggregationResult, report) = userModel.AssociatedTemplate.AcceptGeneric(this, (path, userModel, cancellationToken));
        pathReport = report;
        return aggregationResult;
    }
    (AggregationResult, IPathReport?) ITemplateGenericVisitor<(AggregationResult, IPathReport?), (IPath, IUserModel<ITemplate>, CancellationToken?)>.GenericVisit<TTemplate, TVertexAttributes, TEdgeAttributes>(
        TTemplate template,
        (IPath, IUserModel<ITemplate>, CancellationToken?) otherParams)
    {
        var (path, userModel, cancellationToken) = otherParams;
        if (path is IPath<TVertexAttributes, TEdgeAttributes> p)
        {
            return p.AcceptGeneric(this, (userModel, cancellationToken));
        }
        return (AggregationResult.NonGenericPath, null);
    }
    (AggregationResult, IPathReport?) IPathGenericVisitor<(AggregationResult, IPathReport?), (IUserModel<ITemplate>, CancellationToken?)>.GenericVisit<TPath, TVertexAttributes, TEdgeAttributes>(
        TPath path,
        (IUserModel<ITemplate>, CancellationToken?) otherParams)
    {
        var (userModel, cancellationToken) = otherParams;
        if (userModel is IComputing<ITemplate<TVertexAttributes,TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> usableUserModel)
        {
            var pathReport = ReportSubManager<TVertexAttributes, TEdgeAttributes>.Instance.AggregatePathReport(path, usableUserModel, cancellationToken);
            if (pathReport is not null) return (AggregationResult.Aggregated, pathReport);
            return (AggregationResult.NoUsableAggregator, pathReport);
        }
        return (AggregationResult.NotUsableUserModel, null);
    }
}