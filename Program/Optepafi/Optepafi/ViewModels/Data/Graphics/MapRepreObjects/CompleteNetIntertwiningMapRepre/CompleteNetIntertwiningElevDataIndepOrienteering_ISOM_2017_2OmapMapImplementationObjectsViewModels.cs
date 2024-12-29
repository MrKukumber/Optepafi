using Avalonia.Media;
using Optepafi.Models.GraphicsMan.Objects.MapRepre.CompleteNetIntertwiningMapRepre;
using Optepafi.Models.TemplateMan.Templates;
using Optepafi.Models.Utils;
using Optepafi.ModelViews.Utils;

namespace Optepafi.ViewModels.Data.Graphics.MapRepreObjects.CompleteNetIntertwiningMapRepre;

public class VertexObjectViewModel(VertexObject obj, MapCoordinates mapsTopLeftVertex) : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } = obj.Position.ToCanvasCoordinate(mapsTopLeftVertex) - new CanvasCoordinate(25, 25);
    public override int Priority { get; } = 36;
    public int XCoord { get; } = obj.Position.XPos;
    public int YCoord { get; } = obj.Position.YPos;
    public float FontSize { get; } = 0.5f;
    public int Diameter { get; } = 50;
    
    public int LineThickness { get; } = 10;
}
public class EdgeObjectViewModel : GraphicObjectViewModel
{
    public override CanvasCoordinate Position { get; } 
    public override int Priority { get; } = 35;

    public CanvasCoordinate StartPoint { get; }
    
    public CanvasCoordinate EndPoint { get; } 
    public int TopLineThickness { get; } = 40;
    public int BottomLineThickness { get; } = 50;

    public EdgeObjectViewModel(EdgeObject obj, MapCoordinates mapsTopLeftVertex)
    {
        Position  = obj.From.ToCanvasCoordinate(mapsTopLeftVertex);
        StartPoint = new CanvasCoordinate(0, 0);
        EndPoint  = obj.To.ToCanvasCoordinate(mapsTopLeftVertex) - obj.From.ToCanvasCoordinate(mapsTopLeftVertex);
        var surroundings = obj.Attributes.Surroundings;
        var lines = obj.Attributes.LineFeatures;

        BoulderColor = surroundings.boulders switch
        {
            Orienteering_ISOM_2017_2.Boulders.GiganticBoulder_206 => Colors.Black,
            Orienteering_ISOM_2017_2.Boulders.BoulderField_208 => Colors.Black,
            Orienteering_ISOM_2017_2.Boulders.DenseBoulderField_209 => Colors.Black,
            null => Colors.Transparent
        };
        
        GroundColor = surroundings.ground switch
        {
            Orienteering_ISOM_2017_2.Grounds.BrokenGround_113 => Colors.Chocolate,
            Orienteering_ISOM_2017_2.Grounds.VeryBrokenGround_114 => Colors.Chocolate,
            Orienteering_ISOM_2017_2.Grounds.CultivatedLand_412 => Colors.Gold,
            null => Colors.Transparent,
        };
        
        StonesColor = surroundings.stones switch
        {
            Orienteering_ISOM_2017_2.Stones.StonyGroundSlow_210 => Colors.Black,
            Orienteering_ISOM_2017_2.Stones.StonyGroundWalk_211 => Colors.Black,
            Orienteering_ISOM_2017_2.Stones.StonyGroundFight_212 => Colors.Black,
            Orienteering_ISOM_2017_2.Stones.Sandyground_213 => Colors.NavajoWhite,
            Orienteering_ISOM_2017_2.Stones.BareRock_214 => Colors.Gray,
            null => Colors.Transparent
        };

        VegetationManMadeColor = surroundings.vegetationAndManMade switch
        {
            Orienteering_ISOM_2017_2.VegetationAndManMade.OpenLand_401 => Colors.Gold,
            Orienteering_ISOM_2017_2.VegetationAndManMade.OpenLandWithTrees_402 => Colors.Gold,
            Orienteering_ISOM_2017_2.VegetationAndManMade.RoughOpenLand_403 => Colors.NavajoWhite,
            Orienteering_ISOM_2017_2.VegetationAndManMade.RoughOpenLandWithTrees_404 => Colors.NavajoWhite,
            Orienteering_ISOM_2017_2.VegetationAndManMade.Forest_405 => Colors.White,
            Orienteering_ISOM_2017_2.VegetationAndManMade.VegetationSlow_406 => Colors.DarkSeaGreen,
            Orienteering_ISOM_2017_2.VegetationAndManMade.VegetationWalk_408 => Colors.MediumSeaGreen,
            Orienteering_ISOM_2017_2.VegetationAndManMade.VegetationFight_410 => Colors.SeaGreen,
            Orienteering_ISOM_2017_2.VegetationAndManMade.Orchard_413 => Colors.Gold,
            Orienteering_ISOM_2017_2.VegetationAndManMade.RoughOrchard_413 => Colors.NavajoWhite,
            Orienteering_ISOM_2017_2.VegetationAndManMade.Vineyard_414 => Colors.Gold,
            Orienteering_ISOM_2017_2.VegetationAndManMade.RoughVineyard_414 => Colors.NavajoWhite,
            Orienteering_ISOM_2017_2.VegetationAndManMade.PavedArea_501 => Colors.BurlyWood,
            Orienteering_ISOM_2017_2.VegetationAndManMade.AreaThatShallNotBeEntered_520 => Colors.YellowGreen,
            Orienteering_ISOM_2017_2.VegetationAndManMade.Building_521 => Colors.Black,
            null => Colors.Transparent
        };

        WaterColor = surroundings.water switch
        {
            null => Colors.Transparent,
            _ => Colors.DodgerBlue
        };

        VegetationGoodVisColor = surroundings.vegetationGoodVis switch
        {
            null => Colors.Transparent,
            _ => Colors.DarkSeaGreen
        };

        NaturalObstacleColor = lines.naturalLinearObstacle switch
        {
            Orienteering_ISOM_2017_2.NaturalLinearObstacles.EarthBank_104 => Colors.Chocolate,
            Orienteering_ISOM_2017_2.NaturalLinearObstacles.EarthWall_105 => Colors.Chocolate,
            Orienteering_ISOM_2017_2.NaturalLinearObstacles.ErosionGully_107 => Colors.Chocolate,
            Orienteering_ISOM_2017_2.NaturalLinearObstacles.ImpassableCliff_201 => Colors.Black,
            Orienteering_ISOM_2017_2.NaturalLinearObstacles.Cliff_202 => Colors.Black,
            Orienteering_ISOM_2017_2.NaturalLinearObstacles.CrossableWatercourse_304 => Colors.DodgerBlue,
            Orienteering_ISOM_2017_2.NaturalLinearObstacles.SmallCrossableWatercourse_305 => Colors.DodgerBlue,
            null => Colors.Transparent
        };

        PathsColor = lines.path switch
        {
            Orienteering_ISOM_2017_2.Paths.WideRoad_502 => Colors.BurlyWood,
            null => Colors.Transparent,
            _ => Colors.Black
        };

        ManMadeObstacleColor = lines.manMadeLinearObstacle switch
        {
            null => Colors.Transparent,
            _ => Colors.Black
        };
    }
    public Color BoulderColor { get; }
    public Color GroundColor { get; }
    public Color StonesColor { get; }
    public Color VegetationManMadeColor { get; }
    public Color WaterColor { get; }
    public Color VegetationGoodVisColor { get; }
    public Color NaturalObstacleColor { get; }
    public Color PathsColor { get; }
    public Color ManMadeObstacleColor { get; }
}
