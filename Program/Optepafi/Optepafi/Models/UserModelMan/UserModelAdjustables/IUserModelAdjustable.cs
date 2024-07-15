namespace Optepafi.Models.UserModelMan.UserModelAdjustables;

/// <summary>
/// Main interface representing base contract for adjustable property of user model.
/// 
/// It defines basic identifiers for adjustable property.  
/// Adjustable prop. can be used for (surprisingly) adjusting of user model computations of edge weights for path finding algorithms.  
/// </summary>
public interface IUserModelAdjustable
{
    public string Name { get; }
    public string Caption { get; }
    public string ValueUnit { get; }
}