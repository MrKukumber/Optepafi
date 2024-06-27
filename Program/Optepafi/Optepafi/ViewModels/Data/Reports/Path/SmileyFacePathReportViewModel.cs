using Optepafi.Models.Graphics.Sources;
using Optepafi.Models.ReportMan.Reports.Path;
using Optepafi.ViewModels.Data.Graphics;

namespace Optepafi.ViewModels.Data.Reports.Path;

/// <summary>
/// ViewModel for <c>SmileyFacePathReport</c> type.
/// There should exists appropriate convertor of <c>SmileyFacePathReport</c> to this ViewModel type.
/// For more information on path report ViewModels see <see cref="PathReportViewModel"/>.
/// </summary>
/// <param name="pathGraphicsSouce">Graphics of smiley face path.</param>
/// <param name="horizontallySquishedFacesCountInfo">Textual information about count of horizontally squished faces.</param>
/// <param name="verticallySquishedFacesCountInfo">Textual information about count of vertically squished faces.</param>
/// <param name="notSquishedFacesCountInfo">Textual information about count of faces which are not squished at all.</param>
public class SmileyFacePathReportViewModel(GraphicsSourceViewModel pathGraphicsSouce, string? horizontallySquishedFacesCountInfo, string? verticallySquishedFacesCountInfo, string? notSquishedFacesCountInfo) : PathReportViewModel
{
    /// <summary>
    /// Graphics of smiley face path. It is used for displaying of drawn smiley faces.
    /// </summary>
    public override GraphicsSourceViewModel GraphicsSource { get; } = pathGraphicsSouce;
    /// <summary>
    /// Textual information about count of horizontally squished faces.
    /// </summary>
    public string? HorizontallySquishedFacesCountInfo { get; } = horizontallySquishedFacesCountInfo;
    /// <summary>
    /// Textual information about count of vertically squished faces.
    /// </summary>
    public string? VerticallySquishedFacesCountInfo { get; } = verticallySquishedFacesCountInfo;
    /// <summary>
    /// Textual information about count of faces which are not squished at all.
    /// </summary>
    public string? NotSquishedFacesCountInfo { get; } = notSquishedFacesCountInfo;
}