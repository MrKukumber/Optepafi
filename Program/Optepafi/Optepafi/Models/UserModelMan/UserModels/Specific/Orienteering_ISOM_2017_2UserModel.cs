using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Text.Json;
using System.Text.Json.Serialization;
using Optepafi.Models.GraphicsMan.Objects.Map;
using Optepafi.Models.TemplateMan.Templates;
using Optepafi.Models.UserModelMan.UserModels.Functionalities;
using Optepafi.Models.Utils;

namespace Optepafi.Models.UserModelMan.UserModels.Specific;

//TODO: comment
public class Orienteering_ISOM_2017_2UserModel : 
    IWeightComputing<Orienteering_ISOM_2017_2, Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>,
    IAStarHeuristicsComputing<Orienteering_ISOM_2017_2, Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>,
    IPositionRetrieving<Orienteering_ISOM_2017_2, Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>
{
    [JsonIgnore]
    public Orienteering_ISOM_2017_2 AssociatedTemplate => Orienteering_ISOM_2017_2.Instance;
    [JsonIgnore]
    public string FilePath { get; set; } = "";

    public Orienteering_ISOM_2017_2UserModel() { }
    public Orienteering_ISOM_2017_2UserModel(bool newUserModel)
    {
        if(newUserModel)
        {
            BrokenGroundCoefficient = new(0.95f);
            VeryBrokenGroundCoefficient = new(0.8f);
            CultivatedLandCoefficient = new(0.8f);
            GiganticBoulderCoefficient = new(0);
            BoulderFieldCoefficient = new(0.7f);
            DenseBoulderFieldCoefficient = new(0.55f);
            StonyGroundSlowCoefficient = new(0.65f);
            StonyGroundWalkCoefficient = new(0.5f);
            StonyGroundFightCoefficient = new(0.35f);
            SandyGroundCoefficient = new(0.7f);
            BareRockCoefficient = new(0.95f);
            UncrossableBodyOfWaterCoefficient =new (0);
            ShallowBodyOfWaterCoefficient = new(0.5f);
            UncrossableMarshCoefficient = new(0);
            MarshCoefficient = new(0.8f);
            IndistinctMarshCoefficient = new(0.9f);
            OpenLandCoefficient = new(0.97f);
            OpenLandWithTreesCoefficient = new(0.93f);
            RoughOpenLandCoefficient = new(0.77f);
            RoughOpenLandWithTreesCoefficient = new(0.7f);
            ForestCoefficient = new(0.9f);
            VegetationSlowCoefficient = new(0.7f);
            VegetationWalkCoefficient = new(0.45f);
            VegetationFightCoefficient = new(0.2f);
            OrchardCoefficient = new(0.93f);
            RoughOrchardCoefficient = new(0.8f);
            VineyardCoefficient = new(0.85f);
            RoughVineyardCoefficient = new(0.7f);
            PavedAreaCoefficient = new (1);
            AreaThatShallNotBeEnteredCoefficient = new (0);
            BuildingCoefficient = new(0);
            VegetationSlowGoodVisibilityCoefficient = new(0.8f);
            VegetationWalkGoodVisibilityCoefficient = new(0.6f);
            EarthBankOvercomeCoefficient = new(2);
            EarthWallOvercomeCoefficient = new(2);
            ErosionGullyOvercomeCoefficient = new(5);
            ImpassableCliffOvercomeCoefficient = new (1000_000);
            CliffOvercomeCoefficient = new(7);
            CrossableWaterCourseOvercomeCoefficient = new(3);
            SmallCrossableWaterCourseOvercomeCoefficient = new(2);
            WideRoadCoefficient = new(1);
            RoadCoefficient = new(1);
            VehicleTrackCoefficient = new(1);
            FootpathCoefficient = new (0.99f);
            SmallFootpathCoefficient = new (0.98f);
            LessDistinctSmallFootpathCoefficient = new (1.07f);
            NarrowRideCoefficient = new(1.07f);
            StairwayCoefficient = new(0.93f);
            TrenchOvercomeCoefficient = new(1);
            WallOvercomeCoefficient = new(2);
            ImpassableWallOvercomeCoefficient = new (1000_000);
            FenceOvercomeCoefficient = new(2);
            ImpassableFenceOvercomeCoefficient = new (1000_000);
            ProminentLineFeatureOvercomeCoefficient = new (1.5f);
            ImpassableProminentLineFeatureOvercomeCoefficient = new (1000_000);
        }
    }
    
    public string Serialize()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions { NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals});
    }

    public void SerializeTo(Stream stream)
    {
        JsonSerializer.Serialize(stream, this);
    }

    private const float coefficientLowerBound = 0.15f;
    private const float topPaceInSecondsPerCentimeter = 0.0024f;
    public float ComputeWeight(Orienteering_ISOM_2017_2.VertexAttributes from, Orienteering_ISOM_2017_2.EdgeAttributes through, Orienteering_ISOM_2017_2.VertexAttributes to)
    {
        var surroundingsCoef = ComputeCoeficientOfSurroundings(through.LeftSurroundings);
        var secondSurroundingsCoef = ComputeCoeficientOfSurroundings(through.RightSurroundings);
        var moreEfficientSurroundingsCoef = Math.Max(surroundingsCoef, secondSurroundingsCoef);
        if (moreEfficientSurroundingsCoef < 0.000000001) return float.PositiveInfinity; 
        
        var moreEfficientSurroundingsWithPathCoef = ApplyPathToSurroundingsCoef(through.LineFeatures, moreEfficientSurroundingsCoef);
        if (moreEfficientSurroundingsWithPathCoef < coefficientLowerBound) moreEfficientSurroundingsWithPathCoef = coefficientLowerBound;
        
        var fromToTimeInMoreEfficientSurroundingsWithPath = (float)(topPaceInSecondsPerCentimeter / moreEfficientSurroundingsWithPathCoef * (to.Position - from.Position).Length());
        return AddTimeForLinearObstacles(through.LineFeatures, fromToTimeInMoreEfficientSurroundingsWithPath);
    }

    private float ComputeCoeficientOfSurroundings((Orienteering_ISOM_2017_2.Grounds?, Orienteering_ISOM_2017_2.Boulders?, Orienteering_ISOM_2017_2.Stones?, Orienteering_ISOM_2017_2.Water?, Orienteering_ISOM_2017_2.VegetationAndManMade?, Orienteering_ISOM_2017_2.VegetationGoodVis?) surroundings)
    {
        var (ground, boulders, stones, water, vegetationAndManMade, vegetationGoodVis) = surroundings;
        return (ground is not null ? GroundMappings[ground.Value].Value : 1) *
               (boulders is not null ? BoulderMappings[boulders.Value].Value : 1) *
               (stones is not null ? StoneMappings[stones.Value].Value : 1) *
               (water is not null ? WaterMappings[water.Value].Value : 1) *
               (vegetationAndManMade is not null ? VegetationAndManMadeMappings[vegetationAndManMade.Value].Value : 1) *
               (vegetationGoodVis is not null ? VegetationGoodVisMappings[vegetationGoodVis.Value].Value : 1);
    }

    private float ApplyPathToSurroundingsCoef((Orienteering_ISOM_2017_2.NaturalLinearObstacles?, Orienteering_ISOM_2017_2.Paths?, Orienteering_ISOM_2017_2.ManMadeLinearObstacles?) linearFeatures, float moreEfficientSurroundingsCoef)
    {
        var (_, path, _) = linearFeatures; 
        if (path is null) return moreEfficientSurroundingsCoef;
        if (path is Orienteering_ISOM_2017_2.Paths.LessDistinctSmallFootpath_507 || path is Orienteering_ISOM_2017_2.Paths.NarrowRide_508)
            return moreEfficientSurroundingsCoef * PathsMappings[path.Value].Value;
        return PathsMappings[path.Value].Value;
    }

    private float AddTimeForLinearObstacles((Orienteering_ISOM_2017_2.NaturalLinearObstacles?, Orienteering_ISOM_2017_2.Paths?, Orienteering_ISOM_2017_2.ManMadeLinearObstacles?) linearFeatures, float moreEfficientSurroundingsWithPathTime)
    {
        var (naturalLinearObstacles, _, manMadeLinearObstacles) = linearFeatures; 
        return moreEfficientSurroundingsWithPathTime + 
               (naturalLinearObstacles is null ? 0 : NaturalLinearObstaclesMappings[naturalLinearObstacles.Value].Value) + 
               (manMadeLinearObstacles is null ? 0 : ManMadeLinearObstaclesMappings[manMadeLinearObstacles.Value].Value);
    }

    public float GetWeightFromHeuristics(Orienteering_ISOM_2017_2.VertexAttributes from, Orienteering_ISOM_2017_2.VertexAttributes to)
    {
        float surroundingsMaxCoefficient = 0;
        foreach (var (_,adjustable) in GroundMappings)
             surroundingsMaxCoefficient = Math.Max(adjustable.Value, surroundingsMaxCoefficient);
        foreach (var (_,adjustable) in BoulderMappings)
             surroundingsMaxCoefficient = Math.Max(adjustable.Value, surroundingsMaxCoefficient);
        foreach (var (_,adjustable) in StoneMappings)
             surroundingsMaxCoefficient = Math.Max(adjustable.Value, surroundingsMaxCoefficient);
        foreach (var (_,adjustable) in WaterMappings)
             surroundingsMaxCoefficient = Math.Max(adjustable.Value, surroundingsMaxCoefficient);
        foreach (var (vegetationAndManMade,adjustable) in VegetationAndManMadeMappings)
             if (vegetationAndManMade != Orienteering_ISOM_2017_2.VegetationAndManMade.PavedArea_501) surroundingsMaxCoefficient = Math.Max(adjustable.Value, surroundingsMaxCoefficient); 
        foreach (var (_,adjustable) in VegetationGoodVisMappings)
             surroundingsMaxCoefficient = Math.Max(adjustable.Value, surroundingsMaxCoefficient);
        
        float pathMaxCoefficient = VegetationAndManMadeMappings[Orienteering_ISOM_2017_2.VegetationAndManMade.PavedArea_501].Value;
        foreach (var (path, adjustable) in PathsMappings)
            if (path == Orienteering_ISOM_2017_2.Paths.LessDistinctSmallFootpath_507 || path == Orienteering_ISOM_2017_2.Paths.NarrowRide_508)
                pathMaxCoefficient = Math.Max(adjustable.Value * surroundingsMaxCoefficient, pathMaxCoefficient);
            else pathMaxCoefficient = Math.Max(adjustable.Value, pathMaxCoefficient);
        
        return (float)(topPaceInSecondsPerCentimeter / Math.Max(surroundingsMaxCoefficient, pathMaxCoefficient) * (to.Position - from.Position).Length());
    }
    
    public MapCoordinates RetrievePosition(Orienteering_ISOM_2017_2.VertexAttributes vertexAttributes)
        => vertexAttributes.Position;
    

    private Dictionary<Orienteering_ISOM_2017_2.Grounds, BoundedFloatValueAdjustable>? _groundMappings;
    private Dictionary<Orienteering_ISOM_2017_2.Grounds, BoundedFloatValueAdjustable> GroundMappings
    {
        get
        {
            if (_groundMappings is null) 
                _groundMappings = new()
                {
                    { Orienteering_ISOM_2017_2.Grounds.BrokenGround_113, BrokenGroundCoefficient},
                    { Orienteering_ISOM_2017_2.Grounds.VeryBrokenGround_114, VeryBrokenGroundCoefficient},
                    { Orienteering_ISOM_2017_2.Grounds.CultivatedLand_412, CultivatedLandCoefficient}
                };
            return _groundMappings;
        }
    }
    private class BrokenGroundAdjustable() : BoundedFloatValueAdjustable("", null, 0, 1) { public BrokenGroundAdjustable(float value) : this() => Value = value; }
    private class VeryBrokenGroundAdjustable() : BoundedFloatValueAdjustable("", null, 0, 1) { public VeryBrokenGroundAdjustable(float value) : this() => Value = value; }
    private class CultivatedLandAdjustable() : BoundedFloatValueAdjustable("", null, 0, 1) { public CultivatedLandAdjustable(float value) : this() => Value = value; }

    private Dictionary<Orienteering_ISOM_2017_2.Boulders, BoundedFloatValueAdjustable>? _boulderMappings;
    private Dictionary<Orienteering_ISOM_2017_2.Boulders, BoundedFloatValueAdjustable> BoulderMappings
    {
        get
        {
            if (_boulderMappings is null) 
                _boulderMappings = new()
                {
                    { Orienteering_ISOM_2017_2.Boulders.GiganticBoulder_206, GiganticBoulderCoefficient},
                    { Orienteering_ISOM_2017_2.Boulders.BoulderField_208, BoulderFieldCoefficient},
                    { Orienteering_ISOM_2017_2.Boulders.DenseBoulderField_209, DenseBoulderFieldCoefficient},
                };
            return _boulderMappings;
        }
    } 
    
    private class GiganticBoulderAdjustable() : BoundedFloatValueAdjustable("", null, 0, 1){ public GiganticBoulderAdjustable(float value) : this() => Value = value;}
    private class BoulderFieldAdjustable() : BoundedFloatValueAdjustable("", null, 0, 1){ public BoulderFieldAdjustable(float value) : this() => Value = value;}
    private class DenseBoulderFieldAdjustable() : BoundedFloatValueAdjustable("", null, 0, 1){ public DenseBoulderFieldAdjustable(float value) : this() => Value = value;}

    private Dictionary<Orienteering_ISOM_2017_2.Stones, BoundedFloatValueAdjustable>? _stoneMappings;
    private Dictionary<Orienteering_ISOM_2017_2.Stones, BoundedFloatValueAdjustable> StoneMappings
    {
        get
        {
            if (_stoneMappings is null) 
                _stoneMappings = new()
                {
                    { Orienteering_ISOM_2017_2.Stones.StonyGroundSlow_210, StonyGroundSlowCoefficient },
                    { Orienteering_ISOM_2017_2.Stones.StonyGroundWalk_211, StonyGroundWalkCoefficient},
                    { Orienteering_ISOM_2017_2.Stones.StonyGroundFight_212, StonyGroundFightCoefficient},
                    { Orienteering_ISOM_2017_2.Stones.SandyGround_213, SandyGroundCoefficient},
                    { Orienteering_ISOM_2017_2.Stones.BareRock_214, BareRockCoefficient}
                };
            return _stoneMappings;
        }
    } 
    private class StonyGroundSlowAdjustable() : BoundedFloatValueAdjustable("", null, 0, 1){ public StonyGroundSlowAdjustable(float value) : this() => Value = value;}
    private class StonyGroundWalkAdjustable() : BoundedFloatValueAdjustable("", null, 0, 1){ public StonyGroundWalkAdjustable(float value) : this() => Value = value;}
    private class StonyGroundFightAdjustable() : BoundedFloatValueAdjustable("", null, 0, 1){ public StonyGroundFightAdjustable(float value) : this() => Value = value;}
    private class SandyGroundAdjustable() : BoundedFloatValueAdjustable("", null, 0, 1){ public SandyGroundAdjustable(float value) : this() => Value = value;}
    private class BareRockAdjustable() : BoundedFloatValueAdjustable("", null, 0, 1){ public BareRockAdjustable(float value) : this() => Value = value;}

    private Dictionary<Orienteering_ISOM_2017_2.Water, BoundedFloatValueAdjustable>? _waterMappings;
    private Dictionary<Orienteering_ISOM_2017_2.Water, BoundedFloatValueAdjustable> WaterMappings
    {
        get
        {
            if (_waterMappings is null) 
                _waterMappings = new()
                {
                    { Orienteering_ISOM_2017_2.Water.UncrossableBodyOfWater_301, UncrossableBodyOfWaterCoefficient},
                    { Orienteering_ISOM_2017_2.Water.ShallowBodyOfWater_302, ShallowBodyOfWaterCoefficient},
                    { Orienteering_ISOM_2017_2.Water.UncrossableMarsh_307, UncrossableMarshCoefficient},
                    { Orienteering_ISOM_2017_2.Water.Marsh_308, MarshCoefficient},
                    { Orienteering_ISOM_2017_2.Water.IndistinctMarsh_310, IndistinctMarshCoefficient}
                };
            return _waterMappings;
        }
    }
    
    private class UncrossableBodyOfWaterAdjustable() : BoundedFloatValueAdjustable("", null, 0, 1) { public UncrossableBodyOfWaterAdjustable(float value) : this() => Value = value; }
    private class ShallowBodyOfWaterAdjustable() : BoundedFloatValueAdjustable("", null, 0, 1) { public ShallowBodyOfWaterAdjustable(float value) : this() => Value = value; }
    private class UncrossableMarshAdjustable() : BoundedFloatValueAdjustable("", null, 0, 1) { public UncrossableMarshAdjustable(float value) : this() => Value = value; }
    private class MarshAdjustable() : BoundedFloatValueAdjustable("", null, 0, 1) { public MarshAdjustable(float value) : this() => Value = value; }
    private class IndistinctMarshAdjustable() : BoundedFloatValueAdjustable("", null, 0, 1) { public IndistinctMarshAdjustable(float value) : this() => Value = value; }
    
    private Dictionary<Orienteering_ISOM_2017_2.VegetationAndManMade, BoundedFloatValueAdjustable>? _vegetationAndManMadeMappings;
    private Dictionary<Orienteering_ISOM_2017_2.VegetationAndManMade, BoundedFloatValueAdjustable> VegetationAndManMadeMappings
    {
        get
        {
            if (_vegetationAndManMadeMappings is null) 
                _vegetationAndManMadeMappings = new()
                {
                    { Orienteering_ISOM_2017_2.VegetationAndManMade.OpenLand_401, OpenLandCoefficient},
                    { Orienteering_ISOM_2017_2.VegetationAndManMade.OpenLandWithTrees_402, OpenLandWithTreesCoefficient},
                    { Orienteering_ISOM_2017_2.VegetationAndManMade.RoughOpenLand_403, RoughOpenLandCoefficient },
                    { Orienteering_ISOM_2017_2.VegetationAndManMade.RoughOpenLandWithTrees_404, RoughOpenLandWithTreesCoefficient},
                    { Orienteering_ISOM_2017_2.VegetationAndManMade.Forest_405, ForestCoefficient},
                    { Orienteering_ISOM_2017_2.VegetationAndManMade.VegetationSlow_406, VegetationSlowCoefficient},
                    { Orienteering_ISOM_2017_2.VegetationAndManMade.VegetationWalk_408, VegetationWalkCoefficient},
                    { Orienteering_ISOM_2017_2.VegetationAndManMade.VegetationFight_410, VegetationFightCoefficient},
                    { Orienteering_ISOM_2017_2.VegetationAndManMade.Orchard_413, OrchardCoefficient},
                    { Orienteering_ISOM_2017_2.VegetationAndManMade.RoughOrchard_413, RoughOrchardCoefficient},
                    { Orienteering_ISOM_2017_2.VegetationAndManMade.Vineyard_414, VineyardCoefficient},
                    { Orienteering_ISOM_2017_2.VegetationAndManMade.RoughVineyard_414, RoughVineyardCoefficient},
                    { Orienteering_ISOM_2017_2.VegetationAndManMade.PavedArea_501, PavedAreaCoefficient},
                    { Orienteering_ISOM_2017_2.VegetationAndManMade.AreaThatShallNotBeEntered_520, AreaThatShallNotBeEnteredCoefficient},
                    { Orienteering_ISOM_2017_2.VegetationAndManMade.Building_521, BuildingCoefficient}
                };
            return _vegetationAndManMadeMappings;
        }
    }

    private class OpenLandAdjustable() : BoundedFloatValueAdjustable("", null, 0, 1) { public OpenLandAdjustable(float value) : this() => Value = value; }
    private class OpenLandWithTreesAdjustable() : BoundedFloatValueAdjustable("", null, 0, 1) { public OpenLandWithTreesAdjustable(float value) : this() => Value = value; }
    private class RoughOpenLandAdjustable() : BoundedFloatValueAdjustable("", null, 0, 1) { public RoughOpenLandAdjustable(float value) : this() => Value = value; }
    private class RoughOpenLandWithTreesAdjustable() : BoundedFloatValueAdjustable("", null, 0, 1) { public RoughOpenLandWithTreesAdjustable(float value) : this() => Value = value; } 
    private class ForestAdjustable() : BoundedFloatValueAdjustable("", null, 0, 1) { public ForestAdjustable(float value) : this() => Value = value;} 
    private class VegetationSlowAdjustable() : BoundedFloatValueAdjustable("", null, 0, 1) { public VegetationSlowAdjustable(float value) : this() => Value = value;} 
    private class VegetationWalkAdjustable() : BoundedFloatValueAdjustable("", null, 0, 1) { public VegetationWalkAdjustable(float value) : this() => Value = value;} 
    private class VegetationFightAdjustable() : BoundedFloatValueAdjustable("", null, 0, 1) { public VegetationFightAdjustable(float value) : this() => Value = value;} 
    private class OrchardAdjustable() : BoundedFloatValueAdjustable("", null, 0, 1) { public OrchardAdjustable(float value) : this() => Value = value;} 
    private class RoughOrchardAdjustable() : BoundedFloatValueAdjustable("", null, 0, 1) { public RoughOrchardAdjustable(float value) : this() => Value = value;} 
    private class VineyardAdjustable() : BoundedFloatValueAdjustable("", null, 0, 1) { public VineyardAdjustable(float value) : this() => Value = value;} 
    private class RoughVineyardAdjustable() : BoundedFloatValueAdjustable("", null, 0, 1) { public RoughVineyardAdjustable(float value) : this() => Value = value; } 
    private class PavedAreaAdjustable() : BoundedFloatValueAdjustable("", null, 0, 1) { public PavedAreaAdjustable(float value) : this() => Value = value;} 
    private class AreaThatShallNotBeEnteredAdjustable() : BoundedFloatValueAdjustable("", null, 0, 1) { public AreaThatShallNotBeEnteredAdjustable(float value) : this() => Value = value;} 
    private class BuildingAdjustable() : BoundedFloatValueAdjustable("", null, 0, 1) { public BuildingAdjustable(float value) : this() => Value = value;}

    private Dictionary<Orienteering_ISOM_2017_2.VegetationGoodVis, BoundedFloatValueAdjustable>? _vegetationGoodVisMappings;
    private Dictionary<Orienteering_ISOM_2017_2.VegetationGoodVis, BoundedFloatValueAdjustable> VegetationGoodVisMappings
    {
        get
        {
            if (_vegetationGoodVisMappings is null)
                _vegetationGoodVisMappings = new ()
                {
                    { Orienteering_ISOM_2017_2.VegetationGoodVis.VegetationSlowGoodVis_407, VegetationSlowGoodVisibilityCoefficient},
                    { Orienteering_ISOM_2017_2.VegetationGoodVis.VegetationWalkGoodVis_409, VegetationWalkGoodVisibilityCoefficient}
                };
            return _vegetationGoodVisMappings;
        }
    }

    private class VegetationSlowGoodVisAdjustable() : BoundedFloatValueAdjustable("", null, 0, 1) { public VegetationSlowGoodVisAdjustable(float value) : this() => Value = value;}
    private class VegetationWalkGoodVisAdjustable() : BoundedFloatValueAdjustable("", null, 0, 1) { public VegetationWalkGoodVisAdjustable(float value) : this() => Value = value;}

    private Dictionary<Orienteering_ISOM_2017_2.NaturalLinearObstacles, BoundedFloatValueAdjustable>? _naturalLinearObstaclesMappings;
    private Dictionary<Orienteering_ISOM_2017_2.NaturalLinearObstacles, BoundedFloatValueAdjustable> NaturalLinearObstaclesMappings
    {
        get
        {
            if (_naturalLinearObstaclesMappings is null)
                _naturalLinearObstaclesMappings = new()
                {
                    { Orienteering_ISOM_2017_2.NaturalLinearObstacles.EarthBank_104, EarthBankOvercomeCoefficient },
                    { Orienteering_ISOM_2017_2.NaturalLinearObstacles.EarthWall_105, EarthWallOvercomeCoefficient },
                    { Orienteering_ISOM_2017_2.NaturalLinearObstacles.ErosionGully_107, ErosionGullyOvercomeCoefficient },
                    { Orienteering_ISOM_2017_2.NaturalLinearObstacles.ImpassableCliff_201, ImpassableCliffOvercomeCoefficient },
                    { Orienteering_ISOM_2017_2.NaturalLinearObstacles.Cliff_202, CliffOvercomeCoefficient },
                    { Orienteering_ISOM_2017_2.NaturalLinearObstacles.CrossableWatercourse_304, CrossableWaterCourseOvercomeCoefficient },
                    { Orienteering_ISOM_2017_2.NaturalLinearObstacles.SmallCrossableWatercourse_305, SmallCrossableWaterCourseOvercomeCoefficient },
                };
            return _naturalLinearObstaclesMappings;
        }
    }

    private class EarthBankAdjustable() : BoundedFloatValueAdjustable("", "seconds", 0, float.PositiveInfinity) { public EarthBankAdjustable(float value) : this() => Value = value;}
    private class EarthWallAdjustable() : BoundedFloatValueAdjustable("", "seconds", 0, float.PositiveInfinity) { public EarthWallAdjustable(float value) : this() => Value = value;}
    private class ErosionGullyAdjustable() : BoundedFloatValueAdjustable("", "seconds", 0, float.PositiveInfinity) { public ErosionGullyAdjustable(float value) : this() => Value = value;}
    private class ImpassableCliffAdjustable() : BoundedFloatValueAdjustable("", "seconds", 0, float.PositiveInfinity) { public ImpassableCliffAdjustable(float value) : this() => Value = value;}
    private class CliffAdjustable() : BoundedFloatValueAdjustable("", "seconds", 0, float.PositiveInfinity) { public CliffAdjustable(float value) : this() => Value = value;}
    private class CrossableWatercourseAdjustable() : BoundedFloatValueAdjustable("", "seconds", 0, float.PositiveInfinity) { public CrossableWatercourseAdjustable(float value) : this() => Value = value;}
    private class SmallCrossableWatercourseAdjustable() : BoundedFloatValueAdjustable("", "seconds", 0, float.PositiveInfinity) { public SmallCrossableWatercourseAdjustable(float value) : this() => Value = value;}

    private Dictionary<Orienteering_ISOM_2017_2.Paths, BoundedFloatValueAdjustable>? _pathsMappings;
    private Dictionary<Orienteering_ISOM_2017_2.Paths, BoundedFloatValueAdjustable> PathsMappings
    {
        get
        {
            if (_pathsMappings is null)
                _pathsMappings = new()
                {
                    { Orienteering_ISOM_2017_2.Paths.WideRoad_502, WideRoadCoefficient},
                    { Orienteering_ISOM_2017_2.Paths.Road_503, RoadCoefficient},
                    { Orienteering_ISOM_2017_2.Paths.VehicleTrack_504, VehicleTrackCoefficient},
                    { Orienteering_ISOM_2017_2.Paths.Footpath_505, FootpathCoefficient},
                    { Orienteering_ISOM_2017_2.Paths.SmallFootpath_506, SmallFootpathCoefficient},
                    { Orienteering_ISOM_2017_2.Paths.LessDistinctSmallFootpath_507, LessDistinctSmallFootpathCoefficient},
                    { Orienteering_ISOM_2017_2.Paths.NarrowRide_508, NarrowRideCoefficient},
                    { Orienteering_ISOM_2017_2.Paths.Stairway_532, StairwayCoefficient},
                };
            return _pathsMappings;
        }
    }

    private class WideRoadAdjustable() : BoundedFloatValueAdjustable("", null, 0.1f, 1) { public WideRoadAdjustable(float value) : this() => Value = value;}
    private class RoadAdjustable() : BoundedFloatValueAdjustable("", null, 0.1f, 1) { public RoadAdjustable(float value) : this() => Value = value;}
    private class VehicleTrackAdjustable() : BoundedFloatValueAdjustable("", null, 0.1f, 1) { public VehicleTrackAdjustable(float value) : this() => Value = value;}
    private class FootpathAdjustable() : BoundedFloatValueAdjustable("", null, 0.1f, 1) { public FootpathAdjustable(float value) : this() => Value = value;}
    private class SmallFootpathAdjustable() : BoundedFloatValueAdjustable("", null, 0.1f, 1) { public SmallFootpathAdjustable(float value) : this() => Value = value;}
    private class LessDistinctSmallFootpathAdjustable() : BoundedFloatValueAdjustable("", null, 1, 2) { public LessDistinctSmallFootpathAdjustable(float value) : this() => Value = value;}
    private class NarrowRideAdjustable() : BoundedFloatValueAdjustable("", null, 1, 2) { public NarrowRideAdjustable(float value) : this() => Value = value;}
    private class StairwayAdjustable() : BoundedFloatValueAdjustable("", null, 0.1f, 1) { public StairwayAdjustable(float value) : this() => Value = value;}
    
    private Dictionary<Orienteering_ISOM_2017_2.ManMadeLinearObstacles, BoundedFloatValueAdjustable>? _manMadeLinearObstaclesMappings;
    private Dictionary<Orienteering_ISOM_2017_2.ManMadeLinearObstacles, BoundedFloatValueAdjustable> ManMadeLinearObstaclesMappings
    {
        get
        {
            if (_manMadeLinearObstaclesMappings is null)
                _manMadeLinearObstaclesMappings = new()
                {
                    { Orienteering_ISOM_2017_2.ManMadeLinearObstacles.Trench_215, TrenchOvercomeCoefficient},
                    { Orienteering_ISOM_2017_2.ManMadeLinearObstacles.Wall_513, WallOvercomeCoefficient},
                    { Orienteering_ISOM_2017_2.ManMadeLinearObstacles.ImpassableWall_515, ImpassableWallOvercomeCoefficient},
                    { Orienteering_ISOM_2017_2.ManMadeLinearObstacles.Fence_516, FenceOvercomeCoefficient},
                    { Orienteering_ISOM_2017_2.ManMadeLinearObstacles.ImpassableFence_518, ImpassableFenceOvercomeCoefficient},
                    { Orienteering_ISOM_2017_2.ManMadeLinearObstacles.ProminentLineFeature_528, ProminentLineFeatureOvercomeCoefficient},
                    { Orienteering_ISOM_2017_2.ManMadeLinearObstacles.ImpassableProminentLineFeature_529, ImpassableProminentLineFeatureOvercomeCoefficient},
                };
            return _manMadeLinearObstaclesMappings;
        }
    }
    
    private class TrenchAdjustable() : BoundedFloatValueAdjustable("", "seconds", 0, float.PositiveInfinity) { public TrenchAdjustable(float value) : this() => Value = value;}
    private class WallAdjustable() : BoundedFloatValueAdjustable("", "seconds", 0, float.PositiveInfinity) { public WallAdjustable(float value) : this() => Value = value;}
    private class ImpassableWallAdjustable() : BoundedFloatValueAdjustable("", "seconds", 0, float.PositiveInfinity) { public ImpassableWallAdjustable(float value) : this() => Value = value;}
    private class FenceAdjustable() : BoundedFloatValueAdjustable("", "seconds", 0, float.PositiveInfinity) { public FenceAdjustable(float value) : this() => Value = value;}
    private class ImpassableFenceAdjustable() : BoundedFloatValueAdjustable("", "seconds", 0, float.PositiveInfinity) { public ImpassableFenceAdjustable(float value) : this() => Value = value;}
    private class ProminentLineFeatureAdjustable() : BoundedFloatValueAdjustable("", "seconds", 0, float.PositiveInfinity) { public ProminentLineFeatureAdjustable(float value) : this() => Value = value;}
    private class ImpassableProminentLineFeatureAdjustable() : BoundedFloatValueAdjustable("", "seconds", 0, float.PositiveInfinity) { public ImpassableProminentLineFeatureAdjustable(float value) : this() => Value = value;}
        
    [JsonInclude] private BrokenGroundAdjustable BrokenGroundCoefficient { get; set; } 
    [JsonInclude] private VeryBrokenGroundAdjustable VeryBrokenGroundCoefficient { get; set; }
    
    [JsonInclude] private GiganticBoulderAdjustable GiganticBoulderCoefficient { get; set; }
    [JsonInclude] private BoulderFieldAdjustable BoulderFieldCoefficient { get; set; }
    [JsonInclude] private DenseBoulderFieldAdjustable DenseBoulderFieldCoefficient { get; set; }
    [JsonInclude] private StonyGroundSlowAdjustable StonyGroundSlowCoefficient { get; set; }
    [JsonInclude] private StonyGroundWalkAdjustable StonyGroundWalkCoefficient { get; set; }
    [JsonInclude] private StonyGroundFightAdjustable StonyGroundFightCoefficient { get; set; }
    [JsonInclude] private SandyGroundAdjustable SandyGroundCoefficient { get; set; }
    [JsonInclude] private BareRockAdjustable BareRockCoefficient { get; set; }
    
    [JsonInclude] private UncrossableBodyOfWaterAdjustable UncrossableBodyOfWaterCoefficient { get; set; }
    [JsonInclude] private ShallowBodyOfWaterAdjustable ShallowBodyOfWaterCoefficient { get; set; }
    [JsonInclude] private UncrossableMarshAdjustable UncrossableMarshCoefficient { get; set; }
    [JsonInclude] private MarshAdjustable MarshCoefficient { get; set; }
    [JsonInclude] private IndistinctMarshAdjustable IndistinctMarshCoefficient { get; set; }
    
    [JsonInclude] private OpenLandAdjustable OpenLandCoefficient { get; set; }
    [JsonInclude] private OpenLandWithTreesAdjustable OpenLandWithTreesCoefficient { get; set; }
    [JsonInclude] private RoughOpenLandAdjustable RoughOpenLandCoefficient { get; set; }
    [JsonInclude] private RoughOpenLandWithTreesAdjustable RoughOpenLandWithTreesCoefficient { get; set; }
    [JsonInclude] private ForestAdjustable ForestCoefficient { get; set; }
    [JsonInclude] private VegetationSlowAdjustable VegetationSlowCoefficient { get; set; }
    [JsonInclude] private VegetationSlowGoodVisAdjustable VegetationSlowGoodVisibilityCoefficient { get; set; }
    [JsonInclude] private VegetationWalkAdjustable VegetationWalkCoefficient { get; set; }
    [JsonInclude] private VegetationWalkGoodVisAdjustable VegetationWalkGoodVisibilityCoefficient { get; set; }
    [JsonInclude] private VegetationFightAdjustable VegetationFightCoefficient { get; set; }
    [JsonInclude] private CultivatedLandAdjustable CultivatedLandCoefficient { get; set; }
    [JsonInclude] private OrchardAdjustable OrchardCoefficient { get; set; }
    [JsonInclude] private RoughOrchardAdjustable RoughOrchardCoefficient { get; set; }
    [JsonInclude] private VineyardAdjustable VineyardCoefficient { get; set; }
    [JsonInclude] private RoughVineyardAdjustable RoughVineyardCoefficient { get; set; }
    
    [JsonInclude] private PavedAreaAdjustable PavedAreaCoefficient { get; set; }
    [JsonInclude] private WideRoadAdjustable WideRoadCoefficient { get; set; }
    [JsonInclude] private RoadAdjustable RoadCoefficient { get; set; }
    [JsonInclude] private VehicleTrackAdjustable VehicleTrackCoefficient { get; set; }
    [JsonInclude] private FootpathAdjustable FootpathCoefficient { get; set; }
    [JsonInclude] private SmallFootpathAdjustable SmallFootpathCoefficient { get; set; }
    [JsonInclude] private LessDistinctSmallFootpathAdjustable LessDistinctSmallFootpathCoefficient { get; set; }
    [JsonInclude] private NarrowRideAdjustable NarrowRideCoefficient { get; set; }
    
    [JsonInclude] private AreaThatShallNotBeEnteredAdjustable AreaThatShallNotBeEnteredCoefficient { get; set; }
    [JsonInclude] private BuildingAdjustable BuildingCoefficient { get; set; }
    [JsonInclude] private StairwayAdjustable StairwayCoefficient { get; set; }
    
    [JsonInclude] private EarthBankAdjustable EarthBankOvercomeCoefficient { get; set; } 
    [JsonInclude] private EarthWallAdjustable EarthWallOvercomeCoefficient { get; set; }
    [JsonInclude] private ErosionGullyAdjustable ErosionGullyOvercomeCoefficient { get; set; }
    [JsonInclude] private ImpassableCliffAdjustable ImpassableCliffOvercomeCoefficient { get; set; }
    [JsonInclude] private CliffAdjustable CliffOvercomeCoefficient { get; set; }
    [JsonInclude] private TrenchAdjustable TrenchOvercomeCoefficient { get; set; }
    [JsonInclude] private CrossableWatercourseAdjustable CrossableWaterCourseOvercomeCoefficient { get; set; }
    
    [JsonInclude] private SmallCrossableWatercourseAdjustable SmallCrossableWaterCourseOvercomeCoefficient { get; set; }
    [JsonInclude] private WallAdjustable WallOvercomeCoefficient { get; set; }
    [JsonInclude] private ImpassableWallAdjustable ImpassableWallOvercomeCoefficient { get; set; }
    [JsonInclude] private FenceAdjustable FenceOvercomeCoefficient { get; set; }
    [JsonInclude] private ImpassableFenceAdjustable ImpassableFenceOvercomeCoefficient { get; set; }
    [JsonInclude] private ProminentLineFeatureAdjustable ProminentLineFeatureOvercomeCoefficient { get; set; }
    [JsonInclude] private ImpassableProminentLineFeatureAdjustable ImpassableProminentLineFeatureOvercomeCoefficient { get; set; }
    
    public TOut AcceptGeneric<TOut, TOtherParams>(IUserModelGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams)
        => genericVisitor.GenericVisit<Orienteering_ISOM_2017_2UserModel, Orienteering_ISOM_2017_2>(this, otherParams);
    public TOut AcceptGeneric<TOut>(IUserModelGenericVisitor<TOut> genericVisitor)
        => genericVisitor.GenericVisit<Orienteering_ISOM_2017_2UserModel, Orienteering_ISOM_2017_2>(this);
    public void AcceptGeneric(IUserModelGenericVisitor genericVisitor)
        => genericVisitor.GenericVisit<Orienteering_ISOM_2017_2UserModel, Orienteering_ISOM_2017_2>(this);
}