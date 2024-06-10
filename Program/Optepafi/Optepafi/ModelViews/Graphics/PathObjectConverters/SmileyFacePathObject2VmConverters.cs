using System;
using System.Collections.Generic;
using Optepafi.Models.Graphics.Objects.Path;
using Optepafi.Models.MapMan;
using Optepafi.ViewModels.Data.Graphics;
using Optepafi.ViewModels.Data.Graphics.PathObjects;

namespace Optepafi.ModelViews.Graphics.PathObjectConverters;

public class SmileyFacePathObject2VmConverters
{
    public static Dictionary<Type, IGraphicObjects2VMConverter> AllConverters = new()
    {
        [typeof(SmileyFaceObject)] = SmileyFaceObject2VmConverter.Instance
    };
}

public class SmileyFaceObject2VmConverter: IGraphicObjects2VmConverter<SmileyFaceObject>
{
    public static SmileyFaceObject2VmConverter Instance { get; } = new();
    private SmileyFaceObject2VmConverter(){}
    public GraphicObjectViewModel ConvertToViewModel(SmileyFaceObject graphicsObject, int minimalXPosition, int minimalYPosition)
    {
        return new SmileyFaceObjectViewModel(
            graphicsObject.Nose.center.XPos - minimalXPosition, graphicsObject.Nose.center.YPos - minimalYPosition,
            (graphicsObject.Eye1.center - graphicsObject.Nose.center, graphicsObject.Eye1.radius),
            (graphicsObject.Eye2.center - graphicsObject.Nose.center, graphicsObject.Eye2.radius),
            (new MapCoordinate(0, 0), graphicsObject.Nose.radius),
            (graphicsObject.Mouth.Item1 - graphicsObject.Nose.center, graphicsObject.Mouth.Item2 - graphicsObject.Nose.center,
                graphicsObject.Mouth.Item3 - graphicsObject.Nose.center, graphicsObject.Mouth.Item4 - graphicsObject.Nose.center));
    }
}
