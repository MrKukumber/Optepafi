using Optepafi.Models.Graphics.Sources;
using Optepafi.Models.GraphicsMan;
using Optepafi.Models.ReportMan;
using Optepafi.Models.ReportMan.Reports;
using Optepafi.ViewModels.Data.Reports;

namespace Optepafi.ModelViews.Converters2Vm.Reports.Path;

/// <summary>
/// Represents converter of path report of specific type to its ViewModel. These ViewModels can be than presented to View for displaying to user.
/// It has one non generic predecessor, which should be never implemented right away. It is used mainly as type parameter of collections of convertors. This interface should be always implemented instead.
/// To be visible to applications logic, converters instance must be included in some collection of converters which application can use for searching of correct one. There is bunch of dictionaries, which are assembled in tree structure, where root dictionary is contained in static class <see cref="PathReports2VmConverters"/>. Application directly uses this dictionary.
/// </summary>
/// <typeparam name="TPathReport">Type of path report for which is converter able to create ViewModel.</typeparam>
public interface IPathReport2VmConverter<in TPathReport> : IPathReport2VmConverter
    where TPathReport : IPathReport
{
    
    /// <summary>
    /// Method for converting path report to ViewModel. It retrieves report to be converted to Vm and ground graphic source to which report (or included graphics in it) is related.
    /// Method should convert path report into corresponding ViewModel type.
    /// </summary>
    /// <param name="pathReport">Path report to be converted to ViewModel.</param>
    /// <param name="relatedMapGraphics">Ground graphic source to which report (or included graphics in it) is relative to. It can be used for correct positioning on canvas of graphics included in report.</param>
    /// <returns>Corresponding ViewModel to provided path report.</returns>
    public PathReportViewModel ConvertToViewModel(TPathReport pathReport, IGroundGraphicsSource relatedMapGraphics);
}

/// <summary>
/// Non-generic predecessor of <see cref="IPathReport2VmConverter{TGraphicsObject}"/>. By its design it is suited for being used as type parameter in collections of converters.
/// It should never be implemented right away. Its generic successor should be implemented instead. 
/// </summary>
public interface IPathReport2VmConverter;