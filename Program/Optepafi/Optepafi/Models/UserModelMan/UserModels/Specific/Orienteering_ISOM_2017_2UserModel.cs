using System;
using System.Collections.Generic;
using System.IO;
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
    public Orienteering_ISOM_2017_2 AssociatedTemplate { get; } = Orienteering_ISOM_2017_2.Instance;
    [JsonIgnore]
    public string FilePath { get; set; } = "";
    
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
        var surroundingsCoef = ComputeCoeficientOfSurroundings(through.Surroundings);
        var secondSurroundingsCoef = ComputeCoeficientOfSurroundings(through.SecondSurroundings);
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
        return (ground is not null ? _groundMappings[ground.Value].Value : 1) *
               (boulders is not null ? _boulderMappings[boulders.Value].Value : 1) *
               (stones is not null ? _stoneMappings[stones.Value].Value : 1) *
               (water is not null ? _waterMappings[water.Value].Value : 1) *
               (vegetationAndManMade is not null ? _vegetationAndManMadeMappings[vegetationAndManMade.Value].Value : 1) *
               (vegetationGoodVis is not null ? _vegetationGoodVisMappings[vegetationGoodVis.Value].Value : 1);
    }

    private float ApplyPathToSurroundingsCoef((Orienteering_ISOM_2017_2.NaturalLinearObstacles?, Orienteering_ISOM_2017_2.Paths?, Orienteering_ISOM_2017_2.ManMadeLinearObstacles?) linearFeatures, float moreEfficientSurroundingsCoef)
    {
        var (_, path, _) = linearFeatures; 
        if (path is null) return moreEfficientSurroundingsCoef;
        if (path is Orienteering_ISOM_2017_2.Paths.LessDistinctSmallFootpath_507 || path is Orienteering_ISOM_2017_2.Paths.NarrowRide_508)
            return moreEfficientSurroundingsCoef * _pathsMappings[path.Value].Value;
        return _pathsMappings[path.Value].Value;
    }

    private float AddTimeForLinearObstacles((Orienteering_ISOM_2017_2.NaturalLinearObstacles?, Orienteering_ISOM_2017_2.Paths?, Orienteering_ISOM_2017_2.ManMadeLinearObstacles?) linearFeatures, float moreEfficientSurroundingsWithPathTime)
    {
        var (naturalLinearObstacles, _, manMadeLinearObstacles) = linearFeatures; 
        return moreEfficientSurroundingsWithPathTime + 
               (naturalLinearObstacles is null ? 0 : _naturalLinearObstaclesMappings[naturalLinearObstacles.Value].Value) + 
               (manMadeLinearObstacles is null ? 0 : _manMadeLinearObstaclesMappings[manMadeLinearObstacles.Value].Value);
    }

    public float GetWeightFromHeuristics(Orienteering_ISOM_2017_2.VertexAttributes from, Orienteering_ISOM_2017_2.VertexAttributes to)
    {
        float surroundingsMaxCoefficient = 0;
        foreach (var (_,adjustable) in _groundMappings)
             surroundingsMaxCoefficient = Math.Max(adjustable.Value, surroundingsMaxCoefficient);
        foreach (var (_,adjustable) in _boulderMappings)
             surroundingsMaxCoefficient = Math.Max(adjustable.Value, surroundingsMaxCoefficient);
        foreach (var (_,adjustable) in _stoneMappings)
             surroundingsMaxCoefficient = Math.Max(adjustable.Value, surroundingsMaxCoefficient);
        foreach (var (_,adjustable) in _waterMappings)
             surroundingsMaxCoefficient = Math.Max(adjustable.Value, surroundingsMaxCoefficient);
        foreach (var (vegetationAndManMade,adjustable) in _vegetationAndManMadeMappings)
             if (vegetationAndManMade != Orienteering_ISOM_2017_2.VegetationAndManMade.PavedArea_501) surroundingsMaxCoefficient = Math.Max(adjustable.Value, surroundingsMaxCoefficient); 
        foreach (var (_,adjustable) in _vegetationGoodVisMappings)
             surroundingsMaxCoefficient = Math.Max(adjustable.Value, surroundingsMaxCoefficient);
        
        float pathMaxCoefficient = _vegetationAndManMadeMappings[Orienteering_ISOM_2017_2.VegetationAndManMade.PavedArea_501].Value;
        foreach (var (path, adjustable) in _pathsMappings)
            if (path == Orienteering_ISOM_2017_2.Paths.LessDistinctSmallFootpath_507 || path == Orienteering_ISOM_2017_2.Paths.NarrowRide_508)
                pathMaxCoefficient = Math.Max(adjustable.Value * surroundingsMaxCoefficient, pathMaxCoefficient);
            else pathMaxCoefficient = Math.Max(adjustable.Value, pathMaxCoefficient);
        
        return (float)(topPaceInSecondsPerCentimeter / Math.Max(surroundingsMaxCoefficient, pathMaxCoefficient) * (to.Position - from.Position).Length());
    }

    [JsonInclude]
    private Dictionary<Orienteering_ISOM_2017_2.Grounds, BoundedFloatValueAdjustable> _groundMappings { get; }= new()
    {
        { Orienteering_ISOM_2017_2.Grounds.BrokenGround_113 , new ("", 0.95f, null, 0, 1)},
        { Orienteering_ISOM_2017_2.Grounds.VeryBrokenGround_114, new ("", 0.8f, null, 0, 1)},
        { Orienteering_ISOM_2017_2.Grounds.CultivatedLand_412 , new ("", 0.8f, null, 0, 1)}
    };

    [JsonInclude]
    private Dictionary<Orienteering_ISOM_2017_2.Boulders, BoundedFloatValueAdjustable> _boulderMappings { get; }= new()
    {
        { Orienteering_ISOM_2017_2.Boulders.GiganticBoulder_206, new ("", 0,null, 0, 1)},
        { Orienteering_ISOM_2017_2.Boulders.BoulderField_208, new ("", 0.7f,null, 0, 1)},
        { Orienteering_ISOM_2017_2.Boulders.DenseBoulderField_209, new ("", 0.55f,null, 0, 1)},
    };

    [JsonInclude]
    private Dictionary<Orienteering_ISOM_2017_2.Stones, BoundedFloatValueAdjustable> _stoneMappings { get; }= new()
    {
        { Orienteering_ISOM_2017_2.Stones.StonyGroundSlow_210, new ("", 0.65f,null, 0, 1) },
        { Orienteering_ISOM_2017_2.Stones.StonyGroundWalk_211, new ("", 0.5f,null, 0, 1)},
        { Orienteering_ISOM_2017_2.Stones.StonyGroundFight_212, new ("", 0.35f, null, 0, 1)},
        { Orienteering_ISOM_2017_2.Stones.Sandyground_213, new ("", 0.7f,null, 0, 1)},
        { Orienteering_ISOM_2017_2.Stones.BareRock_214, new ("", 0.95f,null, 0, 1)}
    };

    [JsonInclude]
    private Dictionary<Orienteering_ISOM_2017_2.Water, BoundedFloatValueAdjustable> _waterMappings { get; }= new()
    {
        { Orienteering_ISOM_2017_2.Water.UncrossableBodyOfWater_301, new ("", 0,null, 0, 1)},
        { Orienteering_ISOM_2017_2.Water.ShallowBodyOfWater_302, new ("", 0.5f,null, 0, 1)},
        { Orienteering_ISOM_2017_2.Water.UncrossableMarsh_307, new ("", 0,null, 0, 1)},
        { Orienteering_ISOM_2017_2.Water.Marsh_308, new ("", 0.8f,null, 0, 1)},
        { Orienteering_ISOM_2017_2.Water.IndistinctMarsh_310, new ("", 0.9f,null, 0, 1)}
    };

    [JsonInclude]
    private Dictionary<Orienteering_ISOM_2017_2.VegetationAndManMade, BoundedFloatValueAdjustable> _vegetationAndManMadeMappings { get; }= new()
    {
        { Orienteering_ISOM_2017_2.VegetationAndManMade.OpenLand_401, new ("", 0.97f,null, 0, 1)},
        { Orienteering_ISOM_2017_2.VegetationAndManMade.OpenLandWithTrees_402, new ("", 0.93f,null, 0, 1)},
        { Orienteering_ISOM_2017_2.VegetationAndManMade.RoughOpenLand_403, new ("", 0.77f,null, 0, 1)},
        { Orienteering_ISOM_2017_2.VegetationAndManMade.RoughOpenLandWithTrees_404, new ("", 0.7f,null, 0, 1)},
        { Orienteering_ISOM_2017_2.VegetationAndManMade.Forest_405, new ("", 0.9f,null, 0, 1)},
        { Orienteering_ISOM_2017_2.VegetationAndManMade.VegetationSlow_406, new ("", 0.7f,null, 0, 1)},
        { Orienteering_ISOM_2017_2.VegetationAndManMade.VegetationWalk_408, new ("", 0.45f,null, 0, 1)},
        { Orienteering_ISOM_2017_2.VegetationAndManMade.VegetationFight_410, new ("", 0.2f,null, 0, 1)},
        { Orienteering_ISOM_2017_2.VegetationAndManMade.Orchard_413, new ("", 0.93f,null, 0, 1)},
        { Orienteering_ISOM_2017_2.VegetationAndManMade.RoughOrchard_413, new ("", 0.8f,null, 0, 1)},
        { Orienteering_ISOM_2017_2.VegetationAndManMade.Vineyard_414, new ("", 0.85f,null, 0, 1)},
        { Orienteering_ISOM_2017_2.VegetationAndManMade.RoughVineyard_414, new ("", 0.7f,null, 0, 1)},
        { Orienteering_ISOM_2017_2.VegetationAndManMade.PavedArea_501, new ("", 1,null, 0, 1)},
        { Orienteering_ISOM_2017_2.VegetationAndManMade.AreaThatShallNotBeEntered_520, new ("", 0,null, 0, 1)},
        { Orienteering_ISOM_2017_2.VegetationAndManMade.Building_521, new ("", 0,null, 0, 1)}
    };

    [JsonInclude]
    private Dictionary<Orienteering_ISOM_2017_2.VegetationGoodVis, BoundedFloatValueAdjustable> _vegetationGoodVisMappings { get; }= new()
    {
        { Orienteering_ISOM_2017_2.VegetationGoodVis.VegetationSlowGoodVis_407, new ("", 0.8f,null, 0, 1)},
        { Orienteering_ISOM_2017_2.VegetationGoodVis.VegetationWalkGoodVis_409, new ("", 0.6f,null, 0, 1)}
    };


    [JsonInclude]
    private Dictionary<Orienteering_ISOM_2017_2.NaturalLinearObstacles, BoundedFloatValueAdjustable> _naturalLinearObstaclesMappings { get; }= new()
    {
        { Orienteering_ISOM_2017_2.NaturalLinearObstacles.EarthBank_104, new ("", 2, "seconds", 0, float.PositiveInfinity)},
        { Orienteering_ISOM_2017_2.NaturalLinearObstacles.EarthWall_105, new("", 2, "seconds", 0, float.PositiveInfinity)},
        { Orienteering_ISOM_2017_2.NaturalLinearObstacles.ErosionGully_107, new ("", 5, "seconds", 0, float.PositiveInfinity)},
        { Orienteering_ISOM_2017_2.NaturalLinearObstacles.ImpassableCliff_201, new ("", float.PositiveInfinity, "seconds", 0, float.PositiveInfinity)},
        { Orienteering_ISOM_2017_2.NaturalLinearObstacles.Cliff_202, new ("", 7, "seconds", 0, float.PositiveInfinity)},
        { Orienteering_ISOM_2017_2.NaturalLinearObstacles.CrossableWatercourse_304, new ("", 3, "seconds", 0, float.PositiveInfinity)},
        { Orienteering_ISOM_2017_2.NaturalLinearObstacles.SmallCrossableWatercourse_305, new ("", 2, "seconds", 0, float.PositiveInfinity)},
    };

    [JsonInclude]
    private Dictionary<Orienteering_ISOM_2017_2.Paths, BoundedFloatValueAdjustable> _pathsMappings { get; }= new()
    {
        { Orienteering_ISOM_2017_2.Paths.WideRoad_502, new ("", 1,null, 0.1f, 1)},
        { Orienteering_ISOM_2017_2.Paths.Road_503, new ("", 1,null, 0.1f, 1)},
        { Orienteering_ISOM_2017_2.Paths.VehicleTrack_504, new ("", 1,null, 0.1f, 1)},
        { Orienteering_ISOM_2017_2.Paths.Footpath_505, new ("", 0.99f,null, 0.1f, 1)},
        { Orienteering_ISOM_2017_2.Paths.SmallFootpath_506, new ("", 0.98f,null, 0.1f, 1)},
        { Orienteering_ISOM_2017_2.Paths.LessDistinctSmallFootpath_507, new ("", 1.07f,null, 1, 2)},
        { Orienteering_ISOM_2017_2.Paths.NarrowRide_508, new ("", 1.07f,null, 1, 2)},
        { Orienteering_ISOM_2017_2.Paths.Stairway_532, new ("", 0.93f,null, 0.1f, 1)},
    };

    [JsonInclude]
    private Dictionary<Orienteering_ISOM_2017_2.ManMadeLinearObstacles, BoundedFloatValueAdjustable> _manMadeLinearObstaclesMappings { get; }= new()
    {
        { Orienteering_ISOM_2017_2.ManMadeLinearObstacles.Trench_215, new ("", 1, "seconds", 0, float.PositiveInfinity)},
        { Orienteering_ISOM_2017_2.ManMadeLinearObstacles.Wall_513, new ("", 2, "seconds", 0, float.PositiveInfinity)},
        { Orienteering_ISOM_2017_2.ManMadeLinearObstacles.ImpassableWall_515, new ("", float.PositiveInfinity, "seconds", 0, float.PositiveInfinity)},
        { Orienteering_ISOM_2017_2.ManMadeLinearObstacles.Fence_516, new ("", 2, "seconds", 0, float.PositiveInfinity)},
        { Orienteering_ISOM_2017_2.ManMadeLinearObstacles.ImpassableFence_518, new ("", float.PositiveInfinity, "seconds", 0, float.PositiveInfinity)},
        { Orienteering_ISOM_2017_2.ManMadeLinearObstacles.ProminentLineFeature_528, new ("", 1.5f, "seconds", 0, float.PositiveInfinity)},
        { Orienteering_ISOM_2017_2.ManMadeLinearObstacles.ImpassableProminentLineFeature_529, new ("", float.PositiveInfinity, "seconds", 0, float.PositiveInfinity)},
    };
    
    
        
    // [JsonInclude]
    // private BoundedFloatValueAdjustable BrokenGroundCoefficient { get; set; } = new ("", 0.95f, null, 0, 1);
    // [JsonInclude]
    // private BoundedFloatValueAdjustable VeryBrokenGroundCoefficient { get; set; }= new ("", 0.8f,null, 0, 1);
    
    // [JsonInclude]
    // private BoundedFloatValueAdjustable GiganticBoulderCoefficient { get; set; }= new ("", 0,null, 0, 1);
    // [JsonInclude]
    // private BoundedFloatValueAdjustable BoulderFieldCCoefficient { get; set; }= new ("", 0.7f,null, 0, 1);
    // [JsonInclude]
    // private BoundedFloatValueAdjustable DenseBoulderFieldCoefficient { get; set; }= new ("", 0.55f,null, 0, 1);
    // [JsonInclude]
    // private BoundedFloatValueAdjustable StonyGroundSlowCoefficient { get; set; }= new ("", 0.65f,null, 0, 1);
    // [JsonInclude]
    // private BoundedFloatValueAdjustable StonyGroundWalkCoefficient { get; set; }= new ("", 0.5f,null, 0, 1);
    // [JsonInclude]
    // private BoundedFloatValueAdjustable StonyGroundFightCoefficient { get; set; }= new ("", 0.35f, null, 0, 1);
    // [JsonInclude]
    // private BoundedFloatValueAdjustable SandyGroundCoefficient { get; set; }= new ("", 0.7f,null, 0, 1);
    // [JsonInclude]
    // private BoundedFloatValueAdjustable BareRockCoefficient { get; set; }= new ("", 0.95f,null, 0, 1);
    
    // [JsonInclude]
    // private BoundedFloatValueAdjustable UncrossableBodyOfWaterCoefficient { get; set; }= new ("", 0,null, 0, 1);
    // [JsonInclude]
    // private BoundedFloatValueAdjustable ShallowBodyOfWaterCoefficient { get; set; }= new ("", 0.5f,null, 0, 1);
    // [JsonInclude]
    // private BoundedFloatValueAdjustable UncrossableMarshCoefficient { get; set; }= new ("", 0,null, 0, 1);
    // [JsonInclude]
    // private BoundedFloatValueAdjustable MarshCoefficient { get; set; }= new ("", 0.8f,null, 0, 1);
    // [JsonInclude]
    // private BoundedFloatValueAdjustable IndistinctMarshCoefficient { get; set; }= new ("", 0.9f,null, 0, 1);
    
    // [JsonInclude]
    // private BoundedFloatValueAdjustable OpenLandCoefficient { get; set; }= new ("", 0.97f,null, 0, 1);
    // [JsonInclude]
    // private BoundedFloatValueAdjustable OpenLandWithTreesCoefficient { get; set; }= new ("", 0.93f,null, 0, 1);
    // [JsonInclude]
    // private BoundedFloatValueAdjustable RoughOpenLandCoefficient { get; set; }= new ("", 0.77f,null, 0, 1);
    // [JsonInclude]
    // private BoundedFloatValueAdjustable RoughOpenLandWithTreesCoefficient { get; set; }= new ("", 0.7f,null, 0, 1);
    // [JsonInclude]
    // private BoundedFloatValueAdjustable ForestCoefficient { get; set; }= new ("", 0.9f,null, 0, 1);
    // [JsonInclude]
    // private BoundedFloatValueAdjustable VegetationSlowCoefficient { get; set; }= new ("", 0.7f,null, 0, 1);
    // [JsonInclude]
    // private BoundedFloatValueAdjustable VegetationSlowGoodVisibilityCoefficient { get; set; }= new ("", 0.8f,null, 0, 1);
    // [JsonInclude]
    // private BoundedFloatValueAdjustable VegetationWalkCoefficient { get; set; }= new ("", 0.45f,null, 0, 1);
    // [JsonInclude]
    // private BoundedFloatValueAdjustable VegetationWalkGoodVisibilityCoefficient { get; set; }= new ("", 0.6f,null, 0, 1);
    // [JsonInclude]
    // private BoundedFloatValueAdjustable VegetationFightCoefficient { get; set; }= new ("", 0.2f,null, 0, 1);
    // [JsonInclude]
    // private BoundedFloatValueAdjustable CultivatedLandCoefficient { get; set; }= new ("", 0.8f,null, 0, 1);
    // [JsonInclude]
    // private BoundedFloatValueAdjustable OrchardCoefficient { get; set; }= new ("", 0.93f,null, 0, 1);
    // [JsonInclude]
    // private BoundedFloatValueAdjustable RoughOrchardCoefficient { get; set; }= new ("", 0.8f,null, 0, 1);
    // [JsonInclude]
    // private BoundedFloatValueAdjustable VineyardCoefficient { get; set; }= new ("", 0.85f,null, 0, 1);
    // [JsonInclude]
    // private BoundedFloatValueAdjustable RoughVineyardCoefficient { get; set; }= new ("", 0.7f,null, 0, 1);
    
    // [JsonInclude]
    // private BoundedFloatValueAdjustable PavedAreaCoefficient { get; set; }= new ("", 1,null, 0, 1);
    // [JsonInclude]
    // private BoundedFloatValueAdjustable WideRoadCoefficient { get; set; }= new ("", 1,null, 0, 1);
    // [JsonInclude]
    // private BoundedFloatValueAdjustable RoadCoefficient { get; set; }= new ("", 1,null, 0, 1);
    // [JsonInclude]
    // private BoundedFloatValueAdjustable VehicleRoadCoefficient { get; set; }= new ("", 1,null, 0, 1);
    // [JsonInclude]
    // private BoundedFloatValueAdjustable FootPathCoefficient { get; set; }= new ("", 0.99f,null, 0, 1);
    // [JsonInclude]
    // private BoundedFloatValueAdjustable SmallFootPathCoefficient { get; set; }= new ("", 0.98f,null, 0, 1);
    // [JsonInclude]
    // private BoundedFloatValueAdjustable LessDistinctSmallFootpathCoefficient { get; set; }= new ("", 1.07f,null, 1, 2);
    // [JsonInclude]
    // private BoundedFloatValueAdjustable NarrowRideCoefficient { get; set; }= new ("", 1.07f,null, 1, 2);
    
    // [JsonInclude]
    // private BoundedFloatValueAdjustable AreaThatShallNotBeEnteredCoefficient { get; set; }= new ("", 0,null, 0, 1);
    // [JsonInclude]
    // private BoundedFloatValueAdjustable BuildingCoefficient { get; set; }= new ("", 0,null, 0, 1);
    // [JsonInclude]
    // private BoundedFloatValueAdjustable StairwayCoefficient { get; set; }= new ("", 0.93f,null, 0, 1);

    // [JsonInclude]
    // private BoundedFloatValueAdjustable EarthBankOvercomeCoefficient { get; set; } = new ("", 2, "seconds", 0, float.PositiveInfinity);
    // [JsonInclude]
    // private BoundedFloatValueAdjustable EarthWallOvercomeCoefficient { get; set; }= new("", 2, "seconds", 0, float.PositiveInfinity);
    // [JsonInclude]
    // private BoundedFloatValueAdjustable ErosionGullyOvercomeCoefficient { get; set; }= new ("", 5, "seconds", 0, float.PositiveInfinity);
    // [JsonInclude]
    // private BoundedFloatValueAdjustable ImpassableCliffOvercomeCoefficient { get; set; }= new ("", float.PositiveInfinity, "seconds", 0, float.PositiveInfinity);
    // [JsonInclude]
    // private BoundedFloatValueAdjustable CliffOvercomeCoefficient { get; set; }= new ("", 7, "seconds", 0, float.PositiveInfinity);
    // [JsonInclude]
    // private BoundedFloatValueAdjustable TrenchOvercomeCoefficient { get; set; }= new ("", 1, "seconds", 0, float.PositiveInfinity);
    // [JsonInclude]
    // private BoundedFloatValueAdjustable CrossableWaterCourseOvercomeCoefficient { get; set; }= new ("", 3, "seconds", 0, float.PositiveInfinity);
    // [JsonInclude]
    // private BoundedFloatValueAdjustable SmallCrossableWaterCourseOvercomeCoefficient { get; set; }= new ("", 2, "seconds", 0, float.PositiveInfinity);
    // [JsonInclude]
    // private BoundedFloatValueAdjustable WallOvercomeCoefficient { get; set; }= new ("", 2, "seconds", 0, float.PositiveInfinity);
    // [JsonInclude]
    // private BoundedFloatValueAdjustable UncrossableWallOvercomeCoefficient { get; set; }= new ("", float.PositiveInfinity, "seconds", 0, float.PositiveInfinity);
    // [JsonInclude]
    // private BoundedFloatValueAdjustable FenceOvercomeCoefficient { get; set; }= new ("", 2, "seconds", 0, float.PositiveInfinity);
    // [JsonInclude]
    // private BoundedFloatValueAdjustable ImpassableFenceOvercomeCoefficient { get; set; }= new ("", float.PositiveInfinity, "seconds", 0, float.PositiveInfinity);
    // [JsonInclude]
    // private BoundedFloatValueAdjustable ProminentLineFeatureOvercomeCoefficient { get; set; }= new ("", 1.5f, "seconds", 0, float.PositiveInfinity);
    // [JsonInclude]
    // private BoundedFloatValueAdjustable ImpassableProminentLineFeatureOvercomeCoefficient { get; set; }= new ("", float.PositiveInfinity, "seconds", 0, float.PositiveInfinity);
    
    public MapCoordinates RetrievePosition(Orienteering_ISOM_2017_2.VertexAttributes vertexAttributes)
        => vertexAttributes.Position;
    public TOut AcceptGeneric<TOut, TOtherParams>(IUserModelGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams)
        => genericVisitor.GenericVisit<Orienteering_ISOM_2017_2UserModel, Orienteering_ISOM_2017_2>(this, otherParams);
    public TOut AcceptGeneric<TOut>(IUserModelGenericVisitor<TOut> genericVisitor)
        => genericVisitor.GenericVisit<Orienteering_ISOM_2017_2UserModel, Orienteering_ISOM_2017_2>(this);
    public void AcceptGeneric(IUserModelGenericVisitor genericVisitor)
        => genericVisitor.GenericVisit<Orienteering_ISOM_2017_2UserModel, Orienteering_ISOM_2017_2>(this);

}