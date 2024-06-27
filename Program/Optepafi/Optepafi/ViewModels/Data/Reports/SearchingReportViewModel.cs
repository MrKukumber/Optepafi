using System;
using System.Collections.Generic;
using Optepafi.Models.Graphics.Sources;
using Optepafi.Models.GraphicsMan;
using Optepafi.Models.ReportMan;
using Optepafi.Models.ReportMan.Reports;
using Optepafi.ModelViews.Converters2Vm.Reports.Searching;
using Optepafi.ViewModels.Data.Graphics;
using Optepafi.ViewModels.DataViewModels;
using ReactiveUI;

namespace Optepafi.ViewModels.Data.Reports;

/// <summary>
/// Predecessor for every searching report ViewModel.
/// It uses factory design pattern for creation of appropriate ViewModel for inserted searching report.
/// It contains dictionary of <c>ISearchingReport2VmConverter</c>s which are used for creation of appropriate ViewModel for provided searching report. 
/// For more information on data view models see <see cref="DataViewModel"/>.
/// </summary>
public abstract class SearchingReportViewModel : GraphicsContainingDataViewModel
{
    /// <summary>
    /// Static factory method that lets constructor create a appropriate ViewModel for inserted searching report.
    /// Provided ground graphics is used for correct conversion of potential graphics source in report to its ViewModel.
    /// Implementation is done by private Constructor class which is able to correctly implement "generic visitor pattern" on provided searching report. 
    /// </summary>
    /// <param name="searchingReport">Searching report for which ViewModel is to be created.</param>
    /// <param name="associatedMapGraphics">Ground graphic source of associated map used for correct conversion of potential graphics source in report.</param>
    /// <returns>Correct version of searching reports ViewModel.</returns>
    public static SearchingReportViewModel? Construct(ISearchingReport searchingReport, IGroundGraphicsSource associatedMapGraphics)
    {
        return Constructor.Instance.ConstructReport(searchingReport,associatedMapGraphics);
    }

    /// <summary>
    /// Singleton class used in factory design pattern for correct construction of ViewModel for provided searching report.
    /// </summary>
    private class Constructor : ISearchingReportGenericVisitor<SearchingReportViewModel?, IGroundGraphicsSource>
    {
        public static Constructor Instance { get; } = new();
        private Constructor(){}

        /// <summary>
        /// Dictionary of converters of searching reports to ViewModels. Each converter is saved under type of searching report, which it converts.
        /// </summary>
        private Dictionary<Type, ISearchingReport2VmConverter> _converters = SearchingReports2VmConverters.Converters;
        
        /// <summary>
        /// Method for constructing of ViewModel for provided searching report by using so called "generic visitor pattern" on searching report.
        /// The generic visitor pattern will reveal real type of searching report so then appropriate converter can be chosen to handle reports conversion to ViewModel.
        /// For more information on generic visitor pattern see <see cref="ISearchingReportGenericVisitor{TOut,TOtherParams}"/>.
        /// Provided ground graphics is used for correct conversion of potential graphics source in report to its ViewModel.
        /// </summary>
        /// <param name="searchingReport">Searching report to be converted to its ViewModel.</param>
        /// <param name="associatedMapGraphics">Ground graphic source of associated map used for correct conversion of potential graphics source in report.</param>
        /// <returns>Correct version of searching reports ViewModel.</returns>
        public SearchingReportViewModel? ConstructReport(ISearchingReport searchingReport, IGroundGraphicsSource associatedMapGraphics)
        {
            return searchingReport.AcceptGeneric(this, associatedMapGraphics);
        }
        SearchingReportViewModel? ISearchingReportGenericVisitor<SearchingReportViewModel?, IGroundGraphicsSource>.GenericVisit<TSearchingReport>(TSearchingReport searchingReport, IGroundGraphicsSource associatedMapGraphics)
        {
            if (_converters[typeof(TSearchingReport)] is ISearchingReport2VmConverter<TSearchingReport> searchingReport2VmConverter)
            {
                return searchingReport2VmConverter.ConvertToViewModel(searchingReport, associatedMapGraphics);
            }
            //TODO: log chybajuceho convertoru
            return null;
        }
    }
}