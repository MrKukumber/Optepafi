using Optepafi.Models.TemplateMan;
using ITemplate = Avalonia.Styling.ITemplate;

namespace Optepafi.Models.MapRepreMan;

public interface IMapRepre
{
    public IMapRepreRep<IMapRepre> MapRepreRep { get; }
    public ITemplate Template { get; }
}