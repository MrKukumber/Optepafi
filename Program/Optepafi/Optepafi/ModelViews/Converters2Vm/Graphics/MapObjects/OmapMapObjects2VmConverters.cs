using System;
using System.Collections.Generic;
using Optepafi.Models.GraphicsMan.Objects.Map;
using Optepafi.Models.Utils;
using Optepafi.ViewModels.Data.Graphics;
using Optepafi.ViewModels.Data.Graphics.MapObjects;

namespace Optepafi.ModelViews.Converters2Vm.Graphics.MapObjects;

//TODO: comment
public static class OmapMapObjects2VmConverters
{
    public static Dictionary<Type, IGraphicObjects2VmConverter> Converters = new()
    {
         { typeof(Contour_101), Contour2VmConstructor.Instance},
         { typeof(SlopeLine_101_1), SlopeLine2VmConstructor.Instance },
         { typeof(IndexContour_102) , IndexContour2VmConstructor.Instance }, 
         { typeof(FormLine_103) , FormLine2VmConstructor.Instance },
         { typeof(SlopeLineFormLine_103_1) , SlopeLineFormLine2VmConstructor.Instance },
         { typeof(EarthBank_104) , EarthBank2VmConstructor.Instance }, 
         { typeof(EarthBankMinSize_104_1) , EarthBankMinSize2VmConstructor.Instance }, 
         { typeof(EarthBankTopLine_104_2) , EarthBankTopLine2VmConstructor.Instance }, 
         { typeof(EarthBankTagLine_104_3) , EarthBankTagLine2VmConstructor.Instance }, 
         { typeof(EarthWall_105) , EarthWall2VmConstructor.Instance }, 
         { typeof(RuinedEarthWall_106) , RuinedEarthWall2VmConstructor.Instance }, 
         { typeof(ErosionGully_107) , ErosionGully2VmConstructor.Instance }, 
         { typeof(SmallErosionGully_108) , SmallErosionGully2VmConstructor.Instance }, 
         { typeof(SmallKnoll_109) , SmallKnoll2VmConstructor.Instance }, 
         { typeof(SmallEllongatedKnoll_110) , SmallEllongatedKnoll2VmConstructor.Instance }, 
         { typeof(SmallDepression_111) , SmallDepression2VmConstructor.Instance }, 
         { typeof(Pit_112) , Pit2VmConstructor.Instance }, 
         { typeof(BrokenGround_113) , BrokenGround2VmConstructor.Instance }, 
         { typeof(BrokenGroundIndividualDot_113_1) , BrokenGroundIndividualDot2VmConstructor.Instance }, 
         { typeof(VeryBrokenGround_114) , VeryBrokenGround2VmConstructor.Instance }, 
         { typeof(ProminentLandformFeature_115) , ProminentLandformFeature2VmConstructor.Instance }, 
         { typeof(ImpassableCliff_201) , ImpassableCliff2VmConstructor.Instance }, 
         { typeof(ImpassableCliffMinSize_201_1) , ImpassableCliffMinSize2VmConstructor.Instance }, 
         { typeof(ImpassableCliffTopLine_201_3) , ImpassableCliffTopLine2VmConstructor.Instance }, 
         { typeof(ImpassableCliffTagLine_201_4) , ImpassableCliffTagLine2VmConstructor.Instance }, 
         { typeof(Cliff_202) , CliffConstructor.Instance }, 
         { typeof(CliffMinSize_202_1) , CliffMinSize2V2VmmConstructor.Instance }, 
         { typeof(CliffWithTags_202_2) , CliffWithTags2VmConstructor.Instance }, 
         { typeof(CliffWithTagsMinSize_202_3) , CliffWithTagsMinSize2VmConstructor.Instance }, 
         { typeof(RockyPitOrCave_203_1_2) , RockyPitOrCave2VmConstructor.Instance }, 
         { typeof(Boulder_204) , Boulder2VmConstructor.Instance }, 
         { typeof(BoulderOrLargeBoulder_204_5) , BoulderOrLargeBoulder2VmConstructor .Instance },
         { typeof(LargeBoulder_205) , LargeBoulder2VmConstructor.Instance }, 
         { typeof(GiganticBoulder_206) , GiganticBoulder2VmConstructor.Instance }, 
         { typeof(BoulderCluster_207) , BoulderCluster2VmConstructor.Instance }, 
         { typeof(BoulderClusterLarge_207_1) , BoulderClusterLarge2VmConstructor.Instance }, 
         { typeof(BoulderField_208) , BoulderField2VmConstructor.Instance }, 
         { typeof(BoulderFieldSingleTriangle_208_1) , BoulderFieldSingleTriangle2VmConstructor.Instance }, 
         { typeof(BoulderFieldSingleTriangleEnlarged_208_2) , BoulderFieldSingleTriangleEnlarged2VmConstructor.Instance }, 
         { typeof(DenseBoulderField_209) , DenseBoulderField2VmConstructor.Instance }, 
         { typeof(StonyGroundSlowRunning_210) , StonyGroundSlowRunning2VmConstructor.Instance }, 
         { typeof(StonyGroundIndividualDot_210_1) , StonyGroundIndividualDot2VmConstructor.Instance }, 
         { typeof(StonyGroundWalk_211) , StonyGroundWalk2VmConstructor.Instance }, 
         { typeof(StonyGroundFight_212) , StonyGroundFight2VmConstructor.Instance }, 
         { typeof(SandyGround_213) , SandyGround2VmConstructor.Instance }, 
         { typeof(BareRock_214) , BareRock2VmConstructor.Instance }, 
         { typeof(Trench_215) , Trench2VmConstructor.Instance }, 
         { typeof(UncrossableBodyOfWaterFullCol_301_1) , UncrossableBodyOfWaterFullCol2VmConstructor.Instance }, 
         { typeof(UncrossableBodyOfWaterDominant_301_3) , UncrossableBodyOfWaterDominant2VmConstructor.Instance }, 
         { typeof(UncrossableBodyOfWaterBankLine_301_4) , UncrossableBodyOfWaterBankLine2VmConstructor.Instance }, 
         { typeof(ShallowBodyOfWater_302_1) , ShallowBodyOfWater2VmConstructor.Instance }, 
         { typeof(ShallowBodyOfWaterSolidOutline_302_2) , ShallowBodyOfWaterSolidOutline2VmConstructor.Instance }, 
         { typeof(ShallowBodyOfWaterDashedOutline_302_3) , ShallowBodyOfWaterDashedOutline2VmConstructor.Instance }, 
         { typeof(SmallShallowBodyOfWater_302_5) , SmallShallowBodyOfWater2VmConstructor.Instance }, 
         { typeof(WaterHole_303) , WaterHole2VmConstructor.Instance }, 
         { typeof(CrossableWatercourse_304) , CrossableWatercourse2VmConstructor.Instance }, 
         { typeof(SmallCrossableWatercourse_305) , SmallCrossableWatercourse2VmConstructor.Instance }, 
         { typeof(SeasonalWaterChannel_306) , SeasonalWaterChannel2VmConstructor.Instance },
         { typeof(UncrossableMarsh_307_1) , UncrossableMarsh2VmConstructor.Instance }, 
         { typeof(UncrossableMarshOutline_307_2) , UncrossableMarshOutline2VmConstructor.Instance }, 
         { typeof(Marsh_308) , Marsh2VmConstructor.Instance }, 
         { typeof(MarshMinSize_308_1) , MarshMinSize2VmConstructor.Instance }, 
         { typeof(NarrowMarsh_309) , NarrowMarsh2VmConstructor.Instance }, 
         { typeof(IndistinctMarsh_310) , IndistinctMarsh2VmConstructor.Instance }, 
         { typeof(IndistinctMarshMinSize_310_1) , IndistinctMarshMinSize2VmConstructor.Instance }, 
         { typeof(WellFountainWaterTank_311) , WellFountainWaterTank2VmConstructor.Instance }, 
         { typeof(Spring_312) , Spring2VmConstructor.Instance }, 
         { typeof(ProminentWaterFeature_313) , ProminentWaterFeature2VmConstructor.Instance }, 
         { typeof(OpenLand_401) , OpenLand2VmConstructor.Instance }, 
         { typeof(OpenLandWithTrees_402) , OpenLandWithTrees2VmConstructor.Instance }, 
         { typeof(OpenLandWithBushes_402_1) , OpenLandWithBushes2VmConstructor.Instance }, 
         { typeof(RoughOpenLand_403) , RoughOpenLand2VmConstructor.Instance}, 
         { typeof(RoughOpenLandWithTrees_404) , RoughOpenLandWithTrees2VmConstructor.Instance }, 
         { typeof(RoughOpenLandWithBushes_404_1) , RoughOpenLandWithBushes2VmConstructor.Instance }, 
         { typeof(Forest_405) , Forest2VmConstructor.Instance }, 
         { typeof(VegetationSlow_406) , VegetationSlow2VmConstructor.Instance }, 
         { typeof(VegetationSlowNormalOneDir_406_1) , VegetationSlowNormalOneDir2VmConstructor.Instance },
         { typeof(VegetationSlowGoodVisibility_407) , VegetationSlowGoodVisibility2VmConstructor.Instance }, 
         { typeof(VegetationWalk_408) , VegetationWalk2VmConstructor.Instance }, 
         { typeof(VegetationWalkNormalOneDir_408_1) , VegetationWalkNormalOneDir2VmConstructor.Instance }, 
         { typeof(VegetationWalkSlowOneDir_408_2) , VegetationWalkSlowOneDir2VmConstructor.Instance }, 
         { typeof(VegetationWalkGoodVisibility_409) , VegetationWalkGoodVisibility2VmConstructor.Instance }, 
         { typeof(VegetationFight_410) , VegetationFight2VmConstructor.Instance }, 
         { typeof(VegetationFightNormalOneDir_410_1) , VegetationFightNormalOneDir2VmConstructor.Instance }, 
         { typeof(VegetationFightSlowOneDir_410_2) , VegetationFightSlowOneDir2VmConstructor.Instance }, 
         { typeof(VegetationFightWalkOneDir_410_3) , VegetationFightWalkOneDir2VmConstructor.Instance }, 
         { typeof(VegetationFightMinWidth_410_4) , VegetationFightMinWidth2VmConstructor.Instance }, 
         { typeof(CultivatedLandBlackPattern_412_1) , CultivatedLandBlackPattern2VmConstructor.Instance },
         { typeof(Orchard_413) , Orchard2VmConstructor.Instance }, 
         { typeof(OrchardRoughOpenLand_413_1) , OrchardRoughOpenLand2VmConstructor.Instance }, 
         { typeof(Vineyard_414) , Vineyard2VmConstructor.Instance }, 
         { typeof(VineyardRoughOpenLand_414_1) , VineyardRoughOpenLand2VmConstructor.Instance }, 
         { typeof(DistinctCultivationBoundary_415) , DistinctCultivationBoundary2VmConstructor.Instance }, 
         { typeof(DistinctVegetationBoundary_416) , DistinctVegetationBoundary2VmConstructor.Instance }, 
         { typeof(DistinctVegetationBoundaryGreenDashedLine_416_1) , DistinctVegetationBoundaryGreenDashedLine2VmConstructor.Instance }, 
         { typeof(ProminentLargeTree_417) , ProminentLargeTree2VmConstructor.Instance }, 
         { typeof(ProminentBush_418) , ProminentBush2VmConstructor.Instance }, 
         { typeof(ProminentVegetationFeature_419) , ProminentVegetationFeature2VmConstructor.Instance }, 
         { typeof(PavedArea_501_1) , PavedArea2VmConstructor.Instance }, 
         { typeof(PavedAreaBoundingLine_501_2) , PavedAreaBoundingLine2VmConstructor.Instance }, 
         { typeof(WideRoadMinWidth_502) , WideRoadMinWidth2VmConstructor.Instance }, 
         { typeof(RoadWithTwoCarriageways_502_2) , RoadWithTwoCarriageways2VmConstructor.Instance }, 
         { typeof(Road_503) , Road2VmConstructor.Instance }, 
         { typeof(VehicleTrack_504) , VehicleTrack2VmConstructor.Instance },  
         { typeof(Footpath_505) , Footpath2VmConstructor.Instance }, 
         { typeof(SmallFootpath_506) , SmallFootpath2VmConstructor.Instance }, 
         { typeof(LessDistinctSmallFootpath_507) , LessDistinctSmallFootpath2VmConstructor.Instance }, 
         { typeof(NarrowRide_508) , NarrowRide2VmConstructor.Instance }, 
         { typeof(NarrowRideEasy_508_1) , NarrowRideEasy2VmConstructor.Instance }, 
         { typeof(NarrowRideNormal_508_2) , NarrowRideNormal2VmConstructor.Instance }, 
         { typeof(NarrowRideSlow_508_3) , NarrowRideSlow2VmConstructor.Instance }, 
         { typeof(NarrowRideWalk_508_4) , NarrowRideWalk2VmConstructor.Instance }, 
         { typeof(Railway_509) , Railway2VmConstructor.Instance }, 
         { typeof(PowerLine_510) , PowerLine2VmConstructor.Instance }, 
         { typeof(MajorPowerLineMinWidth_511) , MajorPowerLineMinWidth2VmConstructor.Instance }, 
         { typeof(MajorPowerLine_511_1) , MajorPowerLine2VmConstructor.Instance }, 
         { typeof(MajorPowerLineLargeCarryingMasts_511_2) , MajorPowerLineLargeCarryingMast2VmConstructor.Instance }, 
         { typeof(BridgeTunnel_512) , BridgeTunnel2VmConstructor.Instance }, 
         { typeof(BridgeTunnelMinSize_512_1) , BridgeTunnelMinSize2VmConstructor.Instance }, 
         { typeof(FootBridge_512_2) , FootBridge2VmConstructor.Instance }, 
         { typeof(Wall_513) , Wall2VmConstructor.Instance }, 
         { typeof(RuinedWall_514) , RuinedWall2VmConstructor.Instance }, 
         { typeof(ImpassableWall_515) , ImpassableWall2VmConstructor.Instance }, 
         { typeof(Fence_516) , Fence2VmConstructor.Instance }, 
         { typeof(RuinedFence_517) , RuinedFence2VmConstructor.Instance }, 
         { typeof(ImpassableFence_518) , ImpassableFence2VmConstructor.Instance }, 
         { typeof(CrossingPoint_519) , CrossingPoint2VmConstructor.Instance }, 
         { typeof(NotEnterableArea_520) , NotEnterableArea2VmConstructor.Instance }, 
         { typeof(NotEnterableAreaSolidColourBoundingLine_520_1) , NotEnterableAreaSolidColourBoundingLine2VmConstructor.Instance }, 
         { typeof(NotEnterableAreaStripes_502_2) , NotEnterableAreaStripes2VmConstructor.Instance }, 
         { typeof(NotEnterableAreaStripesBoundingLine_520_3) , NotEnterableAreaStripesBoundingLine2VmConstructor.Instance }, 
         { typeof(Building_521) , Building2VmConstructor.Instance }, 
         { typeof(BuildingMinSize_521_1) , BuildingMinSize2VmConstructor.Instance }, 
         { typeof(LargeBuilding_521_3) , LargeBuilding2VmConstructor.Instance }, 
         { typeof(LargeBuildingOutline_521_4) , LargeBuildingOutline2VmConstructor.Instance }, 
         { typeof(Canopy_522_1) , Canopy2VmConstructor.Instance }, 
         { typeof(CanopyOutline_522_2) , CanopyOutline2VmConstructor.Instance }, 
         { typeof(Ruin_523) , Ruin2VmConstructor.Instance }, 
         { typeof(RuinMinSize_523_1) , RuinMinSize2VmConstructor.Instance }, 
         { typeof(HighTower_524) , HighTower2VmConstructor.Instance }, 
         { typeof(SmallTower_525) , SmallTower2VmConstructor.Instance }, 
         { typeof(Cairn_526) , Cairn2VmConstructor.Instance }, 
         { typeof(FodderRack_527) , FodderRack2VmConstructor.Instance }, 
         { typeof(ProminentLineFeature_528) , ProminentLineFeature2VmConstructor.Instance }, 
         { typeof(ProminentImpassableLineFeature_529) , ProminentImpassableLineFeature2VmConstructor.Instance }, 
         { typeof(ProminentManMadeFeatureRing_530) , ProminentManMadeFeatureRing2VmConstructor.Instance }, 
         { typeof(ProminentManMadeFeatureX_531) , ProminentManMadeFeatureX2VmConstructor.Instance }, 
         { typeof(Stairway_532) , Stairway2VmConstructor.Instance }, 
         { typeof(StairWayWithoutBorderLines_532_1) , StairWayWithoutBorderLines2VmConstructor.Instance }, 
         { typeof(MagneticNorthLine_601_1) , MagneticNorthLine2VmConstructor.Instance }, 
         { typeof(NorthLinesPattern_601_2) , NorthLinesPattern2VmConstructor.Instance }, 
         { typeof(MagneticNorthLineBlue_601_3) , MagneticNorthLineBlue2VmConstructor.Instance }, 
         { typeof(NorthLinesPatternBlue_601_4) , NorthLinesPatternBlue2VmConstructor.Instance }, 
         { typeof(RegistrationMark_602) , RegistrationMark2VmConstructor.Instance }, 
         { typeof(SpotHeightDot_603) , SpotHeightDot2VmConstructor.Instance }, 
         {typeof(Start_701), Start2VmConstructor.Instance},
         {typeof(ControlPoint_703), ControlPoint2VmConstructor.Instance},
         {typeof(CourseLine_705), CourseLine2VmConstructor.Instance},
         {typeof(Finish_706), Finish2VmConstructor.Instance}
         // { typeof(SimpleOrienteeringCourse_799) , SimpleOrienteeringCourse2VmConstructor.Instance },    
    };
}

public class Contour2VmConstructor : IGraphicObjects2VmConverter<Contour_101>
{
    public static Contour2VmConstructor Instance { get; } = new();
    private Contour2VmConstructor() { }
    public GraphicObjectViewModel ConvertToViewModel(Contour_101 graphicsObject, MapCoordinates mapsTopLeftVertex) 
        => new Contour_101_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class SlopeLine2VmConstructor : IGraphicObjects2VmConverter<SlopeLine_101_1>
{

    public static SlopeLine2VmConstructor Instance { get; } = new();
    private SlopeLine2VmConstructor() { }
    public GraphicObjectViewModel ConvertToViewModel(SlopeLine_101_1 graphicsObject, MapCoordinates mapsTopLeftVertex) 
        => new SlopeLine_101_1_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class IndexContour2VmConstructor : IGraphicObjects2VmConverter<IndexContour_102>
{

    public static IndexContour2VmConstructor Instance { get; } = new();
    private IndexContour2VmConstructor() { }
    public GraphicObjectViewModel ConvertToViewModel(IndexContour_102 graphicsObject, MapCoordinates mapsTopLeftVertex) 
        => new IndexContour_102_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class FormLine2VmConstructor : IGraphicObjects2VmConverter<FormLine_103>
{

    public static FormLine2VmConstructor Instance { get; } = new();
    private FormLine2VmConstructor() { }
    public GraphicObjectViewModel ConvertToViewModel(FormLine_103 graphicsObject, MapCoordinates mapsTopLeftVertex)
        => new FormLine_103_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class SlopeLineFormLine2VmConstructor : IGraphicObjects2VmConverter<SlopeLineFormLine_103_1>
{

    public static SlopeLineFormLine2VmConstructor Instance { get; } = new();
    private SlopeLineFormLine2VmConstructor() { }
    public GraphicObjectViewModel ConvertToViewModel(SlopeLineFormLine_103_1 graphicsObject, MapCoordinates mapsTopLeftVertex)
        => new SlopeLineFormLine_103_1_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class EarthBank2VmConstructor : IGraphicObjects2VmConverter<EarthBank_104>
{

    public static EarthBank2VmConstructor Instance { get; } = new();
    private EarthBank2VmConstructor() { }
    public GraphicObjectViewModel ConvertToViewModel(EarthBank_104 graphicsObject, MapCoordinates mapsTopLeftVertex)
        => new EarthBank_104_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class EarthBankMinSize2VmConstructor : IGraphicObjects2VmConverter<EarthBankMinSize_104_1>
{

    public static EarthBankMinSize2VmConstructor Instance { get; } = new();
    private EarthBankMinSize2VmConstructor() { }
    public GraphicObjectViewModel ConvertToViewModel(EarthBankMinSize_104_1 graphicsObject, MapCoordinates mapsTopLeftVertex) 
        => new EarthBankMinSize_104_1_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class EarthBankTopLine2VmConstructor : IGraphicObjects2VmConverter<EarthBankTopLine_104_2>
{

    public static EarthBankTopLine2VmConstructor Instance { get; } = new();
    private EarthBankTopLine2VmConstructor() { }
    public GraphicObjectViewModel ConvertToViewModel(EarthBankTopLine_104_2 graphicsObject, MapCoordinates mapsTopLeftVertex)
        => new EarthBankTopLine_104_2_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class EarthBankTagLine2VmConstructor : IGraphicObjects2VmConverter<EarthBankTagLine_104_3>
{

    public static EarthBankTagLine2VmConstructor Instance { get; } = new();
    private EarthBankTagLine2VmConstructor() { }
    public GraphicObjectViewModel ConvertToViewModel(EarthBankTagLine_104_3 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new EarthBankTagLine_104_3_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class EarthWall2VmConstructor : IGraphicObjects2VmConverter<EarthWall_105>
{

    public static EarthWall2VmConstructor Instance { get; } = new();
    private EarthWall2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(EarthWall_105 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new EarthWall_105_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class RuinedEarthWall2VmConstructor : IGraphicObjects2VmConverter<RuinedEarthWall_106>
{

    public static RuinedEarthWall2VmConstructor Instance { get; } = new();
    private RuinedEarthWall2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(RuinedEarthWall_106 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new RuinedEarthWall_106_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class ErosionGully2VmConstructor : IGraphicObjects2VmConverter<ErosionGully_107>
{

    public static ErosionGully2VmConstructor Instance { get; } = new();
    private ErosionGully2VmConstructor() { }
    public GraphicObjectViewModel ConvertToViewModel(ErosionGully_107 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new ErosionGully_107_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class SmallErosionGully2VmConstructor : IGraphicObjects2VmConverter<SmallErosionGully_108>
{

    public static SmallErosionGully2VmConstructor Instance { get; } = new();
    private SmallErosionGully2VmConstructor() { }
    public GraphicObjectViewModel ConvertToViewModel(SmallErosionGully_108 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new SmallErosionGully_108_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class SmallKnoll2VmConstructor : IGraphicObjects2VmConverter<SmallKnoll_109>
{

    public static SmallKnoll2VmConstructor Instance { get; } = new();
    private SmallKnoll2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(SmallKnoll_109 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new SmallKnoll_109_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class SmallEllongatedKnoll2VmConstructor : IGraphicObjects2VmConverter<SmallEllongatedKnoll_110>
{

    public static SmallEllongatedKnoll2VmConstructor Instance { get; } = new();
    private SmallEllongatedKnoll2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(SmallEllongatedKnoll_110 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new SmallEllongatedKnoll_110_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class SmallDepression2VmConstructor : IGraphicObjects2VmConverter<SmallDepression_111>
{

    public static SmallDepression2VmConstructor Instance { get; } = new();
    private SmallDepression2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(SmallDepression_111 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new SmallDepression_111_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class Pit2VmConstructor : IGraphicObjects2VmConverter<Pit_112>
{
    public static Pit2VmConstructor Instance { get; } = new();
    private Pit2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(Pit_112 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new Pit_112_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class BrokenGround2VmConstructor : IGraphicObjects2VmConverter<BrokenGround_113>
{
    public static BrokenGround2VmConstructor Instance { get; } = new();
    private BrokenGround2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(BrokenGround_113 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new BrokenGround_113_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class BrokenGroundIndividualDot2VmConstructor : IGraphicObjects2VmConverter<BrokenGroundIndividualDot_113_1>
{

    public static BrokenGroundIndividualDot2VmConstructor Instance { get; } = new();
    private BrokenGroundIndividualDot2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(BrokenGroundIndividualDot_113_1 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new BrokenGroundIndividualDot_113_1_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class VeryBrokenGround2VmConstructor : IGraphicObjects2VmConverter<VeryBrokenGround_114>
{
    public static VeryBrokenGround2VmConstructor Instance { get; } = new();
    private VeryBrokenGround2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(VeryBrokenGround_114 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new VeryBrokenGround_114_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class ProminentLandformFeature2VmConstructor : IGraphicObjects2VmConverter<ProminentLandformFeature_115>
{
    public static ProminentLandformFeature2VmConstructor Instance { get; } = new();
    private ProminentLandformFeature2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(ProminentLandformFeature_115 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new ProminentLandformFeature_115_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class ImpassableCliff2VmConstructor : IGraphicObjects2VmConverter<ImpassableCliff_201>
{
    public static ImpassableCliff2VmConstructor Instance { get; } = new();
    private ImpassableCliff2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(ImpassableCliff_201 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new ImpassableCliff_201_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class ImpassableCliffMinSize2VmConstructor : IGraphicObjects2VmConverter<ImpassableCliffMinSize_201_1>
{
    public static ImpassableCliffMinSize2VmConstructor Instance { get; } = new();
    private ImpassableCliffMinSize2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(ImpassableCliffMinSize_201_1 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new ImpassableCliffMinSize_201_1_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class ImpassableCliffTopLine2VmConstructor : IGraphicObjects2VmConverter<ImpassableCliffTopLine_201_3>
{
    public static ImpassableCliffTopLine2VmConstructor Instance { get; } = new();
    private ImpassableCliffTopLine2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(ImpassableCliffTopLine_201_3 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new ImpassableCliffTopLine_201_3_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class ImpassableCliffTagLine2VmConstructor : IGraphicObjects2VmConverter<ImpassableCliffTagLine_201_4>
{

    public static ImpassableCliffTagLine2VmConstructor Instance { get; } = new();
    private ImpassableCliffTagLine2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(ImpassableCliffTagLine_201_4 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new ImpassableCliffTagLine_201_4_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class CliffConstructor : IGraphicObjects2VmConverter<Cliff_202>
{

    public static CliffConstructor Instance { get; } = new();
    private CliffConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(Cliff_202 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new Cliff_202_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class CliffMinSize2V2VmmConstructor : IGraphicObjects2VmConverter<CliffMinSize_202_1>
{

    public static CliffMinSize2V2VmmConstructor Instance { get; } = new();
    private CliffMinSize2V2VmmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(CliffMinSize_202_1 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new CliffMinSize_202_1_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class CliffWithTags2VmConstructor : IGraphicObjects2VmConverter<CliffWithTags_202_2>
{

    public static CliffWithTags2VmConstructor Instance { get; } = new();
    private CliffWithTags2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(CliffWithTags_202_2 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new CliffWithTags_202_2_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class CliffWithTagsMinSize2VmConstructor : IGraphicObjects2VmConverter<CliffWithTagsMinSize_202_3>
{

    public static CliffWithTagsMinSize2VmConstructor Instance { get; } = new();
    private CliffWithTagsMinSize2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(CliffWithTagsMinSize_202_3 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new CliffWithTagsMinSize_202_3_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class RockyPitOrCave2VmConstructor : IGraphicObjects2VmConverter<RockyPitOrCave_203_1_2>
{

    public static RockyPitOrCave2VmConstructor Instance { get; } = new();
    private RockyPitOrCave2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(RockyPitOrCave_203_1_2 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new RockyPitOrCave_203_1_2_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class Boulder2VmConstructor : IGraphicObjects2VmConverter<Boulder_204>
{
    public static Boulder2VmConstructor Instance { get; } = new();
    private Boulder2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(Boulder_204 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new Boulder_204_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class BoulderOrLargeBoulder2VmConstructor  : IGraphicObjects2VmConverter<BoulderOrLargeBoulder_204_5>
{

    public static BoulderOrLargeBoulder2VmConstructor  Instance { get; } = new();
    private BoulderOrLargeBoulder2VmConstructor  () { }
    public GraphicObjectViewModel ConvertToViewModel(BoulderOrLargeBoulder_204_5 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new BoulderOrLargeBoulder_204_5_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class LargeBoulder2VmConstructor : IGraphicObjects2VmConverter<LargeBoulder_205>
{

    public static LargeBoulder2VmConstructor Instance { get; } = new();
    private LargeBoulder2VmConstructor() { }
    public GraphicObjectViewModel ConvertToViewModel(LargeBoulder_205 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new LargeBoulder_205_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class GiganticBoulder2VmConstructor : IGraphicObjects2VmConverter<GiganticBoulder_206>
{

    public static GiganticBoulder2VmConstructor Instance { get; } = new();
    private GiganticBoulder2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(GiganticBoulder_206 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new GiganticBoulder_206_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class BoulderCluster2VmConstructor : IGraphicObjects2VmConverter<BoulderCluster_207>
{
    public static BoulderCluster2VmConstructor Instance { get; } = new();
    private BoulderCluster2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(BoulderCluster_207 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new BoulderCluster_207_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class BoulderClusterLarge2VmConstructor : IGraphicObjects2VmConverter<BoulderClusterLarge_207_1>
{
    public static BoulderClusterLarge2VmConstructor Instance { get; } = new();
    private BoulderClusterLarge2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(BoulderClusterLarge_207_1 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new BoulderClusterLarge_207_1_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class BoulderField2VmConstructor : IGraphicObjects2VmConverter<BoulderField_208>
{

    public static BoulderField2VmConstructor Instance { get; } = new();
    private BoulderField2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(BoulderField_208 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new BoulderField_208_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class BoulderFieldSingleTriangle2VmConstructor : IGraphicObjects2VmConverter<BoulderFieldSingleTriangle_208_1>
{

    public static BoulderFieldSingleTriangle2VmConstructor Instance { get; } = new();
    private BoulderFieldSingleTriangle2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(BoulderFieldSingleTriangle_208_1 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new BoulderFieldSingleTriangle_208_1_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class BoulderFieldSingleTriangleEnlarged2VmConstructor : IGraphicObjects2VmConverter<BoulderFieldSingleTriangleEnlarged_208_2>
{

    public static BoulderFieldSingleTriangleEnlarged2VmConstructor Instance { get; } = new();
    private BoulderFieldSingleTriangleEnlarged2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(BoulderFieldSingleTriangleEnlarged_208_2 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new BoulderFieldSingleTriangleEnlarged_208_2_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class DenseBoulderField2VmConstructor : IGraphicObjects2VmConverter<DenseBoulderField_209>
{

    public static DenseBoulderField2VmConstructor Instance { get; } = new();
    private DenseBoulderField2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(DenseBoulderField_209 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new DenseBoulderField_209_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class StonyGroundSlowRunning2VmConstructor : IGraphicObjects2VmConverter<StonyGroundSlowRunning_210>
{

    public static StonyGroundSlowRunning2VmConstructor Instance { get; } = new();
    private StonyGroundSlowRunning2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(StonyGroundSlowRunning_210 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new StonyGroundSlowRunning_210_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class StonyGroundIndividualDot2VmConstructor : IGraphicObjects2VmConverter<StonyGroundIndividualDot_210_1>
{

    public static StonyGroundIndividualDot2VmConstructor Instance { get; } = new();
    private StonyGroundIndividualDot2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(StonyGroundIndividualDot_210_1 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new StonyGroundIndividualDot_210_1_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class StonyGroundWalk2VmConstructor : IGraphicObjects2VmConverter<StonyGroundWalk_211>
{

    public static StonyGroundWalk2VmConstructor Instance { get; } = new();
    private StonyGroundWalk2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(StonyGroundWalk_211 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new StonyGroundWalk_211_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class StonyGroundFight2VmConstructor : IGraphicObjects2VmConverter<StonyGroundFight_212>
{

    public static StonyGroundFight2VmConstructor Instance { get; } = new();
    private StonyGroundFight2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(StonyGroundFight_212 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new StonyGroundFight_212_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class SandyGround2VmConstructor : IGraphicObjects2VmConverter<SandyGround_213>
{

    public static SandyGround2VmConstructor Instance { get; } = new();
    private SandyGround2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(SandyGround_213 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new SandyGround_213_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class BareRock2VmConstructor : IGraphicObjects2VmConverter<BareRock_214>
{

    public static BareRock2VmConstructor Instance { get; } = new();
    private BareRock2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(BareRock_214 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new BareRock_214_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class Trench2VmConstructor : IGraphicObjects2VmConverter<Trench_215>
{

    public static Trench2VmConstructor Instance { get; } = new();
    private Trench2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(Trench_215 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new Trench_215_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class UncrossableBodyOfWaterFullCol2VmConstructor : IGraphicObjects2VmConverter<UncrossableBodyOfWaterFullCol_301_1>
{

    public static UncrossableBodyOfWaterFullCol2VmConstructor Instance { get; } = new();
    private UncrossableBodyOfWaterFullCol2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(UncrossableBodyOfWaterFullCol_301_1 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new UncrossableBodyOfWaterFullCol_301_1_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class UncrossableBodyOfWaterDominant2VmConstructor : IGraphicObjects2VmConverter<UncrossableBodyOfWaterDominant_301_3>
{

    public static UncrossableBodyOfWaterDominant2VmConstructor Instance { get; } = new();
    private UncrossableBodyOfWaterDominant2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(UncrossableBodyOfWaterDominant_301_3 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new UncrossableBodyOfWaterDominant_301_3_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class UncrossableBodyOfWaterBankLine2VmConstructor : IGraphicObjects2VmConverter<UncrossableBodyOfWaterBankLine_301_4>
{

    public static UncrossableBodyOfWaterBankLine2VmConstructor Instance { get; } = new();
    private UncrossableBodyOfWaterBankLine2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(UncrossableBodyOfWaterBankLine_301_4 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new UncrossableBodyOfWaterBankLine_301_4_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class ShallowBodyOfWater2VmConstructor : IGraphicObjects2VmConverter<ShallowBodyOfWater_302_1>
{

    public static ShallowBodyOfWater2VmConstructor Instance { get; } = new();
    private ShallowBodyOfWater2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(ShallowBodyOfWater_302_1 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new ShallowBodyOfWater_302_1_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class ShallowBodyOfWaterSolidOutline2VmConstructor : IGraphicObjects2VmConverter<ShallowBodyOfWaterSolidOutline_302_2>
{

    public static ShallowBodyOfWaterSolidOutline2VmConstructor Instance { get; } = new();
    private ShallowBodyOfWaterSolidOutline2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(ShallowBodyOfWaterSolidOutline_302_2 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new ShallowBodyOfWaterSolidOutline_302_2_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class ShallowBodyOfWaterDashedOutline2VmConstructor : IGraphicObjects2VmConverter<ShallowBodyOfWaterDashedOutline_302_3>
{
    public static ShallowBodyOfWaterDashedOutline2VmConstructor Instance { get; } = new();
    private ShallowBodyOfWaterDashedOutline2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(ShallowBodyOfWaterDashedOutline_302_3 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new ShallowBodyOfWaterDashedOutline_302_3_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class SmallShallowBodyOfWater2VmConstructor : IGraphicObjects2VmConverter<SmallShallowBodyOfWater_302_5>
{
    public static SmallShallowBodyOfWater2VmConstructor Instance { get; } = new();
    private SmallShallowBodyOfWater2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(SmallShallowBodyOfWater_302_5 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new SmallShallowBodyOfWater_302_5_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class WaterHole2VmConstructor : IGraphicObjects2VmConverter<WaterHole_303>
{
    public static WaterHole2VmConstructor Instance { get; } = new();
    private WaterHole2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(WaterHole_303 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new WaterHole_303_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class CrossableWatercourse2VmConstructor : IGraphicObjects2VmConverter<CrossableWatercourse_304>
{
    public static CrossableWatercourse2VmConstructor Instance { get; } = new();
    private CrossableWatercourse2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(CrossableWatercourse_304 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new CrossableWatercourse_304_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class SmallCrossableWatercourse2VmConstructor : IGraphicObjects2VmConverter<SmallCrossableWatercourse_305>
{
    public static SmallCrossableWatercourse2VmConstructor Instance { get; } = new();
    private SmallCrossableWatercourse2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(SmallCrossableWatercourse_305 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new SmallCrossableWatercourse_305_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class SeasonalWaterChannel2VmConstructor : IGraphicObjects2VmConverter<SeasonalWaterChannel_306>
{
    public static SeasonalWaterChannel2VmConstructor Instance { get; } = new();
    private SeasonalWaterChannel2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(SeasonalWaterChannel_306 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new SeasonalWaterChannel_306_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class UncrossableMarsh2VmConstructor : IGraphicObjects2VmConverter<UncrossableMarsh_307_1>
{
    public static UncrossableMarsh2VmConstructor Instance { get; } = new();
    private UncrossableMarsh2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(UncrossableMarsh_307_1 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new UncrossableMarsh_307_1_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class UncrossableMarshOutline2VmConstructor : IGraphicObjects2VmConverter<UncrossableMarshOutline_307_2>
{
    public static UncrossableMarshOutline2VmConstructor Instance { get; } = new();
    private UncrossableMarshOutline2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(UncrossableMarshOutline_307_2 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new UncrossableMarshOutline_307_2_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class Marsh2VmConstructor : IGraphicObjects2VmConverter<Marsh_308>
{
    public static Marsh2VmConstructor Instance { get; } = new();
    private Marsh2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(Marsh_308 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new Marsh_308_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class MarshMinSize2VmConstructor : IGraphicObjects2VmConverter<MarshMinSize_308_1>
{
    public static MarshMinSize2VmConstructor Instance { get; } = new();
    private MarshMinSize2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(MarshMinSize_308_1 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new MarshMinSize_308_1_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class NarrowMarsh2VmConstructor : IGraphicObjects2VmConverter<NarrowMarsh_309>
{
    public static NarrowMarsh2VmConstructor Instance { get; } = new();
    private NarrowMarsh2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(NarrowMarsh_309 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new NarrowMarsh_309_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class IndistinctMarsh2VmConstructor : IGraphicObjects2VmConverter<IndistinctMarsh_310>
{
    public static IndistinctMarsh2VmConstructor Instance { get; } = new();
    private IndistinctMarsh2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(IndistinctMarsh_310 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new IndistinctMarsh_310_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class IndistinctMarshMinSize2VmConstructor : IGraphicObjects2VmConverter<IndistinctMarshMinSize_310_1>
{
    public static IndistinctMarshMinSize2VmConstructor Instance { get; } = new();
    private IndistinctMarshMinSize2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(IndistinctMarshMinSize_310_1 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new IndistinctMarshMinSize_310_1_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class WellFountainWaterTank2VmConstructor : IGraphicObjects2VmConverter<WellFountainWaterTank_311>
{
    public static WellFountainWaterTank2VmConstructor Instance { get; } = new();
    private WellFountainWaterTank2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(WellFountainWaterTank_311 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new WellFountainWaterTank_311_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class Spring2VmConstructor : IGraphicObjects2VmConverter<Spring_312>
{
    public static Spring2VmConstructor Instance { get; } = new();
    private Spring2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(Spring_312 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new Spring_312_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class ProminentWaterFeature2VmConstructor : IGraphicObjects2VmConverter<ProminentWaterFeature_313>
{
    public static ProminentWaterFeature2VmConstructor Instance { get; } = new();
    private ProminentWaterFeature2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(ProminentWaterFeature_313 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new ProminentWaterFeature_313_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class OpenLand2VmConstructor : IGraphicObjects2VmConverter<OpenLand_401>
{
    public static OpenLand2VmConstructor Instance { get; } = new();
    private OpenLand2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(OpenLand_401 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new OpenLand_401_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class OpenLandWithTrees2VmConstructor : IGraphicObjects2VmConverter<OpenLandWithTrees_402>
{
    public static OpenLandWithTrees2VmConstructor Instance { get; } = new();
    private OpenLandWithTrees2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(OpenLandWithTrees_402 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new OpenLandWithTrees_402_ViewModel(graphicsObject, mapsTopLeftVertex);

}

public class OpenLandWithBushes2VmConstructor : IGraphicObjects2VmConverter<OpenLandWithBushes_402_1>
{
    public static OpenLandWithBushes2VmConstructor Instance { get; } = new();
    private OpenLandWithBushes2VmConstructor() { }
    public GraphicObjectViewModel ConvertToViewModel(OpenLandWithBushes_402_1 graphicsObject, MapCoordinates mapsTopLeftVertex)
        => new OpenLandWithBushes_402_1_ViewModel(graphicsObject, mapsTopLeftVertex);
}

public class RoughOpenLand2VmConstructor : IGraphicObjects2VmConverter<RoughOpenLand_403>
{
    public static RoughOpenLand2VmConstructor Instance { get; } = new();
    private RoughOpenLand2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(RoughOpenLand_403 graphicsObject, MapCoordinates mapsTopLeftVertex)
        => new RoughOpenLand_403_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class RoughOpenLandWithTrees2VmConstructor : IGraphicObjects2VmConverter<RoughOpenLandWithTrees_404>
{
    public static RoughOpenLandWithTrees2VmConstructor Instance { get; } = new();
    private RoughOpenLandWithTrees2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(RoughOpenLandWithTrees_404 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new RoughOpenLandWithTrees_404_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class RoughOpenLandWithBushes2VmConstructor : IGraphicObjects2VmConverter<RoughOpenLandWithBushes_404_1>
{
    public static RoughOpenLandWithBushes2VmConstructor Instance { get; } = new();
    private RoughOpenLandWithBushes2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(RoughOpenLandWithBushes_404_1 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new RoughOpenLandWithBushes_404_1_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class Forest2VmConstructor : IGraphicObjects2VmConverter<Forest_405>
{
    public static Forest2VmConstructor Instance { get; } = new();
    private Forest2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(Forest_405 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new Forest_405_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class VegetationSlow2VmConstructor : IGraphicObjects2VmConverter<VegetationSlow_406>
{
    public static VegetationSlow2VmConstructor Instance { get; } = new();
    private VegetationSlow2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(VegetationSlow_406 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new VegetationSlow_406_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class VegetationSlowNormalOneDir2VmConstructor : IGraphicObjects2VmConverter<VegetationSlowNormalOneDir_406_1>
{
    public static VegetationSlowNormalOneDir2VmConstructor Instance { get; } = new();
    private VegetationSlowNormalOneDir2VmConstructor() { }
    public GraphicObjectViewModel ConvertToViewModel(VegetationSlowNormalOneDir_406_1 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new VegetationSlowNormalOneDir_406_1_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class VegetationSlowGoodVisibility2VmConstructor : IGraphicObjects2VmConverter<VegetationSlowGoodVisibility_407>
{
    public static VegetationSlowGoodVisibility2VmConstructor Instance { get; } = new();
    private VegetationSlowGoodVisibility2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(VegetationSlowGoodVisibility_407 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new VegetationSlowGoodVisibility_407_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class VegetationWalk2VmConstructor : IGraphicObjects2VmConverter<VegetationWalk_408>
{
    public static VegetationWalk2VmConstructor Instance { get; } = new();
    private VegetationWalk2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(VegetationWalk_408 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new VegetationWalk_408_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class VegetationWalkNormalOneDir2VmConstructor : IGraphicObjects2VmConverter<VegetationWalkNormalOneDir_408_1>
{
    public static VegetationWalkNormalOneDir2VmConstructor Instance { get; } = new();
    private VegetationWalkNormalOneDir2VmConstructor() { }
    public GraphicObjectViewModel ConvertToViewModel(VegetationWalkNormalOneDir_408_1 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new VegetationWalkNormalOneDir_408_1_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class VegetationWalkSlowOneDir2VmConstructor : IGraphicObjects2VmConverter<VegetationWalkSlowOneDir_408_2>
{
    public static VegetationWalkSlowOneDir2VmConstructor Instance { get; } = new();
    private VegetationWalkSlowOneDir2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(VegetationWalkSlowOneDir_408_2 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new VegetationWalkSlowOneDir_408_2_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class VegetationWalkGoodVisibility2VmConstructor : IGraphicObjects2VmConverter<VegetationWalkGoodVisibility_409>
{
    public static VegetationWalkGoodVisibility2VmConstructor Instance { get; } = new();
    private VegetationWalkGoodVisibility2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(VegetationWalkGoodVisibility_409 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new VegetationWalkGoodVisibility_409_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class VegetationFight2VmConstructor : IGraphicObjects2VmConverter<VegetationFight_410>
{
    public static VegetationFight2VmConstructor Instance { get; } = new();
    private VegetationFight2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(VegetationFight_410 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new VegetationFight_410_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class VegetationFightNormalOneDir2VmConstructor : IGraphicObjects2VmConverter<VegetationFightNormalOneDir_410_1>
{
    public static VegetationFightNormalOneDir2VmConstructor Instance { get; } = new();
    private VegetationFightNormalOneDir2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(VegetationFightNormalOneDir_410_1 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new VegetationFightNormalOneDir_410_1_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class VegetationFightSlowOneDir2VmConstructor : IGraphicObjects2VmConverter<VegetationFightSlowOneDir_410_2>
{
    public static VegetationFightSlowOneDir2VmConstructor Instance { get; } = new();
    private VegetationFightSlowOneDir2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(VegetationFightSlowOneDir_410_2 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new VegetationFightSlowOneDir_410_2_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class VegetationFightWalkOneDir2VmConstructor : IGraphicObjects2VmConverter<VegetationFightWalkOneDir_410_3>
{
    public static VegetationFightWalkOneDir2VmConstructor Instance { get; } = new();
    private VegetationFightWalkOneDir2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(VegetationFightWalkOneDir_410_3 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new VegetationFightWalkOneDir_410_3_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class VegetationFightMinWidth2VmConstructor : IGraphicObjects2VmConverter<VegetationFightMinWidth_410_4>
{
    public static VegetationFightMinWidth2VmConstructor Instance { get; } = new();
    private VegetationFightMinWidth2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(VegetationFightMinWidth_410_4 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new VegetationFightMinWidth_410_4_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class CultivatedLandBlackPattern2VmConstructor : IGraphicObjects2VmConverter<CultivatedLandBlackPattern_412_1>
{
    public static CultivatedLandBlackPattern2VmConstructor Instance { get; } = new();
    private CultivatedLandBlackPattern2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(CultivatedLandBlackPattern_412_1 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new CultivatedLandBlackPattern_412_1_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class Orchard2VmConstructor : IGraphicObjects2VmConverter<Orchard_413>
{
    public static Orchard2VmConstructor Instance { get; } = new();
    private Orchard2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(Orchard_413 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new Orchard_413_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class OrchardRoughOpenLand2VmConstructor : IGraphicObjects2VmConverter<OrchardRoughOpenLand_413_1>
{
    public static OrchardRoughOpenLand2VmConstructor Instance { get; } = new();
    private OrchardRoughOpenLand2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(OrchardRoughOpenLand_413_1 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new OrchardRoughOpenLand_413_1_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class Vineyard2VmConstructor : IGraphicObjects2VmConverter<Vineyard_414>
{
    public static Vineyard2VmConstructor Instance { get; } = new();
    private Vineyard2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(Vineyard_414 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new Vineyard_414_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class VineyardRoughOpenLand2VmConstructor : IGraphicObjects2VmConverter<VineyardRoughOpenLand_414_1>
{
    public static VineyardRoughOpenLand2VmConstructor Instance { get; } = new();
    private VineyardRoughOpenLand2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(VineyardRoughOpenLand_414_1 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new VineyardRoughOpenLand_414_1_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class DistinctCultivationBoundary2VmConstructor : IGraphicObjects2VmConverter<DistinctCultivationBoundary_415>
{
    public static DistinctCultivationBoundary2VmConstructor Instance { get; } = new();
    private DistinctCultivationBoundary2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(DistinctCultivationBoundary_415 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new DistinctCultivationBoundary_415_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class DistinctVegetationBoundary2VmConstructor : IGraphicObjects2VmConverter<DistinctVegetationBoundary_416>
{
    public static DistinctVegetationBoundary2VmConstructor Instance { get; } = new();
    private DistinctVegetationBoundary2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(DistinctVegetationBoundary_416 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new DistinctVegetationBoundary_416_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class DistinctVegetationBoundaryGreenDashedLine2VmConstructor : IGraphicObjects2VmConverter<DistinctVegetationBoundaryGreenDashedLine_416_1>
{
    public static DistinctVegetationBoundaryGreenDashedLine2VmConstructor Instance { get; } = new();
    private DistinctVegetationBoundaryGreenDashedLine2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(DistinctVegetationBoundaryGreenDashedLine_416_1 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new DistinctVegetationBoundaryGreenDashedLine_416_1_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class ProminentLargeTree2VmConstructor : IGraphicObjects2VmConverter<ProminentLargeTree_417>
{
    public static ProminentLargeTree2VmConstructor Instance { get; } = new();
    private ProminentLargeTree2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(ProminentLargeTree_417 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new ProminentLargeTree_417_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class ProminentBush2VmConstructor : IGraphicObjects2VmConverter<ProminentBush_418>
{
    public static ProminentBush2VmConstructor Instance { get; } = new();
    private ProminentBush2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(ProminentBush_418 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new ProminentBush_418_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class ProminentVegetationFeature2VmConstructor : IGraphicObjects2VmConverter<ProminentVegetationFeature_419>
{
    public static ProminentVegetationFeature2VmConstructor Instance { get; } = new();
    private ProminentVegetationFeature2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(ProminentVegetationFeature_419 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new ProminentVegetationFeature_419_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class PavedArea2VmConstructor : IGraphicObjects2VmConverter<PavedArea_501_1>
{
    public static PavedArea2VmConstructor Instance { get; } = new();
    private PavedArea2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(PavedArea_501_1 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new PavedArea_501_1_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class PavedAreaBoundingLine2VmConstructor : IGraphicObjects2VmConverter<PavedAreaBoundingLine_501_2>
{
    public static PavedAreaBoundingLine2VmConstructor Instance { get; } = new();
    private PavedAreaBoundingLine2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(PavedAreaBoundingLine_501_2 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new PavedAreaBoundingLine_501_2_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class WideRoadMinWidth2VmConstructor : IGraphicObjects2VmConverter<WideRoadMinWidth_502>
{
    public static WideRoadMinWidth2VmConstructor Instance { get; } = new();
    private WideRoadMinWidth2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(WideRoadMinWidth_502 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new WideRoadMinWidth_502_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class RoadWithTwoCarriageways2VmConstructor : IGraphicObjects2VmConverter<RoadWithTwoCarriageways_502_2>
{
    public static RoadWithTwoCarriageways2VmConstructor Instance { get; } = new();
    private RoadWithTwoCarriageways2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(RoadWithTwoCarriageways_502_2 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new RoadWithTwoCarriageways_502_2_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class Road2VmConstructor : IGraphicObjects2VmConverter<Road_503>
{
    public static Road2VmConstructor Instance { get; } = new();
    private Road2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(Road_503 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new Road_503_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class VehicleTrack2VmConstructor : IGraphicObjects2VmConverter<VehicleTrack_504>
{
    public static VehicleTrack2VmConstructor Instance { get; } = new();
    private VehicleTrack2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(VehicleTrack_504 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new VehicleTrack_504_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class Footpath2VmConstructor : IGraphicObjects2VmConverter<Footpath_505>
{
    public static Footpath2VmConstructor Instance { get; } = new();
    private Footpath2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(Footpath_505 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new Footpath_505_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class SmallFootpath2VmConstructor : IGraphicObjects2VmConverter<SmallFootpath_506>
{
    public static SmallFootpath2VmConstructor Instance { get; } = new();
    private SmallFootpath2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(SmallFootpath_506 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new SmallFootpath_506_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class LessDistinctSmallFootpath2VmConstructor : IGraphicObjects2VmConverter<LessDistinctSmallFootpath_507>
{
    public static LessDistinctSmallFootpath2VmConstructor Instance { get; } = new();
    private LessDistinctSmallFootpath2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(LessDistinctSmallFootpath_507 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new LessDistinctSmallFootpath_507_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class NarrowRide2VmConstructor : IGraphicObjects2VmConverter<NarrowRide_508>
{
    public static NarrowRide2VmConstructor Instance { get; } = new();
    private NarrowRide2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(NarrowRide_508 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new NarrowRide_508_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class NarrowRideEasy2VmConstructor : IGraphicObjects2VmConverter<NarrowRideEasy_508_1>
{
    public static NarrowRideEasy2VmConstructor Instance { get; } = new();
    private NarrowRideEasy2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(NarrowRideEasy_508_1 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new NarrowRideEasy_508_1_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class NarrowRideNormal2VmConstructor : IGraphicObjects2VmConverter<NarrowRideNormal_508_2>
{
    public static NarrowRideNormal2VmConstructor Instance { get; } = new();
    private NarrowRideNormal2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(NarrowRideNormal_508_2 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new NarrowRideNormal_508_2_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class NarrowRideSlow2VmConstructor : IGraphicObjects2VmConverter<NarrowRideSlow_508_3>
{
    public static NarrowRideSlow2VmConstructor Instance { get; } = new();
    private NarrowRideSlow2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(NarrowRideSlow_508_3 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new NarrowRideSlow_508_3_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class NarrowRideWalk2VmConstructor : IGraphicObjects2VmConverter<NarrowRideWalk_508_4>
{
    public static NarrowRideWalk2VmConstructor Instance { get; } = new();
    private NarrowRideWalk2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(NarrowRideWalk_508_4 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new NarrowRideWalk_508_4_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class Railway2VmConstructor : IGraphicObjects2VmConverter<Railway_509>
{
    public static Railway2VmConstructor Instance { get; } = new();
    private Railway2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(Railway_509 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new Railway_509_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class PowerLine2VmConstructor : IGraphicObjects2VmConverter<PowerLine_510>
{
    public static PowerLine2VmConstructor Instance { get; } = new();
    private PowerLine2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(PowerLine_510 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new PowerLine_510_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class MajorPowerLineMinWidth2VmConstructor : IGraphicObjects2VmConverter<MajorPowerLineMinWidth_511>
{
    public static MajorPowerLineMinWidth2VmConstructor Instance { get; } = new();
    private MajorPowerLineMinWidth2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(MajorPowerLineMinWidth_511 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new MajorPowerLineMinWidth_511_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class MajorPowerLine2VmConstructor : IGraphicObjects2VmConverter<MajorPowerLine_511_1>
{
    public static MajorPowerLine2VmConstructor Instance { get; } = new();
    private MajorPowerLine2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(MajorPowerLine_511_1 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new MajorPowerLine_511_1_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class MajorPowerLineLargeCarryingMast2VmConstructor : IGraphicObjects2VmConverter<MajorPowerLineLargeCarryingMasts_511_2>
{
    public static MajorPowerLineLargeCarryingMast2VmConstructor Instance { get; } = new();
    private MajorPowerLineLargeCarryingMast2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(MajorPowerLineLargeCarryingMasts_511_2 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new MajorPowerLineLargeCarryingMasts_511_2_ViewModel(graphicsObject, mapsTopLeftVertex);

}
public class BridgeTunnel2VmConstructor : IGraphicObjects2VmConverter<BridgeTunnel_512>
{
    public static BridgeTunnel2VmConstructor Instance { get; } = new();
    private BridgeTunnel2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(BridgeTunnel_512 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new BridgeTunnel_512_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class BridgeTunnelMinSize2VmConstructor : IGraphicObjects2VmConverter<BridgeTunnelMinSize_512_1>
{
    public static BridgeTunnelMinSize2VmConstructor Instance { get; } = new();
    private BridgeTunnelMinSize2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(BridgeTunnelMinSize_512_1 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new BridgeTunnelMinSize_512_1_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class FootBridge2VmConstructor : IGraphicObjects2VmConverter<FootBridge_512_2>
{
    public static FootBridge2VmConstructor Instance { get; } = new();
    private FootBridge2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(FootBridge_512_2 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new FootBridge_512_2_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class Wall2VmConstructor : IGraphicObjects2VmConverter<Wall_513>
{
    public static Wall2VmConstructor Instance { get; } = new();
    private Wall2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(Wall_513 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new Wall_513_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class RuinedWall2VmConstructor : IGraphicObjects2VmConverter<RuinedWall_514>
{
    public static RuinedWall2VmConstructor Instance { get; } = new();
    private RuinedWall2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(RuinedWall_514 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new RuinedWall_514_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class ImpassableWall2VmConstructor : IGraphicObjects2VmConverter<ImpassableWall_515>
{
    public static ImpassableWall2VmConstructor Instance { get; } = new();
    private ImpassableWall2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(ImpassableWall_515 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new ImpassableWall_515_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class Fence2VmConstructor : IGraphicObjects2VmConverter<Fence_516>
{
    public static Fence2VmConstructor Instance { get; } = new();
    private Fence2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(Fence_516 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new Fence_516_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class RuinedFence2VmConstructor : IGraphicObjects2VmConverter<RuinedFence_517>
{
    public static RuinedFence2VmConstructor Instance { get; } = new();
    private RuinedFence2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(RuinedFence_517 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new RuinedFence_517_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class ImpassableFence2VmConstructor : IGraphicObjects2VmConverter<ImpassableFence_518>
{
    public static ImpassableFence2VmConstructor Instance { get; } = new();
    private ImpassableFence2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(ImpassableFence_518 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new ImpassableFence_518_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class CrossingPoint2VmConstructor : IGraphicObjects2VmConverter<CrossingPoint_519>
{
    public static CrossingPoint2VmConstructor Instance { get; } = new();
    private CrossingPoint2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(CrossingPoint_519 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new CrossingPoint_519_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class NotEnterableArea2VmConstructor : IGraphicObjects2VmConverter<NotEnterableArea_520>
{
    public static NotEnterableArea2VmConstructor Instance { get; } = new();
    private NotEnterableArea2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(NotEnterableArea_520 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new NotEnterableArea_520_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class NotEnterableAreaSolidColourBoundingLine2VmConstructor : IGraphicObjects2VmConverter<NotEnterableAreaSolidColourBoundingLine_520_1>
{
    public static NotEnterableAreaSolidColourBoundingLine2VmConstructor Instance { get; } = new();
    private NotEnterableAreaSolidColourBoundingLine2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(NotEnterableAreaSolidColourBoundingLine_520_1 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new NotEnterableAreaSolidColourBoundingLine_520_1_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class NotEnterableAreaStripes2VmConstructor : IGraphicObjects2VmConverter<NotEnterableAreaStripes_502_2>
{
    public static NotEnterableAreaStripes2VmConstructor Instance { get; } = new();
    private NotEnterableAreaStripes2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(NotEnterableAreaStripes_502_2 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new NotEnterableAreaStripes_502_2_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class NotEnterableAreaStripesBoundingLine2VmConstructor : IGraphicObjects2VmConverter<NotEnterableAreaStripesBoundingLine_520_3>
{
    public static NotEnterableAreaStripesBoundingLine2VmConstructor Instance { get; } = new();
    private NotEnterableAreaStripesBoundingLine2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(NotEnterableAreaStripesBoundingLine_520_3 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new NotEnterableAreaStripesBoundingLine_520_3_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class Building2VmConstructor : IGraphicObjects2VmConverter<Building_521>
{
    public static Building2VmConstructor Instance { get; } = new();
    private Building2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(Building_521 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new Building_521_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class BuildingMinSize2VmConstructor : IGraphicObjects2VmConverter<BuildingMinSize_521_1>
{
    public static BuildingMinSize2VmConstructor Instance { get; } = new();
    private BuildingMinSize2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(BuildingMinSize_521_1 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new BuildingMinSize_521_1_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class LargeBuilding2VmConstructor : IGraphicObjects2VmConverter<LargeBuilding_521_3>
{
    public static LargeBuilding2VmConstructor Instance { get; } = new();
    private LargeBuilding2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(LargeBuilding_521_3 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new LargeBuilding_521_3_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class LargeBuildingOutline2VmConstructor : IGraphicObjects2VmConverter<LargeBuildingOutline_521_4>
{
    public static LargeBuildingOutline2VmConstructor Instance { get; } = new();
    private LargeBuildingOutline2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(LargeBuildingOutline_521_4 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new LargeBuildingOutline_521_4_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class Canopy2VmConstructor : IGraphicObjects2VmConverter<Canopy_522_1>
{
    public static Canopy2VmConstructor Instance { get; } = new();
    private Canopy2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(Canopy_522_1 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new Canopy_522_1_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class CanopyOutline2VmConstructor : IGraphicObjects2VmConverter<CanopyOutline_522_2>
{
    public static CanopyOutline2VmConstructor Instance { get; } = new();
    private CanopyOutline2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(CanopyOutline_522_2 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new CanopyOutline_522_2_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class Ruin2VmConstructor : IGraphicObjects2VmConverter<Ruin_523>
{
    public static Ruin2VmConstructor Instance { get; } = new();
    private Ruin2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(Ruin_523 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new Ruin_523_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class RuinMinSize2VmConstructor : IGraphicObjects2VmConverter<RuinMinSize_523_1>
{
    public static RuinMinSize2VmConstructor Instance { get; } = new();
    private RuinMinSize2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(RuinMinSize_523_1 graphicsObject, MapCoordinates mapsTopLeftVertex)
        => new RuinMinSize_523_1_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class HighTower2VmConstructor : IGraphicObjects2VmConverter<HighTower_524>
{
    public static HighTower2VmConstructor Instance { get; } = new();
    private HighTower2VmConstructor (){ }
    public GraphicObjectViewModel ConvertToViewModel(HighTower_524 graphicsObject, MapCoordinates mapsTopLeftVertex)
        => new HighTower_524_ViewModel(graphicsObject, mapsTopLeftVertex); 

}
public class SmallTower2VmConstructor : IGraphicObjects2VmConverter<SmallTower_525>
{
    public static SmallTower2VmConstructor Instance { get; } = new();
    private SmallTower2VmConstructor (){ }
    public GraphicObjectViewModel ConvertToViewModel(SmallTower_525 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new SmallTower_525_ViewModel(graphicsObject, mapsTopLeftVertex); 
}
public class Cairn2VmConstructor : IGraphicObjects2VmConverter<Cairn_526>
{
    public static Cairn2VmConstructor Instance { get; } = new();
    private Cairn2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(Cairn_526 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new Cairn_526_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class FodderRack2VmConstructor : IGraphicObjects2VmConverter<FodderRack_527>
{
    public static FodderRack2VmConstructor Instance { get; } = new();
    private FodderRack2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(FodderRack_527 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new FodderRack_527_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class ProminentLineFeature2VmConstructor : IGraphicObjects2VmConverter<ProminentLineFeature_528>
{
    public static ProminentLineFeature2VmConstructor Instance { get; } = new();
    private ProminentLineFeature2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(ProminentLineFeature_528 graphicsObject, MapCoordinates mapsTopLeftVertex)
        => new ProminentLineFeature_528_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class ProminentImpassableLineFeature2VmConstructor : IGraphicObjects2VmConverter<ProminentImpassableLineFeature_529>
{
    public static ProminentImpassableLineFeature2VmConstructor Instance { get; } = new();
    private ProminentImpassableLineFeature2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(ProminentImpassableLineFeature_529 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new ProminentImpassableLineFeature_529_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class ProminentManMadeFeatureRing2VmConstructor : IGraphicObjects2VmConverter<ProminentManMadeFeatureRing_530>
{
    public static ProminentManMadeFeatureRing2VmConstructor Instance { get; } = new();
    private ProminentManMadeFeatureRing2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(ProminentManMadeFeatureRing_530 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new ProminentManMadeFeatureRing_530_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class ProminentManMadeFeatureX2VmConstructor : IGraphicObjects2VmConverter<ProminentManMadeFeatureX_531>
{
    public static ProminentManMadeFeatureX2VmConstructor Instance { get; } = new();
    private ProminentManMadeFeatureX2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(ProminentManMadeFeatureX_531 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new ProminentManMadeFeatureX_531_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class Stairway2VmConstructor : IGraphicObjects2VmConverter<Stairway_532>
{
    public static Stairway2VmConstructor Instance { get; } = new();
    private Stairway2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(Stairway_532 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new Stairway_532_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class StairWayWithoutBorderLines2VmConstructor : IGraphicObjects2VmConverter<StairWayWithoutBorderLines_532_1>
{
    public static StairWayWithoutBorderLines2VmConstructor Instance { get; } = new();
    private StairWayWithoutBorderLines2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(StairWayWithoutBorderLines_532_1 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new StairWayWithoutBorderLines_532_1_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class MagneticNorthLine2VmConstructor : IGraphicObjects2VmConverter<MagneticNorthLine_601_1>
{
    public static MagneticNorthLine2VmConstructor Instance { get; } = new();
    private MagneticNorthLine2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(MagneticNorthLine_601_1 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new MagneticNorthLine_601_1_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class NorthLinesPattern2VmConstructor : IGraphicObjects2VmConverter<NorthLinesPattern_601_2>
{
    public static NorthLinesPattern2VmConstructor Instance { get; } = new();
    private NorthLinesPattern2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(NorthLinesPattern_601_2 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new NorthLinesPattern_601_2_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class MagneticNorthLineBlue2VmConstructor : IGraphicObjects2VmConverter<MagneticNorthLineBlue_601_3>
{
    public static MagneticNorthLineBlue2VmConstructor Instance { get; } = new();
    private MagneticNorthLineBlue2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(MagneticNorthLineBlue_601_3 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new MagneticNorthLineBlue_601_3_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class NorthLinesPatternBlue2VmConstructor : IGraphicObjects2VmConverter<NorthLinesPatternBlue_601_4>
{
    public static NorthLinesPatternBlue2VmConstructor Instance { get; } = new();
    private NorthLinesPatternBlue2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(NorthLinesPatternBlue_601_4 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new NorthLinesPatternBlue_601_4_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class RegistrationMark2VmConstructor : IGraphicObjects2VmConverter<RegistrationMark_602>
{
    public static RegistrationMark2VmConstructor Instance { get; } = new();
    private RegistrationMark2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(RegistrationMark_602 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new RegistrationMark_602_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class SpotHeightDot2VmConstructor : IGraphicObjects2VmConverter<SpotHeightDot_603>
{
    public static SpotHeightDot2VmConstructor Instance { get; } = new();
    private SpotHeightDot2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(SpotHeightDot_603 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new SpotHeightDot_603_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class Start2VmConstructor : IGraphicObjects2VmConverter<Start_701>
{
    public static Start2VmConstructor Instance { get; } = new();
    private Start2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(Start_701 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new Start_701_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class ControlPoint2VmConstructor : IGraphicObjects2VmConverter<ControlPoint_703>
{
    public static ControlPoint2VmConstructor Instance { get; } = new();
    private ControlPoint2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(ControlPoint_703 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new ControlPoint_703_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class CourseLine2VmConstructor : IGraphicObjects2VmConverter<CourseLine_705>
{
    public static CourseLine2VmConstructor Instance { get; } = new();
    private CourseLine2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(CourseLine_705 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new CourseLine_705_ViewModel(graphicsObject, mapsTopLeftVertex);
}
public class Finish2VmConstructor : IGraphicObjects2VmConverter<Finish_706>
{
    public static Finish2VmConstructor Instance { get; } = new();
    private Finish2VmConstructor () { }
    public GraphicObjectViewModel ConvertToViewModel(Finish_706 graphicsObject, MapCoordinates mapsTopLeftVertex)
    => new Finish_706_ViewModel(graphicsObject, mapsTopLeftVertex);
}
