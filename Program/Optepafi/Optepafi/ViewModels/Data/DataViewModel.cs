namespace Optepafi.ViewModels.Data;

/// <summary>
/// Represents base class for ViewModels for all sorts of data produced by Model.
/// These ViewModels are then passed to View so it could create their graphic representation and display them.
/// Every data view model type therefore should has its corresponding defined View/DataTemplate.
/// DataViewModels are usually created from data by ModelView layer and passed to ViewModel layer of application.
/// 
/// For achieving coupling of ViewModel with one specific instance of datum and backward retrieval of this datum class must implement <see cref="WrappingDataViewModel{TData}"/> succcessor.
/// </summary>
public abstract class DataViewModel : ViewModelBase;