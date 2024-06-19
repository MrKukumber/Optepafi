using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Avalonia.Input;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapRepreMan.Graphs;
using Optepafi.Models.MapRepreMan.MapRepreReps;
using Optepafi.Models.ReportMan;
using Optepafi.Models.ReportMan.Reports;
using Optepafi.Models.ReportMan.Reports.SearchingState;
using Optepafi.Models.SearchingAlgorithmMan.Paths;
using Optepafi.Models.SearchingAlgorithmMan.SearchingStates;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModels;
using Optepafi.Models.UserModelMan.UserModelTypes;

namespace Optepafi.Models.SearchingAlgorithmMan.Implementations;

public class SmileyFacesDrawerGeneral : ISearchingAlgoritmImplementation
{
    public static SmileyFacesDrawerGeneral Instance { get; } = new();
    private SmileyFacesDrawerGeneral(){}
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

    public IPath<TVertexAttributes, TEdgeAttributes>[] SearchForPaths<TVertexAttributes, TEdgeAttributes>(Leg[] track, IGraph<TVertexAttributes, TEdgeAttributes> graph, IList<IComputingUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes>> userModels,
        IProgress<ISearchingReport>? progress, CancellationToken? cancellationToken) where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
    {
        if (userModels.Count == 0) return [];
        var drawnFacePaths = ExecutorSearch(track, graph, userModels[0], progress, cancellationToken);
        return Enumerable.Repeat(drawnFacePaths, userModels.Count).ToArray();
    }

    public IPath<TVertexAttributes, TEdgeAttributes> ExecutorSearch<TVertexAttributes, TEdgeAttributes>(Leg[] track, IGraph<TVertexAttributes, TEdgeAttributes> graph, IComputingUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> userModel,
        IProgress<ISearchingReport>? progress, CancellationToken? cancellationToken) where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
    {
        
        int drawingDuration = 500;
        SmileyFacePathDrawingState<TVertexAttributes, TEdgeAttributes>.SmileyFaceObject[] allFacialObjectsExceptLeftEye =
        [
            SmileyFacePathDrawingState<TVertexAttributes, TEdgeAttributes>.SmileyFaceObject.RightEye,
            SmileyFacePathDrawingState<TVertexAttributes, TEdgeAttributes>.SmileyFaceObject.Nose,
            SmileyFacePathDrawingState<TVertexAttributes, TEdgeAttributes>.SmileyFaceObject.Mouth
        ];
        SmileyFacePath<TVertexAttributes, TEdgeAttributes> drawnFacePaths = new SmileyFacePath<TVertexAttributes, TEdgeAttributes>();
        for(int i = 0; i < track.Length; i++)
        {
            Thread.Sleep(drawingDuration/2);
            if (cancellationToken is not null && cancellationToken.Value.IsCancellationRequested) return drawnFacePaths;
            Thread.Sleep(drawingDuration/2);
            if (cancellationToken is not null && cancellationToken.Value.IsCancellationRequested) return drawnFacePaths;
            SmileyFacePathDrawingState<TVertexAttributes, TEdgeAttributes> drawingState = new SmileyFacePathDrawingState<TVertexAttributes, TEdgeAttributes>(SmileyFacePathDrawingState<TVertexAttributes, TEdgeAttributes>.SmileyFaceObject.LeftEye, track[i], i+1);
            ISearchingReport? report =  ReportSubManager<TVertexAttributes, TEdgeAttributes>.Instance.AggregateSearchingReport(drawingState, userModel);
            if(report is not null && progress is not null) progress.Report(report);
            foreach (var smileyFaceObject in allFacialObjectsExceptLeftEye)
            {
                Thread.Sleep(drawingDuration/2);
                if (cancellationToken is not null && cancellationToken.Value.IsCancellationRequested) return drawnFacePaths;
                Thread.Sleep(drawingDuration/2);
                if (cancellationToken is not null && cancellationToken.Value.IsCancellationRequested) return drawnFacePaths;
                drawingState.Add(smileyFaceObject, track[i], i+1);
                report = ReportSubManager<TVertexAttributes, TEdgeAttributes>.Instance.AggregateSearchingReport(drawingState, userModel);
                if(report is not null && progress is not null) progress.Report(report);
            }
            drawnFacePaths.MergeWith( new SmileyFacePath<TVertexAttributes, TEdgeAttributes>(track[i].Start, track[i].Finish));
        }
        Thread.Sleep(drawingDuration/2);
        return drawnFacePaths;
    }
}