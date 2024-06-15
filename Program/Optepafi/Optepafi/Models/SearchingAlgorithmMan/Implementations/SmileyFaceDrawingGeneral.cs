using System;
using System.Collections.Generic;
using System.Threading;
using Optepafi.Models.MapRepreMan.Graphs;
using Optepafi.Models.MapRepreMan.MapRepreReps;
using Optepafi.Models.ReportMan;
using Optepafi.Models.SearchingAlgorithmMan.Paths;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModels;
using Optepafi.Models.UserModelMan.UserModelTypes;

namespace Optepafi.Models.SearchingAlgorithmMan.Implementations;

public class SmileyFaceDrawingGeneral : ISearchingAlgoritmImplementation
{
    public static SmileyFaceDrawingGeneral Instance { get; } = new();
    private SmileyFaceDrawingGeneral(){}
    public bool DoesRepresentUsableMapRepre(IGraphRepresentative<IGraph<IVertexAttributes, IEdgeAttributes>, IVertexAttributes, IEdgeAttributes> graphRepresentative)
    {
        if (graphRepresentative is IGraphRepresentative<IGraph<IVertexAttributes, IEdgeAttributes>, IVertexAttributes, IEdgeAttributes>) 
            return true;
        return false;
    }

    public bool DoesRepresentUsableUserModel<TVertexAttributes, TEdgeAttributes>(IUserModelType<IComputingUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes>, ITemplate<TVertexAttributes, TEdgeAttributes>> userModelType)
        where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
    {
        if (userModelType is IUserModelType< IComputingUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes>, ITemplate<TVertexAttributes, TEdgeAttributes>>)
            return true;
        return false;
    }

    public bool IsUsableGraph<TVertexAttributes, TEdgeAttributes>(IGraph<TVertexAttributes, TEdgeAttributes> graph) where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
    {
        if (graph is IGraph<TVertexAttributes, TEdgeAttributes>)
            return true;
        return false;
    }

    public bool IsUsableUserModel<TVertexAttributes, TEdgeAttributes>(IComputingUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> userModel) where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
    {
        if (userModel is IComputingUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes>)
            return true;
        return false;
    }

    public IPath<TVertexAttributes, TEdgeAttributes>[][] SearchForPaths<TVertexAttributes, TEdgeAttributes>(Leg[] track, IGraph<TVertexAttributes, TEdgeAttributes> graph, IEnumerable<IComputingUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes>> userModels,
        IProgress<ISearchingReport>? progress, CancellationToken? cancellationToken) where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
    {
        throw new NotImplementedException();
    }

    public IPath<TVertexAttributes, TEdgeAttributes>[] ExecutorSearch<TVertexAttributes, TEdgeAttributes>(Leg[] track, IGraph<TVertexAttributes, TEdgeAttributes> graph, IComputingUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> userModel,
        IProgress<ISearchingReport>? progress, CancellationToken? cancellationToken) where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
    {
        throw new NotImplementedException();
    }
}