using System.Dynamic;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.Utils;

namespace Optepafi.Models.TemplateMan.Templates;

//TODO: comment
public class Orienteering_ISOM_2017_2 : ITemplate<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>
{
    public static Orienteering_ISOM_2017_2 Instance { get; } = new();
    private Orienteering_ISOM_2017_2() {}
    public string TemplateName { get; } = "Orienteering (ISOM 2017-2)";

    public TOut AcceptGeneric<TOut, TGenericParam, TConstraint, TOtherParams>(ITemplateGenericVisitor<TOut, TConstraint, TOtherParams> genericVisitor,
        TGenericParam genericParam, TOtherParams otherParams) where TGenericParam : TConstraint
    {
        return genericVisitor.GenericVisit<Orienteering_ISOM_2017_2, VertexAttributes, EdgeAttributes, TGenericParam>(
            this, genericParam, otherParams);
    }
    
    public TOut AcceptGeneric<TOut, TOtherParams>(ITemplateGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams)
    {
        return  genericVisitor.GenericVisit<Orienteering_ISOM_2017_2,VertexAttributes, EdgeAttributes>(this, otherParams);
    }

    public TOut AcceptGeneric<TOut>(ITemplateGenericVisitor<TOut> genericVisitor)
    {
        return genericVisitor.GenericVisit<Orienteering_ISOM_2017_2,VertexAttributes,EdgeAttributes>(this);
    }

    public void AcceptGeneric(ITemplateGenericVisitor genericVisitor)
    {
        genericVisitor.GenericVisit<Orienteering_ISOM_2017_2,VertexAttributes,EdgeAttributes>(this);
    }

    
    // 2 bits
    public enum Grounds {BrokenGround_113, VeryBrokenGround_114}
    // 2 bits
    public enum Boulders {GiganticBoulder_206, BoulderField_208, DenseBoulderField_209}
    // 3 bits
    public enum Stones {StonyGroundSlow_210, StonyGroundWalk_211, StonyGroundFight_212, Sandyground_213, BareRock_214}
    // 3 bits
    public enum Water {UncrossableBodyOfWater_301, ShallowBodyOfWater_302, UncrossableMarsh_307, Marsh_308, IndistinctMarsh_310}
    // 4 bits
    public enum VegetationAndManMade {OpenLand_401, OpenLandWithTrees_402, RoughOpenLand_403, RoughOpenLandWithTrees_404, Forest_405, VegetationSlow_406, VegetationWalk_408, VegetationFight_410, CultivatedLand_412, Orchard_413, Vineyard_414, PavedArea_501, AreaTahtShallNotBeEntered_520, Building_521}
    // 2 bits
    public enum VegetationGoodVis {VegetationSlowGoodVis_407, VegetationWalkGoodVis_409}
    
    // 3 bits
    public enum NaturalLinearObstacles {EarthBank_104, EarthWall_105, ErosionGully_107, ImapssableCliff_201, Cliff_202, Trench_215, CrossableWatercourse_304}
    // 3 bits
    public enum Paths {WideRoad_502, Road_503, VehicleTrack_504, Footpath_505, SmallFootpath_506, LessDistinctSmallFootpath_507, NarrowRide_508}
    // 3 bits
    public enum ManMadeLinearObstacles {Wall_513, RuinedWall_514, ImpassableWall_515, Fence_516, ImpassableFence_518, ProminentLineFeature_528, ImpassableProminentLineFeature_529, Stairway_532}
    
    public record struct VertexAttributes(
        MapCoordinates Position, 
        short Elevation) : IVertexAttributes { }

    public record struct EdgeAttributes(
        ushort Surrounding,
        ushort SecondSurrounding,
        ushort LineFeatures) : IEdgeAttributes { }
}