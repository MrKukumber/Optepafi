using Optepafi.Models.MapMan;
using Optepafi.Models.Utils;
using Optepafi.ViewModels.Data.Graphics;

namespace Optepafi.ModelViews.Converters2Vm.Graphics;

/// <summary>
/// Represents converter of graphic object of specific type to its ViewModel.
///
/// These ViewModels can be than presented to View for displaying to user.  
/// It has one non-generic predecessor, which should be never implemented right away. It is used mainly as type parameter of collections of convertors. This interface should be always implemented instead.  
/// To be visible to applications logic, converters instance must be included in some collection of converters which application can use for searching of correct one. There is bunch of dictionaries, which are assembled in tree structure, where root dictionary is contained in static class <see cref="GraphicObjects2VmConverters"/>. Application directly uses this dictionary.  
/// </summary>
/// <typeparam name="TGraphicsObject">Type of graphic object for which is converter able to create ViewModel.</typeparam>
public interface IGraphicObjects2VmConverter<in TGraphicsObject> : IGraphicObjects2VmConverter
{
    /// <summary>
    /// Method for converting graphic object to ViewModel.
    /// 
    /// It retrieves object to be converted to Vm and coordinate of maps left-bottom vertex for correct positioning on canvas.  
    /// Method should convert graphic object into corresponding ViewModel type.  
    /// </summary>
    /// <param name="graphicsObject">Graphic object to be converted to ViewModel.</param>
    /// <param name="mapsTopLeftVertex">Coordinate of maps lef-bottom vertex used for correct positioning of graphic object on canvas.</param>
    /// <returns>Corresponding ViewModel to provided graphic object.</returns>
    public GraphicObjectViewModel ConvertToViewModel(TGraphicsObject graphicsObject, MapCoordinates mapsTopLeftVertex);
}

/// <summary>
/// Non-generic predecessor of <see cref="IGraphicObjects2VmConverter{TGraphicsObject}"/>.
///
/// By its design it is suited for being used as type parameter in collections of converters.  
/// It should never be implemented right away. Its generic successor should be implemented instead.  
/// </summary>
public interface IGraphicObjects2VmConverter;