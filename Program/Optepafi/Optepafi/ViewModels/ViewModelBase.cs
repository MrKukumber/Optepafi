using ReactiveUI;

namespace Optepafi.ViewModels;

/// <summary>
/// Base class for every ViewModel. ViewModel classes are represents view model layer of MVVM design pattern.
/// 
/// In classic sense ViewModels contains logic of application, retrieves data and services from Model and provide them to View which displays them to user.  
/// In this application this layer was split in two: ViewModel and ModelView layer.  
/// ViewModel by this split lost some of its responsibilities. Among its current tasks belongs:
/// 
/// - connecting View and ModelView classes
/// - calling methods on ModelViews by which is their inner logic executed. 
/// - retrieving results (more precisely their corresponding ViewModels) of ModelView methods execution and providing them to View.
/// - implement reactive communication with View. Reactively accept and respond to commands initiated by View (mostly on users input).
/// - managing of outer logic of application. Logic behind consistency of application state, securing validity of action taken by user, providing data (ViewModels) to View for displaying
///
/// View models have in some sense their own "consciousness". After Views command they know what actions has to be taken in order to deliver promised service.  
/// They understand and secure the validity of actions that user takes and therefore consistency of application.  
/// ViewModels should be independent from each other as much as possible. All communication between ViewModels should be performed through ModelView layer of an application.  
/// 
/// ViewModels are from default <c>ReactiveObject</c>s. They provide their service to Views by using of ReactiveUI framework. For more information on ReactiveUI and reactive programming see ReactiveUI web sites.  
/// </summary>
public class ViewModelBase : ReactiveObject;