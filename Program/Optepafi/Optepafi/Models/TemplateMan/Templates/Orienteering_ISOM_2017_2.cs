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
    public enum Grounds {BrokenGround_113 = 1, VeryBrokenGround_114}
    // 2 bits
    public enum Boulders {GiganticBoulder_206 = 1, BoulderField_208, DenseBoulderField_209}
    // 3 bits
    public enum Stones {StonyGroundSlow_210 = 1, StonyGroundWalk_211, StonyGroundFight_212, Sandyground_213, BareRock_214}
    // 3 bits
    public enum Water {UncrossableBodyOfWater_301 = 1, ShallowBodyOfWater_302, UncrossableMarsh_307, Marsh_308, IndistinctMarsh_310}
    // 4 bits
    public enum VegetationAndManMade {OpenLand_401 = 1, OpenLandWithTrees_402, RoughOpenLand_403, RoughOpenLandWithTrees_404, Forest_405, VegetationSlow_406, VegetationWalk_408, VegetationFight_410, CultivatedLand_412, Orchard_413, Vineyard_414, PavedArea_501, AreaTahtShallNotBeEntered_520, Building_521}
    // 2 bits
    public enum VegetationGoodVis {VegetationSlowGoodVis_407 = 1, VegetationWalkGoodVis_409}
    
    // 3 bits
    public enum NaturalLinearObstacles {EarthBank_104 = 1, EarthWall_105, ErosionGully_107, ImapssableCliff_201, Cliff_202, Trench_215, CrossableWatercourse_304}
    // 3 bits
    public enum Paths {WideRoad_502 = 1, Road_503, VehicleTrack_504, Footpath_505, SmallFootpath_506, LessDistinctSmallFootpath_507, NarrowRide_508}
    // 3 bits
    public enum ManMadeLinearObstacles {Wall_513 = 1, RuinedWall_514, ImpassableWall_515, Fence_516, ImpassableFence_518, ProminentLineFeature_528, ImpassableProminentLineFeature_529, Stairway_532}

    public struct VertexAttributes(MapCoordinates position, short elevation = 0) : IVertexAttributes
    {
        public readonly MapCoordinates Position = position;
        public readonly short Elevation = elevation;
    }

    public struct EdgeAttributes((Grounds? ground, Boulders? boulders, Stones? stones, Water? water, VegetationAndManMade? vegetationAndManMade, VegetationGoodVis? vegetationGoodVis) surrounding, 
                                             (NaturalLinearObstacles? naturalLinearObstacle, Paths? path, ManMadeLinearObstacles? manMadeLinearObstacle) linearFeatures,
                                             (Grounds? ground, Boulders? boulders, Stones? stones, Water? water, VegetationAndManMade? vegetationAndManMade, VegetationGoodVis? vegetationGoodVis)? secondSurrounding = null) : IEdgeAttributes
    {
        private readonly ushort _surroundings = (ushort)(((((((((((int)(surrounding.ground ?? 0) << 2) + 
                                                                 (int)(surrounding.boulders ?? 0)) << 3) + 
                                                               (int)(surrounding.stones ?? 0)) << 3) + 
                                                             (int)(surrounding.water ?? 0)) << 4) + 
                                                           (int)(surrounding.vegetationAndManMade ?? 0)) << 2) + 
                                                         (int)(surrounding.vegetationGoodVis ?? 0));
        
        private readonly ushort _secondSurroundings = (ushort) (secondSurrounding is null ? 0
            : ((((((((((int)(secondSurrounding.Value.ground ?? 0) << 2) +
                      (int)(secondSurrounding.Value.boulders ?? 0)) << 3) +
                    (int)(secondSurrounding.Value.stones ?? 0)) << 3) +
                  (int)(secondSurrounding.Value.water ?? 0)) << 4) +
                (int)(secondSurrounding.Value.vegetationAndManMade ?? 0)) << 2) +
              (int)(secondSurrounding.Value.vegetationGoodVis ?? 0));
        
        private readonly ushort _lineFeatures = (ushort)(((((int)(linearFeatures.naturalLinearObstacle ?? 0) << 3) + 
                                                           (int)(linearFeatures.path ?? 0)) << 3) + 
                                                         (int)(linearFeatures.manMadeLinearObstacle ?? 0));
        public (Grounds? ground, Boulders? boulders, Stones? stones, Water? water, 
            VegetationAndManMade? vegetationAndManMade, VegetationGoodVis? vegetationGoodVis) Surroundings
        {
            get
            {
                VegetationGoodVis? vegetationGoodVis = (_surroundings & 0x11) == 0 ? (VegetationGoodVis)(_surroundings & 0x11) : null;
                VegetationAndManMade? vegetationAndManMade = ((_surroundings >> 2) & 0x1111) == 0 ? (VegetationAndManMade)((_surroundings >> 2) & 0x1111) : null;
                Water? water = ((_surroundings >> 6) & 0x111) == 0 ? (Water)((_surroundings >> 6) & 0x111) : null;
                Stones? stones = ((_surroundings >> 9) & 0x111) == 0 ? (Stones)((_surroundings >> 9) & 0x111) : null;
                Boulders? boulders = ((_surroundings >> 12) & 0x11) == 0 ? (Boulders)((_surroundings >> 12) & 0x11) : null;
                Grounds? ground =  ((_surroundings >> 14) & 0x11) == 0 ? (Grounds)((_surroundings >> 14) & 0x11) : null;
                return (ground, boulders, stones, water, vegetationAndManMade, vegetationGoodVis);
            }
        }

        public (Grounds? ground, Boulders? boulders, Stones? stones, Water? water,
            VegetationAndManMade? vegetationAndManMade, VegetationGoodVis? vegetationGoodVis)? SecondSurroundings
        {
            get
            {
                if (_secondSurroundings == 0) return null;
                VegetationGoodVis? vegetationGoodVis = (_surroundings & 0x11) == 0 ? (VegetationGoodVis)(_surroundings & 0x11) : null;
                VegetationAndManMade? vegetationAndManMade = ((_surroundings >> 2) & 0x1111) == 0 ? (VegetationAndManMade)((_surroundings >> 2) & 0x1111) : null;
                Water? water = ((_surroundings >> 6) & 0x111) == 0 ? (Water)((_surroundings >> 6) & 0x111) : null;
                Stones? stones = ((_surroundings >> 9) & 0x111) == 0 ? (Stones)((_surroundings >> 9) & 0x111) : null;
                Boulders? boulders = ((_surroundings >> 12) & 0x11) == 0 ? (Boulders)((_surroundings >> 12) & 0x11) : null;
                Grounds? ground = ((_surroundings >> 14) & 0x11) == 0 ? (Grounds)((_surroundings >> 14) & 0x11) : null;
                return (ground, boulders, stones, water, vegetationAndManMade, vegetationGoodVis);
            }
        }

        public (NaturalLinearObstacles? naturalLinearObstacle, Paths? path, 
            ManMadeLinearObstacles? manMadeLinearObstacle) LineFeatures
        {
            get
            {
                ManMadeLinearObstacles? manMadeLinearObstacles = (_lineFeatures & 0x111) == 0 ? (ManMadeLinearObstacles?)(_lineFeatures & 0x111) : null;
                Paths? paths = ((_lineFeatures >> 3) & 0x111) == 0 ? (Paths?)((_lineFeatures >> 3) & 0x111) : null;
                NaturalLinearObstacles? naturalLinearObstacle = ((_lineFeatures >> 6) & 0x111) == 0 ? (NaturalLinearObstacles?)((_lineFeatures >> 6) & 0x111) : null;
                return (naturalLinearObstacle, paths, manMadeLinearObstacles);
            }
        }
    }
}