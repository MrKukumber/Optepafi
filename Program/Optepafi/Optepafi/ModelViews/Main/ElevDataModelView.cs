using System.Collections.Generic;
using Optepafi.Models.ElevationDataMan;

namespace Optepafi.ModelViews.Main;

public sealed class ElevDataModelView : ModelViewBase
{
    private ElevDataModelView(){}
    public static ElevDataModelView Instance { get; } = new();

    public IReadOnlySet<IElevDataSource> ElevDataSources { get; } = ElevationDataManager.Instance.ElevDataSources;
}