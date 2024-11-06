
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Optepafi.Models.GraphicsMan.Objects.Map;
using Optepafi.Models.Utils;
using Optepafi.Models.Utils.Shapes;
using Optepafi.ModelViews.Utils;
using Optepafi.ViewModels.Data.Shapes;

namespace Optepafi.ViewModels.Data.Graphics.MapObjects;

//TODO: comment
public class Contour_101_ViewModel(Contour_101 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 28;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}

public class SlopeLine_101_1_ViewModel(SlopeLine_101_1 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Position.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 26;
    public float Rotation { get; } = obj.Rotation;
}

public class IndexContour_102_ViewModel(IndexContour_102 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 28;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}

public class FormLine_103_ViewModel(FormLine_103 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 28;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);

}

public class SlopeLineFormLine_103_1_ViewModel(SlopeLineFormLine_103_1 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Position.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 28;
    public float Rotation { get; } = obj.Rotation;
}

public class EarthBank_104_ViewModel(EarthBank_104 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 28;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}

public class EarthBankMinSize_104_1_ViewModel(EarthBankMinSize_104_1 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; }= obj.Position.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 28;
    public float Rotation { get; } = obj.Rotation;
}

public class EarthBankTopLine_104_2_ViewModel(EarthBankTopLine_104_2 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 28;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}

public class EarthBankTagLine_104_3_ViewModel(EarthBankTagLine_104_3 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 28;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);

}

public class EarthWall_105_ViewModel(EarthWall_105 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 28;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);

}

public class RuinedEarthWall_106_ViewModel(RuinedEarthWall_106 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 28;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);

}

public class ErosionGully_107_ViewModel(ErosionGully_107 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 28;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);

}

public class SmallErosionGully_108_ViewModel(SmallErosionGully_108 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 28;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);

}

public class SmallKnoll_109_ViewModel(SmallKnoll_109 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; }= obj.Position.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 28;
}

public class SmallEllongatedKnoll_110_ViewModel(SmallEllongatedKnoll_110 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; }= obj.Position.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 28;
    public float Rotation { get; } = obj.Rotation;
}

public class SmallDepression_111_ViewModel(SmallDepression_111 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; }= obj.Position.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 28;

}

public class Pit_112_ViewModel(Pit_112 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; }= obj.Position.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 28;

}

public class BrokenGround_113_ViewModel(BrokenGround_113 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.Segments.Last().LastPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 28;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
    public float Rotation { get; } = obj.Rotation;
}

public class BrokenGroundIndividualDot_113_1_ViewModel(BrokenGroundIndividualDot_113_1 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; }= obj.Position.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 28;
}

public class VeryBrokenGround_114_ViewModel(VeryBrokenGround_114 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.Segments.Last().LastPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 28;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
    public float Rotation { get; } = obj.Rotation;
}

public class ProminentLandformFeature_115_ViewModel(ProminentLandformFeature_115 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; }= obj.Position.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 28;
}

public class ImpassableCliff_201_ViewModel(ImpassableCliff_201 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class ImpassableCliffMinSize_201_1_ViewModel(ImpassableCliffMinSize_201_1 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; }= obj.Position.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public float Rotation { get; } = obj.Rotation;
}
public class ImpassableCliffTopLine_201_3_ViewModel(ImpassableCliffTopLine_201_3 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class ImpassableCliffTagLine_201_4_ViewModel(ImpassableCliffTagLine_201_4 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class Cliff_202_ViewModel(Cliff_202 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class CliffMinSize_202_1_ViewModel(CliffMinSize_202_1 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; }= obj.Position.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public float Rotation { get; } = obj.Rotation;
}
public class CliffWithTags_202_2_ViewModel(CliffWithTags_202_2 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class CliffWithTagsMinSize_202_3_ViewModel(CliffWithTagsMinSize_202_3 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; }= obj.Position.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public float Rotation { get; } = obj.Rotation;
}
public class RockyPitOrCave_203_1_2_ViewModel(RockyPitOrCave_203_1_2 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; }= obj.Position.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public float Rotation { get; } = obj.Rotation;
}
public class Boulder_204_ViewModel(Boulder_204 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; }= obj.Position.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
}
public class BoulderOrLargeBoulder_204_5_ViewModel(BoulderOrLargeBoulder_204_5 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; }= obj.Position.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
}
public class LargeBoulder_205_ViewModel(LargeBoulder_205 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; }= obj.Position.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
}
public class GiganticBoulder_206_ViewModel(GiganticBoulder_206 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.Segments.Last().LastPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class BoulderCluster_207_ViewModel(BoulderCluster_207 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; }= obj.Position.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
}
public class BoulderClusterLarge_207_1_ViewModel(BoulderClusterLarge_207_1 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; }= obj.Position.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
}
public class BoulderField_208_ViewModel(BoulderField_208 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.Segments.Last().LastPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
    public float Rotation { get; } = obj.Rotation;
}

public class BoulderFieldSingleTriangle_208_1_ViewModel(BoulderFieldSingleTriangle_208_1 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; }= obj.Position.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public float Rotation { get; } = obj.Rotation;
}
public class BoulderFieldSingleTriangleEnlarged_208_2_ViewModel(BoulderFieldSingleTriangleEnlarged_208_2 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; }= obj.Position.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public float Rotation { get; } = obj.Rotation;
}
public class DenseBoulderField_209_ViewModel(DenseBoulderField_209 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.Segments.Last().LastPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
    public float Rotation { get; } = obj.Rotation;
}
public class StonyGroundSlowRunning_210_ViewModel(StonyGroundSlowRunning_210 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.Segments.Last().LastPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
    public float Rotation { get; } = obj.Rotation;
}
public class StonyGroundIndividualDot_210_1_ViewModel(StonyGroundIndividualDot_210_1 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; }= obj.Position.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
}
public class StonyGroundWalk_211_ViewModel(StonyGroundWalk_211 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.Segments.Last().LastPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
    public float Rotation { get; } = obj.Rotation;
}
public class StonyGroundFight_212_ViewModel(StonyGroundFight_212 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.Segments.Last().LastPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
    public float Rotation { get; } = obj.Rotation;
}
public class SandyGround_213_ViewModel(SandyGround_213 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.Segments.Last().LastPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 0;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class BareRock_214_ViewModel(BareRock_214 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.Segments.Last().LastPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 10;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class Trench_215_ViewModel(Trench_215 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class UncrossableBodyOfWaterFullCol_301_1_ViewModel(UncrossableBodyOfWaterFullCol_301_1 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.Segments.Last().LastPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 19;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class UncrossableBodyOfWaterDominant_301_3_ViewModel(UncrossableBodyOfWaterDominant_301_3 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.Segments.Last().LastPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 18;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class UncrossableBodyOfWaterBankLine_301_4_ViewModel(UncrossableBodyOfWaterBankLine_301_4 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class ShallowBodyOfWater_302_1_ViewModel(ShallowBodyOfWater_302_1 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.Segments.Last().LastPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 17;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class ShallowBodyOfWaterSolidOutline_302_2_ViewModel(ShallowBodyOfWaterSolidOutline_302_2 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 29;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class ShallowBodyOfWaterDashedOutline_302_3_ViewModel(ShallowBodyOfWaterDashedOutline_302_3 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 29;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class SmallShallowBodyOfWater_302_5_ViewModel(SmallShallowBodyOfWater_302_5 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.Segments.Last().LastPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 19;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class WaterHole_303_ViewModel(WaterHole_303 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; }= obj.Position.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 29;
}
public class CrossableWatercourse_304_ViewModel(CrossableWatercourse_304 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 29;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}

public class SmallCrossableWatercourse_305_ViewModel(SmallCrossableWatercourse_305 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 29;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class SeasonalWaterChannel_306_ViewModel(SeasonalWaterChannel_306 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 29;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class UncrossableMarsh_307_1_ViewModel(UncrossableMarsh_307_1 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.Segments.Last().LastPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 19;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class UncrossableMarshOutline_307_2_ViewModel(UncrossableMarshOutline_307_2 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class Marsh_308_ViewModel(Marsh_308 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.Segments.Last().LastPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 19;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class MarshMinSize_308_1_ViewModel(MarshMinSize_308_1 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; }= obj.Position.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 19;
}
public class NarrowMarsh_309_ViewModel(NarrowMarsh_309 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 29;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class IndistinctMarsh_310_ViewModel(IndistinctMarsh_310 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.Segments.Last().LastPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 19;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class IndistinctMarshMinSize_310_1_ViewModel(IndistinctMarshMinSize_310_1 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; }= obj.Position.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 19;
}
public class WellFountainWaterTank_311_ViewModel(WellFountainWaterTank_311 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; }= obj.Position.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 29;
}

public class Spring_312_ViewModel(Spring_312 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; }= obj.Position.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 29;
}
public class ProminentWaterFeature_313_ViewModel(ProminentWaterFeature_313 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; }= obj.Position.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 29;
}
public class OpenLand_401_ViewModel(OpenLand_401 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.Segments.Last().LastPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 1;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class OpenLandWithTrees_402_ViewModel(OpenLandWithTrees_402 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.Segments.Last().LastPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 1;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class OpenLandWithBushes_402_1_ViewModel(OpenLandWithBushes_402_1 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.Segments.Last().LastPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 1;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class RoughOpenLand_403_ViewModel(RoughOpenLand_403 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.Segments.Last().LastPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 0;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class RoughOpenLandWithTrees_404_ViewModel(RoughOpenLandWithTrees_404 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.Segments.Last().LastPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 0;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class RoughOpenLandWithBushes_404_1_ViewModel(RoughOpenLandWithBushes_404_1 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.Segments.Last().LastPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 0;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class Forest_405_ViewModel(Forest_405 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.Segments.Last().LastPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 12;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class VegetationSlow_406_ViewModel(VegetationSlow_406 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.Segments.Last().LastPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 6;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class VegetationSlowNormalOneDir_406_1_ViewModel(VegetationSlowNormalOneDir_406_1 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.Segments.Last().LastPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 6;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
    public float Rotation { get; } = obj.Rotation;
}
public class VegetationSlowGoodVisibility_407_ViewModel(VegetationSlowGoodVisibility_407 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.Segments.Last().LastPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 5;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class VegetationWalk_408_ViewModel(VegetationWalk_408 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.Segments.Last().LastPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 7;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class VegetationWalkNormalOneDir_408_1_ViewModel(VegetationWalkNormalOneDir_408_1 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.Segments.Last().LastPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 7;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
    public float Rotation { get; } = obj.Rotation;
}
public class VegetationWalkSlowOneDir_408_2_ViewModel(VegetationWalkSlowOneDir_408_2 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.Segments.Last().LastPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 7;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
    public float Rotation { get; } = obj.Rotation;
}
public class VegetationWalkGoodVisibility_409_ViewModel(VegetationWalkGoodVisibility_409 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.Segments.Last().LastPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 5;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}

public class VegetationFight_410_ViewModel(VegetationFight_410 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.Segments.Last().LastPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 8;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class VegetationFightNormalOneDir_410_1_ViewModel(VegetationFightNormalOneDir_410_1 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.Segments.Last().LastPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 8;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
    public float Rotation { get; } = obj.Rotation;
}
public class VegetationFightSlowOneDir_410_2_ViewModel(VegetationFightSlowOneDir_410_2 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.Segments.Last().LastPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 8;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
    public float Rotation { get; } = obj.Rotation;
}
public class VegetationFightWalkOneDir_410_3_ViewModel(VegetationFightWalkOneDir_410_3 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.Segments.Last().LastPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 8;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
    public float Rotation { get; } = obj.Rotation;
}
public class VegetationFightMinWidth_410_4_ViewModel(VegetationFightMinWidth_410_4 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 8;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class CultivatedLandBlackPattern_412_1_ViewModel(CultivatedLandBlackPattern_412_1 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.Segments.Last().LastPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 1;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class Orchard_413_ViewModel(Orchard_413 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.Segments.Last().LastPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 1;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
    public float Rotation { get; } = obj.Rotation;
}
public class OrchardRoughOpenLand_413_1_ViewModel(OrchardRoughOpenLand_413_1 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.Segments.Last().LastPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 0;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
    public float Rotation { get; } = obj.Rotation;
}
public class Vineyard_414_ViewModel(Vineyard_414 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.Segments.Last().LastPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 1;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
    public float Rotation { get; } = obj.Rotation;
}
public class VineyardRoughOpenLand_414_1_ViewModel(VineyardRoughOpenLand_414_1 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.Segments.Last().LastPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 0;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
    public float Rotation { get; } = obj.Rotation;
}
public class DistinctCultivationBoundary_415_ViewModel(DistinctCultivationBoundary_415 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class DistinctVegetationBoundary_416_ViewModel(DistinctVegetationBoundary_416 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class DistinctVegetationBoundaryGreenDashedLine_416_1_ViewModel(DistinctVegetationBoundaryGreenDashedLine_416_1 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 9;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class ProminentLargeTree_417_ViewModel(ProminentLargeTree_417 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; }= obj.Position.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 31;
}
public class ProminentBush_418_ViewModel(ProminentBush_418 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; }= obj.Position.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 31;
}
public class ProminentVegetationFeature_419_ViewModel(ProminentVegetationFeature_419 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; }= obj.Position.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 31;
}
public class PavedArea_501_1_ViewModel(PavedArea_501_1 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.Segments.Last().LastPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 23;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class PavedAreaBoundingLine_501_2_ViewModel(PavedAreaBoundingLine_501_2 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 26;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class WideRoadMinWidth_502_ViewModel(WideRoadMinWidth_502 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 23;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class RoadWithTwoCarriageways_502_2_ViewModel(RoadWithTwoCarriageways_502_2 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 23;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class Road_503_ViewModel(Road_503 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class VehicleTrack_504_ViewModel(VehicleTrack_504 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class Footpath_505_ViewModel(Footpath_505 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class SmallFootpath_506_ViewModel(SmallFootpath_506 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class LessDistinctSmallFootpath_507_ViewModel(LessDistinctSmallFootpath_507 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class NarrowRide_508_ViewModel(NarrowRide_508 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class NarrowRideEasy_508_1_ViewModel(NarrowRideEasy_508_1 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class NarrowRideNormal_508_2_ViewModel(NarrowRideNormal_508_2 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class NarrowRideSlow_508_3_ViewModel(NarrowRideSlow_508_3 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class NarrowRideWalk_508_4_ViewModel(NarrowRideWalk_508_4 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class Railway_509_ViewModel(Railway_509 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class PowerLine_510_ViewModel(PowerLine_510 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class MajorPowerLineMinWidth_511_ViewModel(MajorPowerLineMinWidth_511 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class MajorPowerLine_511_1_ViewModel(MajorPowerLine_511_1 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class MajorPowerLineLargeCarryingMasts_511_2_ViewModel(MajorPowerLineLargeCarryingMasts_511_2 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class BridgeTunnel_512_ViewModel(BridgeTunnel_512 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; }= obj.Position.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public float Rotation { get; } = obj.Rotation;
}
public class BridgeTunnelMinSize_512_1_ViewModel(BridgeTunnelMinSize_512_1 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; }= obj.Position.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public float Rotation { get; } = obj.Rotation;
}
public class FootBridge_512_2_ViewModel(FootBridge_512_2 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; }= obj.Position.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public float Rotation { get; } = obj.Rotation;
}
public class Wall_513_ViewModel(Wall_513 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class RuinedWall_514_ViewModel(RuinedWall_514 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class ImpassableWall_515_ViewModel(ImpassableWall_515 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class Fence_516_ViewModel(Fence_516 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class RuinedFence_517_ViewModel(RuinedFence_517 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class ImpassableFence_518_ViewModel(ImpassableFence_518 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class CrossingPoint_519_ViewModel(CrossingPoint_519 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; }= obj.Position.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public float Rotation { get; } = obj.Rotation;
}
public class NotEnterableArea_520_ViewModel(NotEnterableArea_520 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.Segments.Last().LastPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 11;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class NotEnterableAreaSolidColourBoundingLine_520_1_ViewModel(NotEnterableAreaSolidColourBoundingLine_520_1 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class NotEnterableAreaStripes_502_2_ViewModel(NotEnterableAreaStripes_502_2 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.Segments.Last().LastPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class NotEnterableAreaStripesBoundingLine_520_3_ViewModel(NotEnterableAreaStripesBoundingLine_520_3 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class Building_521_ViewModel(Building_521 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.Segments.Last().LastPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 26;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class BuildingMinSize_521_1_ViewModel(BuildingMinSize_521_1 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; }= obj.Position.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 26;
    public float Rotation { get; } = obj.Rotation;
}
public class LargeBuilding_521_3_ViewModel(LargeBuilding_521_3 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.Segments.Last().LastPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 25;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class LargeBuildingOutline_521_4_ViewModel(LargeBuildingOutline_521_4 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class Canopy_522_1_ViewModel(Canopy_522_1 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.Segments.Last().LastPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 24;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class CanopyOutline_522_2_ViewModel(CanopyOutline_522_2 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class Ruin_523_ViewModel(Ruin_523 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class RuinMinSize_523_1_ViewModel(RuinMinSize_523_1 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; }= obj.Position.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public float Rotation { get; } = obj.Rotation;
}
public class HighTower_524_ViewModel(HighTower_524 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Position.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
}
public class SmallTower_525_ViewModel(SmallTower_525 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; }= obj.Position.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
}
public class Cairn_526_ViewModel(Cairn_526 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; }= obj.Position.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
}
public class FodderRack_527_ViewModel(FodderRack_527 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; }= obj.Position.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
}
public class ProminentLineFeature_528_ViewModel(ProminentLineFeature_528 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class ProminentImpassableLineFeature_529_ViewModel(ProminentImpassableLineFeature_529 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class ProminentManMadeFeatureRing_530_ViewModel(ProminentManMadeFeatureRing_530 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; }= obj.Position.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
}
public class ProminentManMadeFeatureX_531_ViewModel(ProminentManMadeFeatureX_531 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; }= obj.Position.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
}
public class Stairway_532_ViewModel(Stairway_532 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 12;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class StairWayWithoutBorderLines_532_1_ViewModel(StairWayWithoutBorderLines_532_1 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 12;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class MagneticNorthLine_601_1_ViewModel(MagneticNorthLine_601_1 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class NorthLinesPattern_601_2_ViewModel(NorthLinesPattern_601_2 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class MagneticNorthLineBlue_601_3_ViewModel(MagneticNorthLineBlue_601_3 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 29;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class NorthLinesPatternBlue_601_4_ViewModel(NorthLinesPatternBlue_601_4 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 29;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
public class RegistrationMark_602_ViewModel(RegistrationMark_602 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; }= obj.Position.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
}
public class SpotHeightDot_603_ViewModel(SpotHeightDot_603 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; }= obj.Position.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 32;
}
public class SimpleOrienteeringCourse_799_ViewModel(SimpleOrienteeringCourse_799 obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Shape.StartPoint.ToCanvasCoordinate(mapsTopLeftVertex);
    public override int Priority => 27;
    public PathPolygonViewModel Shape { get; } = new PathPolygonViewModel(obj.Shape, mapsTopLeftVertex);
}
