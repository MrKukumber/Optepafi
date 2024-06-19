using System;
using System.Collections.Generic;
using Optepafi.Models.Graphics.Sources;
using Optepafi.Models.GraphicsMan;
using Optepafi.Models.ReportMan;
using Optepafi.Models.ReportMan.Reports;
using Optepafi.ModelViews.Converters2Vm.Reports.Path;
using Optepafi.ViewModels.Data.Graphics;
using ReactiveUI;

namespace Optepafi.ViewModels.Data.Reports;

public abstract class PathReportViewModel : ViewModelBase, IReportWithGraphicsViewModel
{
    public abstract GraphicsSourceViewModel GraphicsSource { get; }

    public static PathReportViewModel? Construct(IPathReport pathReport, IGroundGraphicsSource relatedMapGraphics)
    {
        return Constructor.Instance.ConstructReport(pathReport, relatedMapGraphics);
    }
    
    private class Constructor : IPathReportGenericVisitor<PathReportViewModel?, IGroundGraphicsSource>
    {
        public static Constructor Instance { get; } = new();
        private Constructor(){}

        private Dictionary<Type, IPathReport2VmConverter> _converters = PathReports2VmConverters.Converters;
        
        public PathReportViewModel? ConstructReport(IPathReport pathReport, IGroundGraphicsSource relatedMapGraphics)
        {
            return pathReport.AcceptGeneric(this, relatedMapGraphics);
        }
        
        PathReportViewModel? IPathReportGenericVisitor<PathReportViewModel?, IGroundGraphicsSource>.GenericVisit<TPathReport>(TPathReport pathReport, IGroundGraphicsSource relatedMapGraphics)
        {
            if (_converters[typeof(TPathReport)] is IPathReport2VmConverter<TPathReport> pathReport2VmConverter)
            {
                return pathReport2VmConverter.ConvertToViewModel(pathReport, relatedMapGraphics);
            }
            //TODO: log chybajuceho convertoru
            return null;
        }
    }
}