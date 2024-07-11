using System.Collections.Generic;
using Optepafi.Models.UserModelMan.UserModelAdjustables;

namespace Optepafi.Models.UserModelMan.UserModels;

/// <summary>
/// Represent user models that are able to provide objects by which can be internal values of models adjusted.
/// The adjustable objects are of reference types, so their changes are reflected in their holding user models.
/// User models by implementing this interface are able to be used in model creating sessions, where users can set provided adjustables to satisfy their preferences. 
/// </summary>
public interface ISettableUserModel 
{
    /// <summary>
    /// Method that provides all user models adjustables.
    /// </summary>
    /// <returns>All user models adjustables.</returns>
    public IReadOnlySet<IUserModelAdjustable> GetAdjustables();
}