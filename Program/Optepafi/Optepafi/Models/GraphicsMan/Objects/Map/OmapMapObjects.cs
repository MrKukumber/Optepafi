
using Optepafi.Models.SearchingAlgorithmMan;
using Optepafi.Models.Utils;
using Optepafi.Models.Utils.Shapes;

namespace Optepafi.Models.GraphicsMan.Objects.Map;

//TODO: comment
public record Contour_101(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) => genericVisitor.GenericVisit(this);
}

public record SlopeLine_101_1(MapCoordinates Position, float Rotation) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) => genericVisitor.GenericVisit(this);
}

public record IndexContour_102(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);

    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}

// public record ContourValue_102_1(MapCoordinates Position) : IGraphicObject
// {
    // /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    // public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        // TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);

    // /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    // public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        // genericVisitor.GenericVisit(this);
// }

public record FormLine_103(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);

    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}

public record SlopeLineFormLine_103_1(MapCoordinates Position, float Rotation) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);

    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}

public record EarthBank_104(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);

    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}

public record EarthBankMinSize_104_1(MapCoordinates Position, float Rotation) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);

    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}

public record EarthBankTopLine_104_2(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);

    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}

public record EarthBankTagLine_104_3(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);

    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}

public record EarthWall_105(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);

    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}

public record RuinedEarthWall_106(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);

    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}

public record ErosionGully_107(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);

    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}

public record SmallErosionGully_108(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);

    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}

public record SmallKnoll_109(MapCoordinates Position) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);

    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}

public record SmallEllongatedKnoll_110(MapCoordinates Position, float Rotation) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);

    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}

public record SmallDepression_111(MapCoordinates Position) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);

    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}

public record Pit_112(MapCoordinates Position) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}

public record BrokenGround_113(Polygon Shape, float Rotation) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}

public record BrokenGroundIndividualDot_113_1 (MapCoordinates Position): IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}

public record VeryBrokenGround_114(Polygon Shape, float Rotation): IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}

public record ProminentLandformFeature_115(MapCoordinates Position) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}

public record ImpassableCliff_201(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record ImpassableCliffMinSize_201_1(MapCoordinates Position, float Rotation) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record ImpassableCliffTopLine_201_3(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record ImpassableCliffTagLine_201_4(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record Cliff_202(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record CliffMinSize_202_1(MapCoordinates Position, float Rotation) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record CliffWithTags_202_2(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record CliffWithTagsMinSize_202_3(MapCoordinates Position, float Rotation) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record RockyPitOrCave_203_1_2(MapCoordinates Position, float Rotation) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record Boulder_204(MapCoordinates Position) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record BoulderOrLargeBoulder_204_5(MapCoordinates Position) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record LargeBoulder_205(MapCoordinates Position) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record GiganticBoulder_206(Polygon Shape): IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record BoulderCluster_207(MapCoordinates Position) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record BoulderClusterLarge_207_1(MapCoordinates Position) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record BoulderField_208(Polygon Shape, float Rotation): IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}

public record BoulderFieldSingleTriangle_208_1(MapCoordinates Position, float Rotation) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record BoulderFieldSingleTriangleEnlarged_208_2(MapCoordinates Position, float Rotation) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record DenseBoulderField_209(Polygon Shape, float Rotation) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record StonyGroundSlowRunning_210(Polygon Shape, float Rotation) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record StonyGroundIndividualDot_210_1(MapCoordinates Position) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record StonyGroundWalk_211(Polygon Shape, float Rotation) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record StonyGroundFight_212(Polygon Shape, float Rotation) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record SandyGround_213(Polygon Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record BareRock_214(Polygon Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record Trench_215(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
// public record UncrossableBodyOfWaterFullColWithBankLine_301(Polygon Shape) : IGraphicObject
// {
    // /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    // public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        // TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    // /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    // public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        // genericVisitor.GenericVisit(this);
// }
public record UncrossableBodyOfWaterFullCol_301_1(Polygon Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
// public record UncrossableBodyOfWaterDominantWithBankLine_301_2(Polygon Shape) : IGraphicObject
// {
    // /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    // public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        // TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    // /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    // public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        // genericVisitor.GenericVisit(this);
// }
public record UncrossableBodyOfWaterDominant_301_3(Polygon Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record UncrossableBodyOfWaterBankLine_301_4(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
// public record ShallowBodyOfWaterWithSolidOutline_302(Polygon Shape) : IGraphicObject
// {
    // /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    // public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        // TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    // /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    // public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        // genericVisitor.GenericVisit(this);
// }
public record ShallowBodyOfWater_302_1(Polygon Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record ShallowBodyOfWaterSolidOutline_302_2(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record ShallowBodyOfWaterDashedOutline_302_3(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record SmallShallowBodyOfWater_302_5(Polygon Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record WaterHole_303(MapCoordinates Position) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record CrossableWatercourse_304(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record SmallCrossableWatercourse_305(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record SeasonalWaterChannel_306(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
// public record UncrossableMarshWithOutline_307(Polygon Shape) : IGraphicObject
// {
    // /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    // public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        // TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    // /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    // public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        // genericVisitor.GenericVisit(this);
// }
public record UncrossableMarsh_307_1(Polygon Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record UncrossableMarshOutline_307_2(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record Marsh_308(Polygon Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record MarshMinSize_308_1(MapCoordinates Position) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record NarrowMarsh_309(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record IndistinctMarsh_310(Polygon Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record IndistinctMarshMinSize_310_1(MapCoordinates Position) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record WellFountainWaterTank_311(MapCoordinates Position) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}

public record Spring_312(MapCoordinates Position) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record ProminentWaterFeature_313(MapCoordinates Position) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record OpenLand_401(Polygon Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record OpenLandWithTrees_402(Polygon Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record OpenLandWithBushes_402_1(Polygon Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record RoughOpenLand_403(Polygon Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record RoughOpenLandWithTrees_404(Polygon Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record RoughOpenLandWithBushes_404_1(Polygon Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record Forest_405(Polygon Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record VegetationSlow_406(Polygon Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record VegetationSlowNormalOneDir_406_1(Polygon Shape, float Rotation): IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record VegetationSlowGoodVisibility_407(Polygon Shape): IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record VegetationWalk_408(Polygon Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record VegetationWalkNormalOneDir_408_1(Polygon Shape, float Rotation) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record VegetationWalkSlowOneDir_408_2(Polygon Shape, float Rotation) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record VegetationWalkGoodVisibility_409(Polygon Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}

public record VegetationFight_410(Polygon Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record VegetationFightNormalOneDir_410_1(Polygon Shape, float Rotation) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record VegetationFightSlowOneDir_410_2(Polygon Shape, float Rotation) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record VegetationFightWalkOneDir_410_3(Polygon Shape, float Rotation) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record VegetationFightMinWidth_410_4(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
// public record CultivatedLand_412(Polygon Shape) : IGraphicObject
// {
    // /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    // public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        // TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    // /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    // public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        // genericVisitor.GenericVisit(this);
// }
public record CultivatedLandBlackPattern_412_1(Polygon Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record Orchard_413(Polygon Shape, float Rotation) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record OrchardRoughOpenLand_413_1(Polygon Shape, float Rotation) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record Vineyard_414(Polygon Shape, float Rotation) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record VineyardRoughOpenLand_414_1(Polygon Shape, float Rotation) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record DistinctCultivationBoundary_415(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record DistinctVegetationBoundary_416(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record DistinctVegetationBoundaryGreenDashedLine_416_1(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record ProminentLargeTree_417(MapCoordinates Position) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record ProminentBush_418(MapCoordinates Position) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record ProminentVegetationFeature_419(MapCoordinates Position) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
// public record PavedAreaWithBoundingLine_501(Polygon Shape) : IGraphicObject
// {
    // /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    // public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        // TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    // /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    // public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        // genericVisitor.GenericVisit(this);
// }
public record PavedArea_501_1(Polygon Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record PavedAreaBoundingLine_501_2(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record WideRoadMinWidth_502(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record RoadWithTwoCarriageways_502_2(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record Road_503(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record VehicleTrack_504(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record Footpath_505(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record SmallFootpath_506(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record LessDistinctSmallFootpath_507(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record NarrowRide_508(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record NarrowRideEasy_508_1(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record NarrowRideNormal_508_2(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record NarrowRideSlow_508_3(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record NarrowRideWalk_508_4(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record Railway_509(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record PowerLine_510(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record MajorPowerLineMinWidth_511(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record MajorPowerLine_511_1(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record MajorPowerLineLargeCarryingMasts_511_2(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record BridgeTunnel_512(MapCoordinates Position, float Rotation) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record BridgeTunnelMinSize_512_1(MapCoordinates Position, float Rotation) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record FootBridge_512_2(MapCoordinates Position, float Rotation) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record Wall_513(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record RuinedWall_514(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record ImpassableWall_515(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record Fence_516(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record RuinedFence_517(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record ImpassableFence_518(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record CrossingPoint_519(MapCoordinates Position, float Rotation) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record NotEnterableArea_520(Polygon Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record NotEnterableAreaSolidColourBoundingLine_520_1(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record NotEnterableAreaStripes_502_2(Polygon Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record NotEnterableAreaStripesBoundingLine_520_3(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record Building_521(Polygon Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record BuildingMinSize_521_1(MapCoordinates Position, float Rotation) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
// public record LargeBuildingWithOutline_521_2(Polygon Shape) : IGraphicObject
// {
    // /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    // public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        // TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    // /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    // public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        // genericVisitor.GenericVisit(this);
// }
public record LargeBuilding_521_3(Polygon Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record LargeBuildingOutline_521_4(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
// public record CanopyWithOutline_522(Polygon Shape) : IGraphicObject
// {
    // /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    // public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        // TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    // /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    // public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        // genericVisitor.GenericVisit(this);
// }
public record Canopy_522_1(Polygon Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record CanopyOutline_522_2(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record Ruin_523(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record RuinMinSize_523_1(MapCoordinates Position, float Rotation) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record HighTower_524(MapCoordinates Position) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record SmallTower_525(MapCoordinates Position) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record Cairn_526(MapCoordinates Position) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record FodderRack_527(MapCoordinates Position) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record ProminentLineFeature_528(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record ProminentImpassableLineFeature_529(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record ProminentManMadeFeatureRing_530(MapCoordinates Position) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record ProminentManMadeFeatureX_531(MapCoordinates Position) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record Stairway_532(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record StairWayWithoutBorderLines_532_1(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record MagneticNorthLine_601_1(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record NorthLinesPattern_601_2(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record MagneticNorthLineBlue_601_3(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record NorthLinesPatternBlue_601_4(Utils.Shapes.Path Shape) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record RegistrationMark_602(MapCoordinates Position) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
public record SpotHeightDot_603(MapCoordinates Position) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        genericVisitor.GenericVisit(this);
}
// record SpotHeightText_603_1(MapCoordinates Position) : IGraphicObject
// {
    // /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    // public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        // TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    // /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    // public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        // genericVisitor.GenericVisit(this);
// }

public record Start_701(MapCoordinates Position, float Rotation) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/> 
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/> 
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) => genericVisitor.GenericVisit(this);
}

public record ControlPoint_703(MapCoordinates Position) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/> 
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/> 
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) => genericVisitor.GenericVisit(this);
}
// record ControlNumber_704(MapCoordinates Position) : IGraphicObject
// {
    // /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    // public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        // TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    // /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    // public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        // genericVisitor.GenericVisit(this);
// }

public record CourseLine_705(Utils.Shapes.Path Shape) : IGraphicObject 
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/> 
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/> 
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) => genericVisitor.GenericVisit(this);
}

public record Finish_706(MapCoordinates Position) : IGraphicObject
{
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/> 
    public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/> 
    public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) => genericVisitor.GenericVisit(this);
}

// public record SimpleOrienteeringCourse_799(Utils.Shapes.Path Shape) : IGraphicObject
// {
    // /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut,TOtherParams}"/>
    // public TOut AcceptGeneric<TOut, TOtherParams>(IGraphicObjectGenericVisitor<TOut, TOtherParams> genericVisitor,
        // TOtherParams otherParams) => genericVisitor.GenericVisit(this, otherParams);
    // /// <inheritdoc cref="IGraphicObject.AcceptGeneric{TOut}"/>
    // public TOut AcceptGeneric<TOut>(IGraphicObjectGenericVisitor<TOut> genericVisitor) =>
        // genericVisitor.GenericVisit(this);
// }

    