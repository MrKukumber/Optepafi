using System;
using System.Threading;
using Optepafi.Models.MapRepreMan.Graphs;
using Optepafi.Models.MapRepreMan.MapRepreReps;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModels;

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

    public bool IsUsableGraph<TVertexAttributes, TEdgeAttributes>(IGraph<TVertexAttributes, TEdgeAttributes> graph) where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
    {
        if (graph is IGraph<TVertexAttributes, TEdgeAttributes>)
            return true;
        return false;
    }

    public Path[][] SearchForPaths<TVertexAttributes, TEdgeAttributes>(Leg[] track, IGraph<TVertexAttributes, TEdgeAttributes> graph, IComputingUserModel<TVertexAttributes, TEdgeAttributes>[] userModels,
        IProgress<ISearchingReport>? progress, CancellationToken? cancellationToken) where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
    {
        throw new NotImplementedException();
    }

    public Path[] ExecutorSearch<TVertexAttributes, TEdgeAttributes>(Leg[] track, IGraph<TVertexAttributes, TEdgeAttributes> graph, IComputingUserModel<TVertexAttributes, TEdgeAttributes> userModel,
        IProgress<ISearchingReport>? progress, CancellationToken? cancellationToken) where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
    {
        throw new NotImplementedException();
    }
}