using System;
using System.Collections.Generic;
using Optepafi.Models.GraphicsMan.Sources;
using Optepafi.Models.ReportMan.Reports;
using Optepafi.ModelViews.Converters2Vm.Reports.Path;

namespace Optepafi.ViewModels.Data.Reports;

/// <summary>
/// Predecessor for every path report ViewModel.
/// 
/// It uses factory design pattern for creation of appropriate ViewModel for inserted path report.  
/// It contains dictionary of <c>IPathReport2VmConverter</c>s which are used for creation of appropriate ViewModel for provided path report.  
/// For more information on data view models see <see cref="DataViewModel"/>.
/// </summary>
public abstract class PathReportViewModel : GraphicsContainingDataViewModel
{
    
    /// <summary>
    /// Static factory method that lets constructor create a appropriate ViewModel for inserted path report.
    /// 
    /// Provided ground graphics is used for correct conversion of potential graphics source in report to its ViewModel.  
    /// Implementation is done by private Constructor class which is able to correctly implement "generic visitor pattern" on provided path report.  
    /// </summary>
    /// <param name="pathReport">Path report for which ViewModel is to be created.</param>
    /// <param name="associatedMapGraphics">Ground graphic source of associated map used for correct conversion of potential graphics source in report.</param>
    /// <returns>Correct version of path reports ViewModel.</returns>
    public static PathReportViewModel? Construct(IPathReport pathReport, IGroundGraphicsSource associatedMapGraphics)
    {
        return Constructor.Instance.ConstructReport(pathReport, associatedMapGraphics);
    }
    
    /// <summary>
    /// Singleton class used in factory design pattern for correct construction of ViewModel for provided path report.
    /// </summary>
    private class Constructor : IPathReportGenericVisitor<PathReportViewModel?, IGroundGraphicsSource>
    {
        public static Constructor Instance { get; } = new();
        private Constructor(){}
        
        /// <summary>
        /// Dictionary of converters of path reports to ViewModels. Each converter is saved under type of path report, which it converts.
        /// </summary>
        private Dictionary<Type, IPathReport2VmConverter> _converters = PathReports2VmConverters.Converters;
        
        
        /// <summary>
        /// Method for constructing of ViewModel for provided path report by using so called "generic visitor pattern" on path report.
        /// 
        /// The generic visitor pattern will reveal real type of report so then appropriate converter can be chosen to handle reports conversion to ViewModel.  
        /// For more information on generic visitor pattern see <see cref="IPathReportGenericVisitor{TOut,TOtherParams}"/>.  
        /// Provided ground graphics is used for correct conversion of potential graphics source in report to its ViewModel.  
        /// </summary>
        /// <param name="pathReport">Path report to be converted to its ViewModel.</param>
        /// <param name="associatedMapGraphics">Ground graphic source of associated map used for correct conversion of potential graphics source in report.</param>
        /// <returns>Correct version of path reports ViewModel.</returns>
        public PathReportViewModel? ConstructReport(IPathReport pathReport, IGroundGraphicsSource associatedMapGraphics)
        {
            return pathReport.AcceptGeneric(this, associatedMapGraphics);
        }
        
        PathReportViewModel? IPathReportGenericVisitor<PathReportViewModel?, IGroundGraphicsSource>.GenericVisit<TPathReport>(TPathReport pathReport, IGroundGraphicsSource associatedMapGraphics)
        {
            if (_converters[typeof(TPathReport)] is IPathReport2VmConverter<TPathReport> pathReport2VmConverter)
            {
                return pathReport2VmConverter.ConvertToViewModel(pathReport, associatedMapGraphics);
            }
            //TODO: log chybajuceho convertoru
            return null;
        }
    }
}