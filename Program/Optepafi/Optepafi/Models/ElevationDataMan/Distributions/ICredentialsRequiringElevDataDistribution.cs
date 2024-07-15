using System.Net;
using System.Threading;
using Optepafi.Models.ElevationDataMan.Regions;

namespace Optepafi.Models.ElevationDataMan.Distributions;

/// <summary>
/// Represents elevation data distribution which for data retrieval from source needs credentials for authorisation.
/// 
/// For more information about elevation data distributions see predecessor of this class <see cref="IElevDataDistribution"/>.
/// </summary>
public interface ICredentialsRequiringElevDataDistribution : IElevDataDistribution
{
    
    ///<summary>
    /// Method used for downloading elevation data of this distribution that requires authorising credentials.
    /// 
    /// Downloads of data are done by regions. If given region is not one of defined by this distribution, exception is thrown.  
    /// This method should be able to download elevation data asynchronously. So if two requests for download of the same region are raised concurrently, it will not download those data two times.   
    /// </summary>
    /// <param name="region">Region that should be downloaded.</param>
    /// <param name="credential">Credentials which are used for authorising the access to database.</param>
    /// <param name="cancellationToken">Token for cancelling the process of downloading or region.</param>
    /// <returns>Result about success of download.</returns>
    public ElevDataManager.DownloadingResult Download(Region region, NetworkCredential credential, CancellationToken? cancellationToken);
}
