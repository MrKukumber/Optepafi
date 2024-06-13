using System;
using System.Collections.Generic;
using Optepafi.Models.Graphics.Sources;
using Optepafi.Models.GraphicsMan;
using Optepafi.Models.ReportMan;
using Optepafi.ModelViews.Converters2Vm.Reports.Searching;
using Optepafi.ViewModels.Data.Graphics;
using ReactiveUI;

namespace Optepafi.ViewModels.Data.Reports;

public abstract class SearchingReportViewModel : ReactiveObject
{
    public abstract GraphicsSourceViewModel? SearchingGraphics { get; }

    public static SearchingReportViewModel? Construct(ISearchingReport searchingReport, IGroundGraphicsSource relatedMapGraphics)
    {
        return Constructor.Instance.ConstructReport(searchingReport,relatedMapGraphics);
    }

    private class Constructor : ISearchingReportGenericVisitor<SearchingReportViewModel?, IGroundGraphicsSource>
    {
        public static Constructor Instance { get; } = new();
        private Constructor(){}

        private Dictionary<Type, ISearchingReport2VmConverter> _converters = SearchingReports2VmConverters.Converters;
        
        public SearchingReportViewModel? ConstructReport(ISearchingReport searchingReport, IGroundGraphicsSource relatedMapGraphics)
        {
            return searchingReport.AcceptGeneric(this, relatedMapGraphics);
        }
        
        SearchingReportViewModel? ISearchingReportGenericVisitor<SearchingReportViewModel?, IGroundGraphicsSource>.GenericVisit<TSearchingReport>(TSearchingReport searchingReport, IGroundGraphicsSource relatedMapGraphics)
        {
            if (_converters[typeof(TSearchingReport)] is ISearchingReport2VmConverter<TSearchingReport> searchingReport2VmConverter)
            {
                return searchingReport2VmConverter.ConvertToViewModel(searchingReport, relatedMapGraphics);
            }
            //TODO: log chybajuceho convertoru
            return null;
        }
    }
}