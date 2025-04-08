using System.Security;

namespace Optepafi.Models.Utils.Credentials;

public struct Credentials
{
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string? AuthenticationToken { get; set; }
}