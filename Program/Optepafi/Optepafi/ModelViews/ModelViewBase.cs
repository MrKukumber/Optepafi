namespace Optepafi.ModelViews;

/// <summary>
/// Base class for every ModelView.
///
/// ModelViews are classes which emerged from splitting of ViewModels of MVVM pattern into two parts: ViewModels and ModelViews.  
/// ModelViews communicates only with ViewModels and Models.  
/// Among their tasks belongs:
/// 
/// - connecting Model and ViewModel classes
/// - preparing data from Model for showing to user (converting them to their ViewModel representations)
/// - inner transfer of data created by Model so they can be provided to multiple ViewModels
/// - managing of inner logic of application. Logic behind use of data from Models, data dependencies, etc.
///
/// Model views does not have their own "consciousness". Someone (mostly corresponding ViewModel) has to asks for their service and they will deliver it.  
/// ViewModels have to ensure consistency of their associated ModelViews before they leave control over the application to someone else. ModelViews will not maintain their own consistency.  
/// </summary>
public class ModelViewBase { }