using System.Dynamic;
using Microsoft.VisualBasic.CompilerServices;
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
    public enum Grounds {BrokenGround_113 = 1, VeryBrokenGround_114, CultivatedLand_412}
    // 2 bits
    public enum Boulders {GiganticBoulder_206 = 1, BoulderField_208, DenseBoulderField_209}
    // 3 bits
    public enum Stones {StonyGroundSlow_210 = 1, StonyGroundWalk_211, StonyGroundFight_212, SandyGround_213, BareRock_214}
    // 3 bits
    public enum Water {UncrossableBodyOfWater_301 = 1, ShallowBodyOfWater_302, UncrossableMarsh_307, Marsh_308, IndistinctMarsh_310}
    // 4 bits
    public enum VegetationAndManMade {OpenLand_401 = 1, OpenLandWithTrees_402, RoughOpenLand_403, RoughOpenLandWithTrees_404, Forest_405, VegetationSlow_406, VegetationWalk_408, VegetationFight_410, Orchard_413, RoughOrchard_413, Vineyard_414, RoughVineyard_414, PavedArea_501, AreaThatShallNotBeEntered_520, Building_521}
    // 2 bits
    public enum VegetationGoodVis {VegetationSlowGoodVis_407 = 1, VegetationWalkGoodVis_409}
    
    // 3 bits
    public enum NaturalLinearObstacles {EarthBank_104 = 1, EarthWall_105, ErosionGully_107, ImpassableCliff_201, Cliff_202, CrossableWatercourse_304, SmallCrossableWatercourse_305}
    // 4 bits
    public enum Paths {WideRoad_502 = 1, Road_503, VehicleTrack_504, Footpath_505, SmallFootpath_506, LessDistinctSmallFootpath_507, NarrowRide_508, Stairway_532}
    // 3 bits
    public enum ManMadeLinearObstacles {Trench_215 = 1, Wall_513, ImpassableWall_515, Fence_516, ImpassableFence_518, ProminentLineFeature_528, ImpassableProminentLineFeature_529}

    public struct VertexAttributes(MapCoordinates position, short elevation = 0) : IVertexAttributes
    {
        public readonly MapCoordinates Position = position;
        public readonly short Elevation = elevation;
    }

    public struct EdgeAttributes((Grounds? ground, Boulders? boulders, Stones? stones, Water? water, VegetationAndManMade? vegetationAndManMade, VegetationGoodVis? vegetationGoodVis) leftSurrounding, 
                                             (NaturalLinearObstacles? naturalLinearObstacle, Paths? path, ManMadeLinearObstacles? manMadeLinearObstacle) linearFeatures,
                                             (Grounds? ground, Boulders? boulders, Stones? stones, Water? water, VegetationAndManMade? vegetationAndManMade, VegetationGoodVis? vegetationGoodVis) rightSurrounding) : IEdgeAttributes
    {
        public static bool operator==(EdgeAttributes left, EdgeAttributes right) => left._leftSurroundings == right._leftSurroundings && left._rightSurroundings == right._rightSurroundings && left._linearFeatures == right._linearFeatures;
        public static bool operator!=(EdgeAttributes left, EdgeAttributes right) => !(left == right);
        public override int GetHashCode() => (_leftSurroundings, _rightSurroundings, _linearFeatures).GetHashCode();

        public EdgeAttributes((Grounds? ground, Boulders? boulders, Stones? stones, Water? water, VegetationAndManMade? vegetationAndManMade, VegetationGoodVis? vegetationGoodVis) bothSurroundings, (NaturalLinearObstacles? naturalLinearObstacle, Paths? path, ManMadeLinearObstacles? manMadeLinearObstacle) linearFeatures) : this( (bothSurroundings.ground, bothSurroundings.boulders, bothSurroundings.stones, bothSurroundings.water, bothSurroundings.vegetationAndManMade, bothSurroundings.vegetationGoodVis), (linearFeatures.naturalLinearObstacle, linearFeatures.path, linearFeatures.manMadeLinearObstacle), (bothSurroundings.ground, bothSurroundings.boulders, bothSurroundings.stones, bothSurroundings.water, bothSurroundings.vegetationAndManMade, bothSurroundings.vegetationGoodVis)){}
        public EdgeAttributes((Grounds? ground, Boulders? boulders, Stones? stones, Water? water, VegetationAndManMade? vegetationAndManMade, VegetationGoodVis? vegetationGoodVis) bothSurroundings) : this((bothSurroundings.ground, bothSurroundings.boulders, bothSurroundings.stones, bothSurroundings.water, bothSurroundings.vegetationAndManMade, bothSurroundings.vegetationGoodVis), (null, null, null)){}
        public EdgeAttributes((Grounds? ground, Boulders? boulders, Stones? stones, Water? water, VegetationAndManMade? vegetationAndManMade, VegetationGoodVis? vegetationGoodVis) leftSurrounding, (Grounds? ground, Boulders? boulders, Stones? stones, Water? water, VegetationAndManMade? vegetationAndManMade, VegetationGoodVis? vegetationGoodVis) rightSurroundings) : this((leftSurrounding.ground, leftSurrounding.boulders, leftSurrounding.stones, leftSurrounding.water, leftSurrounding.vegetationAndManMade, leftSurrounding.vegetationGoodVis), (null, null, null), (rightSurroundings.ground, rightSurroundings.boulders, rightSurroundings.stones, rightSurroundings.water, rightSurroundings.vegetationAndManMade, rightSurroundings.vegetationGoodVis)){}
        public EdgeAttributes():this((null, null, null, null, null, null), (null, null, null)){}
        
        private readonly ushort _leftSurroundings = (ushort)(((((((((((int)(leftSurrounding.ground ?? 0) << 2) + 
                                                                 (int)(leftSurrounding.boulders ?? 0)) << 3) + 
                                                               (int)(leftSurrounding.stones ?? 0)) << 3) + 
                                                             (int)(leftSurrounding.water ?? 0)) << 4) + 
                                                           (int)(leftSurrounding.vegetationAndManMade ?? 0)) << 2) + 
                                                         (int)(leftSurrounding.vegetationGoodVis ?? 0));
        
        private readonly ushort _rightSurroundings = (ushort) (((((((((((int)(rightSurrounding.ground ?? 0) << 2) + 
                                                                        (int)(rightSurrounding.boulders ?? 0)) << 3) + 
                                                                      (int)(rightSurrounding.stones ?? 0)) << 3) + 
                                                                    (int)(rightSurrounding.water ?? 0)) << 4) + 
                                                                  (int)(rightSurrounding.vegetationAndManMade ?? 0)) << 2) + 
                                                                (int)(rightSurrounding.vegetationGoodVis ?? 0));
        
        private readonly ushort _linearFeatures = (ushort)(((((int)(linearFeatures.naturalLinearObstacle ?? 0) << 4) + 
                                                           (int)(linearFeatures.path ?? 0)) << 3) + 
                                                         (int)(linearFeatures.manMadeLinearObstacle ?? 0));
        public (Grounds? ground, Boulders? boulders, Stones? stones, Water? water, 
            VegetationAndManMade? vegetationAndManMade, VegetationGoodVis? vegetationGoodVis) LeftSurroundings
        {
            get
            {
                VegetationGoodVis? vegetationGoodVis = (_leftSurroundings & 0b11) != 0 ? (VegetationGoodVis)(_leftSurroundings & 0b11) : null;
                VegetationAndManMade? vegetationAndManMade = ((_leftSurroundings >> 2) & 0b1111) != 0 ? (VegetationAndManMade)((_leftSurroundings >> 2) & 0b1111) : null;
                Water? water = ((_leftSurroundings >> 6) & 0b111) != 0 ? (Water)((_leftSurroundings >> 6) & 0b111) : null;
                Stones? stones = ((_leftSurroundings >> 9) & 0b111) != 0 ? (Stones)((_leftSurroundings >> 9) & 0b111) : null;
                Boulders? boulders = ((_leftSurroundings >> 12) & 0b11) != 0 ? (Boulders)((_leftSurroundings >> 12) & 0b11) : null;
                Grounds? ground =  ((_leftSurroundings >> 14) & 0b11) != 0 ? (Grounds)((_leftSurroundings >> 14) & 0b11) : null;
                return (ground, boulders, stones, water, vegetationAndManMade, vegetationGoodVis);
            }
        }

        public (Grounds? ground, Boulders? boulders, Stones? stones, Water? water,
            VegetationAndManMade? vegetationAndManMade, VegetationGoodVis? vegetationGoodVis) RightSurroundings
        {
            get
            {
                VegetationGoodVis? vegetationGoodVis = (_rightSurroundings & 0b11) != 0 ? (VegetationGoodVis)(_rightSurroundings & 0b11) : null;
                VegetationAndManMade? vegetationAndManMade = ((_rightSurroundings >> 2) & 0b1111) != 0 ? (VegetationAndManMade)((_rightSurroundings >> 2) & 0b1111) : null;
                Water? water = ((_rightSurroundings >> 6) & 0b111) != 0 ? (Water)((_rightSurroundings >> 6) & 0b111) : null;
                Stones? stones = ((_rightSurroundings >> 9) & 0b111) != 0 ? (Stones)((_rightSurroundings >> 9) & 0b111) : null;
                Boulders? boulders = ((_rightSurroundings >> 12) & 0b11) != 0 ? (Boulders)((_rightSurroundings >> 12) & 0b11) : null;
                Grounds? ground = ((_rightSurroundings >> 14) & 0b11) != 0 ? (Grounds)((_rightSurroundings >> 14) & 0b11) : null;
                return (ground, boulders, stones, water, vegetationAndManMade, vegetationGoodVis);
            }
        }

        public (NaturalLinearObstacles? naturalLinearObstacle, Paths? path, 
            ManMadeLinearObstacles? manMadeLinearObstacle) LineFeatures
        {
            get
            {
                ManMadeLinearObstacles? manMadeLinearObstacles = (_linearFeatures & 0b111) != 0 ? (ManMadeLinearObstacles?)(_linearFeatures & 0b111) : null;
                Paths? paths = ((_linearFeatures >> 3) & 0b1111) != 0 ? (Paths?)((_linearFeatures >> 3) & 0b1111) : null;
                NaturalLinearObstacles? naturalLinearObstacle = ((_linearFeatures >> 7) & 0b111) != 0 ? (NaturalLinearObstacles?)((_linearFeatures >> 7) & 0b111) : null;
                return (naturalLinearObstacle, paths, manMadeLinearObstacles);
            }
        }
    }
}