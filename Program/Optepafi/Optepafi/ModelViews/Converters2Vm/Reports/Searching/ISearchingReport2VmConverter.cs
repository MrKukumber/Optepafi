using Optepafi.Models.GraphicsMan;
using Optepafi.Models.GraphicsMan.Sources;
using Optepafi.Models.ReportMan;
using Optepafi.Models.ReportMan.Reports;
using Optepafi.ViewModels.Data.Reports;

namespace Optepafi.ModelViews.Converters2Vm.Reports.Searching;

/// <summary>
/// Represents converter of searching report of specific type to its ViewModel.These ViewModels can be than presented to View for displaying to user.
/// It has one non generic predecessor, which should be never implemented right away. It is used mainly as type parameter of collections of convertors. This interface should be always implemented instead.
/// To be visible to applications logic, converters instance must be included in some collection of converters which application can use for searching of correct one. There is bunch of dictionaries, which are assembled in tree structure, where root dictionary is contained in static class <see cref="SearchingReports2VmConverters"/>. Application directly uses this dictionary.
/// </summary>
/// <typeparam name="TSearchingReport">Type of searching report for which is converter able to create ViewModel.</typeparam>
public interface ISearchingReport2VmConverter<in TSearchingReport> : ISearchingReport2VmConverter
    where TSearchingReport : ISearchingReport
{
    /// <summary>
    /// Method for converting searching report to ViewModel. It retrieves report to be converted to Vm and ground graphic source to which report (or included graphics in it) is related.
    /// Method should convert searching report into corresponding ViewModel type.
    /// </summary>
    /// <param name="searchingReport">Searching report to be converted to ViewModel.</param>
    /// <param name="relatedMapGraphics">Ground graphic source to which report (or included graphics in it) is relative to. It can be used for correct positioning on canvas of graphics included in report.</param>
    /// <returns>Corresponding ViewModel to provided searching report.</returns>
    public SearchingReportViewModel ConvertToViewModel(TSearchingReport searchingReport, IGroundGraphicsSource relatedMapGraphics);
}

/// <summary>
/// Non-generic predecessor of <see cref="ISearchingReport2VmConverter{TGraphicsObject}"/>. By its design it is suited for being used as type parameter in collections of converters.
/// It should never be implemented right away. Its generic successor should be implemented instead. 
/// </summary>
public interface ISearchingReport2VmConverter;
