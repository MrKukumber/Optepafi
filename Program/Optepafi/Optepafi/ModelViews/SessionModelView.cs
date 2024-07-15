using Optepafi.ModelViews.PathFinding;

namespace Optepafi.ModelViews;

/// <summary>
/// Base class for every session ModelView.
///
/// Session ModelViews contain instance of every associated ModelView.  
/// Sessions are huge abstract units that represents whole parts of application containing Views, ViewModels and ModelViews. These constructs work together to deliver applications mechanism to user.  
/// Session ModelViews main task is creating instances of associated ModelViews, creating necessary connections between them and providing them to session ViewModels so the could distribute them to their associated ViewModels.  
/// Session ModelViews should implement private nested successors of their associated ModelViews so that ModelViews could hide their communication private. Good example of such session implementation is <see cref="PathFindingSessionModelView"/>.  
/// </summary>
public class SessionModelView : ModelViewBase { }